using NameSorterApp.Entities;
using NameSorterApp.Interfaces;

namespace NameSorterApp.Services;

public sealed class NameSortingService(
    IComparer<Name> comparer,
    IFileReader reader,
    IOutputWriter output,
    NameParserService parser)
{
    public void Process(string inputPath)
    {
        var names = new List<Name>();
        var ignoredLines = new List<IgnoredLine>();
        var lines = reader.ReadLines(inputPath);

        for (var index = 0; index < lines.Count; index++)
        {
            var line = lines[index];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var result = parser.Parse(line);
            if (result.Name is not null)
            {
                names.Add(result.Name);
            }
            else
            {
                // Keep the source line number, including any blank lines skipped above.
                ignoredLines.Add(new IgnoredLine(index + 1, line, result.Error ?? "The name format is invalid."));
            }
        }

        output.WriteSortedNames(Sort(names));
        output.WriteIgnoredLines(ignoredLines);
    }

    public IReadOnlyList<Name> Sort(IEnumerable<Name> names)
    {
        var sortedNames = names.ToList();
        sortedNames.Sort(comparer);
        return sortedNames;
    }
}