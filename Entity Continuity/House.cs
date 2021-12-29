using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Continuity
{
    class House
    {
        public string HouseName { get; }
        public ConsoleColor HouseColour { get; }

        public House(string houseName, ConsoleColor houseColour)
        {
            HouseName = houseName;
            HouseColour = houseColour;
        }

        public static int GetPopulation(List<List<Cell>> cells, House house)
        {
            int population = 0;

            foreach (var cell in cells)
            {
                foreach (var c in cell)
                {
                    if (c is Entity)
                    {
                        if ((c as Entity).House.HouseName == house.HouseName)
                        {
                            population++;
                        }
                    }
                }
            }

            return population;
        }

        public static int GetLevel(List<List<Cell>> cells, House house)
        {
            int size = 0;

            foreach (var cell in cells)
            {
                foreach (var c in cell)
                {
                    if (c is Entity)
                    {
                        if ((c as Entity).House.HouseName == house.HouseName)
                        {
                            size = size + (c as Entity).Level;
                        }
                    }
                }
            }

            return size;
        }
    }
}
