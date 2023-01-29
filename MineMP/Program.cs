namespace MineMP
{
    internal class Program
    {
        static void RunMineShell(ref MineServer server)
        {
            Console.WriteLine("Welcome To Mine Shell!");
            Console.WriteLine("Type \"help\" for more information.");

            for (; ; )
            {
                Console.Write();
                Console.ReadLine();
            }
        }

        static void RunServer()
        {
            Console.WriteLine("Starting Server");

            MineServer mineServer = new MineServer();
            mineServer.Init();
            mineServer.Start();

            RunMineShell(ref mineServer);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, MineMP!\r\n");
            Console.WriteLine("[Press Any Key to Continue]\r\n");

            Console.ReadKey();

            RunServer();

            Environment.Exit(0);
        }
    }
}