using HafifPlatofrmArayuz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HafifPlatofrmArayuz.Services
{
	public class PacketCapture
	{
		public List<DataPacket> CapturedPackets { get; private set; } = new List<DataPacket>();

		public event Action<String> PacketStatusUpdated; // UI'ya mesaj göndermek için event

		public void CapturePacket(byte[] rawData)
		{
			try
			{
				DataPacket packet = DataPacket.FromByteArray(rawData);
				CapturedPackets.Add(packet);
				PacketStatusUpdated?.Invoke($"Doğru Paket Alındı, ID: {packet.PacketID}, Veri Uzunluğu: {packet.DataLenght}");
			}
			catch (Exception e)
			{
				PacketStatusUpdated?.Invoke($"Hatalı Paket Alındı, Hata: {e.Message}");
			}
		}
	}
}
