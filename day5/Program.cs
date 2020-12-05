using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var partitions = from l in File.ReadAllLines("input.txt")
                             select new SeatPartition(l);
            Console.WriteLine($"Max seat id is {partitions.Max(p => p.Id)}");

            int lastId = 1;
            foreach (var seat in partitions.OrderBy(p => p.Row).ThenBy(p => p.Col))
            {
                if (seat.Id != lastId + 1)
                {
                    Console.WriteLine($"Is your seat #{lastId + 1}?");
                }
                lastId = seat.Id;
            }
        }
    }

    public class SeatPartition
    {
        public SeatPartition(string code)
        {
            int rowLow = 0;
            int rowHigh = 127;
            int colLow = 0;
            int colHigh = 7;

            foreach (var c in code)
            {
                var rowDelta = (rowHigh - rowLow + 1) / 2;
                var colDelta = (colHigh - colLow + 1) / 2;
                if (c == 'F')
                {
                    rowHigh -= rowDelta;
                }
                else if (c == 'B')
                {
                    rowLow += rowDelta;
                }
                else if (c == 'L')
                {
                    colHigh -= colDelta;
                }
                else if (c == 'R')
                {
                    colLow += colDelta;
                }
            }
            Row = rowLow;
            Col = colLow;
        }

        public int Row { get; private set; }
        public int Col { get; private set; }
        public int Id => Row * 8 + Col;
        public override string ToString() => $"({Row},{Col}) = {Id}";
    }
}
