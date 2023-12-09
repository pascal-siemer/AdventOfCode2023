namespace DayFour;

public static class PartOne
{

    public static string Path() =>
#if DEBUG
        "./sample4.txt";
#else
        "./input4.txt";
#endif
    
    public static int Solve()
    {
        return File.ReadLines(Path())
            .Select(line => line.Split(":")[1])
            .Select(ParseCard)
            .Select(FindWinning)
            .Sum();
    }
    
    private static int FindWinning(Card card)
    {
        var result = 0;

        foreach (var number in card.actual)
        {
            if (card.winning.Contains(number))
            {
                result = result >= 1 ? result * 2 : 1;
            }
        }

        return result;
    }

    private static HashSet<int> ParseWinningNumbers(string line)
    {
        return line.Split(" ")
            .Where(block => block.Length > 0)
            .Select(int.Parse)
            .ToHashSet();
    }

    private static int[] ParseActualNumbers(string line)
    {
        return line.Split(" ")
            .Where(block => block.Length > 0)
            .Select(int.Parse)
            .ToArray();
    }

    private static Card ParseCard(string line)
    {
        var parts = line.Split("|");
        var winning = ParseWinningNumbers(parts[0]);
        var actual = ParseActualNumbers(parts[1]);
        return new(winning, actual);
    }

}