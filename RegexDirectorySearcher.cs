using System.Collections.Concurrent;

namespace BKey.Utils.FileContentSearcher;
public class RegexDirectorySearcher : IDirectorySearcher
{
    public async Task<IList<string>> SearchAsync(string filePathToSearch, IEnumerable<string> fileIncludePatterns = null, IEnumerable<string> fileExcludePattern = null)
    {

       // Compile include patterns
       fileIncludePatterns ??= Enumerable.Empty<string>();
        var includeFilter = RegexAggregatorFactory.Create(RegexAggregatorFactory.AggregateType.Union, fileIncludePatterns);

        fileExcludePattern ??= Enumerable.Empty<string>();
        var excludeFilter = RegexAggregatorFactory.Create(RegexAggregatorFactory.AggregateType.Union, fileExcludePattern);

        // Enumerate files with the custom method
        var foundFiles = new ConcurrentBag<string>();
        await EnumerateFilesAsync(filePathToSearch, includeFilter, excludeFilter, foundFiles);

        return foundFiles.ToList();
    }

    private static async Task EnumerateFilesAsync(string path, IRegexAggregator includeFilter, IRegexAggregator excludeFilter, ConcurrentBag<string> foundFiles)
    {
        var tasks = new List<Task>();

        foreach (var file in Directory.EnumerateFiles(path))
        {
            tasks.Add(Task.Run(() =>
            {
                if (includeFilter.IsMatch(file) && !excludeFilter.IsMatch(file))
                {
                    foundFiles.Add(file);
                }
            }));
        }

        foreach (var directory in Directory.EnumerateDirectories(path))
        {
            tasks.Add(EnumerateFilesAsync(directory, includeFilter, excludeFilter, foundFiles));
        }

        await Task.WhenAll(tasks);
    }
}
