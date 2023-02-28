using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineMP.MinecraftModel
{
    public class Protocol
    {
        public List<byte> Data { get; private set; } = new List<byte>();
        public int PackageID { get; private set; } = -1;

        public void Process()
        {
            PackageID = Data[0];
        }

        public Protocol() { }
        public Protocol(List<byte> data) { this.Data = data; }
        public Protocol(byte[] data) { this.Data.AddRange(data); }
    }
}
