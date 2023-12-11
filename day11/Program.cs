var lines = File.ReadAllLines("input").ToList();

//Expand the Universe
var verticalExpansionIndexes = Enumerable.Range(0, lines[0].Length-1)
    .Where(i => lines[i].All(l => l == '.')).ToArray();

var horizontalExpansionIndexes = Enumerable.Range(0, lines.Count-1)
    .Where(i => lines.All(l => l[i] == '.')).ToArray();

var xInsertedCount = 0;
foreach(var i in verticalExpansionIndexes)
{
    lines.Insert(i+xInsertedCount, new String('X', lines[0].Length));
    xInsertedCount++;
}

var yInsertedCount = 0;
foreach(var i in horizontalExpansionIndexes)
{
    for(int j=0; j< lines.Count; j++)
        lines[j] = lines[j].Insert(i+yInsertedCount, "X");
    yInsertedCount++;
}

//Build a map and list of galaxies
var galaxies = new List<(int x, int y)>();
var space = new Dictionary<(int x, int y), char>();

for(int y = 0; y<lines.Count; y++)
    for(int x = 0; x<lines[0].Length; x++)
    {
        space[(x,y)] = lines[y][x];
        if(lines[y][x] == '#')
            galaxies.Add((x,y));
    }

//Get pairs of galaixies
var galaxyPairs = GetPairs(galaxies);

//Profit.
Console.WriteLine($"Part 1 = {galaxyPairs.Sum(p => CalculateDistance(p.From, p.To, 1))}");

Console.WriteLine($"Part 2 = {galaxyPairs.Sum(p => CalculateDistance(p.From, p.To, 1000000-1))}");

IEnumerable<(T From,T To)> GetPairs<T>(IEnumerable<T> list)
{
    if(list.Any())
        return list.Take(1)
            .SelectMany(x => list.Select(y => (x,y)).Skip(1))
            .Union(GetPairs(list.Skip(1)));

    return Enumerable.Empty<(T,T)>();
}

double CalculateDistance((int x, int y) from, (int x, int y) to, double xValue)
{
    var steps = new List<(int x, int y)>();
    var xDirection = from.x < to.x ? 1 : -1;
    
    var current = from;
    while(current.x != to.x)
    {
        current = (current.x + xDirection, current.y);
        steps.Add(current);
    }

    var yDirection = from.y < to.y ? 1 : -1;
    while(current.y != to.y)
    {
        current = (current.x, current.y + yDirection);
        steps.Add(current);
    }

    return steps.Select(x => space[x] == 'X' ? xValue : 1d).Sum();
}

/*
for(int y=0; y<lines[0].Length + horizontalExpansionIndexes.Count(); y++)
{
    var xIndex = 0;
    for(int x=0; x<lines[0].Length + verticalExpansionIndexes.Count(); x++)
    {
        if()
    }
    Console.WriteLine()
}
*/