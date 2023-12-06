
//List<(int time, int distance)> input = new() {(7,9),(15,40),(30,200)}; //test
List<(double time, double distance)> input = new() {(54,302),(94,1476),(65,1029),(92,1404)};

var part1 = input
    .Select(i => CalcWinningCombinations(i.time, i.distance))
    .Aggregate((a,x) => a*x);

Console.WriteLine($"Part 1 = {part1}");

//var part2 = CalcWinningCombinations(71530, 940200); //test
var part2 = CalcWinningCombinations(54946592, 302147610291404);
Console.WriteLine($"Part 2 = {part2}");

double CalcWinningCombinations(double time, double distance)
{
    double minX = 0;
    double maxX = time;
    for(double i = 0; i<time; i++)
    {
        var computedDistance = i*(time-i);
        if(computedDistance > distance && minX == 0)
            minX = i;
        
        if(computedDistance <= distance && minX > 0)
        {
            maxX = i-1;
            break;
        }
    }
    return maxX-minX+1;
}