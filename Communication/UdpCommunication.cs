using HafifPlatofrmArayuz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HafifPlatofrmArayuz.Communication
{
	public class UdpCommunication
	{
		private UdpClient udpClient;
		private int port;
		private bool isListening = false;

		public event Action<DataPacket> PacketReceived;

		public UdpCommunication(int port)
		{
			this.port = port;
			udpClient = new UdpClient(port);
		}
		public void StartListening()
		{
			isListening = true;
			Task.Run(async () =>
			{
				while (isListening)
				{
					try
					{
						UdpReceiveResult receiveResult = await udpClient.ReceiveAsync();
						DataPacket packet = DataPacket.FromByteArray(receiveResult.Buffer);
						PacketReceived?.Invoke(packet);
					}
					catch (Exception e)
					{
						Console.WriteLine($"UDP Listening Error: {e.Message}");
					}
				}
			});
		}
		public void StopListening()
		{
			isListening = false;
			udpClient?.Close();
		}

		public void SendPacket(string ip, int port, DataPacket packet)
		{
			try
			{
				byte[] bytes = packet.ToByteArray();
				udpClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse(ip), port));
			}
			catch (Exception e)
			{
				Console.WriteLine($"UDP Sending Error: {e.Message}");
			}
		}
	}
}
