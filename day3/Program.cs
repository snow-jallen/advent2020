using System;
using System.IO;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            long ans = testPath(1, 1) *
                        testPath(3, 1) *
                        testPath(5, 1) *
                        testPath(7, 1) *
                        testPath(1, 2);
            Console.WriteLine($"Part 2 is {ans}");
        }

        private static long testPath(int xMovement, int yMovement)
        {
            var lines = File.ReadAllLines("input.txt");
            var col = 0;
            long trees = 0;
            for (int row = 0; row < lines.Length; row += yMovement)
            {
                var l = lines[row];

                if (l[col] == '#')
                {
                    //Console.WriteLine($"\tRow {row} has a tree at position {col}");
                    trees++;
                }
                else
                {
                    //Console.WriteLine($"\tRow {row} is tree-free at {col}");
                }

                col += xMovement;

                if (col >= l.Length)
                {
                    //Console.WriteLine($"\t\tcol was {col} but that's longer than {l.Length} so I'm making it {col - l.Length}");
                    col -= l.Length;
                }
            }

            Console.WriteLine($"Moving right {xMovement} and down {yMovement} I ran into {trees} trees.");
            return trees;
        }
    }
}
