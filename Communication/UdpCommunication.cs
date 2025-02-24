using HafifPlatofrmArayuz.Logging;
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
	// UDP Paket gönderimi işlemleri bu dosya üzerinden gerçekleşiyor.
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
						Logger.Info($"UDP Alındı -> PaketId: {packet.PacketID}, Veri Uzunluğu: {packet.DataLenght}, ");
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
				Logger.Info($"UDP Gönderildi -> IP: {ip}, Port: {port}, PaketID: {packet.PacketID}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"UDP Sending Error: {e.Message}");
			}
		}
	}
}
