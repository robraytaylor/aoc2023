var lines = File.ReadAllLines("input");

var part1 = 0;
for(int i=0; i<lines[0].Length; i++)
{
    var ceiling = lines.Length;

    var columnScore = 0;
    for(int j = 0; j<lines.Length; j++)
    {
        var line = lines[j];
        if(line[i] == '#')
            ceiling = lines.Length-j-1;
        
        if(line[i] == 'O')
        {
            columnScore += ceiling;
            ceiling--;
        }
    }
    part1+=columnScore;
}

Console.WriteLine($"Part 1 = {part1}");

var platform = new Dictionary<(int x, int y), char>();

for(int y =0; y<lines.Length; y++)
    for(int x=0; x<lines[y].Length; x++)
        platform.Add((x,y), lines[y][x]);

var xMax = lines.First().Length-1;
var yMax = lines.Length-1;

List<int> scores = new();
for(int c =0; c<100000; c++)
{
    //north
    for(int x=0; x<=xMax; x++)
    {
        var sorted = RockSort(platform.Where(p => p.Key.x == x).OrderBy(p => p.Key.y).Select(p => p.Value).ToArray());
        for(int y=0; y<=yMax; y++)
        {
            platform[(x,y)] = sorted[y];
        }
    }

    //west
    for(int y=0; y<=yMax; y++)
    {
        var sorted = RockSort(platform.Where(p => p.Key.y == y).OrderBy(p => p.Key.x).Select(p => p.Value).ToArray());
        for(int x=0; x<=xMax; x++)
        {
            platform[(x,y)] = sorted[x];
        }
    }

    //south
    for(int x=0; x<=xMax; x++)
    {
        var sorted = RockSort(platform.Where(p => p.Key.x == x).OrderByDescending(p => p.Key.y).Select(p => p.Value).ToArray());
        for(int y=0; y<=yMax; y++)
        {
            platform[(x,y)] = sorted[sorted.Length-y-1];
        }
    }

    //east
    for(int y=0; y<=yMax; y++)
    {
        var sorted = RockSort(platform.Where(p => p.Key.y == y).OrderByDescending(p => p.Key.x).Select(p => p.Value).ToArray());
        for(int x=0; x<=xMax; x++)
        {
            platform[(x,y)] = sorted[sorted.Length-x-1];
        }
    }
    var score = platform.Where(p => p.Value == 'O').Sum(p => yMax-p.Key.y+1);
    scores.Add(score);

    if(scores.Count(x => x==score) > 5)
    {
        var indexes = scores.Select((x,i) => new {score = x, index = i})
            .Where(x => x.score == score)
            .Select(x => x.index)
            .ToArray();
        var gap = indexes[1]-indexes[0];
        bool isPattern=true;
        for(int i =1; i<indexes.Length-1; i++)
        {
            if(indexes[i+1]-indexes[i]!=gap)
            {
                isPattern = false;
                break;
            }
        }
        if(isPattern)
        {
            var remainder = (1000000000-c) % gap;
            scores.IndexOf(score);
            var finalScore = scores[scores.LastIndexOf(score)-gap+remainder-1];
            Console.WriteLine($"Part 1 = {finalScore}");
            break;
        }
    }
    

}

char[] RockSort(char[] toSort)
{
    bool swapped = true;
    while(swapped)
    {
        swapped = false;
        for(var i=0; i< toSort.Length-1; i++)
        {
            if(toSort[i] == '.' && toSort[i+1] == 'O')
            {
                toSort[i] = 'O';
                toSort[i+1] = '.';
                swapped = true;
            }
        }
    }
    return toSort;
}

