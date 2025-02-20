using HafifPlatofrmArayuz.Communication;
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

        public MainWindow()
        {
            InitializeComponent();
            udpService = new UdpCommunication(5000);
            udpService.DataReceived += OnDataReceived;
            udpService.StartListening();
        }

        private void OnDataReceived(string data)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Veri: {data}");
            });
        }

        private void SendBtn(object sender, RoutedEventArgs e)
        {
            udpService.SendData("127.0.0.1", 5000, "Hi, UDP!");
        }

        private void CloseWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            udpService.StopListening();
        }
    }
}