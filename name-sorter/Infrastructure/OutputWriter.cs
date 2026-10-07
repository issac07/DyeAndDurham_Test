using NameSorterApp.Entities;
using NameSorterApp.Interfaces;

namespace NameSorterApp.Infrastructure;

public sealed class OutputWriter(string outputPath) : IOutputWriter
{
    public void WriteSortedNames(IReadOnlyList<Name> names)
    {
        var lines = names.Select(name => name.ToString()).ToArray();
        File.WriteAllLines(outputPath, lines);

        Console.WriteLine($"Sorted names ({lines.Length}):");
        foreach (var line in lines)
        {
            Console.WriteLine($"  {line}");
        }
    }

    public void WriteIgnoredLines(IReadOnlyList<IgnoredLine> ignoredLines)
    {
        if (ignoredLines.Count == 0)
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Ignored lines ({ignoredLines.Count})");
        Console.WriteLine(new string('-', 24));
        foreach (var ignoredLine in ignoredLines)
        {
            Console.WriteLine($"  Line {ignoredLine.LineNumber}: {ignoredLine.Text}");
            Console.WriteLine($"    Reason: {ignoredLine.Reason}");
        }
    }
}