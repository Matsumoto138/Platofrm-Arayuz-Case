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
		public int CorrectPacketCount { get; private set; } = 0; // Doğru paket sayacı
		public int ErrorPacketCount { get; private set; } = 0; // Hatalı paket sayacı
		public List<DataPacket> CapturedPackets { get; private set; } = new List<DataPacket>();

		public event Action<String> PacketStatusUpdated; // UI'ya mesaj göndermek için event

		public void CapturePacket(byte[] rawData)
		{
			try
			{
				DataPacket packet = DataPacket.FromByteArray(rawData);
				CorrectPacketCount++;
				CapturedPackets.Add(packet);
				PacketStatusUpdated?.Invoke($"Doğru Paket Alındı, ID: {packet.PacketID}, Veri Uzunluğu: {packet.DataLenght}");
			}
			catch (Exception e)
			{
				ErrorPacketCount++;
				PacketStatusUpdated?.Invoke($"Hatalı Paket Alındı, Hata: {e.Message}");
			}
		}

		public void ResetCounters()
		{
			CorrectPacketCount = 0;
			CapturedPackets.Clear();
			ErrorPacketCount = 0;
		}
	}
}
