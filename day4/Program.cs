using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var validEcl = new []{"amb","blu","brn","gry","grn","hzl","oth"};
            var passports = new List<Passport>();
            var entries = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            foreach(var entry in entries)
            {
                var passport = new Passport();
                var pairs = entry.Split(new []{'\r', '\n', ' '}, StringSplitOptions.RemoveEmptyEntries);
                foreach(var pair in pairs)
                {
                    var parts = pair.Split(':');
                    var val = parts[1];
                    switch(parts[0])
                    {
                        case "byr":
                            var byr = int.Parse(val);
                            if(byr >= 1920 && byr <= 2002)
                                passport.HasBYR=true;
                            break;
                        case "iyr":
                            var iyr = int.Parse(val);
                            if(iyr >= 2010 && iyr <= 2020)
                                passport.HasIYR=true;
                            break;
                        case "eyr":
                            var eyr = int.Parse(val);
                            if(eyr >= 2020 && eyr <= 2030)
                                passport.HasEYR=true;
                            break;
                        case "hgt":
                            if(val.Contains("cm"))
                            {
                                var cm = int.Parse(val.Replace("cm", "").Trim());
                                if(cm >= 150 && cm <= 193)
                                    passport.HasHGT = true;
                            }
                            else if(val.Contains("in"))
                            {
                                var inches = int.Parse(val.Replace("in", "").Trim());
                                if(inches >= 59 && inches <= 76)
                                    passport.HasHGT = true;
                            }
                            break;
                        case "hcl":
                            if(Regex.IsMatch(val, "#[0-9,a-f]{6}"))
                                passport.HasHCL=true;
                            break;
                        case "ecl":
                            if(validEcl.Contains(val))
                                passport.HasECL=true;
                            break;
                        case "pid":
                            if(val.Length == 9 && int.TryParse(val, out int pid))
                                passport.HasPID=true;
                            break;
                    }
                }
                passports.Add(passport);
            }

            Console.WriteLine($"Found {passports.Count(p=>p.IsValid)} valid passports");

            Console.WriteLine($"Using implementation 2 I found...");
            Day4Implementation2.Run();
        }
    }

    internal class Passport
    {
        public Passport()
        {

        }
        public bool HasBYR{get;set;}
        public bool HasIYR{get;set;}
        public bool HasEYR{get;set;}
        public bool HasHGT{get;set;}
        public bool HasHCL{get;set;}
        public bool HasECL{get;set;}
        public bool HasPID{get;set;}
        public bool IsValid => HasBYR && HasIYR && HasEYR && HasHGT && HasHCL && HasECL && HasPID;
    }
}
