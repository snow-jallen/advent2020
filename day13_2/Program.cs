using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var arrivalTime = int.Parse(lines[0]);
            var busIds = (from item in lines[1].Split(',')
                          where item != "x"
                          select int.Parse(item)).ToList();
            var multiples = (from bus in busIds
                             let x = arrivalTime / bus
                             select new
                             {
                                 BusID = bus,
                                 NextArrival = x * bus + bus
                             }).ToList();
            var soonest = multiples.OrderBy(m => m.NextArrival).First();
            var part1 = soonest.BusID * (soonest.NextArrival - arrivalTime);

            System.Diagnostics.Debug.Assert(part2("17,x,13,19") == 3417);
            System.Diagnostics.Debug.Assert(part2("67,7,59,61") == 754018);
            System.Diagnostics.Debug.Assert(part2("67,x,7,59,61") == 779210);
            System.Diagnostics.Debug.Assert(part2("67,7,x,59,61") == 1261476);
            System.Diagnostics.Debug.Assert(part2("1789,37,47,1889") == 1202161486);
            Console.WriteLine($"Part2 = {part2(lines[1], 110649000000000, shouldExit: true)}");

            Console.WriteLine("you did it!");
        }

        private static IEnumerable<long> getRange(long starting, long increment)
        {
            while(true)
                yield return starting += increment;
        } 

        private static long part2(string timeString, long startingNum = 1, bool shouldExit = false)
        {
            var index = 0;
            var busses = (from item in timeString.Split(',')
                            select new
                            {
                                Matters = item != "x",
                                BusId = int.TryParse(item, out int b) ? b : -1,
                                Index = index++
                            })
                            .Where( b => b.Matters)
                            .ToList();

            var startingBus = busses.OrderByDescending(b => b.BusId).First();
            //var startingBus = busses.First();
            while(startingNum % startingBus.BusId != 0)
                startingNum++;

            ConcurrentBag<long> finalNumbers = new ();
            Parallel.ForEach(getRange(startingNum - startingBus.Index, startingBus.BusId), (t, state) =>
            {
                if(t % 100_000_000 == 0)
                    Console.WriteLine($"{t}");
                if(busses.All(b => (t + b.Index) % b.BusId == 0))
                {
                    Console.WriteLine($"It works at {t}!");
                    finalNumbers.Add(t);
                    state.Stop();
                }
            });
            return finalNumbers.Min();
        }
    }
}
