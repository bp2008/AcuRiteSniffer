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
		Regex rxGetURL = new Regex("(GET|POST) (/weatherstation/updateweatherstation\\?[^ ]*?) HTTP/1\\.", RegexOptions.Compiled & RegexOptions.Singleline);

		public WebServer(int port = 45411, int httpsPort = -1) : base(port, httpsPort)
		{
		}

		public override void handleGETRequest(HttpProcessor p)
		{

			if (HandleRequestFromAcuriteAccessDevice(p))
				return;
			else if (p.requestedPage == "json")
			{
				List<SensorBase> sensors = new List<SensorBase>();
				string uniqueId = p.GetParam("uniqueid");
				if (uniqueId != "")
				{
					if (sensorDataCollection.TryGetValue(uniqueId, out SensorBase single))
						sensors.Add(single);
				}
				else
					foreach (SensorBase sensor in sensorDataCollection.Values)
						sensors.Add(sensor);
				string str = JsonConvert.SerializeObject(sensors.OrderBy(s => s.UniqueID));
				p.writeSuccess("application/json", HttpProcessor.Utf8NoBOM.GetByteCount(str));
				p.outputStream.Write(str);
			}
			else if (p.requestedPage == "params")
			{
				StringBuilder sb = new StringBuilder();
				List<SensorBase> sensors = new List<SensorBase>();
				string uniqueId = p.GetParam("uniqueid");
				if (uniqueId != "")
				{
					if (sensorDataCollection.TryGetValue(uniqueId, out SensorBase single))
						sensors.Add(single);
				}
				else
					foreach (SensorBase sensor in sensorDataCollection.Values)
						sensors.Add(sensor);
				foreach (SensorBase sensor in sensors.OrderBy(s => s.UniqueID))
				{
					sb.Append("[" + sensor.UniqueID + "]<br>");
					sb.Append(sensor.GetParams());
					sb.Append("<br><br>");
				}
				string str = sb.ToString();
				p.writeSuccess(contentLength: HttpProcessor.Utf8NoBOM.GetByteCount(str));
				p.outputStream.Write(str);
			}
			else if (p.requestedPage == "lastrequests")
			{
				string str = JsonConvert.SerializeObject(lastRequests.ToArray(), Formatting.Indented);
				p.writeSuccess("application/json", HttpProcessor.Utf8NoBOM.GetByteCount(str));
				p.outputStream.Write(str);
			}
			else if (p.requestedPage == "lastacuriteaccessrequests")
			{
				string str = "[\r\n" + string.Join("]\r\n\r\n***************************\r\n\r\n[\r\n", lastAcuriteAccessRequests.Select(i => i.ToString())) + "\r\n]\r\n";
				p.writeSuccess("text/plain", HttpProcessor.Utf8NoBOM.GetByteCount(str));
				p.outputStream.Write(str);
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
							p.writeSuccess("text/plain; charset=UTF-8", fi.Length);
							p.outputStream.Write(File.ReadAllText(fi.FullName, Encoding.GetEncoding(1252)));
						}
						else
						{
							p.writeFailure();
						}
						return;
					}
				}

				StringBuilder sb = new StringBuilder();
				sb.Append(@"<!DOCTYPE html>
<html>
<head>
<title>AcuRite Sniffer Home</title>
</head>
<body>
<h2>The following commands are available:</h2>
	<p><a href=""/json"">/json</a> - Get JSON records for all sensors</p>
	<div></div>");

				foreach (SensorBase sensor in sensorDataCollection.Values.OrderBy(s => s.UniqueID))
				{
					sb.Append(Environment.NewLine);
					sb.Append("\t<div><a href=\"/json?uniqueid=" + sensor.UniqueID + "\">/json?uniqueid=" + sensor.UniqueID
						+ "</a> - Get JSON record for the sensor \"" + sensor.UniqueID + "\"</div>");
				}
				sb.Append("<p></p>");
				sb.Append(@"<p><a href=""/params"">/params</a> - Get a list of parameters available for all sensors</p>");
				sb.Append("<p></p>");
				foreach (SensorBase sensor in sensorDataCollection.Values.OrderBy(s => s.UniqueID))
				{
					sb.Append(Environment.NewLine);
					sb.Append("\t<div><a href=\"/params?uniqueid=" + sensor.UniqueID + "\">/params?uniqueid=" + sensor.UniqueID + "</a> - Get a list of parameters available for sensor \"" + sensor.UniqueID + "\"</div>");
				}
				sb.Append("<p></p>");
				foreach (DataFileTemplate template in templates.OrderBy(t => t.FileName))
				{
					sb.Append(Environment.NewLine);
					sb.Append("\t<div><a href=\"/" + template.FileName + "\">/" + template.FileName + "</a> - Get a custom text file for the sensor \"" + template.UniqueID + "\"</div>");
				}
				sb.Append(@"<p><a href=""/lastrequests"">/lastrequests</a> - Get the last 10 requests captured from the SmartHub.</p>");
				sb.Append(@"<p><a href=""/lastacuriteaccessrequests"">/lastacuriteaccessrequests</a> - Get the last 10 requests captured and proxied from configured AcuRite Access IP Addresses.</p>");
				sb.Append(@"
</body>
</html>");
				string str = sb.ToString();
				p.writeSuccess(contentLength: HttpProcessor.Utf8NoBOM.GetByteCount(str));
				p.outputStream.Write(str);
			}
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
			if (Program.settings.GetAcuriteAccessIPs().Contains(p.RemoteIPAddress))
			{
				ProxyDataBuffer proxiedDataBuffer = new ProxyDataBuffer();
				p.ProxyTo("https://atlasapi.myacurite.com" + p.request_url.PathAndQuery, 15000, true, proxiedDataBuffer);

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
							if (sensorData.UniqueID == template.UniqueID)
								sensorData.WriteFile(template);
						}
					}
				}
				return true;
			}
			return false;
		}

		ConcurrentQueue<string> lastRequests = new ConcurrentQueue<string>();
		public void ReceiveData(string str)
		{
			try
			{
				lastRequests.Enqueue(str);
				if (lastRequests.Count > 10)
					lastRequests.TryDequeue(out string removed);
				Match m = rxGetURL.Match(str);
				if (m.Success)
				{
					SensorBase sensorData = new SensorBase(m.Groups[2].Value);
					sensorDataCollection[sensorData.UniqueID] = sensorData;
					List<DataFileTemplate> templates = Program.settings.GetSensorDataTemplates();
					foreach (DataFileTemplate template in templates)
					{
						if (sensorData.UniqueID == template.UniqueID)
							sensorData.WriteFile(template);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex);
			}
		}
	}
}