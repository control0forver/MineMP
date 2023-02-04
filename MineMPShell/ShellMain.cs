using MineMP;
using static MineMP.ConsoleBuffer;

namespace MineMPShell
{
    internal class ShellMain
    {
        static MineServer MineServer;
        static void RunMineShell()
        {
            new Shell().RunShell(ref MineServer);
        }

        static void RunServer()
        {
            Console.WriteLine("[Info]Starting Server");

            MineServer.Start();

            Console.WriteLine("[Info]Server Started");

            Console.WriteLine("[Info]Starting Shell");

            RunMineShell();
        }

        static void SetupCore()
        {
            Console.WriteLine("[Info]Setup Core...");

            MineServer = new MineServer();
            MineServer.ConsoleBuffer.ClearBuffer();
            MineServer.ConsoleBuffer.ErrorBufferAppended += ConsoleBuffer_ErrorBufferAppended;
            MineServer.ConsoleBuffer.InfoBufferAppended += ConsoleBuffer_InfoBufferAppended;
            MineServer.ConsoleBuffer.WarnBufferAppended += ConsoleBuffer_WarnBufferAppended;
            MineServer.ConsoleBuffer.DebugBufferAppended += ConsoleBuffer_DebugBufferAppended;
            MineServer.ConsoleBuffer.ControlSymbolBufferPushed += ConsoleBuffer_ControlSymbolBufferPushed;
            MineServer.ConsoleBuffer.ReadingInputLine += ConsoleBuffer_ReadingInputLine;
            MineServer.ConsoleBuffer.ReadingInputLinePeeking += ConsoleBuffer_ReadingInputLinePeeking;
            MineServer.Init();

        }

        // Ignore
        private static void ConsoleBuffer_ReadingInputLinePeeking()
        {
        }

        private static void ConsoleBuffer_ReadingInputLine()
        {
            MineServer.ConsoleBuffer.MakeInputLine(Console.ReadLine());
        }

        private static void ConsoleBuffer_ControlSymbolBufferPushed(ConsoleControlSymbolBufferPushedEventArgs e)
        {
            switch (e.BufferPushed)
            {
                default: return;

                case ControlSymbols.ClearScreen:
                    Console.Clear();
                    break;
            }

            return;
        }

        private static void ConsoleBuffer_DebugBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Out.WriteLine(e.BufferAppended);
            Console.ResetColor();
        }

        private static void ConsoleBuffer_WarnBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Out.WriteLine(e.BufferAppended);
            Console.ResetColor();
        }

        private static void ConsoleBuffer_InfoBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Out.WriteLine(e.BufferAppended);
            Console.ResetColor();
        }

        private static void ConsoleBuffer_ErrorBufferAppended(ConsoleBuffer.ConsoleBufferAppendEventArgs e)
        {
            Console.BackgroundColor= ConsoleColor.Red;
            Console.Error.WriteLine(e.BufferAppended);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            // Welcome
            Console.WriteLine("Hello, MineMP!\r\n");
            Console.WriteLine("[Press Any Key to Continue]\r\n");

            Console.ReadKey(true);

            // Continued

            // Start Server
            SetupCore();
            RunServer();
            //

            // We Finished
            Console.WriteLine("MineMP Finished");
            Environment.Exit(0);
        }
    }
}