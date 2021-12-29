using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Continuity
{
    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public virtual ConsoleColor Colour { get; set; }
        public virtual int Level { get; set; }
        public virtual House House { get; set; }
        public virtual double Hunger { get; set; }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Output()
        {
            //Output coordinates
            Console.WriteLine($"X: {X} Y: {Y}");
        }
    }
}
