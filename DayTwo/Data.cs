namespace DayTwo;

enum Color
{
    Red,
    Green,
    Blue
}

record Pull(Color Color, int Count);

record Set(Pull[] Pulls);

public record Aggregate(int Red, int Green, int Blue);

record Game(int Identifier, Set[] Sets);

public static class Aggregates
{
    public static Aggregate Empty = new (0, 0, 0);
    
    public static int GetProduct(this Aggregate @this) => 
        Math.Max(@this.Red, 1)
        * Math.Max(@this.Green, 1)
        * Math.Max(@this.Blue, 1);
}

