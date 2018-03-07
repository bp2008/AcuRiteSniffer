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

			txtSmartHubAddress.Text = Program.settings.smartHubIp;

			cbInterface.Items.AddRange(Sniffer.GetNetworkAdapterNames());
			if (cbInterface.Items.Count > 0)
			{
				if (Program.settings.myNetworkInterfaceIndex >= cbInterface.Items.Count)
					Program.settings.myNetworkInterfaceIndex = 0;
				cbInterface.SelectedIndex = Program.settings.myNetworkInterfaceIndex;
			}

			cbEasyParse.Checked = Program.settings.easyParseMethod;
			txtServiceName.Text = Program.settings.serviceName;

			txtAcuriteAccessList.Text = Program.settings.accessIpsSemicolonSeparated;
			nudHttpsPort.Value = Program.settings.myHttpsPort;
		}

		private void nudPort_ValueChanged(object sender, EventArgs e)
		{
			Program.settings.myWebPort = (ushort)nudPort.Value;
			Program.settings.Save(Program.settingsPath);
		}

		private void txtSmartHubAddress_TextChanged(object sender, EventArgs e)
		{
			IPAddress address;
			if (IPAddress.TryParse(txtSmartHubAddress.Text, out address))
			{
				Program.settings.smartHubIp = address.ToString();
				Program.settings.Save(Program.settingsPath);
				txtSmartHubAddress.BackColor = nudPort.BackColor;
			}
			else
				txtSmartHubAddress.BackColor = Color.Pink;
		}

		private void cbInterface_SelectedIndexChanged(object sender, EventArgs e)
		{
			Program.settings.myNetworkInterfaceIndex = cbInterface.SelectedIndex;
			Program.settings.Save(Program.settingsPath);
		}

		private void btnTextFileDefinitions_Click(object sender, EventArgs e)
		{
			TextFileDefinitionEditor editor = new TextFileDefinitionEditor();
			editor.ShowDialog();
		}

		private void cbEasyParse_CheckedChanged(object sender, EventArgs e)
		{
			Program.settings.easyParseMethod = cbEasyParse.Checked;
			Program.settings.Save(Program.settingsPath);
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
	}
}
