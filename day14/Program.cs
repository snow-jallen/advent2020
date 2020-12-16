using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            Registers = new ();

            foreach(var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    Mask = line.Substring(7);
                    continue;
                }

                var parts = line.Split(new char[] { '[', ']', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var address = int.Parse(parts[1]);
                var value = int.Parse(parts[2]);

                var maskedAddress = Convert.ToString(address, 2).PadLeft(Mask.Length, '0').ToArray();
                for(int i = 0; i < Mask.Length; i++)
                {
                    if (Mask[i] == '0')
                        continue;
                    maskedAddress[i] = Mask[i];
                }
                var addresses2 = expand(new String(maskedAddress));

                var addresses = new List<string>();
                addresses.Add("");
                for(int i = 0; i < Mask.Length; i++)
                {
                    if (maskedAddress[i] == 'X')
                    {
                        var copy = addresses.ToArray().ToList();
                        addresses.AppendOnAll("0");
                        copy.AppendOnAll("1");
                        addresses.AddRange(copy);
                    }
                    else
                    {
                        addresses.AppendOnAll(maskedAddress[i].ToString());
                    }
                }

                foreach (var floatedAddress in addresses)
                {
                    var offset = Convert.ToInt64(floatedAddress, 2);
                    Registers[offset] = value;
                }

                //Registers[offset] = Convert.ToInt64(new String(strVal), 2);
            }

        }

        private List<string> expand(string maskedAddress)
        {
            var possibilities = new List<string>();
            visitRemaining("");

            void visitRemaining(string partial)
            {
                if (partial.Length == maskedAddress.Length)
                {
                    possibilities.Add(partial);
                    return;
                }

                var current = maskedAddress[partial.Length];
                if(current == 'X')
                {
                    visitRemaining(partial + "0");
                    visitRemaining(partial + "1");
                }
                else
                {
                    visitRemaining(partial + current);
                }
            }
            return possibilities;
        }

        public string Mask { get; }
        public IEnumerable<Instruction> Instructions { get; private set; }
        public int Result { get; private set; }
        public Dictionary<long, long> Registers { get; }


    }

    public record Instruction(int offset, int value);

    public static class Extensions
    {
        public static void AppendOnAll(this List<string> list, string valueToAppend)
        {
            for (int i = 0; i < list.Count; i++)
                list[i] += valueToAppend;
        }
    }
}
