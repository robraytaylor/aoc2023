var lines = File.ReadAllLines("input");

var caveMap = new Dictionary<(int x, int y), char>();

for(int y =0; y<lines.Length; y++)
    for(int x=0; x<lines[y].Length; x++)
        caveMap.Add((x,y), lines[y][x]);


var part1 = GetEnergisedTiles((-1,0), Direction.Right);

Console.WriteLine($"Part 1 = {part1}");

var xMax = caveMap.Max(l => l.Key.x);
var yMax = caveMap.Max(l => l.Key.y);
var part2 = 0;

for(int y=0; y<=yMax; y++)
{
    var right = GetEnergisedTiles((-1, y), Direction.Right);
    var left = GetEnergisedTiles((xMax+1, y), Direction.Left);
    var max = Math.Max(right, left);
    part2 = part2 < max ? max : part2;
}

for(int x=0; x<=xMax; x++)
{
    var down = GetEnergisedTiles((x, -1), Direction.Down);
    var up = GetEnergisedTiles((x, yMax+1), Direction.Up);
    var max = Math.Max(down, up);
    part2 = part2 < max ? max : part2;
}

Console.WriteLine($"Part 2 = {part2}");

int GetEnergisedTiles((int x, int y) start, Direction direction)
{
    bool found = true;
    var states = caveMap.ToDictionary(x => x.Key, _ => Direction.None);
    states[start]=direction;

    while(found)
    {
        found = false;
        
        foreach(var state in states.Where(x => x.Value != Direction.None).ToArray())
        {
            //Right
            if((state.Value & Direction.Right) != 0)
            {
                var next = (state.Key.x+1, state.Key.y);
                if(caveMap.ContainsKey(next))
                {
                    var prevState = states[next];
                    states[next] = states[next] | ResolveDirection(Direction.Right, next);
                    if(!found)
                        found = prevState != states[next];
                }
            }

            //Down
            if((state.Value & Direction.Down) != 0)
            {
                var next = (state.Key.x, state.Key.y+1);
                if(caveMap.ContainsKey(next))
                {
                    var prevState = states[next];
                    states[next] = states[next] |  ResolveDirection(Direction.Down, next);
                    if(!found)
                        found = prevState != states[next];
                }
            }

            //Left
            if((state.Value & Direction.Left) != 0)
            {
                var next = (state.Key.x-1, state.Key.y);
                if(caveMap.ContainsKey(next))
                {
                    var prevState = states[next];
                    states[next] = states[next] |  ResolveDirection(Direction.Left, next);
                    if(!found)
                        found = prevState != states[next];
                }
            }

            //Up
            if((state.Value & Direction.Up) != 0)
            {
                var next = (state.Key.x, state.Key.y-1);
                if(caveMap.ContainsKey(next))
                {
                    var prevState = states[next];
                    states[next] = states[next] |  ResolveDirection(Direction.Up, next);
                    if(!found)
                        found = prevState != states[next];
                }
            }
        }
    }
    return states.Count(x => x.Value != Direction.None)-1;
}

Direction ResolveDirection(Direction inDirection, (int x, int y) location)
{
    if(caveMap[location] == '-' && 
        (inDirection == Direction.Right || inDirection == Direction.Left))
        return inDirection;

    if(caveMap[location] == '-' && 
        (inDirection == Direction.Up || inDirection == Direction.Down))
        return Direction.Left | Direction.Right;

    if(caveMap[location] == '|' && 
        (inDirection == Direction.Up || inDirection == Direction.Down))
        return inDirection;

    if(caveMap[location] == '|' && 
        (inDirection == Direction.Left || inDirection == Direction.Right))
        return Direction.Up | Direction.Down;    

    if(caveMap[location] == '/')
        return inDirection switch 
        {
            Direction.Up => Direction.Right,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Down,
            Direction.Right => Direction.Up,
        };
    
    if(caveMap[location] == '\\')
        return inDirection switch 
        {
            Direction.Up => Direction.Left,
            Direction.Down => Direction.Right,
            Direction.Left => Direction.Up,
            Direction.Right => Direction.Down,
        };
    return inDirection;
}

[Flags] enum Direction {
    None = 0,
    Up=1,
    Down=2,
    Left=4,
    Right=8
}

/*

>|<<<\....
|v-.\^....
.v...|->>>
.v...v^.|.
.v...v^...
.v...v^..\
.v../2\\..
<->-/vv|..
.|<<<2-|.\
.v//.|.v..

######....
.#...#....
.#...#####
.#...##...
.#...##...
.#...##...
.#..####..
########..
.#######..
.#...#.#..


*/