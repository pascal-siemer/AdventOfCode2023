using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;

namespace DaySix;

public class PartOne
{
    private static string Path =>
    #if DEBUG
        "./sample6.txt";
    #else
        "./input6.txt";
    #endif

    public static int Solve()
    {
        var lines = File.ReadAllLines(Path);
        var data = Parse(lines);

        var results = Enumerable.Empty<int>();
        
        foreach (var set in data)
        {
            var result = Enumerable.Range(0, set.time + 1)
                .Select(charge => Distance(charge, set.time))
                .Count(distance => set.distance < distance);
            
            results = results.Append(result);
        }

        return results.Aggregate(1, (acc, number) => acc * number);
    }

    private static int Distance(int charge, int time)
    {
        return (time - charge) * charge;
    }

    [Pure]
    private static IEnumerable<Dataset> Parse(string[] lines)
    {
        if (lines.Length is not 2) throw new InvalidDataException(nameof(lines));

        var times = ParseLine(lines[0], "Time:");
        var distances = ParseLine(lines[1], "Distance:");
        return times.Zip(distances, (time, distance) => new Dataset(time, distance));
    }

    [Pure]
    private static IEnumerable<int> ParseLine(string line, string identifier) =>
        line.Replace(identifier, String.Empty)
            .Split(' ')
            .Where(text => text is not "")
            .Select(Int32.Parse);
}

