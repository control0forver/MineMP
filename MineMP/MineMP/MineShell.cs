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
                Console.Write("Shell>");
                Console.ReadLine();
            }
        }
    }
}
