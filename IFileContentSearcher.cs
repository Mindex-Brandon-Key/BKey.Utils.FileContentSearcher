using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKey.Utils.FileContentSearcher;
public interface IFileContentSearcher
{
    public Task<List<SearchResult>> SearchAsync(string filePath);
}
