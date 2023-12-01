using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var pattern = @"\d";

var part1 = lines
    .Select(x => Regex.Matches(x, pattern))
    .Select(m => int.Parse($"{m.First()}{m.Last()}"))
    .Sum();

Console.WriteLine($"Part 1 = {part1}");

var part2 = lines
    .Select(x => x.Replace("one", "one1one")
                    .Replace("two", "two2two")
                    .Replace("three", "three3three")
                    .Replace("four", "four4four")
                    .Replace("five", "five5five")
                    .Replace("six", "six6six")
                    .Replace("seven", "seven7seven")
                    .Replace("eight", "eight8eight")
                    .Replace("nine", "nine9nine"))
    .Select(x => Regex.Matches(x, pattern))
    .Select(m => int.Parse($"{m.First()}{m.Last()}"))
    .Sum();

Console.WriteLine($"Part 2 = {part2}");