using System.Diagnostics.SymbolStore;
using Microsoft.VisualBasic.FileIO;

namespace DayThree;

public record Item<T>(T Value, int Column, int Row);

public record Symbol(char Value, int Column, int Row) : Item<char>(Value, Column, Row);

public class PartOneSolution2
{
    private static string Path()
    {
#if DEBUG
        return "./sample.txt";
#else
        return "./input.txt";
#endif
    }
    
    public static int Solve()
    {
        var path = Path();
        var input = Parse(path);
        var symbols = Symbols(input).ToArray();
        var numbers = symbols.SelectMany(symbol => GetAssociatedNumbers(symbol, input).ToHashSet().ToArray());
        return numbers.Sum();
    }

    private static bool IsSymbol(char character) =>
        character is not '.'
        && !char.IsDigit(character);
    
    private static IEnumerable<Symbol> Symbols(char[,] matrix) =>
        matrix.ToEnumerable()
            .Select((character, index) => (character, index))
            .Where(tuple => IsSymbol(tuple.character))
            .Select(tuple =>
            {
                var rowLength = matrix.GetLength(1);
                var column = tuple.index / rowLength;
                var row = tuple.index % rowLength;
                return new Symbol(tuple.character, column, row);
            });

    private static IEnumerable<int> GetAssociatedNumbers(Item<char> symbol, char[,] matrix) =>
        matrix
            .FindInHitbox(symbol.Column, symbol.Row, char.IsDigit)
            .Select(item => ScanNumber(matrix, item.Column, item.Row));

    private static char[,] Parse(string path) =>
        File.ReadLines(path)
            .Select(line => line.ToCharArray())
            .ToArray()
            .ToMultiDimensional();

    private static int ScanNumber(char[,] matrix, int column, int row)
    {
        var item = matrix[column, row];

        if (!char.IsDigit(item))
        {
            return 0;
        }

        var number = Int32.Parse("" + item);
        
        //scan left
        var index = row - 1;
        while (matrix.IsInBounds(column, index) 
            && matrix[column, index] is var newItem 
            && char.IsDigit(newItem))
        {
            var newNumber = Int32.Parse("" + newItem);
            var orderOfMagnitude = number.OrderOfMagnitude();
            var scaledNewNumber = newNumber * ((int)(Math.Pow(10, orderOfMagnitude + 1)));
            number = scaledNewNumber + number;
            index--;
        }
        
        //scan right
        index = row + 1;
        while (matrix.IsInBounds(column, index)
            && matrix[column, index] is var newItem
            && char.IsDigit(newItem))
        {
            
            var newNumber = Int32.Parse("" + newItem);
            number = (number * 10) + newNumber;

            index++;
        }

        return number;
    }

}

public static partial class Extensions
{
    public static int OrderOfMagnitude(this int number) =>
        number / 10 is var result && result is 0 
            ? 0 
            : OrderOfMagnitude(result) + 1;
    
    
    
    public static bool CheckHitbox<T>(this T[,] @this, int column, int row, Predicate<T> condition) =>
        @this.CheckHitbox(column, row, 1, condition);

    public static IEnumerable<T> ToEnumerable<T>(this T[,] @this)
    {
        foreach (var entry in @this)
        {
            yield return entry;
        }
    }

    public static IEnumerable<Item<T>> FindInHitbox<T>(this T[,] matrix, int column, int row, Predicate<T> filter)
    {
        matrix.ThrowIfOutOfBounds(column, row);

        var hitboxStartColumn = Math.Max(column - 1, 0);
        var hitboxEndColumn = Math.Min(column + 1, matrix.GetLength(0) - 1);
        var hitboxStartRow = Math.Max(row - 1, 0);
        var hitboxEndRow = Math.Min(row + 1, matrix.GetLength(1) - 1);

        for (var columnIndex = hitboxStartColumn; columnIndex <= hitboxEndColumn; columnIndex++)
        {
            for (var rowIndex = hitboxStartRow; rowIndex <= hitboxEndRow; rowIndex++)
            {
                var item = matrix[columnIndex, rowIndex];

                if (!filter(item))
                {
                    continue;
                }

                yield return new Item<T>(item, columnIndex, rowIndex);
            }
        }
    }
}