
var lines = File.ReadAllLines("input");

var map = new Dictionary<(int X, int Y), char>();

for(int y=0; y<lines.Length; y++)
    for(int x=0; x<lines[y].Length; x++)
        map[(x,y)] = lines[y][x];

var start = map.FirstOrDefault(m => m.Key.Y == 0 && m.Value == '.').Key;
var end = map.FirstOrDefault(m => m.Key.Y == lines.Length - 1 && m.Value == '.').Key;

var longestRoute = 0;
//GetMaxRouteToEnd(start, new List<(int X, int Y)>(){start});

//Console.WriteLine($"Part 1 = {longestRoute}");

GetMaxRouteToEnd(start, new List<(int X, int Y)>(){start}, true);

Console.WriteLine($"Part 2 = {longestRoute}");


void GetMaxRouteToEnd((int X, int Y) current, List<(int X, int Y)> steps, bool part2 = false)
{
    if(current == end)
    {
        longestRoute = steps.Count-1 > longestRoute ? steps.Count-1 : longestRoute;
        return;
    }

    var possibleSteps = new List<(int X, int Y)>();

    if(map[current] == '.' || map[current] == '>' || part2)
        possibleSteps.Add((current.X+1, current.Y));
    if(map[current] == '.' || map[current] == '^' || part2)
        possibleSteps.Add((current.X, current.Y-1));
    if(map[current] == '.' || map[current] == 'v' || part2)
        possibleSteps.Add((current.X, current.Y+1));
    if(map[current] == '.' || map[current] == '<' || part2)
        possibleSteps.Add((current.X-1, current.Y));

    possibleSteps
    .Where(s => !steps.Contains(s))
    .Where(s => map.ContainsKey(s) && map[s] != '#')
    .ToList()
    .ForEach(s => GetMaxRouteToEnd(s, new List<(int X, int Y)>(steps) {s}, part2));
    
}
