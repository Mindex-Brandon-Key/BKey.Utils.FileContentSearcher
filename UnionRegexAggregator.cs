using System.Text.RegularExpressions;

namespace BKey.Utils.FileContentSearcher;
public class UnionRegexAggregator : IRegexAggregator
{
    private IEnumerable<Regex> Regexes { get; }

    public UnionRegexAggregator(IEnumerable<Regex> regexes)
    {
        Regexes = regexes;
    }

    public bool IsMatch(string input) => Regexes.Any(regex => regex.IsMatch(input));

    public Match Match(string input)
    {
        foreach (var regex in Regexes)
        {
            var match = regex.Match(input);
            if (match.Success)
            {
                return match;
            }
        }
        return null;
    }

    public IList<Match> Matches(string input)
    {
        foreach (var regex in Regexes)
        {
            var matches = regex.Matches(input);
            if (matches.Count > 0)
            {
                return matches;
            }
        }
        return new List<Match>();
    }

    public string Replace(string input, string replacement)
    {
        string result = input;
        foreach (var regex in Regexes)
        {
            result = regex.Replace(result, replacement);
        }
        return result;
    }
}
