using BPUtil;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcuRiteSniffer
{
	public class MqttDevice
	{
		/// <summary>
		/// Device identifier parsed from MQTT.
		/// </summary>
		public string Key = "";
		/// <summary>
		/// Friendly name assigned by user.
		/// </summary>
		public string Name
		{
			get
			{
				if (Program.settings.TryGetFriendlyDeviceName(Key, out string friendly))
					return friendly;
				return "";
			}
		}
		/// <summary>
		/// UTC Timestamp provided by this device's "time" Prop.
		/// </summary>
		[JsonIgnore]
		public DateTime? Updated
		{
			get
			{
				if (Props.TryGetValue("time", out string time) && DateTime.TryParseExact(time, "yyyy-M-d H:m:s", null, System.Globalization.DateTimeStyles.AssumeUniversal, out DateTime dt))
					return dt;
				return null;
			}
		}
		/// <summary>
		/// String to sort devices by. This is the Name if available, otherwise the Key.
		/// </summary>
		[JsonIgnore]
		public string OrderBy
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(Name))
					return Name;
				else
					return Key;
			}
		}
		/// <summary>
		/// Sorted collection of properties learned from MQTT.
		/// </summary>
		[JsonIgnore]
		private ConcurrentDictionary<string, string> Props = new ConcurrentDictionary<string, string>();
		/// <summary>
		/// Generates a SortedList from the Props concurrent dictionary.
		/// </summary>
		[JsonProperty("Props")]
		public IDictionary<string, string> SortedPropsForJson { get { return new SortedList<string, string>(Props); } }
		/// <summary>
		/// Provides the lowest and highest recent wind values in miles per hour.
		/// </summary>
		private RollingMinMaxAvg windTrackerMi = new RollingMinMaxAvg((long)TimeSpan.FromMinutes(5).TotalMilliseconds);
		/// <summary>
		/// Provides the lowest and highest recent wind values in kilometers per hour.
		/// </summary>
		private RollingMinMaxAvg windTrackerKm = new RollingMinMaxAvg((long)TimeSpan.FromMinutes(5).TotalMilliseconds);
		private double WindSpeedMph
		{
			get
			{
				if (Props.TryGetValue("wind_avg_mi_h", out string s) && double.TryParse(s, out double d))
					return d;
				return 0.0;
			}
		}
		private string WindDirAbbr
		{
			get
			{
				if (Props.TryGetValue("wind_dir_abbr", out string s))
					return s;
				return "";
			}
		}
		private double WindGustMph
		{
			get
			{
				if (Props.TryGetValue("wind_5min_high_mi_h", out string s) && double.TryParse(s, out double d))
					return d;
				return 0.0;
			}
		}
		private string WindGustDirAbbr
		{
			get
			{
				if (Props.TryGetValue("wind_5min_high_mi_dir_abbr", out string s))
					return s;
				return "";
			}
		}

		public string GetParams(string separator = "<br>")
		{
			List<string> list = new List<string>();
			list.Add("DEVICE_KEY=" + Key);
			if (!string.IsNullOrEmpty(Name))
				list.Add("DEVICE_NAME=" + Name);
			foreach (KeyValuePair<string, string> kvp in Props.OrderBy(a => a.Key))
				list.Add(kvp.Key + "=" + kvp.Value);
			return string.Join(separator, list);
		}
		public void SetValue(string fieldName, string newValue, ref bool changed)
		{
			if (!Props.TryGetValue(fieldName, out string existingValue) || newValue != existingValue)
			{
				Props[fieldName] = newValue;
				if (fieldName == "time")
				{
					ProcessNewTime(Updated);
				}
				else if (fieldName == "wind_dir_deg" && double.TryParse(newValue, out double windDirDeg))
				{
					CompassDirection dir = Compass.GetCompassDirection((int)Math.Round(windDirDeg));
					Props["wind_dir_abbr"] = dir.ToString();
					Props["wind_dir_full"] = Compass.GetCompassDirectionName(dir);
				}
				else if (fieldName == "humidity" || fieldName == "temperature_C" || fieldName == "temperature_F")
				{
					if (fieldName == "temperature_C")
						Props["temperature_F"] = Convert_C_to_F(newValue);
					else if (fieldName == "temperature_F")
						Props["temperature_C"] = Convert_F_to_C(newValue);

					if (Props.TryGetValue("humidity", out string strHumidity) && double.TryParse(strHumidity, out double humidity)
						&& Props.TryGetValue("temperature_C", out string strC) && double.TryParse(strC, out double C))
					{
						double absoluteHumidityGramsPerCubicMeter = (6.112 * Math.Pow(Math.E, (17.67 * C) / (C + 243.5)) * (humidity * 2.1674)) / (273.15 + C);
						Props["humdity_abs_gpm3"] = absoluteHumidityGramsPerCubicMeter.ToString("0.#");
					}
				}
				changed = true;
			}
		}

		private string Convert_F_to_C(string strF)
		{
			if (double.TryParse(strF, out double F))
				return ((F - 32) * (5.0 / 9.0)).ToString("0.##");
			else
				return double.NaN.ToString();
		}

		private string Convert_C_to_F(string strC)
		{
			if (double.TryParse(strC, out double C))
				return ((C * (5.0 / 9.0)) + 32).ToString("0.##");
			else
				return double.NaN.ToString();
		}

		/// <summary>
		/// Process an updated timestamp.  This does things like parse the time string and update wind low and high values.
		/// </summary>
		/// <param name="newTimeStr"></param>
		private void ProcessNewTime(DateTime? updated)
		{
			if (updated == null)
			{
				Props["time_epoch_ms"] = "0";
				Props["time_local"] = "";
				return;
			}
			Props["time_epoch_ms"] = TimeUtil.GetTimeInMsSinceEpoch(updated.Value).ToString();
			Props["time_local"] = updated.Value.ToString("yyyy-MM-dd hh:mm:ss tt");
			SetTimeout.OnBackground(() =>
			{
				updated = Updated;

				bool hasAnyWindData = false;
				// Compute wind averages over time
				double wind_dir_deg = 0;
				if (!Props.TryGetValue("wind_dir_deg", out string str_wind_dir_deg) || !double.TryParse(str_wind_dir_deg, out wind_dir_deg))
					wind_dir_deg = 0;
				if (Props.TryGetValue("wind_avg_mi_h", out string str_wind_speed_mph) && double.TryParse(str_wind_speed_mph, out double wind_speed_mph))
				{
					hasAnyWindData = true;
					windTrackerMi.Add(wind_speed_mph, wind_dir_deg);

					RollingMinMaxAvg.StoredValue low = windTrackerMi.GetMinimum();
					CompassDirection dir = Compass.GetCompassDirection((int)Math.Round(low.directionDegrees));
					Props["wind_5min_low_mi_h"] = low.speed.ToString();
					Props["wind_5min_low_mi_dir_abbr"] = dir.ToString();
					Props["wind_5min_low_mi_dir_full"] = Compass.GetCompassDirectionName(dir);

					RollingMinMaxAvg.StoredValue high = windTrackerMi.GetMaximum();
					dir = Compass.GetCompassDirection((int)Math.Round(high.directionDegrees));
					Props["wind_5min_high_mi_h"] = high.speed.ToString();
					Props["wind_5min_high_mi_dir_abbr"] = dir.ToString();
					Props["wind_5min_high_mi_dir_full"] = Compass.GetCompassDirectionName(dir);

					RollingMinMaxAvg.StoredValue avg = windTrackerMi.GetAverage();
					dir = Compass.GetCompassDirection((int)Math.Round(avg.directionDegrees));
					Props["wind_5min_avg_mi_h"] = avg.speed.ToString();
					Props["wind_5min_avg_mi_dir_abbr"] = dir.ToString();
					Props["wind_5min_avg_mi_dir_full"] = Compass.GetCompassDirectionName(dir);
				}
				if (Props.TryGetValue("wind_avg_km_h", out string str_wind_speed_kph) && double.TryParse(str_wind_speed_kph, out double wind_speed_kph))
				{
					windTrackerKm.Add(wind_speed_kph, wind_dir_deg);

					RollingMinMaxAvg.StoredValue low = windTrackerKm.GetMinimum();
					CompassDirection dir = Compass.GetCompassDirection((int)Math.Round(low.directionDegrees));
					Props["wind_5min_low_km_h"] = low.speed.ToString();
					Props["wind_5min_low_km_dir_abbr"] = dir.ToString();
					Props["wind_5min_low_km_dir_full"] = Compass.GetCompassDirectionName(dir);

					RollingMinMaxAvg.StoredValue high = windTrackerKm.GetMaximum();
					dir = Compass.GetCompassDirection((int)Math.Round(high.directionDegrees));
					Props["wind_5min_high_km_h"] = high.speed.ToString();
					Props["wind_5min_high_km_dir_abbr"] = dir.ToString();
					Props["wind_5min_high_km_dir_full"] = Compass.GetCompassDirectionName(dir);

					RollingMinMaxAvg.StoredValue avg = windTrackerKm.GetAverage();
					dir = Compass.GetCompassDirection((int)Math.Round(avg.directionDegrees));
					Props["wind_5min_avg_km_h"] = avg.speed.ToString();
					Props["wind_5min_avg_km_dir_abbr"] = dir.ToString();
					Props["wind_5min_avg_km_dir_full"] = Compass.GetCompassDirectionName(dir);
				}
				if (hasAnyWindData)
				{
					int speed = (int)Math.Round(WindSpeedMph);
					int gust = (int)Math.Round(WindGustMph);
					StringBuilder sbWind = new StringBuilder();
					if (speed == 0 && gust == 0)
						sbWind.Append("Calm");
					else
					{
						if (speed > 0)
						{
							sbWind.Append(speed + " MPH " + WindDirAbbr);
							if (gust > speed)
								sbWind.Append(", ");
						}
						if (gust > speed)
						{
							sbWind.Append("gusting to " + gust);
							if (speed == 0)
								sbWind.Append(" MPH " + WindGustDirAbbr);
						}
					}
					Props["wind_desc_mi"] = sbWind.ToString();
				}
			}, 250);
		}

		public void WriteFile(DataFileTemplate template)
		{
			SetTimeout.OnBackground(() =>
			{
				try
				{
					Robust.Retry(() =>
					{
						FileInfo fi = new FileInfo("SensorData/" + template.FileName);
						if (!fi.Directory.Exists)
							Directory.CreateDirectory(fi.Directory.FullName);
						if (!fi.Exists || !fi.IsReadOnly)
							File.WriteAllText(fi.FullName, ApplyTemplate(template.TemplateStr), Encoding.GetEncoding(1252));
					}, 1, 5, 10, 20, 50, 250, 500, 1000);
				}
				catch (Exception ex)
				{
					Logger.Debug(ex, "MqttDevice.WriteFile(\"" + template.ToString() + "\")");
				}
			}, 0, e => Logger.Debug(e));
		}

		private static Regex rxFileTemplate = new Regex("(##([^\\s]+)##)", RegexOptions.Compiled & RegexOptions.Singleline);
		public string ApplyTemplate(string template)
		{
			return rxFileTemplate.Replace(template.Replace("\\n", Environment.NewLine), GetMatchedValue);
		}
		private string GetMatchedValue(Match m)
		{
			string k = m.Groups[2].Value;
			if (k.Equals("DEVICE_KEY", StringComparison.OrdinalIgnoreCase))
				return Key;
			if (k.Equals("DEVICE_NAME", StringComparison.OrdinalIgnoreCase))
				return Name;
			if (Props.TryGetValue(k, out string v))
				return v;
			else
				return "";
		}
	}
	public class MqttReader : IDisposable
	{
		public event EventHandler<string> OnError = delegate { };
		public event EventHandler<string> OnStatusUpdate = delegate { };
		public event EventHandler<object> OnDeviceUpdate = delegate { };

		private ConcurrentDictionary<string, MqttDevice> devices = new ConcurrentDictionary<string, MqttDevice>();

		private string host;
		private int tcpPort;
		private string user;
		private string pass;

		private IManagedMqttClient managedMqttClient;
		private List<DataFileTemplate> templates;

		public MqttReader(string host, int tcpPort, string user, string pass)
		{
			this.host = host;
			this.tcpPort = tcpPort;
			this.user = user;
			this.pass = pass;

			templates = Program.settings.GetSensorDataTemplates();
		}

		/// <summary>
		/// Starts the MQTT Client.
		/// </summary>
		/// <returns></returns>
		public async Task Start()
		{
			if (managedMqttClient != null)
				return;

			managedMqttClient = new MqttFactory().CreateManagedMqttClient();

			IMqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
					.WithTcpServer(host, tcpPort)
					.WithCredentials(user, pass)
					.Build();

			ManagedMqttClientOptions managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
				.WithClientOptions(mqttClientOptions)
				.Build();

			managedMqttClient.UseApplicationMessageReceivedHandler(e =>
			{
				IngestMessage(e.ApplicationMessage);
				return Task.CompletedTask;
			});
			managedMqttClient.UseConnectedHandler(e =>
			{
				OnStatusUpdate(this, "MQTT connected.");
			});
			managedMqttClient.UseDisconnectedHandler(e =>
			{
				if (e.ClientWasConnected)
					OnStatusUpdate(this, "MQTT disconnected.");
				else
					OnStatusUpdate(this, "MQTT failed to connect.");
			});
			managedMqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(e =>
			{
				OnError(this, "MQTT connection failed: " + e.Exception.FlattenMessages());
			});

			await managedMqttClient.StartAsync(managedMqttClientOptions);

			string topic = "rtl_433/+/devices/#";
			OnStatusUpdate(this, "Will subscribe to topic \"" + topic + "\"");
			await managedMqttClient.SubscribeAsync(topic);
		}
		private void IngestMessage(MqttApplicationMessage m)
		{
			try
			{
				int idxDevices = m.Topic.IndexOf("/devices/");
				if (idxDevices < 0)
				{
					OnError(this, "Topic \"" + m.Topic + "\" did not match the expected pattern.");
					return;
				}

				string subtopic = m.Topic.Substring(idxDevices + "/devices/".Length);

				int idxLastSlash = subtopic.LastIndexOf('/');
				if (idxLastSlash < 0)
				{
					OnError(this, "Topic \"" + m.Topic + "\" did not match the expected pattern.");
					return;
				}

				string fieldName = subtopic.Substring(idxLastSlash + 1);
				if (fieldName == "sequence_num" || fieldName == "message_type" || fieldName == "mic")
					return;

				string deviceKey = StringUtil.MakeSafeForFileName(subtopic.Substring(0, idxLastSlash), "_").Replace('=', '_').Replace('[', '_').Replace(']', '_');

				MqttDevice device = devices.GetOrAdd(deviceKey, s =>
				{
					MqttDevice o = new MqttDevice();
					o.Key = deviceKey;
					return o;
				});

				bool changed = false;

				string newValue = ByteUtil.Utf8NoBOM.GetString(m.Payload);
				device.SetValue(fieldName, newValue, ref changed);

				if (changed)
				{
					foreach (DataFileTemplate template in templates)
					{
						if (device.Key == template.UniqueID || device.Name == template.UniqueID)
							device.WriteFile(template);
					}
					OnDeviceUpdate(this, device);
				}
			}
			catch (Exception ex)
			{
				OnError(this, ex.ToString());
			}
		}

		/// <summary>
		/// Returns an array of all devices currently known.
		/// </summary>
		/// <returns></returns>
		public MqttDevice[] GetDevices()
		{
			return devices.Values.ToArray();
		}

		public void Dispose()
		{
			if (managedMqttClient != null)
			{
				managedMqttClient.StopAsync();
				managedMqttClient.Dispose();
			}
		}
	}
}
