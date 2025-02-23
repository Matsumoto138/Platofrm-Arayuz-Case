using HafifPlatofrmArayuz.Communication;
using HafifPlatofrmArayuz.Logging;
using HafifPlatofrmArayuz.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace HafifPlatofrmArayuz.Tests
{
	public class TestRunner
	{
		private readonly UdpCommunication udpCommunication;
		private readonly string testFilePath = "../../../Tests/test_script.json";

		public TestRunner(UdpCommunication udpCommunication)
		{
			this.udpCommunication = udpCommunication;
		}

		public async Task RunTest()
		{
			if (!File.Exists(testFilePath))
			{
				MessageBox.Show("Test scripti bulunamadı");
				return;
			}

			string jsonContent = await File.ReadAllTextAsync(testFilePath);
			List<TestCommand> testCommands = JsonSerializer.Deserialize<List<TestCommand>>(jsonContent);

			foreach (var command in testCommands)
			{
				switch (command.Action)
				{
					case "SendPacket":
						byte[] data = System.Text.Encoding.UTF8.GetBytes(command.Data);
						DataPacket packet = new DataPacket(1, data);
						udpCommunication.SendPacket("127.0.0.1", 5000, packet);
						Logger.Info($"🛠 Test: Paket Gönderildi -> {command.Data}");
						break;

					case "ShowLogs":
						string[] logs = Logger.ReadLog();
						string logText = logs.Length > 0 ? string.Join("\n", logs) : "Kayıtlı log bulunamadı!";
						MessageBox.Show(logText, "Log Kayıtları");
						Logger.Info($"🛠 Test: Loglar gösterildi.");
						break;
				}
				await Task.Delay(1000);
			}
			MessageBox.Show("Testler tamamlandı");
		}

		public class TestCommand
		{
			public string Action { get; set; }
			public string Data { get; set; }
		}
	}
}
