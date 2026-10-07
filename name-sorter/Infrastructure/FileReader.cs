using NameSorterApp.Interfaces;

namespace NameSorterApp.Infrastructure;

public sealed class FileReader : IFileReader
{
    public IReadOnlyList<string> ReadLines(string path) => File.ReadAllLines(path);
}