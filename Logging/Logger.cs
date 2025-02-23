using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HafifPlatofrmArayuz.Logging
{
	public static class Logger
	{
		private static readonly string logDirectory = "logs";
		private static readonly string logFilePath = Path.Combine(logDirectory, "comunication_log.txt");

		static Logger()
		{
			if (!Directory.Exists(logDirectory))
			{
				Directory.CreateDirectory(logDirectory);
			}
		}

		public static void Info(string message)
		{
			string logEntrty = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} - {message}";
			Console.WriteLine(logEntrty);

			try
			{
				File.AppendAllText(logFilePath, logEntrty + Environment.NewLine, Encoding.UTF8);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Log Saving System Error: {e.Message}");
			}
		}

		public static string[] ReadLog()
		{
			if (File.Exists(logFilePath))
			{
				return File.ReadAllLines(logFilePath, Encoding.UTF8);
			}
			return new string[0];
		}
	}
}
