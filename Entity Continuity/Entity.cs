using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Continuity
{
    class Entity : Cell
    {
        public override int Level { get; set; }
        public override ConsoleColor Colour { get; set; }
        public Entity(int x, int y) : base(x, y)
        {
            Level = 0;
            Colour = (ConsoleColor)new Random().Next(0, 16);
        }
    }
}
