using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FluentAssertions;

namespace day15
{
    class Program
    {
        public static List<int> numbers = new List<int>(30_000_000);
        public static int startingNumCount;
        static void Main(string[] args)
        {
            playGame("0,13,16,17,1,10,6");
            playGame("0,3,6").Should().Be(436);
            playGame("1,3,2").Should().Be(1);
            playGame("2,1,3").Should().Be(10);
            playGame("1,2,3").Should().Be(27);
            playGame("2,3,1").Should().Be(78);
            playGame("3,2,1").Should().Be(438);
            playGame("3,1,2").Should().Be(1836);
        }

        private static int playGame(string numberString)
        {
            var endingTurn = 30000000;
            Console.WriteLine();
            Console.WriteLine($"Evaluating {endingTurn:n0} turns for {numberString}");
            numbers.Clear();
            numbers.AddRange(numberString
                        .Split(',')
                        .Select(n => int.Parse(n))
                        .Reverse());
            startingNumCount = numbers.Count;
            var nthNumber = buildSequence(endingTurn);
            Console.WriteLine($"The {endingTurn}th number is {nthNumber}");
            return nthNumber;
        }

        public static int buildSequence(int endingTurn)
        {
            for (int i = 1; i <= endingTurn - startingNumCount; i++)
            {
                takeATurn();
                if(i % 500_000 == 0)
                    Console.Write($"{i}, ");
            }
            return numbers[0];
        }

        public static void takeATurn()
        {
            var prev = numbers[0];
            var lastTurnSpoken = findLastTurnSpoken(prev);
            if (lastTurnSpoken == -1)
            {
                numbers.Insert(0, 0);
                return;
            }

            numbers.Insert(0, penultimateTimeSpoken(prev) - lastTurnSpoken);
        }

        public static int findLastTurnSpoken(int prev)
        {
            if(numbers.IndexOf(prev, 1) < 0)
                return -1;
            return numbers.IndexOf(prev)+1;//zero based
        }

        public static int penultimateTimeSpoken(int numInQuestion)
        {
            return numbers.IndexOf(numInQuestion, numbers.IndexOf(numInQuestion, 1)) + 1;
        }
    }
}
