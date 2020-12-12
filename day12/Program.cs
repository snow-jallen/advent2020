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
            var ship = new Ship(10, 1);

            Console.WriteLine($"Starting => Ship ({ship.ShipX},{ship.ShipY}) Waypoint ({ship.WaypointRelativeX},{ship.WaypointRelativeY})");
            foreach(var line in lines)
            {
                var command = line[0];
                var amount = int.Parse(line.Substring(1));
                //not for part 2
                // if(command == 'F')
                //     command = currentDirection;

                switch(command)
                {
                    case 'F':
                        ship.ShipX += ship.WaypointRelativeX * amount;
                        ship.ShipY += ship.WaypointRelativeY * amount;
                        break;
                    case 'R':
                        while(amount > 0)
                        {
                            ship.ShiftRight();
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
                            ship.ShiftLeft();
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
                        ship.WaypointRelativeY += amount;
                        move[command] += amount;
                        break;
                    case 'E':
                        ship.WaypointRelativeX += amount;
                        move[command] += amount;
                        break;
                    case 'S':
                        ship.WaypointRelativeY -= amount;
                        move['N'] -= amount;
                        break;
                    case 'W':
                        ship.WaypointRelativeX -= amount;
                        move['E'] -= amount;
                        break;
                }

                Console.WriteLine($"{line} => Ship ({ship.ShipX},{ship.ShipY}) Waypoint ({ship.WaypointRelativeX},{ship.WaypointRelativeY})");
            }

            Console.WriteLine($"Part1 = {Math.Abs(move['E']) + Math.Abs(move['N'])}");

            Console.WriteLine($"Part2 = {ship.ManhattanDistance}");
        }
    }

    public class Ship
    {
        public Ship(int waypointRelativeX, int waypointRelativeY)
        {
            ShipX = ShipY = 0;
            WaypointRelativeX = waypointRelativeX;
            WaypointRelativeY = waypointRelativeY;
        }

        public int ShipX{get;set;}
        public int ShipY{get;set;}
        public int ManhattanDistance => Math.Abs(ShipX) + Math.Abs(ShipY);
        public int WaypointRelativeX { get; set;}
        public int WaypointRelativeY { get; set;}

        public void ShiftRight()
        {
            (WaypointRelativeX, WaypointRelativeY) = (WaypointRelativeY, WaypointRelativeX * -1);
        }

        public void ShiftLeft()
        {
            (WaypointRelativeX, WaypointRelativeY) = (WaypointRelativeY * -1, WaypointRelativeX);
        }
    }
}
