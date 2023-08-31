using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MineMP
{
    // From MineMP lib, written by LGF

    public class ConsoleBuffer
    {
        public ConsoleBuffer()
        {
            InputBufferPushed += ConsoleBuffer_InputBufferPushed;
        }

        private void ConsoleBuffer_InputBufferPushed(object _, ConsoleInputBuffeePushedEventArgs __) { }

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
        public delegate void DebugBufferAppendedEvent(object sender, ConsoleBufferAppendEventArgs e);
        public event DebugBufferAppendedEvent? DebugBufferAppended = null;
        // Info
        public delegate void InfoBufferAppendedEvent(object sender, ConsoleBufferAppendEventArgs e);
        public event InfoBufferAppendedEvent? InfoBufferAppended = null;
        // Warn
        public delegate void WarnBufferAppendedEvent(object sender, ConsoleBufferAppendEventArgs e);
        public event WarnBufferAppendedEvent? WarnBufferAppended = null;
        // Error
        public delegate void ErrorBufferAppendedEvent(object sender, ConsoleBufferAppendEventArgs e);
        public event ErrorBufferAppendedEvent? ErrorBufferAppended = null;
        // ControlSymbol
        public delegate void ControlSymbolBufferPushedEvent(object sender, ConsoleControlSymbolBufferPushedEventArgs e);
        public event ControlSymbolBufferPushedEvent? ControlSymbolBufferPushed = null;
        // Input
        private delegate void InputBufferPushedEvent(object sender, ConsoleInputBuffeePushedEventArgs e);
        private event InputBufferPushedEvent? InputBufferPushed = null;
        // ReadLine
        public delegate void ReadingInputLineEvent(object sender);
        public event ReadingInputLineEvent? ReadingInputLine = null;
        public delegate void ReadingInputLinePeekingEvent(object sender);
        public event ReadingInputLinePeekingEvent? ReadingInputLinePeeking = null;

        // Out
        public List<string> InfoBuffers { get; private set; } = new List<string>();
        public List<string> DebugBuffers { get; private set; } = new List<string>();
        public List<string> WarnBuffers { get; private set; } = new List<string>();
        public List<string> ErrorBuffers { get; private set; } = new List<string>();
        // Console Control
        public Queue<ControlSymbols> ControlSymbolBuffers { get; private set; } = new Queue<ControlSymbols>();

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
                        if (InfoBufferAppended != null)
                            InfoBufferAppended(this, new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                    case BufferContentType.Debug:
                        DebugBuffers.Add(Buffer);
                        if (DebugBufferAppended != null)
                            DebugBufferAppended(this, new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                    case BufferContentType.Warn:
                        WarnBuffers.Add(Buffer);
                        if (WarnBufferAppended != null)
                            WarnBufferAppended(this, new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                    case BufferContentType.Error:
                        ErrorBuffers.Add(Buffer);
                        if (ErrorBufferAppended != null)
                            ErrorBufferAppended(this, new ConsoleBufferAppendEventArgs(Buffer));
                        break;

                }

                OutputBufferMutex.ReleaseMutex();
                return;
            });
        }
        public void AppendFormatBuffer(BufferContentType ContentType, string format, params object[]? args)
        {
            AppendBuffer(ContentType, (args == null ? format : string.Format(format, args)));
        }

        public void MakeControl(ControlSymbols control)
        {
            ControlSymbolBuffers.Enqueue(control);
            if (ControlSymbolBufferPushed != null)
                ControlSymbolBufferPushed(this, new ConsoleControlSymbolBufferPushedEventArgs(control));
        }
        public void MakeInputLine(string input_line)
        {
            InputBuffers.Add(input_line);
            if (InputBufferPushed != null)
                InputBufferPushed(this, new ConsoleInputBuffeePushedEventArgs(input_line));
        }
        public string ReadLine(bool emptyBuffer = false, bool peeking = false)
        {
            if (emptyBuffer)
                Clear();
            if (peeking)
            {
                if (ReadingInputLinePeeking != null)
                    ReadingInputLinePeeking(this);
                return InputBuffers.Peek();
            }

            if (ReadingInputLine != null)
                ReadingInputLine(this);

            return InputBuffers.Take();
        }
        public void Clear()
        {
            // Clear Input Buffers
            InputBuffers.Skip(InputBuffers.Count);
        }

        public void ClearBuffer()
        {
            Task.Factory.StartNew(() =>
            {
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
