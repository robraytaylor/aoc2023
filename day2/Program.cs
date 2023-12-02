using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var gamePattern = @"Game (\d+):";
var cubesPattern = @"((\d+ [a-zA-Z]+)+)";

var part1 = 0;
var part2 = 0;

foreach(var line in lines)
{
    //Console.WriteLine(line);
    var gameId = int.Parse(Regex.Match(line, gamePattern).Groups[1].Value);
    var cubeMatches = Regex.Matches(line, cubesPattern)
        .Select(m => m.Value.Split(' '))
        .Select(x => new {Colour = x[1], Count = int.Parse(x[0])});

    if(!cubeMatches.Any(x => x.Colour == "red" && x.Count > 12) &&
        !cubeMatches.Any(x => x.Colour == "green" && x.Count > 13) &&
        !cubeMatches.Any(x => x.Colour == "blue" && x.Count > 14)
        )
        part1 += gameId;

    var power = cubeMatches.GroupBy(x => x.Colour)
        .Select(g => g.Max(x => x.Count))
        .Aggregate((a,x) => a*x);

    part2 += power;
}

Console.WriteLine($"Part 1 = {part1}");
Console.WriteLine($"Part 2 = {part2}");

