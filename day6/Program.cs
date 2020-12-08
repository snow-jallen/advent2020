using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = "input.txt";
            var lineBreak = file == "sample.txt" ? "\r\n" : "\n";
            var groups = File.ReadAllText(file).Split(lineBreak+lineBreak, StringSplitOptions.RemoveEmptyEntries);
            var responses = new List<HashSet<string>>();
            var declarations = new List<Declaration>();
            var part2Sum = 0;
            foreach(var group in groups)
            {
                var declaration = new Declaration();                
                declarations.Add(declaration);
                declaration.Responses.AddRange(group.Split(lineBreak));
                var set = new HashSet<string>();
                foreach(var response in Regex.Matches(group, "[a-z]"))
                {
                    declaration.UniqueAnswers.Add(response.ToString());
                    set.Add(response.ToString());
                }
                responses.Add(set);

                var individuals = group.Split(lineBreak);
                foreach(var ans in set)
                {
                    if(individuals.All(i => i.Contains(ans)))
                    {
                        part2Sum++;
                        declaration.SharedAnswers.Add(ans);
                    }
                }
            }

            Console.WriteLine($"Found {responses.Sum(r => r.Count)}; Part2 sum = {part2Sum}");
            var part2 = (from d in declarations
                        select d.SharedAnswers.Count()).Sum();
                        
            foreach(var d in declarations)
            {

            }
        }
    }

    public class Declaration
    {
        public List<string> Responses{get; private set;}
        public HashSet<string> UniqueAnswers{get; private set;}
        public HashSet<string> SharedAnswers{get; private set;}

        public Declaration()
        {
            Responses  = new List<string>();
            UniqueAnswers = new HashSet<string>();
            SharedAnswers = new HashSet<string>();
        }
    }
}
