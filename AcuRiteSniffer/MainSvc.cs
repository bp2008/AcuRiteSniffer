using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AcuRiteSniffer
{
	public partial class MainSvc : ServiceBase
	{
		WebServer svr = null;

		public MainSvc()
		{
			InitializeComponent();
		}

		public void DoStart()
		{
			OnStart(null);
		}

		public void DoStop()
		{
			OnStop();
		}

		protected override void OnStart(string[] args)
		{
			OnStop();

			Program.settings.Load(Program.settingsPath);

			svr = new WebServer();
			svr.EnableLogging(false);
			svr.SetBindings(Program.settings.myWebPort, Program.settings.myHttpsPort);
		}

		protected override void OnStop()
		{
			if (svr != null)
			{
				svr.Stop();
				svr = null;
			}
		}
	}
}
