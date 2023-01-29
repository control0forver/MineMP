namespace MineMP
{
    internal class MineMP
    {
        static void RunMineShell(ref MineServer server)
        {
            MineShell.RunShell(ref server);
        }

        static void RunServer()
        {
            Console.WriteLine("[Info]Starting Server");

            var mineServer = new MineServer();
            mineServer.Init();
            mineServer.Start();

            Console.WriteLine("[Info]Server Started");

            Console.WriteLine("[Info]Starting Shell");

            RunMineShell(ref mineServer);
        }

        static void Main(string[] args)
        {
            // Welcome
            Console.WriteLine("Hello, MineMP!\r\n");
            Console.WriteLine("[Press Any Key to Continue]\r\n");

            Console.ReadKey(true);
            
            // Continued

            // Start Server
            RunServer();
            //

            // We Finished
            Console.WriteLine("MineMP Finished");
            Environment.Exit(0);
        }
    }
}