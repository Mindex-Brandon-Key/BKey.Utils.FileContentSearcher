namespace BKey.Utils.FileContentSearcher;
public interface IDirectorySearcher
{
    public Task<IList<string>> SearchAsync(string filePathToSearch,
                                            IEnumerable<string> filesToIncludePatterns = null,
                                            IEnumerable<string> filesToExcludePattern = null);
}
