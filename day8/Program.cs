using System.Text.RegularExpressions;
var (instructions, map) = ParseMap("test_input");

Console.WriteLine($"Part 1 = {StepsToTargetNode(instructions, map, "AAA", x => x=="ZZZ")}");

var (instructions2, map2) = ParseMap("input");

long lcm = 1;
foreach(var node in map2.Keys.Where(x => x.Last() == 'A'))
{
    var steps = StepsToTargetNode(instructions2, map2, node, x=>x.EndsWith('Z'));
    lcm = CalcLcm(lcm, steps);
}

Console.WriteLine($"Part 2 = {lcm}");


(string instructions, Dictionary<string, (string Left, string Right)> map)
    ParseMap(string fileName)
{
    var lines = File.ReadAllLines(fileName);

    var instructions = lines[0];

    var mapPattern = @"(?'node'\S\S\S) = .(?'left'\S\S\S), (?'right'\S\S\S).";

    var map = lines.Skip(2)
        .Select(l => Regex.Match(l, mapPattern))
        .ToDictionary<Match, string, (string Left, string Right)>
            (m => m.Groups["node"].Value, 
            m => new (m.Groups["left"].Value, m.Groups["right"].Value));

    return (instructions, map);
}

int StepsToTargetNode(string instructions, 
        Dictionary<string, (string Left, string Right)> map, 
        string startNode,
        Func<string, bool> targetNodeFunc)
{
    var currentNode = startNode;
    var stepCount = 0;
    while(!targetNodeFunc(currentNode))
    {
        var instruction = instructions[stepCount%instructions.Length];
        currentNode = instruction == 'L' ? map[currentNode].Left : map[currentNode].Right;
        stepCount++;
    }
    return stepCount;
}

long CalcLcm(long a, long b)
{
    long CalcGreatestCommonDevisor(long a, long b)
    {
        if (a == 0) 
            return b;  
        return CalcGreatestCommonDevisor(b % a, a);  
    }
    return (a / CalcGreatestCommonDevisor(a, b)) * b; 
}