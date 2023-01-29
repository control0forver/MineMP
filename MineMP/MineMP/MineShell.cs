namespace MineMP
{
    internal static class MineShell
    {
        public static void RunShell(ref MineServer server)
        {
            Console.WriteLine("Welcome To Mine Shell!");
            Console.WriteLine("Type \"help\" for more information.");

            for (; ; )
            {
                string CmdLine = "";

                Console.Write("Shell>");
                CmdLine = Console.ReadLine();
                if (CmdLine != null)
                    continue;
            }
        }
    }
}
