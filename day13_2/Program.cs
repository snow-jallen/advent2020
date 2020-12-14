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
            Console.WriteLine($"Part2 = {part2(lines[1], 329898300000000, shouldExit: true)}");
        }

        private static IEnumerable<long> getRange(long starting, long increment)
        {
            while (true)
                yield return starting += increment;
        }

        private static ulong part2(string timeString, long startingNum = 1, bool shouldExit = false)
        {
            var index = 0;
            var busses = (from item in timeString.Split(',')
                          select new
                          {
                              Matters = item != "x",
                              BusId = int.TryParse(item, out int b) ? b : -1,
                              Index = index++
                          })
                            .Where(b => b.Matters)
                            .ToList();

            var a = busses.Select(b => (ulong)b.BusId).ToArray();
            var n = busses.Select(b => (ulong)b.BusId - (ulong)b.Index).ToArray();
            return ChineseRemainderTheorem.Solve(a, n);

            //my brute-force attempt:

            var startingBus = busses.OrderByDescending(b => b.BusId).First();
            //var startingBus = busses.First();
            while (startingNum % startingBus.BusId != 0)
                startingNum++;

            ConcurrentBag<long> finalNumbers = new();
            Parallel.ForEach(getRange(startingNum - startingBus.Index, startingBus.BusId), (t, state) =>
            {
                if (t % 100_000_000 == 0)
                    Console.WriteLine($"{t}");
                if (busses.All(b => (t + b.Index) % b.BusId == 0))
                {
                    Console.WriteLine($"It works at {t}!");
                    finalNumbers.Add(t);
                    state.Stop();
                }
            });
            var myAns = finalNumbers.Min();

            return (ulong)myAns;
        }
    }

    public static class ChineseRemainderTheorem
    {
        public static ulong Solve(ulong[] n, ulong[] a)
        {
            try
            {
                ulong prod = n.Aggregate((ulong)1, (i, j) => i * j);
                ulong p;
                ulong sm = 0;
                for (ulong i = 0; i < (ulong)n.LongLength; i++)
                {
                    p = (ulong)prod / (ulong)n[i];
                    sm += (ulong)a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
                }
                return sm % (ulong)prod;
            }
            catch
            {
                return (ulong)0;
            }
        }

        private static ulong ModularMultiplicativeInverse(ulong a, ulong mod)
        {
            ulong b = a % (ulong)mod;
            for (ulong x = 1; x < (ulong)mod; x++)
            {
                if ((b * x) % (ulong)mod == 1)
                {
                    return x;
                }
            }
            return 1;
        }
    }

    public class GFG
    {
        // Returns modulo inverse of
        // 'a' with respect to 'm'
        // using extended Euclid Algorithm.
        // Refer below post for details:
        // https://tutorialspoint.dev/slugresolver/multiplicative-inverse-under-modulo-m/
        static ulong inv(ulong a, ulong m)
        {
            ulong m0 = m, t, q;
            ulong x0 = 0, x1 = 1;

            if (m == 1)
                return 0;

            // Apply extended
            // Euclid Algorithm
            while (a > 1)
            {
                // q is quotient
                q = a / m;

                t = m;

                // m is remainder now,
                // process same as
                // euclid's algo
                m = a % m; a = t;

                t = x0;

                x0 = x1 - q * x0;

                x1 = t;
            }

            // Make x1 positive
            if (x1 < 0)
                x1 += m0;

            return x1;
        }

        // k is size of num[] and rem[].
        // Returns the smallest number
        // x such that:
        // x % num[0] = rem[0],
        // x % num[1] = rem[1],
        // ..................
        // x % num[k-2] = rem[k-1]
        // Assumption: Numbers in num[]
        // are pairwise coprime (gcd
        // for every pair is 1)
        public static ulong findMinX(int[] num,
                            int[] rem)
        {
            var k = num.Length;
            // Compute product
            // of all numbers
            ulong prod = 1;
            for (int i = 0; i < k; i++)
                prod *= (ulong)num[i];

            // Initialize result
            ulong result = 0;

            // Apply above formula
            for (int i = 0; i < k; i++)
            {
                ulong pp = prod / (ulong)num[i];
                result += (ulong)rem[i] *
                          inv(pp, (ulong)num[i]) * pp;
            }

            return result % prod;
        }
    }
}
