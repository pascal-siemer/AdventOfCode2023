using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using System.Security;

namespace DayThree;

public class PartOne
{
    
    private static string Path()
    {
#if DEBUG
        return "./sample.txt";
#else
        return  "./input.txt";
#endif
    }

    private static char[,] ParseInput(string path) =>
        File.ReadLines(path)
            .Select(line => line.Trim())
            .Select(line => line.ToCharArray())
            .ToArray()
            .ToMultiDimensional();
    
    private static Number? IdentifyNumber(char[,] matrix ,int column, int row)
    {
        matrix.ThrowIfOutOfBounds(column, row);

        var item = matrix[column, row];

        if (!char.IsDigit(item))
        {
            return null;
        }

        var length = 1;
        var value = int.Parse("" + item);
        while(matrix.IsInBounds(column, row + length) && matrix[column, row + length] is var newItem && char.IsDigit(newItem))
        {
            length++;

            var digit = int.Parse("" + newItem);

            value = (value * 10) + digit;
        }

        return new(value, column, row, length);
    }

    public static IEnumerable<Number> IdentifyNumbers(char[,] input)
    {
        var columns = input.GetLength(0);
        var rows = input.GetLength(1);
        for (var column = 0; column < columns; column++)
        {
            for (var row = 0; row < rows; row++)
            {
                var number = IdentifyNumber(input, column, row);
        
                if (number is null)
                {
                    continue;
                }
        
                row += number.Lenght;

                yield return number;
            }
        }
    }
    
    public static int Solve()
    {
        var path = Path();
        var input = ParseInput(path);
        return IdentifyNumbers(input)
            .Where(number => input.CheckHitbox(number.Column, number.Row, number.Lenght, IsSymbol))
            .Select(number => number.Value)
            .Sum();
    }

    private static bool IsSymbol(char character) => character is not '.' && !char.IsDigit(character) && character is not ' ' && character is not '\r' && character is not '\n';
    
}

public record Number(int Value, int Column, int Row, int Lenght) : Item<int>(Value, Column, Row);

public static partial class Extensions
{
    
    public static T[,] ToMultiDimensional<T>(this T[][] @this)
    {
        var rows = @this[0].Length;
        var columns = @this.Length;

        var result = new T[columns, rows];

        for (var column = 0; column < columns; column++)
        {
            if (@this[column].Length != rows)
            {
                throw new IndexOutOfRangeException();
            }
            
            for (var row = 0; row < rows; row++)
            {
                result[column, row] = @this[column][row];
            }
        }

        return result;
    }
    
    public static void ThrowIfOutOfBounds<T>(this T[,] @this, int column, int row)
    {
        if (@this.IsInBounds(column, row))
        {
            return;
        }

        throw new ArgumentOutOfRangeException();
    }
    
    public static bool IsInBounds<T>(this T[,] @this, int column, int row) =>
        0 <= column && column < @this.GetLength(0)
        && 0 <= row && row < @this.GetLength(1);
    
    public static bool CheckHitbox<T>(this T[,] matrix, int column, int row, int length, Predicate<T> condition)
    {
        matrix.ThrowIfOutOfBounds(column, row);
        matrix.ThrowIfOutOfBounds(column, row + length - 1);

        var hitboxStartColumn = Math.Max(column - 1, 0);
        var hitboxEndColumn = Math.Min(column + 1, matrix.GetLength(0) - 1);
        var hitboxStartRow = Math.Max(row - 1, 0);
        var hitboxEndRow = Math.Min(row + (length - 1) + 1, matrix.GetLength(1) - 1);

        for (var columnIndex = hitboxStartColumn; columnIndex <= hitboxEndColumn; columnIndex++)
        {
            for (var rowIndex = hitboxStartRow; rowIndex <= hitboxEndRow; rowIndex++)
            {
                var item = matrix[columnIndex, rowIndex];

                if (!condition(item))
                {
                    continue;
                }
                
                return true;
            }
        }

        return false;
    }
}