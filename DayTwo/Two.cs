using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Runtime.InteropServices;
using Exception = System.Exception;

namespace DayTwo;

public static class Two
{
    public static int Solve() =>
        File.ReadLines("./input.txt")
            .Select(ParseGame)
            .Select(game => game.Sets
                .Select(SumSet)
                .Aggregate(Aggregates.Empty, AccumulateMaxAggregate).GetProduct()
            )
            .Sum();

    private static Aggregate SumSet(Set set) => 
        set.Pulls.Aggregate(Aggregates.Empty, (accumulator, pull) => pull switch
        {
            (Color.Red, var count) => accumulator with { Red = accumulator.Red + count },
            (Color.Green, var count) => accumulator with { Green = accumulator.Green + count },
            (Color.Blue, var count) => accumulator with { Blue = accumulator.Blue + count },
        });

    private static Aggregate AccumulateMaxAggregate(Aggregate accumulator, Aggregate aggregate) =>
        accumulator with
        {
            Red = Math.Max(accumulator.Red, aggregate.Red),
            Green = Math.Max(accumulator.Green, aggregate.Green),
            Blue = Math.Max(accumulator.Blue, aggregate.Blue)
        };
    
    private static Aggregate maximizeAggregate(IEnumerable<Aggregate> aggregates) =>
        aggregates.Aggregate(Aggregates.Empty, AccumulateMaxAggregate);

    private static int ProductAggregate(Aggregate aggregate) =>
        Math.Max(aggregate.Red, 1)
        * Math.Max(aggregate.Green, 1)
        * Math.Max(aggregate.Blue, 1);

    // Parsing
    
    private static Game[] Parse(IEnumerable<string> lines) =>
        lines
            .Select(ParseGame)
            .ToArray();

    private static Game ParseGame(string line)
    {
        var split = line.Split(":");
        var gameIdString = split[0].Replace("Game", "").Trim();
        var identifier = int.Parse(gameIdString);
        var sets = split[1]
            .Split(";")
            .Select(ParseSet)
            .ToArray();

        return new(identifier, sets);
    }

    private static Set ParseSet(string line)
    {
        var pulls = line
            .Split(',')
            .Select(ParsePull)
            .ToArray();

        return new(pulls);
    }

    private static Pull ParsePull(string line)
    {
        var values = line.TrimStart().TrimEnd().Split(' ');
        return values switch
        {
            [var count, "red"] => new(Color.Red, int.Parse(count)),
            [var count, "green"] => new(Color.Green, int.Parse(count)),
            [var count, "blue"] => new(Color.Blue, int.Parse(count)),
            _ => throw new Exception("invalid input")
        };
    }
}