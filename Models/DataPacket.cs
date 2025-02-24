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
			byte[] allBytes = new byte[] { this.Sync1, this.Sync2, this.PacketID, this.DataLenght }
				.Concat(this.Data)
				.ToArray();

			byte crcValue = CRC8.ComputeChecksum(allBytes);

			Console.WriteLine($"CRC Hesaplandı! Veri: {BitConverter.ToString(allBytes)} | CRC: {crcValue}");
			return crcValue;
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
			byte calculatedCRC = packet.CalculateCRC();
			if (calculatedCRC != crc)
			{
				Console.WriteLine($"CRC Hatası! Beklenen: {crc}, Hesaplanan: {calculatedCRC}");
				return packet;
			}

			Console.WriteLine($"CRC Doğrulandı! Paket geçerli.");

			return packet;
		}
	}
}
