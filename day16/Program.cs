using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var rules = (from line in lines
                         let parts = line.Split(new[] { ":", " or ", "-" }, StringSplitOptions.RemoveEmptyEntries)
                        where parts.Length == 5
                        let ruleName = parts[0]
                        select new Rule(ruleName, int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]))).ToList();

            var myTicket = new Ticket(lines.Skip(rules.Count + 2).First());

            var nearbyTickets = from line in lines.Skip(rules.Count + 5)
                                select new Ticket(line);

            var fields = new List<Field>();
            for(int fieldNum = 0; fieldNum < myTicket.Values.Length; fieldNum++)
            {
                var values = nearbyTickets.Select(t => t.Values[fieldNum]);
                fields.Add(new Field(fieldNum, values.Max(), values.Min(), values.ToArray()));
            }

            var invalidFields = new List<int>();
            foreach(var ticket in nearbyTickets)
            {
                foreach(var value in ticket.Values)
                {
                    if (rules.Any(r => r.IsInRange(value)) is false)
                        invalidFields.Add(value);
                }
            }
        }
    }

    public record Rule(string ruleName, int min1, int max1, int min2, int max2)
    {
        public bool IsInRange(int testNum)
        {
            return (testNum >= min1 && testNum <= max1) || (testNum >= min2 && testNum <= max2);
        }
    }
    public record Field(int position, int max, int min, int[] values);
    public class Ticket
    {
        public Ticket(string csv)
        {
            Values = csv.Split(',')
                        .Select(s => int.Parse(s))
                        .ToArray();
        }

        public int[] Values { get; }
    }
}
