using NameSorterApp.Infrastructure;
using NameSorterApp.Services;

internal static class Program
{
    private static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Input file missing. Please provide the path to the input file as a command-line argument.");
            return 1;
        }

        if (!string.Equals(Path.GetExtension(args[0]), ".txt", StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("Invalid input file type. Please provide a .txt file.");
            return 1;
        }

        try
        {
            var service = new NameSortingService(
                new ComparerService(),
                new FileReader(),
                new OutputWriter("sorted-names-list.txt"),
                new NameParserService());
            service.Process(args[0]);

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}