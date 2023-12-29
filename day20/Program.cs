using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input");
var parseRegex = @"(?'module'\S+) -> (?'targets'.+)";

Dictionary<string, IModule> modules = new Dictionary<string, IModule>();
List<string> conjunctions = new List<string>();
foreach(var line in lines)
{
    var match = Regex.Match(line, parseRegex);
    var name = match.Groups["module"].Value;
    var targets = match.Groups["targets"].Value.Split(",").Select(x => x.Trim()).ToList();

    if(name.StartsWith("%")) //flipFlop
        modules.Add(name.Substring(1), new FlipFlop(targets));
    else if(name.StartsWith("&")) //Conjunction
    {
        conjunctions.Add(name.Substring(1));
        modules.Add(name.Substring(1), new Conjunction(targets));
    }
    else //brodcaster
        modules.Add(name, new Broadcaster(targets));
}
//Conjunctions last as need to know inputs
foreach(var conjunctionKey in conjunctions)
{
    var conjunction = (Conjunction)modules[conjunctionKey];
    conjunction.BuildMemory(modules.Where(x => x.Value.Targets.Contains(conjunctionKey)).Select(x => x.Key).ToList());
}

double lowPulseCount = 0;
double highPulseCount = 0;
double part1 = 0;

long i =0;

Dictionary<string, long> part2Conjunctions = new ()
    {{"sj",0},{"qq",0},{"ls",0},{"bg",0}};

while(part2Conjunctions.Any(k => k.Value == 0) || i <1000)
{
    i++;
    if(i % 100000 == 0)
        Console.WriteLine(i);

    var commandQueue = new Queue<PulseSignal>();
    commandQueue.Enqueue(new PulseSignal("", "broadcaster", Pulse.Low));
    lowPulseCount++;
    while(commandQueue.TryDequeue(out var command))
    {
        if(modules.ContainsKey(command.Destination))
        {
            var newCommands = modules[command.Destination].Process(command);

            lowPulseCount += newCommands.Count(x => x.Pulse == Pulse.Low);
            highPulseCount += newCommands.Count(x => x.Pulse == Pulse.High);
            newCommands.ForEach(commandQueue.Enqueue);
        }

        if(part2Conjunctions.ContainsKey(command.Source) && part2Conjunctions[command.Source]==0 && command.Pulse == Pulse.High)
            part2Conjunctions[command.Source] = i;
    }

    if(i==1000)
        part1 = lowPulseCount * highPulseCount;

}


Console.WriteLine($"Part 1 = {part1}");

long part2 = 1;
foreach(var presses in part2Conjunctions.Values)
    part2 = CalcLcm(part2, presses);

Console.WriteLine($"Part 2 = {part2}");


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

enum Pulse{
    Low,
    High
}

interface IModule
{
    List<PulseSignal> Process(PulseSignal pulseSignal);
    List<string> Targets {get;}
}

class FlipFlop : IModule
{
    public List<string> Targets {init; get;}
    bool state = false;
    public FlipFlop(List<string> targets)
    {
        this.Targets = targets;
    }

    public List<PulseSignal> Process(PulseSignal pulseSignal)
    {
        var signals = new List<PulseSignal>();
        if(pulseSignal.Pulse == Pulse.High)
            return signals;
        
        signals.AddRange(Targets
            .Select(t => new PulseSignal(pulseSignal.Destination, t, state ? Pulse.Low : Pulse.High)));
        state = !state;

        return signals;
    }
}

class Conjunction : IModule
{
    private Dictionary<string, Pulse> memory;
    public List<string> Targets {init; get;}

    public void BuildMemory(List<string> inputs)
    {
        this.memory = inputs.ToDictionary(x => x, _ => Pulse.Low);
    }

    public Conjunction(List<string> targets)
    {
        this.Targets = targets;
    }

    public List<PulseSignal> Process(PulseSignal pulseSignal)
    {
        memory[pulseSignal.Source] = pulseSignal.Pulse;

        var outSignal = memory.All(x => x.Value == Pulse.High) ? Pulse.Low : Pulse.High;

        return Targets.Select(t => new PulseSignal(pulseSignal.Destination, t, outSignal)).ToList();
    }
}

class Broadcaster : IModule
{
    public List<string> Targets {init; get;}

    public Broadcaster(List<string> targets)
    {
        this.Targets = targets;
    }

    public List<PulseSignal> Process(PulseSignal pulseSignal)
    {
        return Targets.Select(t => new PulseSignal(pulseSignal.Destination, t, pulseSignal.Pulse)).ToList();
    }
}

record PulseSignal(string Source, string Destination, Pulse Pulse);