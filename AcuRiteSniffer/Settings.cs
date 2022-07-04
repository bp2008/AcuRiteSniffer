using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BPUtil;
using Newtonsoft.Json;

namespace AcuRiteSniffer
{
	public class Settings : SerializableObjectBase
	{
		public string accessIpsSemicolonSeparated = "192.168.0.171";
		/// <summary>
		/// If -1, we won't listen with http.
		/// </summary>
		public ushort myWebPort = 45411;
		/// <summary>
		/// If -1, we won't listen with https.
		/// </summary>
		public int myHttpsPort = 443;
		public string serviceName = "AcuRiteSniffer";
		public string sensorDataFiles = "[24C86E000000_12345678]Temperature: ##tempf##°\\nHumidity: ##humidity##\\nAt ##date## ##time##"
			+ Environment.NewLine + "[24C86E000000_00001234=myWindSpeed.txt]##windspeedmph## MPH"
			+ Environment.NewLine + "[24C86E000000_00001234=myWindDirection.txt]##winddir##";

		public string mqttHost = "";
		public int mqttTcpPort = 1883;
		public string mqttUser = "mqttuser";
		public string mqttPass = "";

		public string friendlyNamesJson = "";

		private static Regex rxReadSensorDataFileLine = new Regex("\\[(.*?)\\](.*)", RegexOptions.Compiled);
		private List<DataFileTemplate> templates;
		private ConcurrentDictionary<string, string> friendlyNamesDict = new ConcurrentDictionary<string, string>();
		public override bool Load(string filePath = null)
		{
			bool result = base.Load(filePath);
			try
			{
				friendlyNamesDict = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(friendlyNamesJson);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			if (friendlyNamesDict == null)
				friendlyNamesDict = new ConcurrentDictionary<string, string>();
			return result;
		}
		public override bool Save(string filePath = null)
		{
			friendlyNamesJson = JsonConvert.SerializeObject(friendlyNamesDict);
			return base.Save(filePath);
		}

		public List<DataFileTemplate> GetSensorDataTemplates()
		{
			if (templates == null)
			{
				templates = new List<DataFileTemplate>();
				if (sensorDataFiles != null)
					foreach (string line in sensorDataFiles.Split(new char[] { '\r', '\n' }))
					{
						Match m = rxReadSensorDataFileLine.Match(line);
						if (m.Success)
							templates.Add(new DataFileTemplate(m.Groups[1].Value, m.Groups[2].Value));
					}
			}
			return templates;
		}

		private HashSet<string> _accessIps;

		public HashSet<string> GetAcuriteAccessIPs()
		{
			if (_accessIps == null)
			{
				_accessIps = new HashSet<string>();
				if (accessIpsSemicolonSeparated != null)
					foreach (string addressStr in accessIpsSemicolonSeparated.Split(new char[] { ';' }))
					{
						if (IPAddress.TryParse(addressStr, out IPAddress address))
							_accessIps.Add(address.ToString());
					}
			}
			return _accessIps;
		}
		/// <summary>
		/// Tries to get the friendly name for the given device key, returning true if successful.
		/// </summary>
		/// <param name="deviceKey">Device unique ID string</param>
		/// <param name="deviceFriendlyName">Friendly name</param>
		/// <returns></returns>
		public bool TryGetFriendlyDeviceName(string deviceKey, out string deviceFriendlyName)
		{
			if (friendlyNamesDict != null && friendlyNamesDict.TryGetValue(deviceKey, out deviceFriendlyName))
				return true;
			deviceFriendlyName = "";
			return false;

		}
		/// <summary>
		/// Gets an array of key/value pairs ordered by key, mapping the key to the friendly device name.
		/// </summary>
		/// <returns></returns>
		public KeyValuePair<string, string>[] GetFriendlyDeviceNames()
		{
			return friendlyNamesDict.OrderBy(kvp => kvp.Key).ToArray();
		}
		/// <summary>
		/// Tries to set the friendly device name for a given key, returning true if successful. Returns false if there was an input validation error or if setting the value would have caused a duplicate value to exist (the duplication check is not thread-safe).
		/// </summary>
		/// <returns></returns>
		public bool TrySetFriendlyDeviceName(string deviceKey, string deviceFriendlyName, out string errorMessage)
		{
			if (deviceKey == deviceFriendlyName)
			{
				errorMessage = "It is not allowed to set the friendly name to be identical to the device key.  Perhaps you would like to set an empty friendly name (that is allowed).";
				return false;
			}
			if (string.IsNullOrWhiteSpace(deviceKey))
			{
				errorMessage = "The given device key is invalid.";
				return false;
			}
			if (friendlyNamesDict.ContainsKey(deviceFriendlyName))
			{
				errorMessage = "The given friendly name conflicts with an existing device key.";
				return false;
			}
			if (deviceFriendlyName == "")
			{
				friendlyNamesDict.TryRemove(deviceKey, out string ignored);
				Save(Program.settingsPath);
			}
			else
			{
				deviceFriendlyName = deviceFriendlyName.Trim();
				if (!StringUtil.IsPrintableName(deviceFriendlyName) || deviceFriendlyName.Contains(','))
				{
					errorMessage = "The given friendly name does not achieve minimum readability. It must contain at least one alphanumeric character and consist only of ASCII printable characters or basic whitespace. It is also allowed to set an empty friendly name. Commas are not allowed in friendly names.";
					return false;
				}
				if (friendlyNamesDict.Values.Any(existing => existing.Equals(deviceFriendlyName, StringComparison.OrdinalIgnoreCase)))
				{
					errorMessage = "The given friendly name already exists. Refusing to add.";
					return false;
				}
				friendlyNamesDict[deviceKey] = deviceFriendlyName;
				Save(Program.settingsPath);
			}
			errorMessage = "";
			return true;
		}
	}
	public class DataFileTemplate
	{
		public string UniqueID;
		public string FileName;
		public string TemplateStr;

		public DataFileTemplate(string identifier, string templateStr)
		{
			int idxEqualSign = identifier.IndexOf('=');
			if (idxEqualSign == -1)
			{
				UniqueID = identifier;
				FileName = UniqueID + ".txt";
			}
			else
			{
				UniqueID = identifier.Remove(idxEqualSign);
				FileName = identifier.Substring(idxEqualSign + 1);
			}
			TemplateStr = templateStr;
		}
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
