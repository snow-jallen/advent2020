using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace day17
{
    class Program
    {

        static void Main(string[] args)
        {
            Dictionary<Cell, bool> initial = new();
            string[] lines = File.ReadAllLines("input.txt");
            int z = 0;
            int w = 0;
            for (int y = 1; y <= lines.Length; y++)
            {
                string line = lines[lines.Length - y];
                for (int x = 1; x <= line.Length; x++)
                {
                    char cell = line[x - 1];
                    initial.Add(new Cell(x, y, z, w), cell == '#');
                }
            }
            var currentLive = GetTotalLive(initial);
            Console.WriteLine(PrintBoard(initial));

            var gen1 = DoGeneration(initial);
            var gen1Alive = GetTotalLive(gen1);
            Console.WriteLine(PrintBoard(gen1));

            var generations = new Dictionary<Cell, bool>[7];
            generations[0] = initial;

            for (int gen = 1; gen <= 6; gen++)
            {
                generations[gen] = DoGeneration(generations[gen - 1]);
            }
            var liveCount = GetTotalLive(generations[6]);
        }

        public static string PrintBoard(Dictionary<Cell, bool> board)
        {
            var maxX = board.Keys.Max(k => k.x);
            int minY = board.Keys.Min(k => k.y);
            var maxZ = board.Keys.Max(k => k.z);
            var maxW = board.Keys.Max(k => k.w);

            var str = new StringBuilder($"\n\n{GetTotalLive(board)} alive in this generation\n");
            for (int w = board.Keys.Min(k => k.w); w <= maxW; w++)
            {
                str.AppendLine($"w={w}");
                for (int z = board.Keys.Min(k => k.z); z <= maxZ; z++)
                {
                    str.AppendLine($"z={z}");
                    for (int y = board.Keys.Max(k => k.y); y >= minY; y--)
                    {
                        for (int x = board.Keys.Min(k => k.x); x <= maxX; x++)
                        {
                            var cell = new Cell(x, y, z, w);
                            str.Append(board.ContainsKey(cell) && board[cell] ? '#' : '.');
                        }
                        str.AppendLine();
                    }
                    str.AppendLine();
                }
                str.AppendLine();
            }
            return str.ToString();
        }

        public static IEnumerable<Cell> GetNeighbors(Cell cell)
        {
            return new[]{
                new Cell(cell.x-1, cell.y-1, cell.z-1, cell.w-1),//top left  //in front
                new Cell(cell.x-0, cell.y-1, cell.z-1, cell.w-1),//top center
                new Cell(cell.x+1, cell.y-1, cell.z-1, cell.w-1),//top right
                new Cell(cell.x-1, cell.y-0, cell.z-1, cell.w-1),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z-1, cell.w-1),//middle center
                new Cell(cell.x+1, cell.y-0, cell.z-1, cell.w-1),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z-1, cell.w-1),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z-1, cell.w-1),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z-1, cell.w-1),//bottom right                
                new Cell(cell.x-1, cell.y-1, cell.z-0, cell.w-1),//top left  //center
                new Cell(cell.x-0, cell.y-1, cell.z-0, cell.w-1),//top center
                new Cell(cell.x+1, cell.y-1, cell.z-0, cell.w-1),//top right
                new Cell(cell.x-1, cell.y-0, cell.z-0, cell.w-1),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z-0, cell.w-1),//middle center is the current cell
                new Cell(cell.x+1, cell.y-0, cell.z-0, cell.w-1),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z-0, cell.w-1),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z-0, cell.w-1),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z-0, cell.w-1),//bottom right                
                new Cell(cell.x-1, cell.y-1, cell.z+1, cell.w-1),//top left  //behind
                new Cell(cell.x-0, cell.y-1, cell.z+1, cell.w-1),//top center
                new Cell(cell.x+1, cell.y-1, cell.z+1, cell.w-1),//top right
                new Cell(cell.x-1, cell.y-0, cell.z+1, cell.w-1),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z+1, cell.w-1),//middle center
                new Cell(cell.x+1, cell.y-0, cell.z+1, cell.w-1),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z+1, cell.w-1),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z+1, cell.w-1),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z+1, cell.w-1),//bottom right
                
                new Cell(cell.x-1, cell.y-1, cell.z-1, cell.w-0),//top left  //in front
                new Cell(cell.x-0, cell.y-1, cell.z-1, cell.w-0),//top center
                new Cell(cell.x+1, cell.y-1, cell.z-1, cell.w-0),//top right
                new Cell(cell.x-1, cell.y-0, cell.z-1, cell.w-0),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z-1, cell.w-0),//middle center
                new Cell(cell.x+1, cell.y-0, cell.z-1, cell.w-0),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z-1, cell.w-0),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z-1, cell.w-0),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z-1, cell.w-0),//bottom right                
                new Cell(cell.x-1, cell.y-1, cell.z-0, cell.w-0),//top left  //center
                new Cell(cell.x-0, cell.y-1, cell.z-0, cell.w-0),//top center
                new Cell(cell.x+1, cell.y-1, cell.z-0, cell.w-0),//top right
                new Cell(cell.x-1, cell.y-0, cell.z-0, cell.w-0),//middle left
                                                                 //middle center is the current cell
                new Cell(cell.x+1, cell.y-0, cell.z-0, cell.w-0),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z-0, cell.w-0),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z-0, cell.w-0),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z-0, cell.w-0),//bottom right                
                new Cell(cell.x-1, cell.y-1, cell.z+1, cell.w-0),//top left  //behind
                new Cell(cell.x-0, cell.y-1, cell.z+1, cell.w-0),//top center
                new Cell(cell.x+1, cell.y-1, cell.z+1, cell.w-0),//top right
                new Cell(cell.x-1, cell.y-0, cell.z+1, cell.w-0),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z+1, cell.w-0),//middle center
                new Cell(cell.x+1, cell.y-0, cell.z+1, cell.w-0),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z+1, cell.w-0),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z+1, cell.w-0),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z+1, cell.w-0),//bottom right

                new Cell(cell.x-1, cell.y-1, cell.z-1, cell.w+1),//top left  //in front
                new Cell(cell.x-0, cell.y-1, cell.z-1, cell.w+1),//top center
                new Cell(cell.x+1, cell.y-1, cell.z-1, cell.w+1),//top right
                new Cell(cell.x-1, cell.y-0, cell.z-1, cell.w+1),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z-1, cell.w+1),//middle center
                new Cell(cell.x+1, cell.y-0, cell.z-1, cell.w+1),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z-1, cell.w+1),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z-1, cell.w+1),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z-1, cell.w+1),//bottom right                
                new Cell(cell.x-1, cell.y-1, cell.z-0, cell.w+1),//top left  //center
                new Cell(cell.x-0, cell.y-1, cell.z-0, cell.w+1),//top center
                new Cell(cell.x+1, cell.y-1, cell.z-0, cell.w+1),//top right
                new Cell(cell.x-1, cell.y-0, cell.z-0, cell.w+1),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z-0, cell.w+1),//middle center is the current cell
                new Cell(cell.x+1, cell.y-0, cell.z-0, cell.w+1),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z-0, cell.w+1),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z-0, cell.w+1),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z-0, cell.w+1),//bottom right                
                new Cell(cell.x-1, cell.y-1, cell.z+1, cell.w+1),//top left  //behind
                new Cell(cell.x-0, cell.y-1, cell.z+1, cell.w+1),//top center
                new Cell(cell.x+1, cell.y-1, cell.z+1, cell.w+1),//top right
                new Cell(cell.x-1, cell.y-0, cell.z+1, cell.w+1),//middle left
                new Cell(cell.x-0, cell.y-0, cell.z+1, cell.w+1),//middle center
                new Cell(cell.x+1, cell.y-0, cell.z+1, cell.w+1),//middle right
                new Cell(cell.x-1, cell.y+1, cell.z+1, cell.w+1),//bottom left
                new Cell(cell.x-0, cell.y+1, cell.z+1, cell.w+1),//bottom center
                new Cell(cell.x+1, cell.y+1, cell.z+1, cell.w+1),//bottom right
            };
        }

        private static int GetTotalLive(Dictionary<Cell, bool> current)
        {
            return current.Values.Where(isAlive => isAlive).Count();
        }

        public static int LiveNeighborCount(Cell cell, Dictionary<Cell, bool> board) =>
            GetNeighbors(cell).Count(c => board.ContainsKey(c) && board[c] == true);

        public static bool ShouldLive(Cell cell, Dictionary<Cell, bool> board)
        {
            var isAlive = board.ContainsKey(cell) && board[cell];
            var neighbors = LiveNeighborCount(cell, board);
            return (isAlive && (neighbors is 2 or 3)) || (!isAlive && neighbors == 3);
        }

        public static Dictionary<Cell, bool> DoGeneration(Dictionary<Cell, bool> currentGeneration)
        {
            var nextGeneration = new Dictionary<Cell, bool>();

            var currentLiveCells = currentGeneration.Keys.Where(k => currentGeneration[k] == true);
            foreach (var cell in currentLiveCells)
            {
                var neighbors = GetNeighbors(cell);
                foreach (var neighbor in neighbors)
                {
                    if (ShouldLive(neighbor, currentGeneration))
                        nextGeneration[neighbor] = true;
                }
            }

            return nextGeneration;
        }
    }

    public record Cell(int x, int y, int z, int w);
}
