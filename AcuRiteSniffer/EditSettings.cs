using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcuRiteSniffer
{
	public partial class EditSettings : Form
	{
		public EditSettings()
		{
			InitializeComponent();
		}

		private void EditSettings_Load(object sender, EventArgs e)
		{
			nudPort.Value = Program.settings.myWebPort;

			txtServiceName.Text = Program.settings.serviceName;

			txtAcuriteAccessList.Text = Program.settings.accessIpsSemicolonSeparated;
			nudHttpsPort.Value = Program.settings.myHttpsPort;

			txtMqttHost.Text = Program.settings.mqttHost;
			nudMqttPort.Value = Program.settings.mqttTcpPort;
			txtMqttUser.Text = Program.settings.mqttUser;
			txtMqttPass.Text = Program.settings.mqttPass;
		}

		private void nudPort_ValueChanged(object sender, EventArgs e)
		{
			Program.settings.myWebPort = (ushort)nudPort.Value;
			Program.settings.Save(Program.settingsPath);
		}

		private void btnTextFileDefinitions_Click(object sender, EventArgs e)
		{
			TextFileDefinitionEditor editor = new TextFileDefinitionEditor();
			editor.ShowDialog();
		}

		private void txtServiceName_TextChanged(object sender, EventArgs e)
		{
			Program.settings.serviceName = txtServiceName.Text;
			Program.settings.Save(Program.settingsPath);
		}

		private void txtAcuriteAccessList_TextChanged(object sender, EventArgs e)
		{
			Program.settings.accessIpsSemicolonSeparated = txtAcuriteAccessList.Text;
			Program.settings.Save(Program.settingsPath);
		}

		private void nudHttpsPort_ValueChanged(object sender, EventArgs e)
		{
			Program.settings.myHttpsPort = (int)nudHttpsPort.Value;
			Program.settings.Save(Program.settingsPath);
		}

		private void btnMqttTest_Click(object sender, EventArgs e)
		{
			MQTT_Test mqttTestForm = new MQTT_Test();
			mqttTestForm.ShowDialog();
		}

		private void txtMqttHost_TextChanged(object sender, EventArgs e)
		{
			Program.settings.mqttHost = txtMqttHost.Text;
			Program.settings.Save(Program.settingsPath);
		}

		private void nudMqttPort_ValueChanged(object sender, EventArgs e)
		{
			Program.settings.mqttTcpPort = (int)nudMqttPort.Value;
			Program.settings.Save(Program.settingsPath);
		}

		private void txtMqttUser_TextChanged(object sender, EventArgs e)
		{
			Program.settings.mqttUser = txtMqttUser.Text;
			Program.settings.Save(Program.settingsPath);
		}

		private void txtMqttPassword_TextChanged(object sender, EventArgs e)
		{
			Program.settings.mqttPass = txtMqttPass.Text;
			Program.settings.Save(Program.settingsPath);
		}
	}
}
