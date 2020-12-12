using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            int east, north, south, west;
            east = north = south = west = 0;
            char currentDirection = 'E';
            Dictionary<char, int> move = new (){
                {'N', 0},{'E',0},{'S', 0},{'W',0}
            };
            foreach(var line in lines)
            {
                var command = line[0];
                var amount = int.Parse(line.Substring(1));
                if(command == 'F')
                    command = currentDirection;

                switch(command)
                {
                    case 'R':
                        while(amount > 0)
                        {
                            currentDirection = currentDirection switch
                            {
                                'E' => 'S',
                                'S' => 'W',
                                'W' => 'N',
                                'N' => 'E'
                            };
                            amount -= 90;
                        }
                        break;
                    case 'L':
                        while(amount > 0)
                        {
                            currentDirection = currentDirection switch
                            {
                                'E' => 'N',
                                'S' => 'E',
                                'W' => 'S',
                                'N' => 'W'
                            };
                            amount -= 90;
                        }
                        break;
                    case 'N':
                    case 'E':
                        move[command] += amount;
                        break;
                    case 'S':
                        move['N'] -= amount;
                        break;
                    case 'W':
                        move['E'] -= amount;
                        break;
                }
            }

            Console.WriteLine($"Part1 = {Math.Abs(move['E']) + Math.Abs(move['N'])}");
        }
    }
}
