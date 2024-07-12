using System.Text.RegularExpressions;

namespace BKey.Utils.FileContentSearcher;
public static class RegexAggregatorFactory
{
    public enum AggregateType
    {
        Intersection,
        Union,
    }

    public static IRegexAggregator Create(AggregateType type, IEnumerable<string> patterns, RegexOptions regexOptions = RegexOptions.None)
    {
        var regexes = patterns.Select(pattern => new Regex(pattern, regexOptions | RegexOptions.Compiled)).ToList();

        return type switch
        {
            AggregateType.Intersection => new IntersectionRegexAggregator(regexes),
            AggregateType.Union => new UnionRegexAggregator(regexes),
            _ => throw new NotSupportedException($"Unsupported AggregateType: {type}")
        };
    }
}

