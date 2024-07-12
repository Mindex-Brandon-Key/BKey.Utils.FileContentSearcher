using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKey.Utils.FileContentSearcher;
public class SearchResult
{
    public string FoundFilePath { get; set; }
    public int FoundFileLineNumber { get; set; }
    public string FoundFileLine { get; set; }
}
