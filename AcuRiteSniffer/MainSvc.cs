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
		Sniffer sniffer = null;

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

			svr = new WebServer(Program.settings.myWebPort, Program.settings.myHttpsPort);
			svr.Start();
			sniffer = new Sniffer(Program.settings.smartHubIp, Program.settings.myNetworkInterfaceIndex);
			sniffer.onRequestReceived += Sniffer_onRequestReceived;
			sniffer.Start();
		}

		private void Sniffer_onRequestReceived(object sender, string str)
		{
			if (svr != null)
				svr.ReceiveData(str);
		}

		protected override void OnStop()
		{
			if (svr != null)
			{
				svr.Stop();
				svr = null;
				sniffer.Stop();
				sniffer = null;
			}
		}
	}
}
