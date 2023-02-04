using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MineMP
{
    public class ConsoleBuffer
    {
        public ConsoleBuffer()
        {
            InputBufferPushed += ConsoleBuffer_InputBufferPushed;
        }

        private void ConsoleBuffer_InputBufferPushed(ConsoleInputBuffeePushedEventArgs e)
        {

        }

        public class ConsoleBufferAppendEventArgs : EventArgs
        {
            public readonly string BufferAppended;

            public ConsoleBufferAppendEventArgs(string Buffer)
            {
                BufferAppended = Buffer;
            }
        }
        public class ConsoleControlSymbolBufferPushedEventArgs : EventArgs
        {
            public readonly ControlSymbols BufferPushed;

            public ConsoleControlSymbolBufferPushedEventArgs(ControlSymbols Buffer)
            {
                BufferPushed = Buffer;
            }
        }
        public class ConsoleInputBuffeePushedEventArgs : EventArgs
        {
            public readonly string BufferPushed;

            public ConsoleInputBuffeePushedEventArgs(string Buffer)
            {
                BufferPushed = Buffer;
            }
        }

        public enum BufferContentType
        {
            None = 0,
            Info = 1,
            Debug = 2,
            Warn = 3,
            Error = 4
        }

        public enum ControlSymbols
        {
            ClearScreen = 1
        }

        // Events
        // Debug
        public delegate void DebugBufferAppendedEvent(ConsoleBufferAppendEventArgs e);
        public event DebugBufferAppendedEvent DebugBufferAppended;
        // Info
        public delegate void InfoBufferAppendedEvent(ConsoleBufferAppendEventArgs e);
        public event InfoBufferAppendedEvent InfoBufferAppended;
        // Warn
        public delegate void WarnBufferAppendedEvent(ConsoleBufferAppendEventArgs e);
        public event WarnBufferAppendedEvent WarnBufferAppended;
        // Error
        public delegate void ErrorBufferAppendedEvent(ConsoleBufferAppendEventArgs e);
        public event ErrorBufferAppendedEvent ErrorBufferAppended;
        // ControlSymbol
        public delegate void ControlSymbolBufferPushedEvent(ConsoleControlSymbolBufferPushedEventArgs e);
        public event ControlSymbolBufferPushedEvent ControlSymbolBufferPushed;
        // Input
        private delegate void InputBufferPushedEvent(ConsoleInputBuffeePushedEventArgs e);
        private event InputBufferPushedEvent InputBufferPushed;
        // ReadLine
        public delegate void ReadingInputLineEvent();
        public event ReadingInputLineEvent ReadingInputLine;
        public delegate void ReadingInputLinePeekingEvent();
        public event ReadingInputLinePeekingEvent ReadingInputLinePeeking;

        // Out
        public List<string> InfoBuffers { get; private set; } = new List<string>();
        public List<string> DebugBuffers { get; private set; } = new List<string>();
        public List<string> WarnBuffers { get; private set; } = new List<string>();
        public List<string> ErrorBuffers { get; private set; } = new List<string>();
        // Console Control
        public Queue<ControlSymbols> ControlSymbolBuffers{ get; private set; } = new Queue<ControlSymbols>();

        // In
        // Input
        public Qoollo.Turbo.Collections.Concurrent.BlockingQueue<string> InputBuffers { get; private set; } = new Qoollo.Turbo.Collections.Concurrent.BlockingQueue<string>();

        // Tools
        // Buffer Sync
        public Mutex OutputBufferMutex = new Mutex();

        public void AppendBuffer(BufferContentType ContentType, string Buffer)
        {
            Task.Factory.StartNew(() =>
            {
                OutputBufferMutex.WaitOne();

                switch (ContentType)
                {
                    default: OutputBufferMutex.ReleaseMutex(); return;


                    case BufferContentType.Info:
                        InfoBuffers.Add(Buffer);
                        InfoBufferAppended(new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                    case BufferContentType.Debug:
                        DebugBuffers.Add(Buffer);
                        DebugBufferAppended(new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                    case BufferContentType.Warn:
                        WarnBuffers.Add(Buffer);
                        WarnBufferAppended(new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                    case BufferContentType.Error:
                        ErrorBuffers.Add(Buffer);
                        ErrorBufferAppended(new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                }

                OutputBufferMutex.ReleaseMutex();
                return;
            });
        }
        public void AppendFormatBuffer(BufferContentType ContentType, string format, params object[]? args)
        {
            AppendBuffer(ContentType, string.Format(format, args));
        }

        public void MakeControl(ControlSymbols control)
        {
            ControlSymbolBuffers.Enqueue(control);
            ControlSymbolBufferPushed(new ConsoleControlSymbolBufferPushedEventArgs(control));
        }
        public void MakeInputLine(string input_line)
        {
            InputBuffers.Add(input_line);
            InputBufferPushed(new ConsoleInputBuffeePushedEventArgs(input_line));
        }
        public string ReadLine(bool emptyBuffer = false,bool peeking = false)
        {
            if (emptyBuffer)
                Clear();
            if (peeking)
            {
                ReadingInputLinePeeking();
                return InputBuffers.Peek();
            }
            
            ReadingInputLine();
            
            return InputBuffers.Take();
        }
        public void Clear()
        {
            // Clear Input Buffers
            InputBuffers.Skip(InputBuffers.Count);
        }

        public void ClearBuffer()
        {
            Task.Factory.StartNew(() => {
                InfoBuffers.Clear();
                DebugBuffers.Clear();
                WarnBuffers.Clear();
                ErrorBuffers.Clear();
                ControlSymbolBuffers.Clear();
                InputBuffers.Skip(InputBuffers.Count);

                return;
            });
        }
    }
}
