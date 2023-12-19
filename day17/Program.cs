
var lines = File.ReadAllLines("input");

var map = new Dictionary<(int x, int y), int>();

for(int y =0; y<lines.Length; y++)
    for(int x=0; x<lines[y].Length; x++)
        map.Add((x,y), int.Parse(lines[y][x].ToString()));

var yMax = lines.Length-1;
var xMax = lines[0].Length-1;

int highest = map.Values.Sum();
var bestResults = new Dictionary<(int x, int y, int dx, int dy, int dCount), int>();
var bestResult = highest;

void FollowPath((int x, int y) current, List<(int x, int y)> prev, int directionCount, int pathValue, Queue<Action> actions)
{
    (int x, int y) direction = (current.x-prev.Last().x, current.y-prev.Last().y);
    (int x, int y, int dx, int dy, int dCount) key = 
        (current.x, current.y, direction.x, direction.y, directionCount);

    if(bestResults.ContainsKey(key) && pathValue >= bestResults[key])
        return;

    bestResults[key] = pathValue;
    
    if(current == (xMax,yMax))
        return;

    var steps = new List<(int x, int y)> {(current.x+1, current.y),(current.x-1, current.y),(current.x, current.y+1),(current.x, current.y-1)};
    steps.RemoveAll(s => prev.Contains(s) || !map.ContainsKey(s));

    //start 1,1, prev 0,1 direction (1,0) next step in direction = 
    (int x, int y) directionStep = ((2*current.x)-prev.Last().x, (2*current.y)-prev.Last().y);
    if(directionCount > 2)
        steps.Remove(directionStep);

    foreach(var step in steps)
    {
        var newPrev = new List<(int x, int y)>(prev)
        {
            current
        };
        int newDirectionCount = step == directionStep ? directionCount+1 : 1;
        var action = ()=>FollowPath(step, newPrev, newDirectionCount, pathValue + map[step], actions);
        actions.Enqueue(action);
    }
    
}


Queue<Action> actions = new Queue<Action>();
var startAction = () => FollowPath((0,0), new List<(int x, int y)>(){(0,0)}, 1, 0, actions);
actions.Enqueue(startAction);

while(actions.Any())
{
    var action = actions.Dequeue();
    action();
}
/*
for(int y =0; y<lines.Length; y++)
{
    for(int x=0; x<lines[y].Length; x++)
    {
        Console.Write(bestPath.Contains((x,y)) ? "#" : map[(x,y)]);
    }
    Console.WriteLine();
}
*/

var part1 = bestResults
    .Where(x=> x.Key.x == xMax && x.Key.y == yMax)
    .Min(x => x.Value);

Console.WriteLine($"Part 1 = {part1}");

/*
2>>34^>>>1323
32v>>>35v5623
32552456v>>54
3446585845v52
4546657867v>6
14385987984v4
44578769877v6
36378779796v>
465496798688v
456467998645v
12246868655<v
25465488877v5
43226746555v>
*/