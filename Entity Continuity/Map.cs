using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Continuity
{
    class Map
    {
        public int Width { get; }
        public int Height { get; }
        public List<List<Cell>> Cells { get; }

        public int CellCount => Width * Height;

        public Map(int width = 32, int height = 32)
        {
            Width = width * 2;
            Height = height;
            Cells = new List<List<Cell>>();
            Generate();
        }

        public void ReplaceCell(int x, int y, Cell cell)
        {
            Cells[y][x] = cell;
        }

        //Sets the cell at the given coordinates to a blank cell.
        public void ResetCell(int x, int y)
        {
            ReplaceCell(x, y, new Cell(x, y));
        }

        //Return the type of the cell at the given coordinates
        public int LookupCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return 0;
            }
            else if (Cells[y][x] is Entity)
            {
                return 1;
            }
            else if (Cells[y][x] is Food)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        private void Generate()
        {
            Cells.Clear();

            for (int y = 0; y < Height; y++)
            {
                List<Cell> row = new List<Cell>();

                for (int x = 0; x < Width; x++)
                {
                    row.Add(new Cell(x, y));
                }

                Cells.Add(row);
            }
        }

        public void Write(string value, ConsoleColor colour = ConsoleColor.White)
        {
            //Console.ForegroundColor = (ConsoleColor)new Random().Next(1, 15);
            Console.ForegroundColor = colour;
            Console.Write(value);
            Console.ResetColor();
        }

        public Food GetClosestFood(int x, int y)
        {
            Food closest = null;
            int closestDistance = int.MaxValue;

            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    if (Cells[i][j] is Food)
                    {
                        int distance = (int)Math.Sqrt(Math.Pow(x - j, 2) + Math.Pow(y - i, 2));

                        if (distance < closestDistance)
                        {
                            closest = (Food)Cells[i][j];
                            closestDistance = distance;
                        }
                    }
                }
            }

            return closest;
        }

        public void GenHorsBorderLine(int length)
        {
            //Generate the horizontal border line according
            //to the length x 2 and the extra 2 for the vertical borders.
            for (int i = 0; i < Width + 2; i++)
                Console.Write("#");
            Console.WriteLine();
        }

        public void GenVertBorderLine(bool withGap = false)
        {
            Console.Write(withGap ? " #" : "#");
        }

        //Get the distance between two cells using the pythagorean theorem
        public static int Distance(int x, int y, int x2, int y2)
        {
            return (int)Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2));
        }

        private List<Cell> GetSurrounding(int x, int y)
        {
            List<Cell> surrounding = new List<Cell>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < Width && newY >= 0 && newY < Height)
                    {
                        surrounding.Add(Cells[newY][newX]);
                    }
                }
            }

            return surrounding;
        }

        //Gets the surrounding 8 cells of the given cell
        public List<Cell> GetSurroundingFoods(int x, int y)
        {
            List<Cell> surrounding = GetSurrounding(x, y);
            return surrounding.Where(c => c is Food).ToList();
        }

        //Gets the surrounding 8 cells of the given cell
        public List<Cell> GetSurroundingEntities(int x, int y)
        {
            List<Cell> surrounding = GetSurrounding(x, y);
            return surrounding.Where(c => c is Entity).ToList();
        }

        //Gets the surrounding 8 cells of the given cell
        public List<Cell> GetSurroundingBlanks(int x, int y)
        {
            List<Cell> surrounding = GetSurrounding(x, y);
            return surrounding.Where(c => c is not Entity).ToList();
        }

        public List<List<Cell>> CloneCells()
        {
            List<List<Cell>> newCells = new List<List<Cell>>();

            for (int y = 0; y < Height; y++)
            {
                List<Cell> row = new List<Cell>();

                for (int x = 0; x < Width; x++)
                {
                    row.Add(Cells[y][x]);
                }

                newCells.Add(row);
            }

            return newCells;
        }
    }
}
