using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day10
{
    static class Program
    {
        static Dictionary<(int inputJolts, int previouslyConsideredAdapters), long> previouslyComputedPaths = new ();
        static List<int> adapters;

        static void Main(string[] args)
        {
            adapters = File.ReadAllLines("input.txt").Select(l => int.Parse(l)).ToList();
            adapters.Add(0);
            adapters.Sort();
            adapters.Add(adapters.Last()+3);
            var items = new List<Item>();
            var possibilities = new List<int>();
            var runs = new List<int>();            

            for(int i = 1; i < adapters.Count -1; i++)
            {
                if(adapters[i-1] + 1 == adapters[i] && adapters[i+1] - 1 == adapters[i])
                {
                    possibilities.Add(adapters[i]);
                }
            }

            for(int index = 1; index < adapters.Count; index++)
            {
                var item = new Item(adapters[index-1], adapters[index]);
                items.Add(item);
            }

            var differencesOfThree = items.Count(i => i.Delta == 3);
            var differncesOfOne = items.Count(i => i.Delta == 1);
            Console.WriteLine($"Part1 = {differencesOfThree * differncesOfOne}");

            var part2 = validCombinationsFromHere(0, 0)/2;

            Console.WriteLine($"Part2: {part2}");
        }

        static long validCombinationsFromHere(int inputJolts, int previouslyConsideredAdapters)
        {
            var myArgs = (inputJolts, previouslyConsideredAdapters);
            if (previouslyComputedPaths.ContainsKey(myArgs))
                return previouslyComputedPaths[myArgs];

            if(adapters[previouslyConsideredAdapters] - inputJolts > 3)
                return 0;//no good
            
            if(previouslyConsideredAdapters == adapters.Count - 1)//penultimate
                return 1;

            long returnVal =
                validCombinationsFromHere(inputJolts, previouslyConsideredAdapters+1) + 
                validCombinationsFromHere(adapters[previouslyConsideredAdapters], previouslyConsideredAdapters+1);
            previouslyComputedPaths[(inputJolts, previouslyConsideredAdapters)] = returnVal;
            return returnVal;
        }
    }



    public record Item(int Prev, int Next)
    {
        public int Delta => Next - Prev;
    }
}
