using System.Text.RegularExpressions;

namespace BKey.Utils.FileContentSearcher;
public interface IRegexAggregator
{
    bool IsMatch(string input);
    Match Match(string input);
    IList<Match> Matches(string input);
    string Replace(string input, string replacement);
}
