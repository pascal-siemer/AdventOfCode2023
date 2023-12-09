using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

namespace DayOne;

 /*
  * --- Day 1: Trebuchet?! ---
  * Something is wrong with global snow production, and you've been selected to take a look. The Elves have even given you a map; on it, they've used stars to mark the top fifty locations that are likely to be having problems.
  * 
  * You've been doing this long enough to know that to restore snow operations, you need to check all fifty stars by December 25th.
  * 
  * Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!
  * 
  * You try to ask why they can't just use a weather machine ("not powerful enough") and where they're even sending you ("the sky") and why your map looks mostly blank ("you sure ask a lot of questions") and hang on did you just say the sky ("of course, where do you think snow comes from") when you realize that the Elves are already loading you into a trebuchet ("please hold still, we need to strap you in").
  * 
  * As they're making the final adjustments, they discover that their calibration document (your puzzle input) has been amended by a very young Elf who was apparently just excited to show off her art skills. Consequently, the Elves are having trouble reading the values on the document.
  * 
  * The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.
  * 
  * For example:
  * 
  * 1abc2
  * pqr3stu8vwx
  * a1b2c3d4e5f
  * treb7uchet
  * In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.
  * 
  * Consider your entire calibration document. What is the sum of all of the calibration values?
  */

public static class One
{
    public static int Solve() =>
        File.ReadAllLines("./input.txt")
            .Select(FindDigits)
            .Aggregate(0, (accumulator, tuple) => accumulator + tuple.first * 10 + tuple.last);
    
    private static (int first, int last) FindDigits(string line) =>
        (
            first: line.FirstOrDefault(char.IsDigit, '0'),
            last: line.LastOrDefault(char.IsDigit, '0')
        )
        .Map(value => value.ToDigit() ?? 0);
}

public class Two
{
    public static int Solve() =>
        File.ReadAllLines("./input.txt")
            .Select(MapDigits)
            .Select(FindDigits)
            .Aggregate(0, (accumulator, tuple) => accumulator + tuple.first * 10 + tuple.last);

    private static (int first, int last) FindDigits(string line) =>
        (
            first: line.FirstOrDefault(char.IsDigit, '0'),
            last: line.LastOrDefault(char.IsDigit, '0')
        )
        .Map(value => value.ToDigit() ?? 0);

    private static string MapDigits(string line)
    {
        var segment = new ArraySegment<char>(line.ToCharArray());
        var accumulator = new List<char>();
        var result = Parse(segment, accumulator);
        return string.Join(string.Empty, result);
    }

    private static IEnumerable<char> Parse(ArraySegment<char> segment, IEnumerable<char> accumulator) => 
        segment switch
        {
            [] => accumulator,
            ['o', 'n', 'e', ..] => Parse(segment[1..], accumulator.Append('1')),
            ['t', 'w', 'o', ..] => Parse(segment[1..], accumulator.Append('2')),
            ['t', 'h', 'r', 'e', 'e', ..] => Parse(segment[1..], accumulator.Append('3')),
            ['f', 'o', 'u', 'r', ..] => Parse(segment[1..], accumulator.Append('4')),
            ['f', 'i', 'v', 'e', ..] => Parse(segment[1..], accumulator.Append('5')),
            ['s', 'i', 'x', ..] => Parse(segment[1..], accumulator.Append('6')),
            ['s', 'e', 'v', 'e', 'n', ..] => Parse(segment[1..], accumulator.Append('7')),
            ['e', 'i', 'g', 'h', 't', ..] => Parse(segment[1..], accumulator.Append('8')),
            ['n', 'i', 'n', 'e', ..] => Parse(segment[1..], accumulator.Append('9')),
            ['z', 'e', 'r', 'o', ..] => Parse(segment[1..], accumulator.Append('0')),
            [var character, ..] => Parse(segment[1..], accumulator.Append(character)),
        };
    
}

public static class Extensions
{
    public static int? ToDigit(this char character) =>
        char.IsDigit(character)
            ? character - '0'
            : null;

    public static ValueTuple<TResult, TResult> Map<T, TResult>(this ValueTuple<T, T> tuple, Func<T, TResult> function) =>
        (
            function(tuple.Item1), 
            function(tuple.Item2)
        );
}