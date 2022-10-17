using BPUtil;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcuRiteSniffer
{
	public partial class MQTT_Test : Form
	{
		MqttReader reader;
		public MQTT_Test()
		{
			InitializeComponent();
		}

		private async void MQTT_Test_Load(object sender, EventArgs e1)
		{
			WriteLine("Connecting to " + Program.settings.mqttHost);
			reader = new MqttReader(Program.settings.mqttHost, Program.settings.mqttTcpPort, Program.settings.mqttUser, Program.settings.mqttPass);
			reader.OnError += Reader_OnError;
			reader.OnStatusUpdate += Reader_OnStatusUpdate;
			reader.OnDeviceUpdate += Reader_OnDeviceUpdate;
			await reader.Start();
		}

		private void Reader_OnDeviceUpdate(object sender, dynamic e)
		{
			WriteLine("Updated device: " + JsonConvert.SerializeObject(e));
		}

		private void Reader_OnError(object sender, string e)
		{
			WriteLine("ERROR from " + (sender is MqttDevice ? (sender as MqttDevice).GetNameOrKey() : sender?.ToString()) + ": " + e);
		}

		private void Reader_OnStatusUpdate(object sender, string e)
		{
			WriteLine("STATUS: " + e);
		}

		private void WriteLine(string msg)
		{
			if (txtOut.InvokeRequired)
				txtOut.BeginInvoke((Action<string>)WriteLine, new object[] { msg });
			else
				txtOut.AppendText(msg + Environment.NewLine);
		}

		private void MQTT_Test_FormClosing(object sender, FormClosingEventArgs e)
		{
			reader?.Dispose();
		}
	}
}
