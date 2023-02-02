﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MineMP
{
    public class MineServer
    {
        private bool StopServer = true;

        public Socket Socket { get; private set; }
        public List<Socket> Clients { get; private set; } = new List<Socket>();
        public List<MinecraftModel.Player> Players { get; private set; } = new List<MinecraftModel.Player>();
        public bool IsListening { get; private set; } = false;

        public string SP_Motd { get; private set; } = "A Minecraft Server";
        public uint SP_MaxPlayers = 25;

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

        public bool UpdateClient(Socket clientConnect)
        {
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
                if (!SockExHandler(ex))
                {
                    Console.WriteLine("[Info]Failed to close client connection.");
                    Console.WriteLine("Client: {0};{1}", ((IPEndPoint)ClientConnect.RemoteEndPoint).Address, ((IPEndPoint)ClientConnect.RemoteEndPoint).Port);
                    Console.WriteLine("Error Message({0}): {1}", ex.ErrorCode, ex.Message);
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
                catch (SocketException ex)
                {
                    if (!SockExHandler(ex))
                    {
                        Console.WriteLine("[Info]Failed to close client connection.");
                        Console.WriteLine("Client: {0};{1}", ((IPEndPoint)client.RemoteEndPoint).Address, ((IPEndPoint)client.RemoteEndPoint).Port);
                        Console.WriteLine("Error Message({0}): {1}", ex.ErrorCode, ex.Message);
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

                if (buffer[0] == 0xFE) // Legacy Minecraft Client Ping in Handshaking
                {
                    Console.WriteLine("[Info]0xFE: Minecraft Client Ping");
                    Console.WriteLine("Client: {0};{1}", ((IPEndPoint)client.RemoteEndPoint).Address, ((IPEndPoint)client.RemoteEndPoint).Port);

                    List<byte> bytes = new List<byte>();
                    // Set Bytes
                    byte[] bytesSplit = { 0xA7, 0x00 };
                    byte[] bytesBegin = { 0xFF, 0x00, 0x17, 0x00 };
                    byte[] bytesMotd = Encoding.GetEncoding("UTF-16").GetBytes(SP_Motd);
                    byte[] bytesPlayersCount = Encoding.GetEncoding("UTF-16").GetBytes(Players.Count.ToString());
                    byte[] bytesMaxPlayersLimit = Encoding.GetEncoding("UTF-16").GetBytes(SP_MaxPlayers.ToString());

                    // Index Bytes
                    bytes.AddRange(bytesBegin);
                    bytes.AddRange(bytesMotd);
                    bytes.AddRange(bytesSplit);
                    bytes.AddRange(bytesPlayersCount);
                    bytes.AddRange(bytesSplit);
                    bytes.AddRange(bytesMaxPlayersLimit);

                    // Return Info bytes
                    client.Send(bytes.ToArray());

                    RemoveClient(client);
                    return;
                }

                if (buffer[0] == 0x02) // Player Join
                {
                    Console.WriteLine("[Info]0x02: Minecraft Player Join");
                    Console.WriteLine("Client: {0};{1}", ((IPEndPoint)client.RemoteEndPoint).Address, ((IPEndPoint)client.RemoteEndPoint).Port);

                    MinecraftModel.Player player;
                    int length = 0;
                    string name = "";
                    length = buffer[2];
                    name = Encoding.GetEncoding("UTF-16").GetString(buffer).Substring(2).Substring(0,length);
                    player = new MinecraftModel.Player(length, name);

                    Console.WriteLine("Name Length: {0}", length);
                    Console.WriteLine("Name: {0}", name);

                    Players.Add(player);

                    List<byte> bytes = new List<byte>();
                    // Set Bytes
                    byte[] bytesBegin = { 0x02, 0x00 };
                    byte[] bytesUUID = { (byte)length, 0x00 };
                    byte[] bytesUsername = Encoding.GetEncoding("ASCII").GetBytes(player.Name);

                    // Index Bytes
                    bytes.AddRange(bytesBegin);
                    bytes.AddRange(bytesUUID);
                    bytes.AddRange(bytesUsername);

                    // Send
                    client.Send(bytes.ToArray());

                    return;
                }

                Console.WriteLine("[DEBUG]message from client:\r\n{0}", BitConverter.ToString(buffer));
                Console.WriteLine("[DEBUG]Decoded message from client:\r\n{0}", Encoding.GetEncoding("UTF-16").GetString(buffer));

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

        private bool SockExHandler(SocketException ex)
        {
            switch (ex.ErrorCode)
            {
                default:
                    return false;

                case 10057: // SocketException(10057) Maybe Tcping Client
                    break;

                case 10054: // SocketException(10054) Connection Closed by Remote
                    break;
            }

            return true;
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
