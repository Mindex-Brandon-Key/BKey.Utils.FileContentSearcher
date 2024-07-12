using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKey.Utils.FileContentSearcher;
public static class FileContentSearcherFactory
{
    public enum SearchType
    {
        Text,
        Regex,
    }

    public static IFileContentSearcher Create(SearchType searchType, string pattern)
    {
        switch (searchType)
        {
            case SearchType.Text:
                return new TextFileContentSearcher(pattern);
            case SearchType.Regex:
                return new RegexFileContentSearcher(pattern);
            default:
                throw new NotImplementedException();
        }
    }
}
