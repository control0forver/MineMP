using System.Net;
using System.Net.Sockets;

namespace MineMP
{
    internal class MineServer
    {
        private bool Stopped = true;

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

            Stopped = false;
            IsListening = true;

            Socket.Listen(3);
            new Thread(ListenClientConnect).Start();
        }
        public void Stop()
        {
            Stopped = true;
            IsListening = false;

            foreach (Socket Client in Clients)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            Socket.Close();
        }


        private void RemoveClient(Socket ClientConnect)
        {
            Clients.Remove(ClientConnect);
        }

        private void AddClient(Socket ClientConnect)
        {
            Clients.Add(ClientConnect);
        }

        private void ListenClientConnect()
        {
            for (; !Stopped;)
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
