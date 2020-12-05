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
            var lines = File.ReadAllLines("input.txt");
            var partitions = from l in lines
                select new SeatPartition(l);
            Console.WriteLine($"Max seat id is {partitions.Max(p => p.Id)}");
            var ordered = partitions.OrderBy(p => p.Row).ThenBy(p => p.Col).ToArray();

            int lastId = 0;
            foreach(var seat in ordered)
            {
                if(seat.Id != lastId + 1)
                {
                    Console.WriteLine($"Was it {lastId + 1}?");
                }
                lastId = seat.Id;
            }
        }
    }

    public class SeatPartition
    {
        const int MinRow = 0;
        const int MaxRow = 127;
        const int MinCol = 0;
        const int MaxCol = 7;

        public SeatPartition(string code)
        {
            int rowLow=MinRow;
            int rowHigh = MaxRow;
            int colLow = MinCol;
            int colHigh = MaxCol;

            foreach(var c in code)
            {
                var rowDelta = (rowHigh - rowLow + 1) / 2;
                var colDelta = (colHigh - colLow + 1) / 2;
                if(c == 'F')
                {
                    rowHigh-=rowDelta;
                }
                else if(c == 'B')
                {
                    rowLow += rowDelta;
                }
                else if(c == 'L')
                {
                    colHigh -= colDelta;
                }
                else if(c == 'R')
                {
                    colLow += colDelta;
                }
            }
            Row = rowLow;
            Col = colLow;
        }

        public int Row{get; private set;}
        public int Col{get; private set;}
        public int Id => Row * 8 + Col;
        public override string ToString() => $"({Row},{Col}) = {Id}";
    }
}
