var lines = File.ReadAllLines("input");

var map = new Dictionary<(int X, int Y), char>();

for(int y=0; y<lines.Length; y++)
    for(int x=0; x<lines[y].Length; x++)
        map[(x,y)] = lines[y][x];

var start = map.FirstOrDefault(m => m.Value == 'S').Key;

var maxSteps = 64;
var visited = new HashSet<Node>();
visited.Add(new Node(start, maxSteps));
AddValidSteps(new Node(start, maxSteps), visited);

var part1 = visited.Count(x => x.Steps == 0);
Console.WriteLine($"Part 1 = {part1}");

void AddValidSteps(Node start, HashSet<Node> visited)
{
    if(start.Steps == 0)
        return;


    var availableSteps = (new [] {
        new Node ((start.Location.X, start.Location.Y+1), start.Steps-1),
        new Node ((start.Location.X, start.Location.Y-1), start.Steps-1),
        new Node ((start.Location.X+1, start.Location.Y), start.Steps-1),
        new Node ((start.Location.X-1, start.Location.Y), start.Steps-1),
    })
    .Where(n => !visited.Contains(n))
    .Where(n => map.ContainsKey(n.Location) && map[n.Location] != '#')
    .ToList();

    visited.UnionWith(availableSteps);
    availableSteps.ForEach(n => AddValidSteps(n, visited));
    
}

record Point(int X, int Y);
record Node((int X, int Y) Location, int Steps);