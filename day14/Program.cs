using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program(File.ReadAllLines("input.txt"));
            Console.WriteLine($"{p.Registers.Count()} distinct registers w/sum of {p.Registers.Values.Sum()}");
        }

        public Program(string[] lines)
        {
            Registers = new Dictionary<int, long>();

            foreach(var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    Mask = line.Substring(7);
                    continue;
                }

                var parts = line.Split(new char[] { '[', ']', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var offset = int.Parse(parts[1]);
                var value = int.Parse(parts[2]);

                var strVal = Convert.ToString(value, 2).PadLeft(Mask.Length, '0').ToArray();
                for(int i = 0; i < Mask.Length; i++)
                {
                    if (Mask[i] == 'X')
                        continue;
                    strVal[i] = Mask[i];
                }
                Registers[offset] = Convert.ToInt64(new String(strVal), 2);
            }

        }

        public string Mask { get; }
        public IEnumerable<Instruction> Instructions { get; private set; }
        public int Result { get; private set; }
        public Dictionary<int, long> Registers { get; }

    }

    public record Instruction(int offset, int value);
}
