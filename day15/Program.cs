var rawData = File.ReadAllText("input").Split(',');

var part1 = rawData.Sum(CalculateHash);

Console.WriteLine($"Part 1 = {part1}");

var instructionData = rawData
    .Select(instruction =>
        {
            var label = "";
            var action = 'x';
            var focalLength = -1;

            if(instruction.Contains('='))
            {
                var split = instruction.Split('=');
                label = split[0];
                action = '=';
                focalLength = int.Parse(split[1]);
            }
            else
            {
                label = instruction.Split('-')[0];
                action = '-';
            }


            return new Instruction(label, CalculateHash(label), action, focalLength);
        });

var boxes = Enumerable.Range(0,256).ToDictionary(x => x, _ => new List<(string label, int focalLength)>());

foreach(var instruction in instructionData)
{
    if(instruction.Action == '-')
    {
        if(boxes[instruction.Box].Any(x => x.label == instruction.Label))
            boxes[instruction.Box].Remove(boxes[instruction.Box].First(x => x.label == instruction.Label));
    }
    else
    {
        if(boxes[instruction.Box].Any(x => x.label == instruction.Label))
        {
            var currentLens = boxes[instruction.Box].First(x => x.label == instruction.Label);
            var index = boxes[instruction.Box].IndexOf(currentLens);
            boxes[instruction.Box].Remove(currentLens);
            boxes[instruction.Box].Insert(index, new (instruction.Label, instruction.FocalLength));
        }
        else
        {
            boxes[instruction.Box].Add(new (instruction.Label, instruction.FocalLength));
        }
    }
}

var totalFocussingPower = 0;

foreach(var box in boxes)
{
    for(int i=0; i<box.Value.Count; i++)
    {
        var focusingPower = (box.Key+1) * (i+1) * box.Value[i].focalLength;
        totalFocussingPower += focusingPower;
    }
}

Console.WriteLine($"Part 2 = {totalFocussingPower}");


int CalculateHash(string input)
{
    var value = 0;
    foreach(var c in input)
    {
        value += (int)c;
        value *= 17;
        value %= 256;
    }
    return value;
}

record Instruction(string Label, int Box, char Action, int FocalLength);