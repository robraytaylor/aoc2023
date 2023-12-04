using System.Text.RegularExpressions;

static int[] ParseNumberString(string numberString)
    => numberString.Split(" ")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(x => int.Parse(x))
            .ToArray();

var lines = File.ReadAllLines("input.txt");

var pattern = @"Card\s+\d+:((?:\s+\d+)+)\s\S+((?:\s+\d+)+)";

var matchCounts = lines
    .Select(l => Regex.Match(l, pattern))
    .Select((m, i) => new {
        gameId = i,
        winningNumbers = ParseNumberString(m.Groups[1].Value),
        myNumbers = ParseNumberString(m.Groups[2].Value)
    })
    .Select(x => x.myNumbers.Count(n => x.winningNumbers.Contains(n)));

var part1 = matchCounts
    .Where(x => x>0)
    .Sum(x => Math.Pow(2, x-1));

Console.WriteLine($"Part 1 = {part1}");

var cardCounter = matchCounts
    .Select<int, (int matches, int cards)>(x => new (x, 1))
    .ToArray();
    
for(int c=0; c<cardCounter.Length; c++)
{
    for(int i = 1; i<=cardCounter[c].matches; i++)
    {
        cardCounter[c+i].cards += cardCounter[c].cards;
    }
}

Console.WriteLine($"Part 2 = {cardCounter.Sum(x => x.cards)}");