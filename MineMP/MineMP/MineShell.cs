using System.Net;

namespace MineMP
{
    internal static class MineShell
    {
        public static void RunShell(ref MineServer server)
        {
            Console.WriteLine("\r\nWelcome To Mine Shell!");
            Console.WriteLine("Type \"help\" for more information.");

            for (; ; )
            {
                string CmdLine = "";

                // Console.Write("Shell>");
                CmdLine = Console.ReadLine();
                if (CmdLine.Trim() == string.Empty)
                    continue;

                string[] argv = CmdLine.Split(' ');
                int argc = argv.Length;

                switch (argv[0])
                {
                    default:
                        Console.WriteLine("unknown command: {0}", argv[0]);
                        break;


                    case "help":
                        Console.WriteLine("Help|Commands:");
                        Console.WriteLine("  help       ----    Show help information");
                        Console.WriteLine("  stop       ----    Stop server && exit shell");
                        Console.WriteLine("  list       ----    List total client connections");
                        break;

                    case "stop":
                        server.Stop();
                        return;

                    case "list":
                        Console.WriteLine("Total Clients: {0}", server.Clients.Count);
                        for (int i = 0; i < server.Clients.Count; ++i)
                        {
                            Console.WriteLine("{0}| {1}:{2}", i, ((IPEndPoint)server.Clients[i].RemoteEndPoint).Address, ((IPEndPoint)server.Clients[i].RemoteEndPoint).Port);
                        }
                        break;
                }

                continue;
            }

        }
    }
}
