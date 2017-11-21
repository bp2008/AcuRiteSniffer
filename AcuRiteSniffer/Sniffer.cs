using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BPUtil;
using PcapDotNet.Core;
using PcapDotNet.Core.Extensions;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace AcuRiteSniffer
{
	/// <summary>
	/// Sniffs packets sent by an AcuRite smarthub that have been mirrored to this PC.
	/// 
	/// One way to configure a router to mirror the necessary packets is:
	///		iptables -A PREROUTING -t mangle -s ipSmartHub -j ROUTE --gw ipThisPC --tee
	///		
	/// List such rules with:
	///		iptables -t mangle -L
	///		
	/// Delete the rules with:
	///		iptables -D PREROUTING -t mangle -s ipSmartHub -j ROUTE --gw ipThisPC --tee
	/// </summary>
	public class Sniffer
	{
		private IPAddress addressSenderDevice;
		private int networkAdapterIndex = 1;

		private Thread thrDataStream;

		public event EventHandler<string> onRequestReceived = delegate { };

		public Sniffer(string ipAddressOfSenderDevice, int networkAdapterIndex)
		{
			this.addressSenderDevice = IPAddress.Parse(ipAddressOfSenderDevice);
			this.networkAdapterIndex = networkAdapterIndex;
		}

		public void Start()
		{
			Stop();

			thrDataStream = new Thread(doDataStream);
			thrDataStream.Name = "Packet Sniffing Thread";
			thrDataStream.Start();
		}

		public void Stop()
		{
			try
			{
				if (thrDataStream != null)
					thrDataStream.Abort();

				thrDataStream = null;
			}
			catch (Exception) { }
		}

		/// <summary>
		/// Processes incoming packets.  This method should be run on a new thread.
		/// </summary>
		private void doDataStream()
		{
			try
			{
				PacketDevice selectedDevice = LivePacketDevice.AllLocalMachine[networkAdapterIndex];

				// Open the device
				using (PacketCommunicator communicator =
					selectedDevice.Open(65536,  // portion of the packet to capture
												// 65536 guarantees that the whole packet will be captured on all the link layers
										PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
										1000))                                  // read timeout
				{
					// Check the link layer. We support only Ethernet for simplicity.
					if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
					{
						Console.WriteLine("This program works only on Ethernet networks.");
						return;
					}

					// Compile and set the filter
					communicator.SetFilter("ip and tcp and ip src " + addressSenderDevice.ToString()); //  

					// start the capture
					var query = from packet in communicator.ReceivePackets()
								where packet.Ethernet.EtherType == EthernetType.IpV4
									&& packet.Ethernet.IpV4.Protocol == IpV4Protocol.Tcp
									&& packet.Ethernet.IpV4.Tcp.DestinationPort == 80
								select packet;

					StringBuilder sb = new StringBuilder();
					foreach (Packet packet in query)
					{
						// Add lengths of all headers together: ethernet, ipv4, and udp
						TcpDatagram tcp = packet.Ethernet.IpV4.Tcp;
						Datagram datagram = tcp.Payload;
						if (tcp.SequenceNumber == tcp.NextSequenceNumber)
						{
							if (sb.Length > 0)
							{
								onRequestReceived(this, sb.ToString());
								sb.Clear();
							}
						}
						else
							sb.Append(Encoding.ASCII.GetString(datagram.ToArray()));
					}
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public static string[] GetNetworkAdapterNames()
		{
			List<string> names = new List<string>(LivePacketDevice.AllLocalMachine.Count);
			foreach (LivePacketDevice dev in LivePacketDevice.AllLocalMachine)
			{
				if (!string.IsNullOrWhiteSpace(dev.Description))
					names.Add(dev.Description);
				else
					names.Add(dev.Name);
			}
			return names.ToArray();
		}

		#region Util
		public static byte[] HexStringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}
		#endregion

	}
}