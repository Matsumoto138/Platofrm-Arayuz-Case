﻿using HafifPlatofrmArayuz.Communication;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UdpCommunication udpService;
        private PacketCapture packetCapture;
		private TestRunner testRunner;

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
            byte[] data = Encoding.UTF8.GetBytes("Test Message");
            DataPacket packet = new DataPacket(1, data);
            udpService.SendPacket("127.0.0.1", 5000, packet);
        }

        private void ResetCountersBtn(object sender, RoutedEventArgs e)
        {
            packetCapture.ResetCounters();
            MessageBox.Show("Sayaçlar sıfırlandı!");
        }

        private void ShowLogsBtn(object sender, RoutedEventArgs e)
        {
            string[] logs = Logger.ReadLog();
            string logText = logs.Length > 0 ? string.Join("\n", logs) : "Kayıtlı Log Bulunmuyor";
            MessageBox.Show(logText, "Log Kayıtları");
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

		private void CloseWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            udpService.StopListening();
        }
    }
}