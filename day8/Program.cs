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
            var visited = new List<int>();
            var current = 0;
            var acc = 0;
            while(visited.Contains(current) is false)
            {
                visited.Add(current);
                var parts = lines[current].Split(' ');
                var cmd = parts[0];                
                var val = int.Parse(parts[1]);
                switch(cmd)
                {
                    case "nop":
                        current ++;
                        continue;
                    case "jmp":
                        current += val;
                        continue;
                    case "acc":
                        acc += val;
                        current ++;
                        continue;
                }
            }
            Console.WriteLine($"last val of acc is {acc}");
        }
    }
}
