using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MineMP
{
    internal class MineServer
    {
        private bool StopServer = true;

        public Socket Socket { get; private set; }
        public List<Socket> Clients { get; private set; } = new List<Socket>();
        public bool IsListening { get; private set; } = false;

        public bool Init(IPAddress address, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(address, port));

            return true;
        }
        public bool Init()
        {
            return Init(IPAddress.Any, 25565);
        }
        public void Start()
        {
            if (IsListening)
                return;

            StopServer = false;
            IsListening = true;

            Socket.Listen(3);
            new Thread(ListenClientConnect).Start();
        }
        public void Stop()
        {
            StopServer = true;
            IsListening = false;

            UpdateAllClient();
            foreach (Socket Client in Clients)
            {
                try
                {
                    Client.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != 10057) // SocketException(10057) Maybe Tcping Client
                    {
                        Console.WriteLine("[Info]Failed to close client connection.");
                        Console.WriteLine("Client: {0};{1}", ((IPEndPoint)Client.RemoteEndPoint).Address, ((IPEndPoint)Client.RemoteEndPoint).Port);
                        Console.WriteLine("Error Message: {0}", ex.Message);
                    }
                }

                Client.Close();
            }

            Clients.Clear();
            Socket.Close();
        }

        public void UpdateAllClient()
        {
            byte[] bs = Encoding.ASCII.GetBytes("test");
            foreach (Socket client in Clients)
            {
                if (client.Poll(1000, SelectMode.SelectRead))
                {
                    client.Close();
                    Clients.Remove(client);
                    continue;
                }
            }
        }

        private void RemoveClient(Socket ClientConnect)
        {
            Clients.Remove(ClientConnect);
        }

        private void AddClient(Socket ClientConnect)
        {
            Clients.Add(ClientConnect);
            Console.WriteLine("[Info]New Client Connected: {0}:{1}", ((IPEndPoint)ClientConnect.RemoteEndPoint).Address, ((IPEndPoint)ClientConnect.RemoteEndPoint).Port);
        }

        private void ListenClientConnect()
        {
            for (; !StopServer;)
            {
                AddClient(Socket.Accept());
            }
        }

        public void Broadcast(byte[] bytes)
        {
            foreach (Socket Client in Clients)
            {
                Client.Send(bytes);
            }
        }
    }
}
