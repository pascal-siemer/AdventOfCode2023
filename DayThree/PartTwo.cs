using System.Diagnostics.SymbolStore;
using Microsoft.VisualBasic.FileIO;

namespace DayThree;

public record Item<T>(T Value, int Column, int Row);

public record Symbol(char Value, int Column, int Row) : Item<char>(Value, Column, Row);

public class PartTwo
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
        return Symbols(input)
            .Select(symbol => GetAssociatedNumbers(symbol, input).ToList())
            .Where(numbers => numbers.Count is 2)
            .Select(numbers => numbers[0].Value * numbers[1].Value)
            .Sum();
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

    private static IEnumerable<Number> GetAssociatedNumbers(Item<char> symbol, char[,] matrix) =>
        matrix
            .FindInHitbox(symbol.Column, symbol.Row, char.IsDigit)
            .Select(item => ScanNumber(matrix, item.Column, item.Row))
            .Distinct()
            .Where(number => number is not null)!;

    private static char[,] Parse(string path) =>
        File.ReadLines(path)
            .Select(line => line.ToCharArray())
            .ToArray()
            .ToMultiDimensional();

    private static Number? ScanNumber(char[,] matrix, int column, int row)
    {
        var item = matrix[column, row];

        if (!char.IsDigit(item))
        {
            return null;
        }
        
        var digits = Enumerable.Empty<char>()
            .Append(matrix[column, row]);
        
        //scan left
        var indexLeft = row - 1;
        while (matrix.IsInBounds(column, indexLeft) 
            && matrix[column, indexLeft] is var newItem 
            && char.IsDigit(newItem))
        {
            digits = digits.Prepend(newItem);
            indexLeft--;
        }
        
        //scan right
        var indexRight = row + 1;
        while (matrix.IsInBounds(column, indexRight)
            && matrix[column, indexRight] is var newItem
            && char.IsDigit(newItem))
        {
            digits = digits.Append(newItem);
            indexRight++;
        }

        var number = digits
            .Select(digit => int.Parse("" + digit))
            .Aggregate(0, (accumulator, digit) => (accumulator * 10) + digit);
        
        var length = indexRight - indexLeft - 1;
        return new Number(number, column, indexLeft + 1, length);
    }

}

public static partial class Extensions
{
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