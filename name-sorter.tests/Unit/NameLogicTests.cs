using NameSorterApp.Entities;
using NameSorterApp.Interfaces;
using NameSorterApp.Services;
using Xunit;

namespace NameSorter.Tests.Unit;

public sealed class NameLogicTests
{
    [Theory]
    [InlineData("Ada Lovelace", "Ada Lovelace")]
    [InlineData("Ada Augusta Lovelace", "Ada Augusta Lovelace")]
    [InlineData("Ada Augusta Byron Lovelace", "Ada Augusta Byron Lovelace")]
    [InlineData("  Ada   Augusta   Lovelace  ", "Ada Augusta Lovelace")]
    public void Parse_ValidName_ReturnsNormalizedName(string line, string expected)
    {
        var result = new NameParserService().Parse(line);

        Assert.NotNull(result.Name);
        Assert.Equal(expected, result.Name!.ToString());
    }

    [Theory]
    [InlineData("Cher", "A valid name must have at least two parts: a first name and a last name.")]
    [InlineData("A B C D E", "A valid name must have at most four parts: up to three given names and a last name.")]
    public void Parse_InvalidTokenCount_ReturnsReason(string line, string expectedError)
    {
        var result = new NameParserService().Parse(line);

        Assert.Null(result.Name);
        Assert.Equal(expectedError, result.Error);
    }

    [Theory]
    [InlineData("Zoe Adams", "Amy Brown", -1)]
    [InlineData("Amy Smith", "Zoe Smith", -1)]
    [InlineData("Alex Morgan Smith", "Alex Taylor Smith", -1)]
    [InlineData("Alex Smith", "Alex Morgan Smith", -1)]
    [InlineData("ada lovelace", "Ada Lovelace", 0)]
    public void Compare_OrdersNamesAsRequired(string first, string second, int expected)
    {
        Assert.Equal(expected, Math.Sign(new ComparerService().Compare(Parse(first), Parse(second))));
    }

    [Fact]
    public void Sort_ExampleInput_SortsAndKeepsDuplicates()
    {
        // The repeated name verifies that sorting does not remove duplicates.
        var names = new[] { "Janet Parsons", "Vaughn Lewis", "Adonis Julius Archer", "Beau Tristan Bentley", "Elroy Stanley", "Vaughn Lewis" }
            .Select(Parse);

        var service = new NameSortingService(new ComparerService(), new NoOpReader(), new NoOpOutput(), new NameParserService());
        var sorted = service.Sort(names);

        Assert.Equal(
            ["Adonis Julius Archer", "Beau Tristan Bentley", "Vaughn Lewis", "Vaughn Lewis", "Janet Parsons", "Elroy Stanley"],
            sorted.Select(name => name.ToString()));
    }

    [Fact]
    public void Sort_EmptyInput_ReturnsEmptyList()
    {
        var service = new NameSortingService(new ComparerService(), new NoOpReader(), new NoOpOutput(), new NameParserService());
        Assert.Empty(service.Sort([]));
    }

    // Create valid Name values directly so these tests focus on comparison and sorting.
    private static Name Parse(string line)
    {
        var words = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        return new Name(words[..^1], words[^1]);
    }

    private sealed class NoOpReader : IFileReader
    {
        public IReadOnlyList<string> ReadLines(string path) => [];
    }

    private sealed class NoOpOutput : IOutputWriter
    {
        public void WriteSortedNames(IReadOnlyList<Name> names)
        {
        }

        public void WriteIgnoredLines(IReadOnlyList<IgnoredLine> ignoredLines)
        {
        }
    }
}