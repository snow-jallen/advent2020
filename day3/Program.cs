using System;
using System.IO;

var lines = File.ReadAllLines("input.txt");
long ans = testPath(lines, 1, 1) *
           testPath(lines, 3, 1) *
           testPath(lines, 5, 1) *
           testPath(lines, 7, 1) *
           testPath(lines, 1, 2);
Console.WriteLine($"Part 2 is {ans}");

static long testPath(string[] lines, int xMovement, int yMovement)
{
    var col = 0;
    long trees = 0;
    for (int row = 0; row < lines.Length; row += yMovement)
    {
        var l = lines[row];

        if (l[col] == '#')
        {
            trees++;
        }

        col += xMovement;

        if (col >= l.Length)
        {
            col -= l.Length;
        }
    }

    Console.WriteLine($"Right {xMovement} / Down {yMovement} => {trees} trees");
    return trees;
}
