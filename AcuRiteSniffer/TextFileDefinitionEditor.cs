using BPUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcuRiteSniffer
{
	public partial class TextFileDefinitionEditor : Form
	{
		public TextFileDefinitionEditor()
		{
			InitializeComponent();
			txtFileDefinitions.Text = Program.settings.sensorDataFiles.Replace("\r","").Replace("\n",Environment.NewLine);
			txtOut.Text = EvaluateTemplates();
		}

		private void btnDefault_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("This will cause your custom template strings to be lost. Are you sure?", "Confirm RESET", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				txtFileDefinitions.Text = Program.settings.sensorDataFiles = new Settings().sensorDataFiles;
				Program.settings.Save(Program.settingsPath);
			}
		}

		private void txtFileDefinitions_TextChanged(object sender, EventArgs e)
		{
			Program.settings.sensorDataFiles = txtFileDefinitions.Text;
			Program.settings.Save(Program.settingsPath);

			txtOut.Text = EvaluateTemplates();
		}

		private string EvaluateTemplates()
		{
			try
			{
				return WebServer.mqttReader.ApplyTemplatesForGUI();
			}
			catch (Exception ex)
			{
				return ex.ToHierarchicalString();
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			txtOut.Text = EvaluateTemplates();
		}
	}
}
