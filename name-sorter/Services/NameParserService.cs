using NameSorterApp.Entities;

namespace NameSorterApp.Services;

public sealed class NameParserService
{
    public NameParseResult Parse(string line)
    {
        // A null separator array splits on whitespace; the final token is the last name.
        var words = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        var wordsCount = words.Length;

        if (wordsCount is < 2)
        {
            return new NameParseResult(null, "A valid name must have at least two parts: a first name and a last name.");
        }
        else if (wordsCount is > 4)
        {
            return new NameParseResult(null, "A valid name must have at most four parts: up to three given names and a last name.");
        }

        return new NameParseResult(new Name(words[..^1], words[^1]), null);
    }
}