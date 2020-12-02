using System;
using System.IO;
using System.Linq;
using static System.Console;

var passwords = from l in File.ReadAllLines("input.txt")
                let words = l.Split('-', ' ', ':')
                let minOccurances = int.Parse(words[0])
                let maxOccurances = int.Parse(words[1])
                let charInQuestion = words[2][0]
                let password = words[4].Trim()
                let charCount = password.Count(c => c == charInQuestion) 
                where charCount >= minOccurances && charCount <= maxOccurances
                select password;

WriteLine($"There are {passwords.Count()} valid passwords for part 1");

var part2 = from l in File.ReadAllLines("input.txt")
                let words = l.Split('-', ' ', ':')
                let firstIndex = int.Parse(words[0])
                let secondIndex = int.Parse(words[1])
                let charInQuestion = words[2][0]
                let password = words[4].Trim()
                let hasCharInFirstIndex = password[firstIndex -1] == charInQuestion
                let hasCharInSecondIndex = password[secondIndex - 1] == charInQuestion
                where (hasCharInFirstIndex && !hasCharInSecondIndex) ||
                      (!hasCharInFirstIndex && hasCharInSecondIndex)
                select password;

WriteLine($"There are {part2.Count()} valid passwords for part 2");