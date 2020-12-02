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
                let isValid = charCount >= minOccurances && charCount <= maxOccurances
                where isValid == true
                select password;

WriteLine($"There are {passwords.Count()} valid passwords");

