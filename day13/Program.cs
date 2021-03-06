﻿using System;
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

            Console.WriteLine($"Part2 = {part2(lines[1])}");
            System.Diagnostics.Debug.Assert(part2("17,x,13,19") == 3417);
            System.Diagnostics.Debug.Assert(part2("67,7,59,61") == 754018);
            System.Diagnostics.Debug.Assert(part2("67,x,7,59,61") == 779210);
            System.Diagnostics.Debug.Assert(part2("67,7,x,59,61") == 1261476);
            System.Diagnostics.Debug.Assert(part2("1789,37,47,1889") == 1202161486);

            Console.WriteLine("you did it!");
        }

        private static IEnumerable<long> getRange(long starting, long increment)
        {
            while(true)
                yield return starting += increment;
        } 

        private static long part2(string timeString)
        {
            var index = 0;
            var busTimes = (from item in timeString.Split(',')
                            select new
                            {
                                WhoCares = item == "x",
                                Matters = item != "x",
                                BusId = int.TryParse(item, out int b) ? b : -1,
                                Index = index++
                            }).ToList();
            var bussesThatMatter = (from b in busTimes
                                    where b.Matters
                                    select b).ToList();

            long startingNum = 100000000000000L;
            while(startingNum % busTimes[0].BusId != 0)
                startingNum++;
            // for (long t = startingNum; ; t+=bussesThatMatter[0].BusId)
            // {
            //     if(t % 1_000_000 == 0)
            //         Console.WriteLine($"{t/1000000:n0}");
            //     if(bussesThatMatter.All(b => (t + b.Index) % b.BusId == 0))
            //     {
            //         Console.WriteLine($"It works at {t}?!");
            //         return t;
            //     }

            // }

            Parallel.ForEach(getRange(startingNum, bussesThatMatter[0].BusId), t =>
            {
                if(t % 1_000_000 == 0)
                    Console.WriteLine($"{t/1000000:n0}");
                if(bussesThatMatter.All(b => (t + b.Index) % b.BusId == 0))
                {
                    Console.WriteLine($"It works at {t}!");
                    Environment.Exit(0);
                }
            });
            return 0L;

            // Parallel.For(startingNum, long.MaxValue, (t => {
            //     if(t % 1_000_000 == 0)
            //         Console.WriteLine($"{t/1000000:n0}, ");
            //     if(bussesThatMatter.All(b => (t + b.Index) % b.BusId == 0))
            //     {
            //         Console.WriteLine($"It works at {t}?!");
            //         return t;
            //     }
            // }));
        }
    }
}
