using System.Security.Cryptography.X509Certificates;

// var lines = File.ReadAllLines("test_input");
// var testAreaMin = 7;
// var testAreaMax = 27;
var lines = File.ReadAllLines("input");
var testAreaMin = 200000000000000;
var testAreaMax = 400000000000000;

var hailstones = new List<Hailstone>();

foreach(var line in lines)
{
    
    var atSplit = line.Split("@");

    var positionSplit = atSplit[0].Trim().Split(",");
    var velocitySplit = atSplit[1].Trim().Split(",");
    
    var hailstone = new Hailstone(
        Position: new Vector(
            X: double.Parse(positionSplit[0].Trim()),
            Y: double.Parse(positionSplit[1].Trim()),
            Z: double.Parse(positionSplit[2].Trim())
        ),
        Velocity: new Vector(
            X: double.Parse(velocitySplit[0].Trim()),
            Y: double.Parse(velocitySplit[1].Trim()),
            Z: double.Parse(velocitySplit[2].Trim())
        )
    );
    hailstones.Add(hailstone);
}

var line2dEquations = hailstones.Select(Get2dLineEquation).ToList();
var part1 = 0;

for(int i=0; i<hailstones.Count; i++)
    for(int j=i+1; j<hailstones.Count; j++)
    {
        var a = hailstones[i];
        var b = hailstones[j];
        var intersection = GetIntersectionPoint(a,b);

        if(intersection.HasValue 
            && intersection.Value.X >= testAreaMin
            && intersection.Value.Y >= testAreaMin
            && intersection.Value.X <= testAreaMax
            && intersection.Value.Y <= testAreaMax)
        {
            var t1 = (intersection.Value.X - a.Position.X) / a.Velocity.X;
            var t2 = (intersection.Value.X - b.Position.X) / b.Velocity.X;
            if(t1 > 0 && t2 > 0 )
                part1++;
        }
    }

Console.WriteLine(part1);

(double X, double Y)? GetIntersectionPoint(Hailstone h1, Hailstone h2)
{
    var a = Get2dLineEquation(h1);
    var b = Get2dLineEquation(h2);
    if(a.M == b.M)
        return null;

    var x = (b.C-a.C)/(a.M-b.M);
    var y = (a.M * x) + a.C;
    return (x,y);
}

/*

Hailstone A: 19, 13, 30 @ -2, 1, -2
Hailstone B: 18, 19, 22 @ -1, -1, -2
Hailstones' paths will cross inside the test area (at x=14.333, y=15.333).

Hailstone A: 19, 13, 30 @ -2, 1, -2
Hailstone B: 20, 25, 34 @ -2, -2, -4
Hailstones' paths will cross inside the test area (at x=11.667, y=16.667).

Hailstone A: 19, 13, 30 @ -2, 1, -2
Hailstone B: 12, 31, 28 @ -1, -2, -1
Hailstones' paths will cross outside the test area (at x=6.2, y=19.4).

Hailstone A: 19, 13, 30 @ -2, 1, -2
Hailstone B: 20, 19, 15 @ 1, -5, -3
Hailstones' paths crossed in the past for hailstone A.

Hailstone A: 18, 19, 22 @ -1, -1, -2
Hailstone B: 20, 25, 34 @ -2, -2, -4
Hailstones' paths are parallel; they never intersect.

Hailstone A: 18, 19, 22 @ -1, -1, -2
Hailstone B: 12, 31, 28 @ -1, -2, -1
Hailstones' paths will cross outside the test area (at x=-6, y=-5).

Hailstone A: 18, 19, 22 @ -1, -1, -2
Hailstone B: 20, 19, 15 @ 1, -5, -3
Hailstones' paths crossed in the past for both hailstones.

Hailstone A: 20, 25, 34 @ -2, -2, -4
Hailstone B: 12, 31, 28 @ -1, -2, -1
Hailstones' paths will cross outside the test area (at x=-2, y=3).

Hailstone A: 20, 25, 34 @ -2, -2, -4
Hailstone B: 20, 19, 15 @ 1, -5, -3
Hailstones' paths crossed in the past for hailstone B.

Hailstone A: 12, 31, 28 @ -1, -2, -1
Hailstone B: 20, 19, 15 @ 1, -5, -3
Hailstones' paths crossed in the past for both hailstones.

*/

LineEquation Get2dLineEquation(Hailstone hailstone)
{
    var c = hailstone.Position.Y - (hailstone.Velocity.Y * (hailstone.Position.X / hailstone.Velocity.X));
    var m = (hailstone.Position.Y-c)/hailstone.Position.X;
    return new LineEquation(m, c);
}

record Vector(double X, double Y, double Z);

record Hailstone(Vector Position, Vector Velocity);

record LineEquation(double M, double C);