namespace BKey.Utils.FileContentSearcher;

public class TextFileContentSearcher : IFileContentSearcher
{
    public string Pattern { get; }

    public TextFileContentSearcher(string pattern)
    {
        Pattern = pattern;
    }

    public async Task<List<SearchResult>> SearchAsync(string filePath)
    {
        var results = new List<SearchResult> ();
        var lines = await File.ReadAllLinesAsync(filePath);

        for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            var line = lines[lineNumber];
            if (line.Contains(Pattern))
            {
                results.Add(new SearchResult
                {
                    FoundFilePath = filePath,
                    FoundFileLineNumber = lineNumber + 1,
                    FoundFileLine = line,
                });
            }
        }
        return results;
    }
}

