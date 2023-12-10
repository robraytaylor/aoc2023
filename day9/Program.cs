
var data = File
    .ReadAllLines("input")
    .Select(x => x.Split(' ').Select(x => int.Parse(x.Trim())).ToArray())
    .ToArray();
    
var part1 = data.Sum(x => GetNextValue(x));
Console.WriteLine($"Part 1 = {part1}");

var part2 = data.Sum(x => GetNextValue(x, false));
Console.WriteLine($"Part 2 = {part2}");


int GetNextValue(int[] readings, bool forwards = true)
{
    if(readings.Distinct().Count() == 1)
        return readings.First();

    List<int> differences = new ();
    for(int i = 0; i< readings.Length -1; i++)
        differences.Add(readings[i+1]-readings[i]);
    
    if(forwards)
        return readings.Last() + GetNextValue(differences.ToArray());
    else
        return readings.First() - GetNextValue(differences.ToArray(), false);
}