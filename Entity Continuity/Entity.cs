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
        public override House House { get; set; }
        public override double Hunger { get; set; }
        public Entity(int x, int y, House house) : base(x, y)
        {
            Level = 0;
            Hunger = 0;
            House = house;

            Colour = house.HouseColour;
        }
    }
}
