
using System.Collections.Concurrent;

var conditionRecords = File.ReadAllLines("input")
    .Select(x => x.Split(' '))
    .Select<string[], (string record, int[] groups)>(x=> 
        new (x[0], x[1]
            .Split(',')
            .Select(y => int.Parse(y.Trim()))
            .ToArray())).ToArray();

Console.WriteLine($"Part 1 = {conditionRecords.Sum(r => GetValidCombinations(r.record, r.groups))}");

var unfoldedRecords = conditionRecords
    .Select<(string record, int[] groups),(string record, int[] groups)>(x=> 
    ($"{x.record}?{x.record}?{x.record}?{x.record}?{x.record}", Enumerable.Repeat(x.groups, 5).SelectMany(x => x).ToArray()))
    .ToArray();

var part2 = 0d;

var combinationCounts = new ConcurrentBag<double>();

Parallel.ForEach(unfoldedRecords, record =>
{
    combinationCounts.Add(GetValidCombinations(record.record, record.groups));
    Console.WriteLine(combinationCounts.Count);
});

part2 = combinationCounts.Sum();

//Console.WriteLine($"Part 1 = {unfoldedRecords.Sum(r => GetValidCombinations(r.record, r.groups))}");
Console.WriteLine($"Part 2 = {part2}");

bool RecordValid(string record, int[] groupPattern)
{
    var recordPattern = record
    .Split('.')
    .Where(x => x.Contains('#'))
    .Select(x => x.Length)
    .ToArray();

    return recordPattern.SequenceEqual(groupPattern);
}
IEnumerable<string> GetRecordPermutations(string record)
{
    if(!record.Contains('?'))
        return new string[] {record};

    var firstIndex = record.IndexOf('?');

    return GetRecordPermutations(record.Substring(firstIndex+1))
        .SelectMany(x => (new string[]{"#", "."}).Select(y => $"{record.Substring(0,firstIndex)}{y}{x}"));
}

double GetValidCombinations(string record, IEnumerable<int> groupPattern)
{

    if(groupPattern.Count() == 0)
    {
        if(record.Contains('#'))
            return 0;
        
        return 1;
    }

    var groupLength = groupPattern.First();

    var foundIndex = -1;
    for(int i=0; i<record.Length - groupLength +1; i++)
    {
        if(record.Skip(i).Take(groupLength).All(x => x!='.')
            && (record.Length == i+groupLength || record[i+groupLength] != '#')
            && !(i>0 && record[i-1]=='#'))
        {
            foundIndex = i;
            break;
        }
    }

    if(foundIndex < 0)
        return 0;

    if(record.Substring(0, foundIndex).Contains('#'))
        return 0;

    var nextString = record.Substring(foundIndex+groupLength);
    if(nextString.StartsWith('?'))
        nextString = $".{nextString.Substring(1)}";

    var newMatches = GetValidCombinations(nextString, groupPattern.Skip(1));

    if(record.Substring(foundIndex).StartsWith('?'))
        newMatches += GetValidCombinations(record.Substring(foundIndex+1), groupPattern);

    return newMatches;
}

/*
    ?###???????? 3,2,1 - 506250 arrangements

*/