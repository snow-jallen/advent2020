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

            var nearbyTickets = (from line in lines.Skip(rules.Count + 5)
                                select new Ticket(line)).ToList();

            var fields = new List<Field>();
            for(int fieldNum = 0; fieldNum < myTicket.Values.Length; fieldNum++)
            {
                var values = nearbyTickets.Select(t => t.Values[fieldNum]);
                fields.Add(new Field(fieldNum, values.Max(), values.Min(), values.ToArray()));
            }

            var invalidFields = new List<int>();
            foreach(var ticket in nearbyTickets)
            {
                ticket.IsValid = ticket.Values.All(v => rules.Any(r => r.IsInRange(v)));
                invalidFields.AddRange(ticket.Values.Where(v => rules.Any(r => r.IsInRange(v)) == false));
            }

            Console.WriteLine($"Part1: {invalidFields.Sum()}");

            var allTickets = nearbyTickets.Where(t=>t.IsValid).ToList();
            allTickets.Add(myTicket);

            foreach (var fieldNumber in Enumerable.Range(0, myTicket.Values.Length))
            {
                var allValues = allTickets.Select(t => t.Values[fieldNumber]);

                foreach(var rule in rules)
                {
                    if(rule.AreAllInRange(allValues))
                    {
                        rule.PossibleFieldNumbers.Add(fieldNumber);
                    }
                }
            }

            foreach(var rule in rules.OrderBy(r=>r.PossibleFieldNumbers.Count))
            {
                if(rule.PossibleFieldNumbers.Count == 1)
                {
                    var ruleNum = rule.PossibleFieldNumbers[0];
                    rule.ActualFieldNumber = ruleNum;
                    rules.Where(r => r.PossibleFieldNumbers.Contains(ruleNum))
                        .ToList()
                        .ForEach(r => r.PossibleFieldNumbers.Remove(ruleNum));
                }
            }

            if(rules.Any(r => r.ActualFieldNumber == null))
            {
                throw new Exception("unable to properly assign rules");
            }

            var departureFields = rules.Where(r => r.ruleName.Contains("departure"));
            var product = 1L;
            foreach(var field in departureFields)
            {
                Console.WriteLine($"{field.ruleName,-18}: position {field.ActualFieldNumber,2}; my ticket value={myTicket.Values[field.ActualFieldNumber.Value]}");
                product *= myTicket.Values[field.ActualFieldNumber.Value];
            }
            Console.WriteLine($"Part2: {product}");
        }
    }

    public record Rule(string ruleName, int min1, int max1, int min2, int max2)
    {
        public bool IsInRange(int testNum)
        {
            return (testNum >= min1 && testNum <= max1) || (testNum >= min2 && testNum <= max2);
        }

        public bool AreAllInRange(IEnumerable<int> values)
        {
            return values.All(v => IsInRange(v));
        }

        public List<int> PossibleFieldNumbers { get; } = new ();
        public int? ActualFieldNumber { get; set; } = null;
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
        public bool IsValid { get; set; }
        public int TicketNum { get; }
        public override string ToString() => $"{String.Join(", ", Values)} (Valid? {IsValid})";
    }
}
