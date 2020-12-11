using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text;

namespace day11
{
    class Program
    {
        const string sample_gen1 = @"
#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##";

    const string sample_gen2 = @"
#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##";

    const string sample_gen3 = @"
#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##";

    const string sample_gen4 = @"
#.#L.L#.##
#LLL#LL.L#
L.L.L..#..
#LLL.##.L#
#.LL.LL.LL
#.LL#L#.##
..L.L.....
#L#LLLL#L#
#.LLLLLL.L
#.#L#L#.##";

    const string sample_gen5 = @"
#.#L.L#.##
#LLL#LL.L#
L.#.L..#..
#L##.##.L#
#.#L.LL.LL
#.#L#L#.##
..L.L.....
#L#L##L#L#
#.LLLLLL.L
#.#L#L#.##";

        public static Dictionary<(int row, int col), bool?> board = new ();

        static void Main(string[] args)
        {
            List<(int row, int col, bool? state)> initialState = new ();
            var items = File.ReadAllLines("sample.txt");
            for(int row = 0; row < items.Length; row++)
            {
                var seats = items[row];
                for(int col = 0; col < seats.Length; col++)
                {
                    initialState.Add((row, col, seats[col] switch 
                    {
                        'L' => false,
                        _ => null
                    }));
                }
            }
            var initialGeneration = Generation.Initialize(initialState);
            var prevGen = initialGeneration;
            Generation finalGen;
            while(true)
            {
                var nextGen = new Generation(prevGen);

                Console.Write($"{nextGen.GenerationNumber}, ");

                if(nextGen.IsSameAs(prevGen))
                {
                    finalGen = nextGen;
                    break;
                }
                prevGen = nextGen;
            }
            Console.WriteLine($"Part1: {finalGen.OccupiedCount}");
        }
    }

    public class Generation
    {
        private static int generationCount = 0;
        public int GenerationNumber{get; init;}

        public static Generation Initialize(List<(int row, int col, bool? state)> initialState)
        {
            var firstGeneration = new Generation();
            foreach(var (row, col, state) in initialState)
            {
                firstGeneration.board[(row, col)] = state;
            }
            return firstGeneration;
        }

        public bool IsSameAs(Generation other) => 
            OccupiedCount == other.OccupiedCount && board.SequenceEqual(other.board);

        public int OccupiedCount => 
            board.Values.Count(v => v.HasValue && v.Value);

        private bool getValue((int row, int col) cell)
        {
            if(board.ContainsKey(cell))
                return board[cell] == true;
            return false;
        }

        private int getNeighborCount((int row, int col) cell )
        {
            var neighbors = new []{
                getValue((cell.row+1, cell.col-1)),
                getValue((cell.row+1, cell.col+0)),
                getValue((cell.row+1, cell.col+1)),
                getValue((cell.row, cell.col - 1)),
                getValue((cell.row, cell.col + 1)),
                getValue((cell.row-1, cell.col-1)),
                getValue((cell.row-1, cell.col+0)),
                getValue((cell.row-1, cell.col+1))                
            };
            var neighborCount = neighbors.Count(n => n == true);
            return neighborCount;
        }

        private int getVisibleNeighborCount((int row, int col) cell)
        {
            var visibleNeighbors = 0;
            if(visibleNeighbor(cell, -1, -1))
                visibleNeighbors++;
            if(visibleNeighbor(cell, -1, 0))
                visibleNeighbors++;
            if(visibleNeighbor(cell, -1, +1))
                visibleNeighbors++;
            if(visibleNeighbor(cell, 0, -1))
                visibleNeighbors++;
            if(visibleNeighbor(cell, 0, +1))
                visibleNeighbors++;
            if(visibleNeighbor(cell, +1, -1))
                visibleNeighbors++;
            if(visibleNeighbor(cell, +1, 0))
                visibleNeighbors++;
            if(visibleNeighbor(cell, +1, +1))
                visibleNeighbors++;
            return visibleNeighbors;
        }

        private bool visibleNeighbor((int row, int col) cell, int deltaRow, int deltaCol)
        {
            var testRow = cell.row;
            var testCol = cell.col;

            while(testRow >= 0 && testRow < maxRow && testCol >= 0 && testCol < maxCol)
            {
                if(board[(testRow, testCol)] == true)
                    return true;
                testRow += deltaRow;
                testCol += deltaCol;
            }
            return false;
        }
        
        int maxRow;
        int maxCol;

        //private ConcurrentDictionary<(int row, int col), bool?> board = new ();
        private Dictionary<(int row, int col), bool?> board = new ();

        private Generation()
        {
            GenerationNumber = generationCount++;
        }

        public Generation(Generation prev) : this()
        {
            maxRow = prev.board.Keys.Max(c => c.row);
            maxCol = prev.board.Keys.Max(c => c.col);
            //Parallel.ForEach(prev.board.Keys, cell =>
            foreach(var cell in prev.board.Keys)
            {
                var neighborCount = prev.getVisibleNeighborCount(cell);
                if(prev.board[cell] == false && neighborCount == 0) //was empty
                    board[cell] = true;
                else if(prev.board[cell] == true && neighborCount >= 5) //occupied w/4 neighbors
                    board[cell] = false;
                else
                    board[cell] = prev.board[cell];
            //});            
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            for(var row = 0; row < maxRow; row++)
            {
                var cells = board.Keys.Where(c => c.row == row).OrderBy(c => c.col);
                foreach(var cell in cells)
                {
                    str.Append(board[cell] switch
                    {
                        true => '#',
                        false => 'L',
                        _ => '.'
                    });
                }
                str.AppendLine();
            }
            return str.ToString();
        }

    }
}
