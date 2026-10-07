namespace NameSorterApp.Interfaces;

public interface IFileReader
{
    IReadOnlyList<string> ReadLines(string path);
}