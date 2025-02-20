using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HafifPlatofrmArayuz.Communication
{
	public class TcpCommunication
	{
		private TcpListener tcpListener;
		private bool isListening;
		private int port;

		public event Action<string> DataReceived;

		public TcpCommunication(int port)
		{
			this.port = port;
		}

		public void StartServer()
		{
			tcpListener = new TcpListener(IPAddress.Any, port);
			tcpListener.Start();
			isListening = true;
			Task.Run(async () =>
			{
				while (isListening)
				{
					try
					{
						TcpClient client = await tcpListener.AcceptTcpClientAsync();
						NetworkStream stream = client.GetStream();
						byte[] buffer = new byte[1024];
						int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
						string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
						DataReceived?.Invoke(receivedData);
					}
					catch (Exception e)
					{
						Console.WriteLine($"TCP Server Error: {e.Message}");
					}
				}
			});
		}

		public void StopServer()
		{
			isListening = false;
			tcpListener?.Stop();
		}

		public void SendData(string ip, int port, string data)
		{
			Task.Run(async () =>
			{
				try
				{
					using (TcpClient client = new TcpClient())
					{
						await client.ConnectAsync(IPAddress.Parse(ip), port);
						byte[] bytes = Encoding.UTF8.GetBytes(data);
						NetworkStream stream = client.GetStream();
						await stream.WriteAsync(bytes, 0, bytes.Length);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"TCP Sending Error: {e.Message}");
				}
			});
		}
	}
}
