using System;
using System.IO;
using System.Linq;

namespace day9
{
    class Program
    {
        const int PreviousLimit = 5;
        const int PreambleSize = 5;
        static void Main(string[] args)
        {
            var sample = new Scenario(5, 5, File.ReadAllLines("sample.txt").Select(l=>long.Parse(l)).ToArray());
            var final = new Scenario(25, 25, File.ReadAllLines("input.txt").Select(l=>long.Parse(l)).ToArray());

            var scenario = final;
            var (part1, index) = findFirstNumberDoesntFit(scenario);
            Console.WriteLine($"The first number that doesn't fit is {part1}");

            var part2 = findPart2(scenario, index);
            Console.WriteLine($"part 2 {part2}");
        }

        private static long findPart2(Scenario scenario, int index)
        {
            var invalidNumber = scenario.Numbers[index];
            for(var start = 0; start < scenario.Numbers.Length - 2; start++)
            {
                for(var stop = 2; stop < scenario.Numbers.Length; stop++)
                {
                    var range = scenario.Numbers.Skip(start).Take(stop-start);
                    if(range.Sum() == invalidNumber)
                    {
                        return range.Min() + range.Max();
                    }
                }
            }
            return -1;
        }

        private static (long, int) findFirstNumberDoesntFit(Scenario scenario)
        {
            for(int index = scenario.PreambleSize; index < scenario.Numbers.Length; index++)
            {
                var sum = from one in scenario.Numbers.Skip(index - scenario.PreambleSize).Take(scenario.PreviousLimit)
                          from two in scenario.Numbers.Skip(index - scenario.PreambleSize).Take(scenario.PreviousLimit)
                          where one + two == scenario.Numbers[index] && one != two
                          select new 
                            {
                                Index = index,
                                Number = scenario.Numbers[index],
                                One = one,
                                Two = two
                            };
                if(!sum.Any())
                    return (scenario.Numbers[index], index);
            }
            return (-1,-1);
        }
    }

    public record Scenario(int PreviousLimit, int PreambleSize, long[] Numbers);
}
