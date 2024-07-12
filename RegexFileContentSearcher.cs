using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BKey.Utils.FileContentSearcher;
public class RegexFileContentSearcher : IFileContentSearcher
{
    public Regex Regex { get; }

    public RegexFileContentSearcher(string pattern)
    {
        Regex = new Regex(pattern, RegexOptions.Compiled);
    }

    public async Task<List<SearchResult>> SearchAsync(string filePath)
    {
        var results = new List<SearchResult>();
        var lines = await File.ReadAllLinesAsync(filePath);

        for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            var line = lines[lineNumber];
            if (Regex.IsMatch(line)) {
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
