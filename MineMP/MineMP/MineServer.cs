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
        }

        public bool UpdateClient(Socket clientConnect) {
            if (clientConnect.Poll(1100, SelectMode.SelectRead))
            {
                return false;
            }

            return true;
        }

        public void UpdateAllClient()
        {
            foreach (Socket client in Clients)
            {
                if (!UpdateClient(client))
                {
                    RemoveClient(client);
                    continue;
                }
            }
        }

        private void RemoveClient(Socket ClientConnect)
        {
            try
            {
                ClientConnect.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode != 10057) // SocketException(10057) Maybe Tcping Client
                {
                    Console.WriteLine("[Info]Failed to close client connection.");
                    Console.WriteLine("Client: {0};{1}", ((IPEndPoint)ClientConnect.RemoteEndPoint).Address, ((IPEndPoint)ClientConnect.RemoteEndPoint).Port);
                    Console.WriteLine("Error Message: {0}", ex.Message);
                }
            }
            ClientConnect.Close();

            Clients.Remove(ClientConnect);
        }

        private void AddClient(Socket ClientConnect)
        {
            Clients.Add(ClientConnect);
            Console.WriteLine("[Info]New Client Connected: {0}:{1}", ((IPEndPoint)ClientConnect.RemoteEndPoint).Address, ((IPEndPoint)ClientConnect.RemoteEndPoint).Port);

            new Thread(ClientTcpContent).Start(ClientConnect);
        }

        private void ClientTcpContent(object o_client)
        {
            Socket client = (Socket)o_client;

            for (; ; )
            {
                byte[] buffer = new byte[512];
                try
                {
                    client.Receive(buffer);
                }
                catch (SocketException ex) { 
                    if (ex.ErrorCode != 10054)
                    {
                        Console.WriteLine("[Info]Failed to close client connection.");
                        Console.WriteLine("Client: {0};{1}", ((IPEndPoint)client.RemoteEndPoint).Address, ((IPEndPoint)client.RemoteEndPoint).Port);
                        Console.WriteLine("Error Message: {0}", ex.Message);
                    }
                    Console.WriteLine("Invalid Socket Client Connection.");
                    RemoveClient(client);
                    Console.WriteLine("Removed.");
                    return;
                }
                if (!UpdateClient(client))
                {
                    Console.WriteLine("\r\nInvalid Socket Client Connection.");
                    RemoveClient(client);
                    Console.WriteLine("Removed.");
                    return;
                }

                if (buffer[0] == 0xFE)
                {
                    Console.WriteLine("[Info]Minecraft Client Ping");
                    Console.WriteLine("Client: {0};{1}", ((IPEndPoint)client.RemoteEndPoint).Address, ((IPEndPoint)client.RemoteEndPoint).Port);

                    client.Send(Encoding.Default.GetBytes("abc6123"));
                }

                //Console.WriteLine("[DEBUG]message from client:\r\n{0}", BitConverter.ToString(buffer));

            }
        }

        private void ListenClientConnect()
        {
            for (; !StopServer;)
            {
                AddClient(Socket.Accept());
            }

            // Stop
            UpdateAllClient();
            foreach (Socket Client in Clients)
            {
                RemoveClient(Client);
            }

            Clients.Clear();
            Socket.Close();
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
