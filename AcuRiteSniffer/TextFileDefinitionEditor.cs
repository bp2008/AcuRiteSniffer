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
		}

		private void btnDefault_Click(object sender, EventArgs e)
		{
			txtFileDefinitions.Text = Program.settings.sensorDataFiles = new Settings().sensorDataFiles;
			Program.settings.Save(Program.settingsPath);
		}

		private void txtFileDefinitions_TextChanged(object sender, EventArgs e)
		{
			Program.settings.sensorDataFiles = txtFileDefinitions.Text;
			Program.settings.Save(Program.settingsPath);
		}
	}
}
