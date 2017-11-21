using System;
using System.Collections.Generic;
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
				string ServiceName = "AcuRiteSniffer";
				ButtonDefinition btnRegKey = new ButtonDefinition("Test Service (Start)", btnTestWebServer_Click);
				ButtonDefinition btnEditSettings = new ButtonDefinition("Edit Settings", btnEditSettings_Click);

				System.Windows.Forms.Application.Run(new ServiceManager(Title, ServiceName, new ButtonDefinition[] { btnRegKey, btnEditSettings }));

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
			if (svc == null)
			{
				svc = new MainSvc();
				svc.DoStart();
				((Button)sender).Text = "Test Service (Stop)";
			}
			else
			{
				svc.DoStop();
				svc = null;
				((Button)sender).Text = "Test Service (Start)";
			}
		}
		private static void btnEditSettings_Click(object sender, EventArgs e)
		{
			settings.Load(settingsPath);

			EditSettings editor = new EditSettings();
			editor.ShowDialog();
		}
	}
}
