using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var adapters = File.ReadAllLines("input.txt").Select(l => int.Parse(l)).ToList();
            adapters.Sort();
            var items = new List<Item>();

            items.Add(new Item(0, adapters[0]));
            for(int index = 1; index < adapters.Count; index++)
            {
                var item = new Item(adapters[index-1], adapters[index]);
                items.Add(item);
            }
            items.Add(new Item(adapters.Last(), adapters.Last()+3));

            var differencesOfThree = items.Count(i => i.Delta == 3);
            var differncesOfOne = items.Count(i => i.Delta == 1);

            Console.WriteLine($"Part1 = {differencesOfThree * differncesOfOne}");
        }
    }

    public record Item(int Prev, int Next)
    {
        public int Delta => Next - Prev;
    }
}
