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

            var part1 = findFirstNumberDoesntFit(final);
            Console.WriteLine($"The first number that doesn't fit is {part1}");
        }

        private static long findFirstNumberDoesntFit(Scenario scenario)
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
                    return scenario.Numbers[index];
            }
            return -1;
        }
    }

    public record Scenario(int PreviousLimit, int PreambleSize, long[] Numbers);
}
