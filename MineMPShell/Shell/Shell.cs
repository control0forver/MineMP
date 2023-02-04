using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MineMP;

namespace MineMPShell
{
    public class Shell
    {
        public void RunShell(ref MineServer server)
        {
            server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info,"\r\nWelcome To Mine Shell!");
            server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info, "Type \"help\" for more information.");

            for (; ; )
            {
                string CmdLine = "";

                // Console.Write("Shell>");
                CmdLine = server.ConsoleBuffer.ReadLine().Trim();
                if (CmdLine == string.Empty)
                    continue;

                string[] argv = CmdLine.Split(' ');
                int argc = argv.Length;

                switch (argv[0])
                {
                    default:
                        server.ConsoleBuffer.AppendFormatBuffer(ConsoleBuffer.BufferContentType.Info, "unknown command: {0}", argv[0]);
                        break;


                    case "help":
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info, "Help|Commands:");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info, "  help       ----    Show help information");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info, "  stop       ----    Stop server && exit shell");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info, "  list       ----    List total client connections");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info, "  clear      ----    Clear Console Screen");
                        break;

                    case "stop":
                        server.Stop();
                        return;

                    case "list":
                        server.ConsoleBuffer.AppendFormatBuffer(ConsoleBuffer.BufferContentType.Info,"--------------\r\nTotal Clients: {0}", server.Clients.Count);
                        for (int i = 0; i < server.Clients.Count; ++i)
                        {
                            server.ConsoleBuffer.AppendFormatBuffer(ConsoleBuffer.BufferContentType.Info, "{0}| {1}:{2}", i, ((IPEndPoint)server.Clients[i].RemoteEndPoint).Address, ((IPEndPoint)server.Clients[i].RemoteEndPoint).Port);
                        }
                        server.ConsoleBuffer.AppendFormatBuffer(ConsoleBuffer.BufferContentType.Info, "--------------\r\nTotal Players: {0}", server.Players.Count);
                        for (int i = 0; i < server.Players.Count; ++i)
                        {
                            server.ConsoleBuffer.AppendFormatBuffer(ConsoleBuffer.BufferContentType.Info, "{0}", server.Players[i].Name);
                        }
                        break;

                    case "test":
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.None, "None");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Info, "Info");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Error, "Error");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Warn, "Warn");
                        server.ConsoleBuffer.AppendBuffer(ConsoleBuffer.BufferContentType.Debug, "Debug");
                        break;

                    case "clear":
                        server.ConsoleBuffer.MakeControl(ConsoleBuffer.ControlSymbols.ClearScreen);
                        break;
                }

                continue;
            }

        }
    }
}
