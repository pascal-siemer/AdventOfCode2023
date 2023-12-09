using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;

namespace DaySix;

public class PartTwo
{
    private static string Path =>
#if DEBUG
        "./sample6.txt";
#else
        "./input6.txt";
#endif

    public static long Solve()
    {
        var lines = File.ReadAllLines(Path);
        var data = Parse(lines);

        var results = Enumerable.Empty<int>();
        
        foreach (var set in data)
        {
            var result = Enumerable.Range(0, (int)set.time + 1)
                .Select(charge => Distance(charge, set.time))
                .Count(distance => set.distance < distance);
            
            results = results.Append(result);
        }

        return results.Aggregate((long)1, (acc, number) => acc * number);
    }

    private static long Distance(long charge, long time)
    {
        return (time - charge) * charge;
    }

    [Pure]
    private static IEnumerable<Dataset64> Parse(string[] lines)
    {
        if (lines.Length is not 2) throw new InvalidDataException(nameof(lines));
        var times = ParseLine(lines[0], "Time:");
        var distances = ParseLine(lines[1], "Distance:");
        return times.Zip(distances, (time, distance) => new Dataset64(time, distance));
    }

    [Pure]
    private static IEnumerable<long> ParseLine(string line, string identifier)
    {
        var text = line
            .Replace(identifier, String.Empty)
            .Replace(" ", string.Empty);
        yield return long.Parse(text);
    }
}

