using System.Text.RegularExpressions;
using System.Windows.Markup;

var lines = File.ReadAllLines("input.txt");

var symbols = new Dictionary<(int x, int y), string>();
var numbers = new Dictionary<(int x1, int x2, int y), int>();

for(int y=0; y<lines.Length; y++)
{
    var currentLine = lines[y];
    var x = 0;
    while(x<currentLine.Length)
    {
        if(currentLine[x]=='.')
        {
            x++;
            continue;
        }
        var numberMatch = Regex.Match(currentLine.Substring(x), @"^(?!\D)\d+");
        if(numberMatch.Success)
        {
            numbers[(x,x+numberMatch.Value.Length-1,y)]=int.Parse(numberMatch.Value);
            //Console.WriteLine($"({x},{y}) = {numberMatch.Value}");
            x+= numberMatch.Value.Length;
        }
        else
        {
            symbols[(x,y)]=currentLine[x].ToString();
            //Console.WriteLine($"({x},{y}) = {currentLine[x]}");
            x++;
        }
    }

}

var validPartNumbers = numbers
    .Where(numEntry => symbols.Keys
        .Any(symbol => symbol.x >= numEntry.Key.x1-1 
            && symbol.x <=numEntry.Key.x2+1
            && symbol.y >= numEntry.Key.y-1
            && symbol.y <= numEntry.Key.y+1));

Console.WriteLine($"Part 1 = {validPartNumbers.Sum(x => x.Value)}");


var part2=0;
foreach(var gear in symbols.Where(s => s.Value == "*").Select(s => s.Key))
{
    var gearParts = numbers
        .Where(numEntry => gear.x >= numEntry.Key.x1-1 
                && gear.x <=numEntry.Key.x2+1
                && gear.y >= numEntry.Key.y-1
                && gear.y <= numEntry.Key.y+1);

    if(gearParts.Count() == 2)
        part2 += gearParts.First().Value*gearParts.Last().Value;
}

Console.WriteLine($"Part 2 = {part2}");