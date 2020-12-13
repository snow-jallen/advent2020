using System;
using System.IO;
using System.Linq;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var arrivalTime = int.Parse(lines[0]);
            var busIds = (from item in lines[1].Split(',')
                         where item != "x"
                         select int.Parse(item)).ToList();
            var multiples = (from bus in busIds
                            let x = arrivalTime / bus
                            select new
                            {
                                BusID = bus,
                                NextArrival = x * bus + bus
                            }).ToList();
            var soonest = multiples.OrderBy(m => m.NextArrival).First();
            var part1 = soonest.BusID * (soonest.NextArrival - arrivalTime);

        }
    }
}
