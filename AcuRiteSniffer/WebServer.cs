using System;
using System.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using AcuRiteSniffer.Sensors;
using BPUtil;
using BPUtil.SimpleHttp;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace AcuRiteSniffer
{
	public class WebServer : HttpServer
	{
		ConcurrentDictionary<string, SensorBase> sensorDataCollection = new ConcurrentDictionary<string, SensorBase>();
		MqttReader mqttReader;
		Regex rxGetURL = new Regex("(GET|POST) (/weatherstation/updateweatherstation\\?[^ ]*?) HTTP/1\\.", RegexOptions.Compiled & RegexOptions.Singleline);

		public WebServer(int port = 45411, int httpsPort = -1) : base(port, httpsPort)
		{
			if (!string.IsNullOrWhiteSpace(Program.settings.mqttHost) && Program.settings.mqttTcpPort > 0 && Program.settings.mqttTcpPort < 65536 && !string.IsNullOrWhiteSpace(Program.settings.mqttUser) && !string.IsNullOrWhiteSpace(Program.settings.mqttPass))
			{
				mqttReader = new MqttReader(Program.settings.mqttHost, Program.settings.mqttTcpPort, Program.settings.mqttUser, Program.settings.mqttPass);
				mqttReader.OnError += MqttReader_OnError;
				_ = mqttReader.Start();
			}
		}

		private void MqttReader_OnError(object sender, string e)
		{
			Logger.Debug("WebServer.mqttReader OnError from " + (sender is MqttDevice ? (sender as MqttDevice).GetNameOrKey() : sender?.ToString()) + ": " + e);
		}

		public override bool shouldLogRequestsToFile()
		{
			return false;
		}

		public override void handleGETRequest(HttpProcessor p)
		{
			if (HandleRequestFromAcuriteAccessDevice(p))
				return;
			else if (p.requestedPage == "json")
			{
				IEnumerable<object> sensors = BuildSensorList(p);
				string str = JsonConvert.SerializeObject(sensors);
				p.writeSuccess("application/json", HttpProcessor.Utf8NoBOM.GetByteCount(str), additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
				p.outputStream.Write(str);
			}
			else if (p.requestedPage == "params")
			{
				StringBuilder sb = new StringBuilder();
				IEnumerable<object> sensors = BuildSensorList(p);
				foreach (object o in sensors)
				{
					if (o is SensorBase)
					{
						SensorBase sensor = (SensorBase)o;
						sb.Append("[" + sensor.UniqueID + "]<br>");
						sb.Append(sensor.GetParams());
						sb.Append("<br><br>");
					}
					else
					{
						MqttDevice d = (MqttDevice)o;
						sb.Append("[" + d.OrderBy + "]<br>");
						sb.Append(d.GetParams());
						sb.Append("<br><br>");
					}
				}
				string str = sb.ToString();
				p.writeSuccess(contentLength: HttpProcessor.Utf8NoBOM.GetByteCount(str), additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
				p.outputStream.Write(str);
			}
			else if (p.requestedPage == "lastacuriteaccessrequests")
			{
				string str = "[\r\n" + string.Join("]\r\n\r\n***************************\r\n\r\n[\r\n", lastAcuriteAccessRequests.Select(i => i.ToString())) + "\r\n]\r\n";
				p.writeSuccess("text/plain", HttpProcessor.Utf8NoBOM.GetByteCount(str), additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
				p.outputStream.Write(str);
			}
			else if (p.requestedPage == "getfriendlydevicenames")
			{
				string str = JsonConvert.SerializeObject(Program.settings.GetFriendlyDeviceNames(), Formatting.Indented);
				p.writeSuccess("application/json", HttpProcessor.Utf8NoBOM.GetByteCount(str), additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
				p.outputStream.Write(str);
			}
			else if (p.requestedPage == "setfriendlydevicename")
			{
				string key = p.GetParam("key");
				string value = p.GetParam("value");
				if (!Program.settings.TrySetFriendlyDeviceName(key, value, out string errorMessage))
				{
					p.writeSuccess("text/plain", HttpProcessor.Utf8NoBOM.GetByteCount(errorMessage), responseCode: "400 Bad Request", additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
					p.outputStream.Write(errorMessage);
				}
				else
				{
					p.writeSuccess("text/plain", 0, additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
				}
			}
			else
			{
				List<DataFileTemplate> templates = Program.settings.GetSensorDataTemplates();
				string pageLower = p.requestedPage.ToLower();
				foreach (DataFileTemplate template in templates)
				{
					if (template.FileName.ToLower() == pageLower)
					{
						FileInfo fi = new FileInfo("SensorData/" + template.FileName);
						if (fi.Exists)
						{
							p.writeSuccess("text/plain; charset=UTF-8", fi.Length, additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
							p.outputStream.Write(File.ReadAllText(fi.FullName, Encoding.GetEncoding(1252)));
						}
						else
						{
							p.writeFailure();
						}
						return;
					}
				}

				IEnumerable<object> sensors = BuildSensorList(p);
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(@"<!DOCTYPE html>
<html>
<head>
<title>AcuRite Sniffer Home</title>
</head>
<body>
<h2>The following commands are available:</h2>
	<p><a href=""/json"">/json</a> - Get JSON records for all sensors</p>
	<div></div>");

				foreach (object sensor in sensors)
				{
					string key = OrderSelector(sensor);
					sb.AppendLine();
					sb.AppendLine("\t<div><a href=\"/json?uniqueid=" + StringUtil.HtmlAttributeEncode(Uri.EscapeDataString(key)) + "\">/json?uniqueid=" + StringUtil.HtmlEncode(Uri.EscapeDataString(key))
						+ "</a> - Get JSON record for the sensor \"" + StringUtil.HtmlEncode(key) + "\"</div>");
				}
				sb.AppendLine("<p></p>");
				sb.AppendLine("<p>Add multiple uniqueid arguments to retrieve values from multiple sensors.</p>");
				sb.AppendLine("<p></p>");
				sb.AppendLine(@"<p><a href=""/params"">/params</a> - Get a list of parameters available for all sensors</p>");
				sb.AppendLine("<p></p>");
				foreach (object sensor in sensors)
				{
					string key = OrderSelector(sensor);
					sb.AppendLine();
					sb.AppendLine("\t<div><a href=\"/params?uniqueid=" + StringUtil.HtmlAttributeEncode(Uri.EscapeDataString(key)) + "\">/params?uniqueid=" + StringUtil.HtmlEncode(Uri.EscapeDataString(key)) + "</a> - Get a list of parameters available for sensor \"" + StringUtil.HtmlEncode(key) + "\"</div>");
				}
				sb.AppendLine("<p></p>");
				foreach (DataFileTemplate template in templates.OrderBy(t => t.FileName))
				{
					sb.AppendLine();
					sb.AppendLine("\t<div><a href=\"/" + template.FileName + "\">/" + template.FileName + "</a> - Get a custom text file for the sensor \"" + template.UniqueID + "\"</div>");
				}
				sb.AppendLine(@"<p><a href=""/lastacuriteaccessrequests"">/lastacuriteaccessrequests</a> - Get the last 10 requests captured and proxied from configured AcuRite Access IP Addresses.</p>");
				sb.AppendLine("<p></p>");
				sb.AppendLine(@"<p><a href=""/getfriendlydevicenames"">/getfriendlydevicenames</a> - Get a list of key/value pairs mapping unique device IDs to unique friendly names.  Friendly names can be used as an alternative to the device ID when querying device data and writing custom text file definitions.</p>");
				sb.AppendLine(@"<p><a href=""/setfriendlydevicename?key=XXX&value=XXX"">/setfriendlydevicename?key=XXX&value=XXX</a> - Set a mapping for device ID to unique friendly name.</p>");
				sb.AppendLine("<p></p>");
				sb.AppendLine("<p>Set Friendly Names:</p>");
				int id = 0;
				foreach (object sensor in sensors)
				{
					string key = GetDeviceKey(sensor);
					sb.AppendLine();
					string friendlyName;
					Program.settings.TryGetFriendlyDeviceName(key, out friendlyName);
					sb.Append("\t<div><span style=\"font-family: consolas, monospace;\">");
					sb.Append(StringUtil.HtmlEncode(key));
					sb.AppendLine(":</span>");
					sb.AppendLine("<input type=\"text\" id=\"friendlyName" + id + "\" value=\"" + StringUtil.HtmlAttributeEncode(friendlyName) + "\">");
					sb.AppendLine("<input type=\"button\" value=\"<- Set\" onclick=\"setDeviceFriendlyName(event, '" + StringUtil.HtmlAttributeEncode(key) + "', 'friendlyName" + id + "')\">");
					sb.AppendLine("</div>");
					id++;
				}
				sb.Append(@"
<script type=""text/javascript"">
function setDeviceFriendlyName(e, key, inputId)
{
	try
	{
		e.target.setAttribute('disabled', 'disabled');
		var value = document.getElementById(inputId).value; 
		fetch('/setfriendlydevicename?key=' + encodeURIComponent(key) + '&value=' + encodeURIComponent(value))
			.then(function (result)
			{
				e.target.removeAttribute('disabled');
			})
			.catch(function (err)
			{
				alert(err);
			});
	}
	catch(ex)
	{
		alert(ex.message);
	}
}
</script>
</body>
</html>");
				string str = sb.ToString();
				p.writeSuccess(contentLength: HttpProcessor.Utf8NoBOM.GetByteCount(str), additionalHeaders: getAdditionalHeaders(), keepAlive: p.keepAliveRequested);
				p.outputStream.Write(str);
			}
		}

		private IEnumerable<object> BuildSensorList(HttpProcessor p)
		{
			List<object> sensors = new List<object>();
			string[] uniqueIds = p.GetParam("uniqueid").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (uniqueIds.Length > 0)
			{
				HashSet<string> idHash = new HashSet<string>(uniqueIds);
				SensorBase[] matches = sensorDataCollection.Values.Where(d => idHash.Contains(d.UniqueID) || idHash.Contains(d.DeviceName)).ToArray();
				if (matches.Length > 0)
					sensors.AddRange(matches);
				if (mqttReader != null)
				{
					MqttDevice[] mqttDevices = mqttReader.GetDevices().Where(d => idHash.Contains(d.Key) || idHash.Contains(d.Name)).ToArray();
					if (mqttDevices.Length > 0)
						sensors.AddRange(mqttDevices);
				}
			}
			else
			{
				sensors.AddRange(sensorDataCollection.Values);
				if (mqttReader != null)
					sensors.AddRange(mqttReader.GetDevices());
			}
			return sensors.OrderBy(OrderSelector);
		}

		private static string OrderSelector(object o)
		{
			if (o is SensorBase)
				return ((SensorBase)o).OrderBy;
			else
				return ((MqttDevice)o).OrderBy;
		}
		private static string GetDeviceKey(object o)
		{
			if (o is SensorBase)
				return ((SensorBase)o).UniqueID;
			else
				return ((MqttDevice)o).Key;
		}

		private List<KeyValuePair<string, string>> getAdditionalHeaders()
		{
			List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
			headers.Add(new KeyValuePair<string, string>("Access-Control-Allow-Origin", "*"));
			return headers;
		}

		public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
		{
			if (HandleRequestFromAcuriteAccessDevice(p))
				return;
		}
		protected override void stopServer()
		{
		}

		ConcurrentQueue<ProxyDataBuffer> lastAcuriteAccessRequests = new ConcurrentQueue<ProxyDataBuffer>();
		/// <summary>
		/// If the remote IP address is that of a known Acurite Access device, we will sniff whatever data we want from it and proxy the request to Acurite.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		private bool HandleRequestFromAcuriteAccessDevice(HttpProcessor p)
		{
			if (Program.settings.GetAcuriteAccessIPs().Contains(p.RemoteIPAddressStr))
			{
				ProxyDataBuffer proxiedDataBuffer = new ProxyDataBuffer();
				p.ProxyTo("https://atlasapi.myacurite.com" + p.request_url.PathAndQuery, 30000, true, proxiedDataBuffer);

				lastAcuriteAccessRequests.Enqueue(proxiedDataBuffer);
				if (lastAcuriteAccessRequests.Count > 10)
					lastAcuriteAccessRequests.TryDequeue(out ProxyDataBuffer removed);

				foreach (string str in proxiedDataBuffer.Items.Select(i => i.PayloadAsString))
				{
					Match m = rxGetURL.Match(str);
					if (m.Success)
					{
						SensorBase sensorData = new SensorBase(m.Groups[2].Value);
						sensorDataCollection[sensorData.UniqueID] = sensorData;
						List<DataFileTemplate> templates = Program.settings.GetSensorDataTemplates();
						foreach (DataFileTemplate template in templates)
						{
							if (sensorData.UniqueID == template.UniqueID || sensorData.DeviceName == template.UniqueID)
								sensorData.WriteFile(template);
						}
					}
				}
				return true;
			}
			return false;
		}
	}
}