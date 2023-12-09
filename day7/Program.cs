var hands = File.ReadAllLines("input")
    .Select(l => l.Split(" "))
    .Select<string[], (string hand, int bet)>(s => new (s[0], int.Parse(s[1])))
    .ToList();

var part1 = hands.OrderByDescending(x => x.hand, new HandComparer())
    .Select((x,i) => x.bet * (i+1))
    .Sum();

Console.WriteLine($"Part 1 = {part1}");

var part2 = hands.OrderByDescending(x => x.hand, new HandComparer(true))
    .Select((x,i) => x.bet * (i+1))
    .Sum();

Console.WriteLine($"Part 2 = {part2}");

class HandComparer : IComparer<string>
{
    private bool jokersWild;
    public HandComparer(bool jokersWild = false)
    {
        this.jokersWild = jokersWild;
        cardValues['J'] = jokersWild ? 0 : 10;
    }

    Dictionary<char, int> cardValues = new () {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'J', 10},
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1},
    };

    public int Compare(string? x, string? y)
    {
        var xHand = x;
        var yHand = y;
        if(jokersWild)
        {
            xHand = ResolveWildCards(x);
            yHand = ResolveWildCards(y);
        }

        var xRank = GetHandRank(xHand);
        var yRank = GetHandRank(yHand);

        if (xRank == yRank)
        {
            for(int i=0; i<5; i++)
            {
                if(x[i] == y[i])
                    continue;
                
                xRank += cardValues[x[i]] - cardValues[y[i]];
                break;
            }
        }

        return xRank > yRank ? -1 : xRank == yRank ? 0 : 1;
    }

    private string ResolveWildCards(string hand)
    {
        if(hand.All(x => x=='J'))
            return "AAAAA";

        if(hand.Any(x => x == 'J'))
        {
            var jokerSwap = hand.Where(x => x != 'J')
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .ThenByDescending(x => cardValues[x.Key])
                .First().Key;

            return hand.Replace('J', jokerSwap);
        }
        return hand;
    }

    private int GetHandRank(string hand)
    {
        var groupedHand = hand.GroupBy(x => x);

        //five of a kind
        if(groupedHand.Any(x => x.Count() == 5))
            return 7;

        //four of a kind
        if(groupedHand.Any(x => x.Count() == 4))
            return 6;

        //full house
        if(groupedHand.Any(x => x.Count() == 3) 
            && groupedHand.Any(x => x.Count() == 2))
            return 5;

        //three of a kind
        if(groupedHand.Any(x => x.Count() == 3))
            return 4;

        //two pair
        if(groupedHand.Where(x => x.Count() == 2).Count() == 2)
            return 3;

        //one pair
        if(groupedHand.Any(x => x.Count() == 2))
            return 2;

        //high card
        return 1;
    }
}