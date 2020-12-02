using static System.Console;
using System.IO;
using System.Linq;

var entries = from l in File.ReadAllLines("input.txt")
              select int.Parse(l);

var part1 = from e1 in entries
            from e2 in entries
            where e1 + e2 == 2020
            select e1 * e2;

var part2 = from e1 in entries
            from e2 in entries
            from e3 in entries
            where e1 + e2 + e3 == 2020
            select e1 * e2 * e3;

WriteLine($"Part 1 {part1.First()}; Part 2 {part2.First()}");
