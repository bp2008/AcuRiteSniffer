using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BPUtil;
using BPUtil.Forms;

namespace AcuRiteSniffer
{
	static class Program
	{
		private static MainSvc svc = null;
		public static Settings settings;
		public const string settingsPath = "AcuRiteSnifferSettings.cfg";
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			Globals.Initialize(exePath);

			Directory.SetCurrentDirectory(Globals.ApplicationDirectoryBase);

			settings = new Settings();
			settings.Load(settingsPath);
			settings.SaveIfNoExist(settingsPath);

			if (Environment.UserInteractive)
			{
				string Title = "AcuRiteSniffer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Service Manager";
				string ServiceName = settings.serviceName;
				ButtonDefinition btnTestWebServer = new ButtonDefinition("Test Service (Start)", btnTestWebServer_Click);
				ButtonDefinition btnEditSettings = new ButtonDefinition("Edit Settings", btnEditSettings_Click);
				ButtonDefinition btnOpenWebApp = new ButtonDefinition("Open Web App", btnOpenWebApp_Click);

				if (System.Diagnostics.Debugger.IsAttached)
					btnTestWebServer_Click(btnTestWebServer, null);

				System.Windows.Forms.Application.Run(
					new ServiceManager(Title, ServiceName, new ButtonDefinition[] { btnTestWebServer, btnEditSettings, btnOpenWebApp })
					{
						StartPosition = FormStartPosition.CenterScreen
					}
				);

				if (svc != null)
					svc.DoStop();
			}
			else
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new MainSvc()
				};
				ServiceBase.Run(ServicesToRun);
			}
		}
		private static void btnTestWebServer_Click(object sender, EventArgs e)
		{
			string newLabel;
			if (svc == null)
			{
				svc = new MainSvc();
				svc.DoStart();
				newLabel = "Test Service (Stop)";
			}
			else
			{
				svc.DoStop();
				svc = null;
				newLabel = "Test Service (Start)";
			}
			if (sender is ButtonDefinition)
				((ButtonDefinition)sender).Text = newLabel;
			else
				((Button)sender).Text = newLabel;
		}
		private static void btnEditSettings_Click(object sender, EventArgs e)
		{
			settings.Load(settingsPath);

			EditSettings editor = new EditSettings();
			editor.ShowDialog();
		}
		private static void btnOpenWebApp_Click(object sender, EventArgs e)
		{
			settings.Load(settingsPath);

			if (settings.myWebPort > 0)
				Process.Start("http://localhost:" + settings.myWebPort + "/");
			else if (settings.myHttpsPort > 0)
				Process.Start("https://localhost:" + settings.myHttpsPort + "/");
			else
				MessageBox.Show("Embedded web server is not configured.");
		}
	}
}
