using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

Dictionary<(int x, int y), char> map = File.ReadAllLines("input")
    .SelectMany((l, y) => l.Select((p,x) => new {x, y, p}))
    .ToDictionary(a => (a.x,a.y), a => a.p);

var startXY = map.First(x => x.Value == 'S').Key;

var loop = new List<(int x, int y)>();
var prevXY = startXY;
var currentXY = startXY;
var stepCount = 0;
do
{
    stepCount++;

    var n = GetNeighbours(currentXY);

    //Must be first step
    if(map[currentXY] == 'S')
    {
        (int x, int y) first = (-1,-1);
        if(map.ContainsKey(n.north) && (new char[]{'|','7','F'}).Contains(map[n.north]))
            currentXY = n.north;
        else if(map.ContainsKey(n.south) && (new char[]{'|','J','L'}).Contains(map[n.south]))
            currentXY = n.south;
        else if(map.ContainsKey(n.east) && (new char[]{'-','7','J'}).Contains(map[n.east]))
            currentXY = n.east;
        else
            currentXY = n.west;

    }
    else
    {
        var next = map[currentXY] switch 
        {
            '|' => prevXY == n.north ? n.south : n.north,
            '-' => prevXY == n.east ? n.west : n.east,
            'L' => prevXY == n.north ? n.east : n.north,
            '7' => prevXY == n.south ? n.west : n.south,
            'F' => prevXY == n.south ? n.east : n.south,
            'J' => prevXY == n.north ? n.west : n.north,
        };
        prevXY = currentXY;
        currentXY = next;
    }
    loop.Add(currentXY);
} while(currentXY != startXY);

Console.WriteLine($"Part 1 = {stepCount/2}");

var maxX = map.Keys.Max(k => k.x);
var maxY = map.Keys.Max(k => k.y);

Dictionary<(int x, int y), char> doubleMap = new();
var newMapLines = new List<string>();
var insideCount = 0;
for(int y = 0; y<maxY; y++)
{
    for(int x=0; x<maxX; x++)
    {
        if(loop.Contains((x,y)))
            continue;
        else 
        {
            var pipesToLeft = loop.Where(n => n.x> x && n.y == y)
                .Where(n => (new char []{'|','S','L','J'}).Contains(map[n]))
                .Count();
            if(pipesToLeft % 2 == 1)
                insideCount++;
        }
    }
}

Console.WriteLine($"Part 2 = {insideCount}");


((int x, int y) north, (int x, int y) south,
(int x, int y) east,(int x, int y) west) GetNeighbours ((int x, int y) xy)
{
    var north = (xy.x, xy.y-1);
    var south = (xy.x, xy.y+1);
    var east = (xy.x+1, xy.y);
    var west = (xy.x-1, xy.y);

    return (north, south, east, west);
}