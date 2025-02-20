using HafifPlatofrmArayuz.Utilities;
using System;
using System.Linq;
using System.Text;

namespace HafifPlatofrmArayuz.Models
{
	public struct DataPacket
	{
		public byte Sync1;
		public byte Sync2;
		public byte PacketID;
		public byte DataLenght;
		public byte[] Data;
		public byte CRC;

		public DataPacket(byte packetID, byte[] data)
		{
			Sync1 = 0xAA;
			Sync2 = 0x55;
			PacketID = packetID;
			DataLenght = (byte)data.Length;
			Data = data;
			  
		}

		private byte CalculateCRC()
		{
			byte[] allBytes = new byte[] { Sync1, Sync2, PacketID, DataLenght }.Concat(Data).ToArray();
			return CRC8.ComputeChecksum(allBytes);
		}
		public byte[] ToByteArray()
		{
			byte[] packet = new byte[5 + Data.Length];
			packet[0] = Sync1;
			packet[1] = Sync2;
			packet[2] = PacketID;
			packet[3] = DataLenght;
			Array.Copy(Data, 0, packet, 4, Data.Length);
			packet[packet.Length - 1] = CRC;
			return packet;
		}

		public static DataPacket FromByteArray(byte[] bytes)
		{
			if (bytes.Length < 5) throw new ArgumentNullException("Geçersiz Paket Boyu");

			byte sync1 = bytes[0];
			byte sync2 = bytes[1];
			byte packetID = bytes[2];
			byte dataLenght = bytes[3];
			byte[] data = bytes.Skip(4).Take(dataLenght).ToArray();
			byte crc = bytes[bytes.Length - 1];

			DataPacket packet = new DataPacket(packetID, data);
			if (packet.CRC != crc) throw new Exception("CRC Hatası: Bozuk paket!");

			return packet;
		}
	}
}
