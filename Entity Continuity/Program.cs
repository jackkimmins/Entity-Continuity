using System;
using System.Collections.Generic;

namespace Entity_Continuity
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Entity Continuity";

            // Map map = new Map(32, 32);
            // Map map = new Map(64, 64);
            Map map = new Map(45, 32);
            // Map map = new Map(100, 64);

            Simulation simulation = new Simulation(map);

            simulation.Houses = new List<House>()
            {
                new House("House 1", ConsoleColor.Red),
                new House("House 2", ConsoleColor.Cyan),
                new House("House 3", ConsoleColor.Blue),
                new House("House 4", ConsoleColor.Green),
                new House("House 5", ConsoleColor.Yellow),
                new House("House 6", ConsoleColor.Magenta),
                new House("House 7", ConsoleColor.Gray),
                new House("House 8", ConsoleColor.DarkCyan),
                new House("House 9", ConsoleColor.DarkYellow),
            };

            foreach (House house in simulation.Houses)
            {
                simulation.AddEntity(house);
                simulation.AddEntity(house);
                simulation.AddEntity(house);
            }

            //simulation.AddDead();

            simulation.AddFood(simulation.Houses.Count * 30);

            simulation.Start();
        }
    }
}
