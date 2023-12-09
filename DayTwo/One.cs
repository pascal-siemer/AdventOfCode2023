using System.ComponentModel;
using System.Data.Common;
using Exception = System.Exception;

namespace DayTwo;

public static class One
{
    private const int maxRed = 12;
    private const int maxGreen = 13;
    private const int maxBlue = 14;

    public static int Solve()
    {
        var input = File.ReadAllLines("./input.txt");
        var games = Parse(input);
        var sum = 0;
        
        foreach (var game in games)
        {
            var isPossible = false;

            foreach (var set in game.Sets)
            {
                var red = 0;
                var green = 0;
                var blue = 0;
                
                foreach (var pull in set.Pulls)
                {
                    if (pull.Color is Color.Red)
                    {
                        red += pull.Count;
                    }

                    if (pull.Color is Color.Green)
                    {
                        green += pull.Count;
                    }

                    if (pull.Color is Color.Blue)
                    {
                        blue += pull.Count;
                    }
                }
                
                isPossible = red <= 12 && green <= 13 && blue <= 14;

                if (!isPossible)
                {
                    break;
                }
            }

            if (!isPossible)
            {
                continue;
            }

            sum += game.Identifier;
        }

        return sum;
    }


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
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    // private static Game[] Init() => new[]
    // {
    //     new Game(1, new Set[]
    //     {
    //         new(Color.Blue, 3),
    //         new(Color.Red, 4),
    //         new(Color.Red, 1),
    //         new(Color.Green, 2),
    //         new(Color.Blue, 6),
    //         new(Color.Green, 2)
    //     }),
    //     new Game(2, new Set[]
    //     {
    //         new(Color.Blue, 1),
    //         new(Color.Green, 2),
    //         new(Color.Green, 3),
    //         new(Color.Blue, 4),
    //         new(Color.Red, 1),
    //         new(Color.Green, 1),
    //         new(Color.Blue, 1)
    //     }),
    //     new Game(3, new Set[]
    //     {
    //         new(Color.Green, 8),
    //         new(Color.Blue, 6),
    //         new(Color.Red, 20),
    //         new(Color.Blue, 5),
    //         new(Color.Red, 4),
    //         new(Color.Green, 13),
    //         new(Color.Green, 5),
    //         new(Color.Red, 1)
    //     }),
    //     new Game(4, new Set[]
    //     {
    //         new(Color.Green, 1),
    //         new(Color.Red, 3),
    //         new(Color.Blue, 6),
    //         new(Color.Green, 3),
    //         new(Color.Red, 6),
    //         new(Color.Green, 3),
    //         new(Color.Blue, 15),
    //         new(Color.Red, 14)
    //     }),
    //     new Game(5, new Set[]
    //     {
    //         new(Color.Red, 6),
    //         new(Color.Blue, 1),
    //         new(Color.Green, 3),
    //         new(Color.Blue, 2),
    //         new(Color.Red, 1),
    //         new(Color.Green, 2)
    //     })
    // };

}