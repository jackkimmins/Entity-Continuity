using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Continuity
{
    class Simulation
    {
        public Map Map { get; set; }

        public Simulation(Map map)
        {
            Console.CursorVisible = false;
            Map = map;
        }

        public void AddEntity()
        {
            Random random = new Random();
            int x = 0;
            int y = 0;

            do {
                x = random.Next(0, Map.Width);
                y = random.Next(0, Map.Height);
            } while (Map.LookupCell(x, y) != 0);

            Entity entity = new Entity(x, y);

            Map.ReplaceCell(x, y, entity);
        }

        public void AddFood()
        {
            Random random = new Random();
            int x = 0;
            int y = 0;

            do {
                x = random.Next(0, Map.Width);
                y = random.Next(0, Map.Height);
            } while (Map.LookupCell(x, y) != 0);

            Food food = new Food(x, y);

            Map.ReplaceCell(x, y, food);
        }

        public void AddEntity(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddEntity();
            }
        }

        public void AddFood(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddFood();
            }
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, 0);

            Map.GenHorsBorderLine(Map.Width);

            for (int i = 0; i < Map.Height; i++)
            {
                for (int j = 0; j < Map.Width; j++)
                {
                    if (j == 0)
                    {
                        Map.GenVertBorderLine();
                    }

                    if (Map.Cells[i][j] is Entity)
                    {
                        string level = ((Entity)Map.Cells[i][j]).Level.ToString();

                        var consoleColour = ((Entity)Map.Cells[i][j]).Colour;

                        Map.Write(level, consoleColour);
                        // Console.Write(Map.Cells[i][j].ToString());
                    }
                    else if (Map.Cells[i][j] is Food)
                    {
                        Map.Write("x", ConsoleColor.White);
                    }
                    else
                    {
                        Map.Write(" ");
                    }
                    
                    if (j == Map.Width - 1)
                    {
                        Map.GenVertBorderLine();
                    }
                }

                Console.WriteLine();
            }

            Map.GenHorsBorderLine(Map.Width);
        }

        public void RawDraw()
        {
            Console.Clear();

            for (int i = 0; i < Map.Cells.Count; i++)
            {
                for (int j = 0; j < Map.Cells[i].Count; j++)
                {
                    if (Map.Cells[i][j] is Entity)
                    {
                        Console.Write('0');
                    }
                    else if (Map.Cells[i][j] is Food)
                    {
                        Console.Write('x');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private void Eat(Entity entity, Cell cell)
        {
            if (cell is Food)
            {
                Entity newEntity = new Entity(cell.X, cell.Y);
                newEntity.Level = entity.Level + 1;
                newEntity.Colour = entity.Colour;
                newEntity.X = cell.X;
                newEntity.Y = cell.Y;

                Map.ReplaceCell(cell.X, cell.Y, newEntity);
                Map.ResetCell(entity.X, entity.Y);

                // Console.WriteLine("Entity ate food");
                // Console.WriteLine("Entity level: " + newEntity.Level);
                // Console.WriteLine("Entity position: " + newEntity.X + " " + newEntity.Y);
            }
        }

        private void EntityPaths(Entity entity)
        {
            //Check the surrounding 8 cells for food.
            var surroundingCells = Map.GetSurroundingFoods(entity.X, entity.Y);

            //If there is food, move to it.
            if (surroundingCells.Count > 0)
            {
                //Get the closest food.
                var closestFood = surroundingCells[new Random().Next(0, surroundingCells.Count)];

                //Move to the closest food.
                Eat(entity, closestFood);

                AddFood();
            }
            else
            {
                var surroundingBlanks = Map.GetSurroundingBlanks(entity.X, entity.Y);

                var closestFood = Map.GetClosestFood(entity.X, entity.Y);

                // closestFood.Output();
                // Console.ReadKey();

                //Pick the cell from surroundingBlanks that is closest to the food.
                int best = Int16.MaxValue;
                int bestIndex = 0;

                foreach (var cell in surroundingBlanks)
                {
                    int distance = Map.Distance(cell.X, cell.Y, closestFood.X, closestFood.Y);

                    //Output the distance.
                    //Console.WriteLine("Distance: " + distance);
                    
                    if (best > distance)
                    {
                        best = distance;
                        bestIndex = surroundingBlanks.IndexOf(cell);
                    }
                }

                //Output best
                //Console.WriteLine("Best: " + best);

                Cell nextCell = surroundingBlanks[bestIndex];

                Entity newEntity = new Entity(nextCell.X, nextCell.Y);
                newEntity.Level = entity.Level;
                newEntity.Colour = entity.Colour;

                Map.ResetCell(entity.X, entity.Y);
                Map.ReplaceCell(nextCell.X, nextCell.Y, newEntity);
            }
        }

        public void Start()
        {
            int loopNum = 1;

            int entityCount2 = Map.Cells.SelectMany(x => x).Count(x => x is Entity);
            // Console.WriteLine("Entity count: " + entityCount2);
            // Console.ReadKey();

            while (true)
            {
                Draw();

                List<List<Cell>> cells = Map.CloneCells();

                //Loop over all entities in Map.Cells
                for (int i = 0; i < Map.Height; i++)
                {
                    for (int j = 0; j < Map.Width; j++)
                    {
                        if (cells[i][j] is Entity)
                        {
                            EntityPaths((Entity)cells[i][j]);
                            //Console.WriteLine(loopNum + ": Coordinates: " + i + " " + j);
                                                    // cells[i][j].Output();
                        }
                    }
                }

                loopNum++;  

                if (entityCount2 != Map.Cells.SelectMany(x => x).Count(x => x is Entity))
                {
                    entityCount2 = Map.Cells.SelectMany(x => x).Count(x => x is Entity);
                    AddEntity();
                }

                //Count the number of entities.


                //Console.ReadKey();

                if (loopNum >= 1000)
                {
                    Draw();

                    int entityCount = Map.Cells.SelectMany(x => x).Count(x => x is Entity);
                    Console.WriteLine("Entity count: " + entityCount);

                    break;
                }

                System.Threading.Thread.Sleep(50);
            }
        }
    }
}
