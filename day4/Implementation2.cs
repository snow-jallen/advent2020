using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day4
{
    public static class Day4Implementation2
    {
        public static void Run()
        {
            var passports = from entry in File.ReadAllText("input.txt").Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
                            let pairs = entry.Split(new []{'\r', '\n', ' '}, StringSplitOptions.RemoveEmptyEntries)
                            let passport = MakePassport(pairs)
                            where passport.IsValid
                            select passport;
            Console.WriteLine($"Found {passports.Count()} valid passports");
        }

        static Passport2 MakePassport(string[] pairs) =>
            pairs.Aggregate(new Passport2(), (passport, pair) =>
            {
                var parts = pair.Split(':');
                return (parts[0], parts[1]) switch
                {
                    ("byr", string str) when int.TryParse(str, out int byr) && byr is >= 1920 and <= 2002
                        => passport with { HasByr = true },

                    ("iyr", string str) when int.TryParse(str, out int iyr) && iyr is >= 2010 and <= 2020
                        => passport with { HasIyr = true },

                    ("eyr", string str) when int.TryParse(str, out int eyr) && eyr is >= 2020 and <= 2030
                        => passport with { HasEyr = true },

                    ("hgt", string str) when str.Contains("cm") && int.TryParse(str.Replace("cm",""), out int cm) && cm is >= 150 and <= 193
                        => passport with { HasHgt = true },

                    ("hgt", string str) when str.Contains("in") && int.TryParse(str.Replace("in",""), out int inches) && inches is >= 59 and <= 76
                        => passport with { HasHgt = true },

                    ("hcl", string str) when Regex.IsMatch(str, "#[0-9,a-f]{6}")
                        => passport with { HasHcl = true },

                    ("ecl", string str) when new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(str)
                        => passport with { HasEcl = true },

                    ("pid", string str) when str.Length == 9 && int.TryParse(str, out int pid)
                        => passport with { HasPid = true },

                    _ => passport
                };
            });
    }

    public record Passport2()
    {
        public bool HasByr, HasIyr, HasEyr, HasHgt, HasHcl, HasEcl, HasPid;
        public bool IsValid => HasByr && HasIyr && HasEyr && HasHgt && HasHcl && HasEcl && HasPid;
    }
}