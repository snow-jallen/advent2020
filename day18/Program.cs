using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;

namespace day18
{
    class Program
    {
        static void Main(string[] args)
        {
            SolveSimple("1 + 2 * 3 + 4 * 5 + 6").Should().Be(71);
            Solve("1 + (2 * 3) + (4 * (5 + 6))").Should().Be(51);
            Solve("5 + (8 * 3 + 9 + 3 * 4 * 3)").Should().Be(437);
            Solve("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))").Should().Be(12240);
            Solve("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2").Should().Be(13632);
            Console.WriteLine("looks good!");

            long sum = 0;
            foreach(var line in File.ReadAllLines("input.txt"))
            {
                sum+=Solve(line);
            }
            Console.WriteLine($"Part1: {sum}");
        }

        public static long Solve(string problem)
        {
            return SolveSimple(ResolveParenthases(problem));
        }

        public static long SolveSimple(string problem)
        {       
            var parts = problem.Split(' ');
            long left = int.Parse(parts[0]);
            for (int offset = 1; offset <= parts.Length - 2; offset += 2)
            {
                var operation = parts[offset];
                var right = long.Parse(parts[offset+1]);
                left = operation switch
                {
                    "+" => left + right,
                    "-" => left - right,
                    "*" => left * right,
                    "/" => left / right
                };
            }
            return left;
        }

        public static string ResolveParenthases(string problem)
        {
            var pairs = new List<Pair>();
            for(int i = 0; i < problem.Length; i++)
            {
                if(problem[i] == '(')
                {
                    pairs.Add(new Pair(i));
                }
                if(problem[i] == ')')
                {
                    pairs.Last(p => p.Right == 0).Right = i;
                }                
            }
            if(pairs.Any())
            {
                var left = pairs.Last().Left;
                var len = pairs.Last().Right - left+1;
                var substr = problem.Substring(left, len);
                var ans = SolveSimple(substr.Substring(1, substr.Length-2));
                var newProblem = problem.Replace(substr, ans.ToString());
                problem = ResolveParenthases(newProblem);
            }
            return problem;
        }
    }
    public class Pair
    {
        public Pair(int left)
        {
            Left = left;
        }

        public int Left { get; }
        public int Right{get;set;}
        public override string ToString() => $"{Left}, {Right}";
    }
}
