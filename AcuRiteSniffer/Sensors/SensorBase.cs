using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BPUtil;
using Newtonsoft.Json;

namespace AcuRiteSniffer.Sensors
{
	/// <summary>
	/// Base class for sensor data.
	/// Some information used to create this class was found here: https://github.com/jonasacres/weathersnarf
	/// </summary>
	public class SensorBase
	{
		/// <summary>
		/// The URL this instance was built from.
		/// </summary>
		public readonly string originalUrl;
		/// <summary>
		/// DateTime in UTC that this sensor reading was received.
		/// </summary>
		public readonly DateTime dateUtc;

		/// <summary>
		/// MAC address of the SmartHUB.
		/// </summary>
		public readonly string MAC;

		/// <summary>
		/// Sensor ID.
		/// </summary>
		public readonly string ID;

		/// <summary>
		/// [MAC address of the SmartHUB] + "_" + [ID of the sensor]
		/// e.g. "24C86E000000_12345678"
		/// 
		/// </summary>
		public readonly string UniqueID;

		/// <summary>
		/// Model number.
		/// </summary>
		public readonly string Model;

		/// <summary>
		/// Battery level "normal" is normal.  It is unknown what low battery status looks like.
		/// </summary>
		public readonly string Battery;

		/// <summary>
		/// Battery level of the AcuRite Access device.  Battery level "normal" is normal.  It is unknown what low battery status looks like.
		/// </summary>
		public readonly string HubBattery;

		/// <summary>
		/// Signal strength (from 1 to 5?).
		/// </summary>
		public readonly int SignalStrength;

		/// <summary>
		/// Barometric pressure in inches of mercury.
		/// </summary>
		public readonly double BarometricPressure;

		/// <summary>
		/// Humidity at indoor room sensor.
		/// </summary>
		public readonly int IndoorHumidity;

		/// <summary>
		/// Temperature in degrees fahrenheit at indoor room sensor.
		/// </summary>
		public readonly double IndoorTempF;

		/// <summary>
		/// Humidity at probe or main device.
		/// </summary>
		public readonly int Humidity;

		/// <summary>
		/// Temperature in degrees fahrenheit at probe or main device.
		/// </summary>
		public readonly double TempF;

		/// <summary>
		/// Unknown.
		/// </summary>
		public readonly int Probe;

		/// <summary>
		/// Unknown.
		/// </summary>
		public readonly int Check;

		/// <summary>
		/// Output of water sensor attached to room sensor.  0 for no water.  Presumably 1 when water is detected.
		/// </summary>
		public readonly int Water;

		/// <summary>
		/// Wind speed in MPH.
		/// </summary>
		public readonly int WindSpeed;

		/// <summary>
		/// Wind direction in degrees.  It is unclear what this is relative to.
		/// </summary>
		public readonly int WindDirection;

		/// <summary>
		/// Rainfall in inches, resets periodically.
		/// </summary>
		public readonly double RainRecent;

		/// <summary>
		/// Rainfall in inches, reset daily.
		/// </summary>
		public readonly double RainDaily;

		/// <summary>
		/// Dew point in degrees fahrenheit.
		/// </summary>
		public readonly int DewPointF;


		[JsonIgnore]
		public SortedList<string, string> raw_arguments;

		public SensorBase(string url)
		{
			originalUrl = url;
			dateUtc = DateTime.UtcNow;
			DateTime now = DateTime.Now;
			ParseQueryStringArguments(url);
			raw_arguments["date"] = now.ToString("yyyy-MM-dd");
			raw_arguments["time"] = now.ToString("hh:mm:ss tt");

			// Assumed to be in all packets
			MAC = GetString("id");
			ID = GetString("sensor");
			UniqueID = MAC + "_" + ID;
			Model = GetString("mt");
			Battery = GetString("battery");
			if (string.IsNullOrWhiteSpace(Battery))
				Battery = GetString("sensorbattery");
			HubBattery = GetString("hubbattery");
			SignalStrength = GetInt("rssi");
			BarometricPressure = GetDouble("baromin");

			// Not in all packets
			IndoorHumidity = GetInt("indoorhumidity");
			IndoorTempF = GetDouble("indoortempf");
			Humidity = GetInt("humidity");
			TempF = GetDouble("tempf");
			Probe = GetInt("probe");
			Check = GetInt("check");
			Water = GetInt("water");
			WindSpeed = GetInt("windspeedmph");
			WindDirection = GetInt("winddir");
			RainRecent = GetDouble("rainin");
			RainDaily = GetDouble("dailyrainin");
			DewPointF = GetInt("dewptf");
		}
		protected string GetString(string key)
		{
			if (raw_arguments.TryGetValue(key.ToLower(), out string value))
				return value;
			return "";
		}
		protected int GetInt(string key, int defaultValue = default(int))
		{
			if (int.TryParse(GetString(key), out int value))
				return value;
			return defaultValue;
		}
		protected long GetLong(string key, long defaultValue = default(long))
		{
			if (long.TryParse(GetString(key), out long value))
				return value;
			return defaultValue;
		}
		protected float GetFloat(string key, float defaultValue = default(float))
		{
			if (float.TryParse(GetString(key), out float value))
				return value;
			return defaultValue;
		}
		protected double GetDouble(string key, double defaultValue = default(double))
		{
			if (double.TryParse(GetString(key), out double value))
				return value;
			return defaultValue;
		}
		/// <summary>
		/// Parses the specified URL returns a sorted list containing the arguments found in the query string.
		/// </summary>
		/// <param name="url">The URL to parse.</param>
		/// <returns></returns>
		private void ParseQueryStringArguments(string url)
		{
			raw_arguments = new SortedList<string, string>();
			int idx = url.IndexOf('?');
			if (idx > -1)
				url = url.Substring(idx + 1);
			else
				return;
			idx = url.LastIndexOf('#');
			string hash = null;
			if (idx > -1)
			{
				hash = url.Substring(idx + 1);
				url = url.Remove(idx);
			}
			string[] parts = url.Split(new char[] { '&' });
			for (int i = 0; i < parts.Length; i++)
			{
				string[] argument = parts[i].Split(new char[] { '=' });
				if (argument.Length == 2)
				{
					string key = Uri.UnescapeDataString(argument[0]);
					key = key.ToLower();
					raw_arguments[key] = Uri.UnescapeDataString(argument[1]);
				}
			}
			if (hash != null)
				raw_arguments["#"] = hash;
		}

		public string GetParams(string separator = "<br>")
		{
			List<string> list = new List<string>();
			foreach (var kvp in raw_arguments)
				list.Add(kvp.Key + "=" + kvp.Value);
			return string.Join(separator, list);
		}

		public void WriteFile(DataFileTemplate template)
		{
			try
			{
				FileInfo fi = new FileInfo("SensorData/" + template.FileName);
				if (!fi.Directory.Exists)
					Directory.CreateDirectory(fi.Directory.FullName);
				if (!fi.Exists || !fi.IsReadOnly)
					File.WriteAllText(fi.FullName, ApplyTemplate(template.TemplateStr), Encoding.GetEncoding(1252));
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "SensorBase.WriteFile(\"" + template.ToString() + "\"): " + originalUrl);
			}
		}

		private static Regex rxFileTemplate = new Regex("(##([^\\s]+)##)", RegexOptions.Compiled & RegexOptions.Singleline);
		public string ApplyTemplate(string template)
		{
			return rxFileTemplate.Replace(template.Replace("\\n", Environment.NewLine), GetMatchedValue);
		}
		private string GetMatchedValue(Match m)
		{
			return GetString(m.Groups[2].Value);
		}
	}
}
