using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HafifPlatofrmArayuz.Logging
{
	public static class LogConverter
	{
		private static readonly string matlabFolder = "MATLAB_Logs";
		private static readonly string matlabFilePath = Path.Combine(matlabFolder, "log_data.mat");

		static LogConverter()
		{
			if (!Directory.Exists(matlabFolder))
			{
				Directory.CreateDirectory(matlabFolder);
			}
		}

		public static void ConvertToMatlab()
		{
			string[] logs = Logger.ReadLog();
			if (logs.Length == 0)
			{
				Console.WriteLine("Log bulunamadı.");
				return;
			}

			try
			{
				File.WriteAllLines(matlabFilePath, logs);
				Console.WriteLine($"Loglar MATLAB formatına döüştürüldü: {matlabFilePath}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Dönüştürme işlemi başarısız: {e.Message}");
			}
		}
	}
}
