using System.Diagnostics;
using System.Text.RegularExpressions;

var fileContent = File.ReadAllText("input");

var seedPattern = @"seeds: (?'seeds'(?:\d+\s)+)";
var mapPattern = @"(?'from'.+)-to-(?'to'.+)map:\n(?'map'(?:\d+\s*)+)";

var seeds = Regex.Match(fileContent, seedPattern)
    .Groups["seeds"].Value
    .Split(" ")
    .Select(x => double.Parse(x))
    .ToArray();

List<Map> maps = new ();
foreach(Match match in Regex.Matches(fileContent, mapPattern))
{
    var mapEntries = match.Groups["map"].Value
        .Split("\n")
        .Where(s => !string.IsNullOrWhiteSpace(s))
        .Select(s => s.Split(" "))
        .Select(s =>new MapEntry(double.Parse(s[0]),double.Parse(s[1]),double.Parse(s[2])))
        .ToArray();

    maps.Add(new Map(match.Groups["from"].Value.Trim(), match.Groups["to"].Value.Trim(), mapEntries));
}

var sw = Stopwatch.StartNew();
var closestLocation = seeds.Max();
foreach(var seed in seeds)
{
    string item = "seed";
    double currentValue = seed;
    do
    {
        var map = maps.First(m => m.From == item);
        item = map.To;
        var entry = map.Entries
            .FirstOrDefault(x => currentValue >= x.From && currentValue < x.From + x.Interval);
        if(entry != null)
            currentValue = entry.To + (currentValue - entry.From);

    } while(item != "location");

    if(currentValue < closestLocation)
        closestLocation = currentValue;
}

Console.WriteLine($"Part 1 = {closestLocation} {sw.Elapsed.TotalSeconds}s");

//For Part 2 - reverse search from locations from 0
// to find first valid seed value.
sw = Stopwatch.StartNew();
var seedRanges = new List<(double start, double end)>();
for(var i = 0; i<seeds.Length; i+=2)
{
    seedRanges.Add((seeds[i], seeds[i]+seeds[i+1]));
}

double part2closestLocation = 0;

bool locationFound = false;
while(!locationFound)
{
    string item = "location";
    double currentValue = part2closestLocation;
    do
    {
        var map = maps.First(m => m.To == item);
        item = map.From;
        var entry = map.Entries
            .FirstOrDefault(x => currentValue >= x.To && currentValue < x.To + x.Interval);
        if(entry != null)
            currentValue = entry.From + (currentValue - entry.To);
    } while(item != "seed");

    if(seedRanges.Any(x => x.start<= currentValue && x.end>=currentValue))
        locationFound = true;
    else
        part2closestLocation++;
}

Console.WriteLine($"Part 2 = {part2closestLocation} - {sw.Elapsed.TotalSeconds}s");

public record MapEntry(double To, double From, double Interval);
public record Map(string From, string To, MapEntry[] Entries);