namespace DayFour;

public static class PartTwo
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
            .Part2()
            .Count();
    }
    
    private static IEnumerable<int> Part2(this IEnumerable<int> winnings)
    {
        var repeats = new List<int>();
        
        foreach (var matches in winnings)
        {
            if (repeats.Count > 0)
            {
                for (int repeat = 0; repeat < repeats[0]; repeat++)
                {
                    yield return matches;
                }
                
                repeats.RemoveAt(0);
            }

            if (matches > 0)
            {
                for (int repeat = 0; repeat < matches; repeat++)
                {
                    if (repeats.Count > repeat)
                    {
                        repeats[repeat]++;
                    }
                    else
                    {
                        repeats.Insert(repeat, 1);
                    }
                    
                }
            }
            
        }
    }



    private static int FindWinning(Card card)
    {
        var result = 0;

        foreach (var number in card.actual)
        {
            if (card.winning.Contains(number))
            {
                result++;
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