using System;

namespace Entity_Continuity
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(52, 52);

            Simulation simulation = new Simulation(map);

            const int entityCount = 10;

            simulation.AddEntity(entityCount);
            simulation.AddFood(entityCount * 2);

            simulation.Start();
        }
    }
}
