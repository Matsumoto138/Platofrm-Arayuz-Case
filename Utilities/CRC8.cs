using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HafifPlatofrmArayuz.Utilities
{
	public static class CRC8
	{
		private static readonly byte[] Crc8Table = new byte[256];

		static CRC8()
		{
			const byte polynomial = 0x07;
			for (int i = 0; i < 256; i++)
			{
				byte crc = (byte)i;
				for (int j = 0; j < 8; j++)
				{
					if ((crc & 0x80) != 0)
						crc = (byte)((crc << 1) ^ polynomial);
					else
						crc <<= 1;
				}
				Crc8Table[i] = crc;
			}
		}

		public static byte ComputeChecksum(byte[] data)
		{
			byte crc = 0x00;
			foreach (byte b in data)
			{
				crc = Crc8Table[crc ^ b];
			}
			return crc;
		}
	}
}
