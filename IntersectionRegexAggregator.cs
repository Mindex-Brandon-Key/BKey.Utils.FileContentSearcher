using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BKey.Utils.FileContentSearcher;
public class IntersectionRegexAggregator : IRegexAggregator
{
    private IEnumerable<Regex> Regexes { get; }

    public IntersectionRegexAggregator(IEnumerable<Regex> regexes)
    {
        Regexes = regexes;
    }

    public bool IsMatch(string input) => Regexes.All(regex => regex.IsMatch(input));

    public Match Match(string input)
    {
        foreach (var regex in Regexes)
        {
            var match = regex.Match(input);
            if (!match.Success)
            {
                return null;
            }
        }
        return null;
    }

    public IList<Match> Matches(string input)
    {
        var matchCollection = new List<Match>();
        foreach (var regex in Regexes)
        {
            var matches = regex.Matches(input);
            matchCollection.AddRange(matches.Cast<Match>());
        }
        return matchCollection;
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

