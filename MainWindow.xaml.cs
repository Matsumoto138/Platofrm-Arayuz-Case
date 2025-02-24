using HafifPlatofrmArayuz.Communication;
using HafifPlatofrmArayuz.Logging;
using HafifPlatofrmArayuz.Models;
using HafifPlatofrmArayuz.Services;
using HafifPlatofrmArayuz.Tests;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HafifPlatofrmArayuz
{
	// Bu sayfada UI ile bağlantılı şekilde ana işlemler gerçekleştirilir
    public partial class MainWindow : Window
    {
        private UdpCommunication udpService;
        private PacketCapture packetCapture;
		private TestRunner testRunner;

		private int successCount = 0;
		private int errorCount = 0;

		public MainWindow()
        {
            InitializeComponent();
            udpService = new UdpCommunication(5000);
            packetCapture = new PacketCapture();

            udpService.PacketReceived += OnPacketReceived;
            packetCapture.PacketStatusUpdated += OnPacketStatusUpdated;

			testRunner = new TestRunner(udpService);

			udpService.StartListening();
        }

        private void OnPacketReceived(DataPacket packet)
        {
            Dispatcher.Invoke(() =>
            {
                string receivedText = Encoding.UTF8.GetString(packet.Data);
                MessageBox.Show($"Paket Alındı ID: {packet.PacketID}, Veri: {receivedText}");
            });

        }
        private void OnPacketStatusUpdated(string message)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message);
            });
        }

        private void SendBtn(object sender, RoutedEventArgs e)
        {
            string packetData = "Test Message";
			byte[] data = Encoding.UTF8.GetBytes(packetData);
            DataPacket packet = new DataPacket(1, data);

			try
			{
				udpService.SendPacket("127.0.0.1", 5000, packet);
				UpdateSentPacketUI(packetData);
				UpdateSuccessCounter();
			}
			catch (Exception)
			{
				UpdateErrorCounter();
			}

            
        }
		private void UpdateSuccessCounter()
		{
			successCount++;
			Dispatcher.Invoke(() =>
			{
				SuccessCountText.Text = successCount.ToString();
			});
		}

		private void UpdateErrorCounter()
		{
			errorCount++;
			Dispatcher.Invoke(() =>
			{
				ErrorCountText.Text = errorCount.ToString();
			});
		}

		private void ResetCountersBtn(object sender, RoutedEventArgs e)
        {
			successCount = 0;
			errorCount = 0;
			Dispatcher.Invoke(() =>
			{
				SuccessCountText.Text = "0";
				ErrorCountText.Text = "0";
			});
			MessageBox.Show("Sayaçlar sıfırlandı!");
        }

        private void ShowLogsBtn(object sender, RoutedEventArgs e)
        {
            LogListBox.Items.Clear(); // Eski logları temizle
            string[] logs = Logger.ReadLog();
            int maxLogs = 100; // UI içinde göstereceğimiz maksimum log sayısı

            foreach (var log in logs.Skip(Math.Max(0, logs.Length - maxLogs)))
            {
                UpdateLogUI(log);

            }
        }
		private void ConvertToMatlabBtn(object sender, RoutedEventArgs e)
		{
            LogConverter.ConvertToMatlab();
            MessageBox.Show("Loglar MATLAB formatına dönüştürüldü");
		}

		private async void RunTestsBtn(object sender, RoutedEventArgs e)
		{
			await testRunner.RunTest();
		}

        private void GenerateTestReport(object sender, RoutedEventArgs e)
        {
            string[] logs = Logger.ReadLog();
            TestReportGenerator.GenerateReport(logs);
            MessageBox.Show("Test Raporu PDF Olarak Kaydedildi");
        }

		private void UpdateSentPacketUI(string packetContent)
		{
			Dispatcher.Invoke(() =>
			{
				SentPacketContent.Text = packetContent;
			});
		}

		private void UpdateLogUI(string logMessage)
		{
			Dispatcher.Invoke(() =>
			{
				LogListBox.Items.Add(logMessage);
				LogListBox.ScrollIntoView(LogListBox.Items[LogListBox.Items.Count - 1]);
			});
		}

		private void CloseWindow(object sender, System.ComponentModel.CancelEventArgs e)
		{
			udpService.StopListening();
		}
	}
}