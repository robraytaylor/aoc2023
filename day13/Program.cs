using System.Data.Common;

var line = File.ReadAllLines("input");

var mirrors = new List<List<string>>();
var lineIndex = 0;
while(lineIndex < line.Length)
{
    var mirror = new List<string>();
    while(lineIndex < line.Length && !string.IsNullOrEmpty(line[lineIndex].Trim()))
    {
        mirror.Add(line[lineIndex].Trim());
        lineIndex++;
    }
    mirrors.Add(mirror);
    lineIndex++;
}

var part1MirrorScore = 0;
var part2MirrorScore = 0;
foreach(var mirror in mirrors)
{
    var horizontalLines = mirror;

    var verticalLines = new List<string>();

    for(int i=0; i<horizontalLines.First().Length; i++)
        verticalLines.Add(new string(horizontalLines.Select(x => x[i]).ToArray()));

    var p1Horizontal = GetMirrorIndex(horizontalLines);
    var p1Vertical = GetMirrorIndex(verticalLines);
    part1MirrorScore += p1Horizontal*100 + p1Vertical;

    var p2Horizontal = GetMirrorIndex(horizontalLines,p1Horizontal,1);
    var p2verical = GetMirrorIndex(verticalLines,p1Vertical,1);
    p2verical = p2Horizontal == 0 ? p2verical: 0;

    part2MirrorScore += 100*p2Horizontal + p2verical;
}

Console.WriteLine($"Part 1 = {part1MirrorScore}");
Console.WriteLine($"Part 2 = {part2MirrorScore}");

int GetMirrorIndex(IEnumerable<string> lines, int skipIndex = 0, int maxSmudges = 0)
{
    var lineArray = lines.ToArray();
    //find duplicate lines to test
    for(int i=0; i<lineArray.Length-1; i++)
    {
        //test if mirror
        if(skipIndex > 0 && i+1 == skipIndex)
            continue;

        bool isMirror = true;
        int smudgeCount = 0;
        var steps = Math.Min(i, lineArray.Length-i-2);
        for(int j = 0; j<=steps; j++)
            if(lineArray[i-j] != lineArray[i+j+1])
            {
                if(smudgeCount < maxSmudges && CanFixSmudge(lineArray[i-j],lineArray[i+j+1]))
                {
                    smudgeCount++;
                }
                else
                {
                    isMirror = false;
                    break;
                }
            }

        if(isMirror)
            return i+1;
    }
    return 0;
}

bool CanFixSmudge(string line1, string line2)
{
    var diffCount = 0;
    for(int i=0; i<line1.Length; i++)
        diffCount += line1[i] == line2[i] ? 0 : 1;

    return diffCount == 1;
}
