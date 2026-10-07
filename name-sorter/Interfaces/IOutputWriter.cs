using NameSorterApp.Entities;

namespace NameSorterApp.Interfaces;

public interface IOutputWriter
{
    void WriteSortedNames(IReadOnlyList<Name> names);
    void WriteIgnoredLines(IReadOnlyList<IgnoredLine> ignoredLines);
}