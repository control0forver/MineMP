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
        public class ConsoleBufferAppendEventArgs : EventArgs
        {
            public readonly string BufferAppending;

            public ConsoleBufferAppendEventArgs(string Buffer)
            {
                BufferAppending = Buffer;
            }
        }

        public enum BufferContentType
        {
            Info = 1,
            Debug = 2,
            Warn = 3,
            Error = 4
        }

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

        public List<string> InfoBuffers { get; private set; } = new List<string>();
        public List<string> DebugBuffers { get; private set; } = new List<string>();
        public List<string> WarnBuffers { get; private set; } = new List<string>();
        public List<string> ErrorBuffers { get; private set; } = new List<string>();

        public Mutex BufferMutex = new Mutex();

        public void AppendBuffer(BufferContentType ContentType, string Buffer)
        {
            Task.Factory.StartNew(() =>
            {
                BufferMutex.WaitOne();

                switch (ContentType)
                {
                    default: BufferMutex.ReleaseMutex(); return;


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

                BufferMutex.ReleaseMutex();
                return;
            });
        }
        public void AppendFormatBuffer(BufferContentType ContentType, string format, params object[]? args)
        {
            AppendBuffer(ContentType, string.Format(format, args));
        }
        public void ClearBuffer()
        {
            Task.Factory.StartNew(() => {
                InfoBuffers.Clear();
                DebugBuffers.Clear();
                WarnBuffers.Clear();
                ErrorBuffers.Clear();

                return;
            });
        }
    }
}
