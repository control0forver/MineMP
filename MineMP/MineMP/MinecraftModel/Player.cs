using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineMP.MinecraftModel
{
    public class Player
    {
        public static readonly string DefaultName = "Unknown";
        public static readonly int DefaultNameLength = DefaultName.Length;

        public int NameLength { get; private set; } = DefaultNameLength;
        public string Name { get; private set; } = DefaultName;

        public Player(int NameLength, string Name)
        {
            this.NameLength = NameLength;
            this.Name = Name;
        }
    }
}
