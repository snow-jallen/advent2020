using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace day7
{
    class Program
    {
        static Dictionary<string, List<ChildBag>> rules = new Dictionary<string, List<ChildBag>>();
        static List<List<string>> paths = new List<List<string>>();

        static void Main(string[] args)
        {
            foreach(var line in File.ReadAllLines("input.txt"))
            {
                var parts = line.Split(new[] { " ", ",", ".", "bags contain", "bags", "bag" }, StringSplitOptions.RemoveEmptyEntries);
                (var key, var children) = parseRule(parts.Select(p=>p.Trim()).ToArray());
                rules.Add(key, children);
            }

            var myColor = "shiny gold";
            foreach(var testColor in rules.Keys)
            {
                canContain(testColor, myColor, new List<string>(new[] { testColor }));
            }
            var uniqueStartingBags = paths.Select(p => p.First()).Except(new[] { myColor }).Distinct();

            Console.WriteLine($"Part 1: There are {uniqueStartingBags.Count()} paths for {myColor}");

            var bagCount = countBags(myColor, 0);
        }

        private static long countBags(string myColor, int tabCount)
        {
            Console.WriteLine($"{tabs(tabCount)}How about {myColor}?");
            var bagCount = 0L;
            if (rules[myColor].Count == 0)
            {
                Console.WriteLine($"{tabs(tabCount)}No child bags for {myColor}, returning 1");
                return 0;
            }

            foreach(var child in rules[myColor])
            {
                Console.WriteLine($"{tabs(tabCount)}Adding {child.Quantity} bags for {child.Color}; bagCount = {bagCount}");
                bagCount += child.Quantity;
                long childBagCount = countBags(child.Color, tabCount+1);
                Console.WriteLine($"{tabs(tabCount)}Found {childBagCount} for {child.Color} * {child.Quantity} => bagCount goes from {bagCount} to {bagCount + childBagCount * child.Quantity}");
                bagCount += childBagCount * child.Quantity;
            }

            Console.WriteLine($"{tabs(tabCount)}Returning bagCount {bagCount}");
            return bagCount;
        }

        private static string tabs(int tabCount)
        {
            var tabs = "";
            for (int i = 0; i < tabCount; i++)
                tabs += "\t";
            return tabs;
        }

        private static void canContain(string testColor, string myColor, List<string> path)
        {
            if (testColor == myColor)
            {
                paths.Add(path);
                return;
            }
            foreach(var child in rules[testColor])
            {
                var newPath = new List<string>(path);
                newPath.Add(child.Color);
                canContain(child.Color, myColor, newPath);
            }
        }

        private static (string, List<ChildBag>) parseRule(string[] parts)
        {
            var key = $"{parts[0]} {parts[1]}";
            var children = new List<ChildBag>();
            if (parts[2] != "no")
            {
                for (int i = 2; i < parts.Length; i += 3)
                {
                    children.Add(new ChildBag { Quantity = int.Parse(parts[i]), Color = $"{parts[i + 1]} {parts[i + 2]}" });
                }
            }
            return (key, children);
        }
    }

    public record ChildBag
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
    }
}
