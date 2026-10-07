using NameSorterApp.Entities;
using NameSorterApp.Interfaces;
using NameSorterApp.Services;
using Xunit;

namespace NameSorter.Tests.Unit;

public sealed class NameSortingServiceTests
{
    [Fact]
    public void Process_ValidLines_SortsAndWritesNames()
    {
        var output = Run("John Smith", "Ada Lovelace");

        Assert.Equal(["Ada Lovelace", "John Smith"], output.Names.Select(name => name.ToString()));
        Assert.Empty(output.IgnoredLines);
    }

    [Fact]
    public void Process_InvalidLines_ReportsOriginalLineNumbers()
    {
        var output = Run("Ada Lovelace", "Cher", "John Smith", "A B C D E");

        Assert.Equal(["Ada Lovelace", "John Smith"], output.Names.Select(name => name.ToString()));
        Assert.Equal(
            [
                new IgnoredLine(2, "Cher", "A valid name must have at least two parts: a first name and a last name."),
                new IgnoredLine(4, "A B C D E", "A valid name must have at most four parts: up to three given names and a last name.")
            ],
            output.IgnoredLines);
    }

    [Fact]
    public void Process_BlankLines_SkipsThemAndPreservesLineNumbers()
    {
        var output = Run("", "  ", "Cher");

        Assert.Empty(output.Names);
        // The invalid name stays at line 3 because blank lines still count in the source file.
        Assert.Equal(
            [new IgnoredLine(3, "Cher", "A valid name must have at least two parts: a first name and a last name.")],
            output.IgnoredLines);
    }

    // Exercise the workflow with in-memory input and output instead of touching files or the console.
    private static MemoryOutput Run(params string[] lines)
    {
        var output = new MemoryOutput();
        var service = new NameSortingService(
            new ComparerService(),
            new MemoryReader(lines),
            output,
            new NameParserService());

        service.Process("input.txt");
        return output;
    }

    private sealed class MemoryReader(IReadOnlyList<string> lines) : IFileReader
    {
        public IReadOnlyList<string> ReadLines(string path) => lines;
    }

    private sealed class MemoryOutput : IOutputWriter
    {
        public IReadOnlyList<Name> Names { get; private set; } = [];
        public IReadOnlyList<IgnoredLine> IgnoredLines { get; private set; } = [];

        public void WriteSortedNames(IReadOnlyList<Name> names) => Names = names;
        public void WriteIgnoredLines(IReadOnlyList<IgnoredLine> ignoredLines) => IgnoredLines = ignoredLines;
    }
}