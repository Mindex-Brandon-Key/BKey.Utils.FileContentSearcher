using CommandLine.Text;
using CommandLine;

namespace BKey.Utils.FileContentSearcher;

public static class Program
{
    public class Options
    {
        [Option('p', "searchPath", Required = true, HelpText = "The path to search.")]
        public string SearchPath { get; set; }

        [Option('t', "searchPattern", Required = true, HelpText = "The pattern to search for.")]
        public string SearchPattern { get; set; }

        [Option('s', "searchType", Default = FileContentSearcherFactory.SearchType.Text, HelpText = "The search type. (Text or Binary)")]
        public FileContentSearcherFactory.SearchType SearchType { get; set; }

        [Option('i', "fileIncludePattern", Default = null, HelpText = "The pattern to include files.")]
        public string FileIncludePattern { get; set; }

        [Option('e', "fileExcludePattern", Default = null, HelpText = "The pattern to exclude files.")]
        public string FileExcludePattern { get; set; }

        [Option('o', "outputPath", Default = null, HelpText = "The path to output results.")]
        public string OutputPath { get; set; }

        // TODO case sensitivity
        // TODO directory search results
        // TODO excel output
    }

    public static void Main(string[] args)
    {
        var x = Parser.Default.ParseArguments<Options>(args)
            .WithNotParsedAsync<Options>(HandleParseErrorAsync).Result;
        x.WithParsedAsync(RunOptionsAsync).Wait();
    }

    private static async Task RunOptionsAsync(Options opts)
    {
        var includes = opts.FileIncludePattern?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var excludes = opts.FileExcludePattern?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var directorySearcher = new GlobDirectorySearcher();

        Console.WriteLine("Searching for files...");
        var files = await directorySearcher.SearchAsync(opts.SearchPath, includes, excludes);
        Console.WriteLine($"Found {files.Count} files.");

        var contentSearcher = FileContentSearcherFactory.Create(opts.SearchType, opts.SearchPattern);

        var results = new List<SearchResult>();

        opts.OutputPath ??= "out.csv";
        using var outFile = File.Open(opts.OutputPath, FileMode.Create);
        using var writer = new StreamWriter(outFile);

        var functionLocator = new FunctionLocator();

        foreach ( var file in files )
        //Parallel.ForEach(files, (file) =>
        {
            var fileResults = contentSearcher.SearchAsync(file).Result;
            //lock(fileResults)
            {
                results.AddRange(fileResults);
                foreach (var result in fileResults)
                {
                    var function = functionLocator.GetFunctionName(result.FoundFilePath, result.FoundFileLineNumber);

                    var msg = $"{result.FoundFilePath},{result.FoundFileLineNumber},{EscapeForCsv(result.FoundFileLine)},{function}";
                    writer.WriteLine(msg);
                }
            }
        }
        //);
    }

    public static string EscapeForCsv(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // If the string contains double quotes or commas, enclose it in double quotes and escape any double quotes inside the string.
        if (input.Contains("\"") || input.Contains(",") || input.Contains("\n"))
        {
            // Escape double quotes by replacing them with two double quotes.
            input = input.Replace("\"", "\"\"");

            // Enclose the string in double quotes.
            input = $"\"{input}\"";
        }

        return input;
    }

    private static async Task HandleParseErrorAsync(IEnumerable<Error> errs)
    {
        // Handle errors
        // ...
    }
}
