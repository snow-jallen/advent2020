using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FluentAssertions;
using System.Collections;

namespace day15
{
    class Program
    {
        public static List<int> numbers = new List<int>();
        public static Dictionary<int, int> lastUsed = new ();
        public static Dictionary<int, int> twoTimesAgoUsed = new ();
        public static StreamWriter log;
        public static int startingNumCount;
        static void Main(string[] args)
        {
            log = new StreamWriter("log.txt");
            playGame("0,3,6", 2020).Should().Be(436);
            playGame("1,3,2", 2020).Should().Be(1);
            playGame("2,1,3", 2020).Should().Be(10);
            playGame("1,2,3", 2020).Should().Be(27);
            playGame("2,3,1", 2020).Should().Be(78);
            playGame("3,2,1", 2020).Should().Be(438);
            playGame("3,1,2", 2020).Should().Be(1836);
            playGame("0,13,16,17,1,10,6", 2020).Should().Be(276);
            playGame("0,13,16,17,1,10,6", 30000000);
        }

        private static int playGame(string numberString, int endingTurn)
        {
            Console.WriteLine();
            string msg = $"Evaluating {endingTurn:n0} turns for {numberString}";
            Console.WriteLine(msg);
            log.WriteLine(msg);

            lastUsed.Clear();
            numbers.Clear();

            numbers.AddRange(numberString
                        .Split(',')
                        .Select(n => int.Parse(n)));
            for(int i = 1; i < numbers.Count; i++)
                lastUsed[numbers[i-1]] = i;
            startingNumCount = numbers.Count;

            var nthNumber = BuildSequence(endingTurn);

            msg = $"The {endingTurn}th number is {nthNumber}";
            Console.WriteLine(msg);
            log.WriteLine($"{msg}\n\n\n\n\n");
            log.Flush();
            return nthNumber;
        }

        public static int BuildSequence(int endingTurn)
        {
            var lastNum = numbers.Last();
            for (int turnNum = numbers.Count+1; turnNum <= endingTurn; turnNum++)
            {
                lastNum = TakeATurn(lastNum, turnNum);
                if(turnNum % 500_000 == 0)
                    Console.Write($"{turnNum}, ");
            }
            return lastNum;
        }

        public static int TakeATurn(int prev, int turnNum)
        {
            string details;
            int delta;
            if (lastUsed.ContainsKey(prev) is false || lastUsed[prev] == -1)
            {
                details = $"First time we've used {prev} - using 0";
                delta = 0;
            }
            else
            {
                details = $"last showed up on turn {turnNum - 1,6}, and {lastUsed[prev],6} before that.";
                delta = turnNum-1 - lastUsed[prev];
            }
            lastUsed[prev] = turnNum-1;
            //log.WriteLine($"Turn {turnNum,6}: consider {prev,6}: {details,-55} {delta,6} (Turn {turnNum,6})");
            return delta;
        }
    }
}
