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
        public List<House> Houses { get; set; }

        public Simulation(Map map)
        {
            Console.CursorVisible = false;
            Map = map;
        }

        public void AddEntity(House house = null)
        {
            Random random = new Random();
            int x = 0;
            int y = 0;

            do {
                x = random.Next(0, Map.Width);
                y = random.Next(0, Map.Height);
            } while (Map.LookupCell(x, y) != 1);

            if (house == null)
            {
                house = Houses[random.Next(0, Houses.Count)];
            }

            Entity entity = new Entity(x, y, house);
            //entity.Level = 7;

            Map.ReplaceCell(x, y, entity);
        }

        public void AddDead()
        {
            Random random = new Random();
            int x = 0;
            int y = 0;

            do {
                x = random.Next(0, Map.Width);
                y = random.Next(0, Map.Height);
            } while (Map.LookupCell(x, y) != 1);

            Dead dead = new Dead(x, y);
            Map.ReplaceCell(x, y, dead);
        }

        public void AddFood()
        {
            Random random = new Random();
            int x = 0;
            int y = 0;

            do {
                x = random.Next(0, Map.Width);
                y = random.Next(0, Map.Height);
            } while (Map.LookupCell(x, y) != 1);

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
                        Map.Write("·", ConsoleColor.White);
                    }
                    else if (Map.Cells[i][j] is Dead)
                    {
                        Map.Write("*", ConsoleColor.White);
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

            Console.SetCursorPosition(Map.Width + 3, 0);
            Console.WriteLine("Houses:");

            //Sort the houses by population
            var sortedHouses = Houses.OrderByDescending(h => House.GetPopulation(Map.Cells, h));

            for (int i = 0; i < sortedHouses.Count(); i++)
            {
                Console.SetCursorPosition(Map.Width + 3, 1 + i);
                Console.ForegroundColor = sortedHouses.ElementAt(i).HouseColour;
                Console.WriteLine("* {0} - Size: {1} / Level: {2}", sortedHouses.ElementAt(i).HouseName, House.GetPopulation(Map.Cells, sortedHouses.ElementAt(i)), House.GetLevel(Map.Cells, sortedHouses.ElementAt(i)));
                Console.ForegroundColor = ConsoleColor.White;
            }
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
                        Console.Write('.');
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
                Entity newEntity = new Entity(cell.X, cell.Y, entity.House);
                newEntity.Level = entity.Level + 1;
                newEntity.Colour = entity.Colour;
                newEntity.X = cell.X;
                newEntity.Y = cell.Y;

                entity.Level = entity.Level + 1;

                Map.ReplaceCell(cell.X, cell.Y, newEntity);
                Map.ResetCell(entity.X, entity.Y);

                // Console.WriteLine("Entity ate food");
                // Console.WriteLine("Entity level: " + newEntity.Level);
                // Console.WriteLine("Entity position: " + newEntity.X + " " + newEntity.Y);
            }
            else if (cell is Entity)
            {
                Entity newEntity = new Entity(cell.X, cell.Y, entity.House);
                newEntity.Level = entity.Level + cell.Level;
                newEntity.Colour = entity.Colour;
                newEntity.X = cell.X;
                newEntity.Y = cell.Y;

                entity.Level = entity.Level + cell.Level;

                Map.ReplaceCell(cell.X, cell.Y, newEntity);
                Map.ResetCell(entity.X, entity.Y);
            }
        }

        private void EntityPaths(Entity entity)
        {
            var surroundingEntities = Map.GetSurroundingEntities(entity);

            if (surroundingEntities.Count > 0)
            {
                var closestEntity = surroundingEntities.OrderBy(x => Map.Distance(entity.X, entity.Y, x.X, x.Y)).First();

                Eat(entity, closestEntity);
                Reproduce(ref entity, closestEntity.X, closestEntity.Y);
                return;
            }

            var avoid = Map.GetAvoid(entity, 10);

            if (avoid is not null)
            {
                Navigate(entity, avoid, false);
                return;
            }

            var target = Map.GetTarget(entity);

            if (target is not null)
            {
                // Console.WriteLine(entity.House.HouseName + ": " + entity.Level + " " + entity.X + " " + entity.Y);
                // Console.WriteLine(target.House.HouseName + ": " + target.Level + " " + target.X + " " + target.Y);
                // Console.ReadKey();
                Navigate(entity, target);
                return;
            }

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

                //10% Randon chance to add food.
                if (new Random().Next(0, 15) == 0)
                {
                    AddFood();
                }

                Reproduce(ref entity, closestFood.X, closestFood.Y);
            }
            else
            {
                var closestFood = Map.GetClosestFood(entity.X, entity.Y, entity.House.HouseName);
                Navigate(entity, closestFood);
            }
        }

        private void DeadPaths(Dead entity)
        {
            var target = Map.GetTarget(entity, Map.Width);

            if (target is not null)
            {
                Navigate(entity, target);
                return;
            }
        }

        //Pick a random chance, the higher the chance, the higher the chance of reproduction.
        public bool RandomChance(int chance)
        {
            int random = new Random().Next(6, 14);

            if (random < chance)
            {
                return true;
            }

            return false;
        }

        private void Navigate(Cell entity, Cell target, bool towards = true)
        {
            var surroundingBlanks = Map.GetSurroundingBlanks(entity.X, entity.Y);

            if (surroundingBlanks.Count <= 0)
                return;

            // closestFood.Output();
            // Console.ReadKey();

            //Pick the cell from surroundingBlanks that is closest to the food.
            int best = towards ? Int16.MaxValue : 0;
            int bestIndex = 0;

            foreach (var cell in surroundingBlanks)
            {
                int distance = Map.Distance(cell.X, cell.Y, target.X, target.Y);

                //Output the distance.
                //Console.WriteLine("Distance: " + distance);
                
                if (towards)
                {
                    if (distance < best)
                    {
                        best = distance;
                        bestIndex = surroundingBlanks.IndexOf(cell);
                    }
                }
                else
                {
                    if (distance > best)
                    {
                        best = distance;
                        bestIndex = surroundingBlanks.IndexOf(cell);
                    }
                }
            }

            //Output best
            //Console.WriteLine("Best: " + best);

            if (entity is Entity)
            {
                Cell nextCell = surroundingBlanks[bestIndex];

                entity.Hunger = entity.Hunger + ((entity.Level - 2) * 1.05);

                if (entity.Hunger >= 20)
                {
                    entity.Hunger = 0;
                    entity.Level = entity.Level - 1;
                }

                Entity newEntity = new Entity(nextCell.X, nextCell.Y, entity.House);
                newEntity.Hunger = entity.Hunger;

                newEntity.Level = entity.Level;

                newEntity.Colour = entity.Colour;

                Map.ResetCell(entity.X, entity.Y);
                if (newEntity.Level >= 0 && entity.Hunger < 20)
                {
                    Map.ReplaceCell(nextCell.X, nextCell.Y, newEntity);
                }
                else
                {
                    Food food = new Food(nextCell.X, nextCell.Y);
                    Map.ReplaceCell(nextCell.X, nextCell.Y, food);
                }
            }
            else if (entity is Dead)
            {
                Cell nextCell = surroundingBlanks[bestIndex];

                Dead newDead = new Dead(nextCell.X, nextCell.Y);

                Map.ResetCell(entity.X, entity.Y);
                Map.ReplaceCell(nextCell.X, nextCell.Y, newDead);
            }
        }

        private void Reproduce(ref Entity entity, int x, int y)
        {
            if (entity.Level >= 10)
            {
                var spawnCells = Map.GetSurroundingBlanks(entity.X, entity.Y);

                if (spawnCells.Count > 0)
                {
                    var nextCell = spawnCells[new Random().Next(0, spawnCells.Count)];

                    entity.Level = 0;

                    Entity resetEntity = new Entity(x, y, entity.House);
                    resetEntity.Level = 0;
                    resetEntity.Colour = entity.Colour;

                    Entity newEntity = new Entity(nextCell.X, nextCell.Y, entity.House);
                    newEntity.Level = 0;
                    newEntity.Colour = entity.Colour;

                    Map.ReplaceCell(nextCell.X, nextCell.Y, newEntity);
                    Map.ReplaceCell(x, y, resetEntity);
                }
            }
        }

        private void DrawAsync()
        {
            Task.Run(() =>
            {
                Draw();
            });
        }

        public void Start()
        {
            int cycle = 0;

            // DrawLoop();

            Console.Write("Cycle: 0");

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
                        }
                        else if (cells[i][j] is Dead)
                        {
                            DeadPaths((Dead)cells[i][j]);
                        }
                    }
                }

                cycle++;

                // System.Threading.Thread.Sleep(250);

                //if (cycle >= 500)
                //{
                //   Draw();
                //}
                //else
                //{
                //   Console.SetCursorPosition(7, 0);
                //   Console.Write(cycle);
                //}
            }
        }
    }
}
