using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").ToList();
            var possibleIndicies = new Queue<int>();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("jmp") || lines[i].StartsWith("nop"))
                {
                    possibleIndicies.Enqueue(i);
                }
            }

            var modifiedLines = lines.ToArray();
            while (possibleIndicies.Count >= 0)
            {
                if (runInstructions(modifiedLines) == 0)
                {
                    break;
                }
                modifiedLines = swapCommand(lines, possibleIndicies.Dequeue());
            }
        }

        private static string[] swapCommand(List<string> lines, int index)
        {
            var copy = lines.ToArray();
            if (copy[index].StartsWith("jmp"))
                copy[index] = copy[index].Replace("jmp", "nop");
            else
                copy[index] = copy[index].Replace("nop", "jmp");
            return copy;
        }

        private static int runInstructions(string[] lines)
        {
            var visited = new List<int>();
            var current = 0;
            var acc = 0;
            while (visited.Contains(current) is false)
            {
                visited.Add(current);
                var parts = lines[current].Split(' ');
                var cmd = parts[0];
                var val = int.Parse(parts[1]);
                switch (cmd)
                {
                    case "nop":
                        current++;
                        break;
                    case "jmp":
                        current += val;
                        break;
                    case "acc":
                        acc += val;
                        current++;
                        break;
                }

                if (current >= lines.Length)
                {
                    Console.WriteLine($"Success - finished the instructions!  Last val of acc is {acc}");
                    return 0;
                }
            }
            Console.WriteLine($"No good.  Infinite.  last val of acc is {acc}");
            return -1;
        }
    }
}
