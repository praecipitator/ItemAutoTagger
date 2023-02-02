using ItemTagger.Helper;
using System.Text.RegularExpressions;

namespace ItemTagger.ItemTypeFinder
{
    internal class MatchingList
    {
        private readonly SortedSet<string> exactMatchList = new(new CaseInsensitiveStringComparer());
        private readonly List<string> prefixMatchList = new();
        private readonly List<string> suffixMatchList = new();
        private readonly List<string> substringMatchList = new();
        private readonly List<Regex> regexMatchList = new();

        public MatchingList()
        {
            //exactMatchList.Comparer = new CaseInsensitiveComparer<string>();
            //prefixMatchList.inter
        }

        public MatchingList AddExactMatch(params string[] text)
        {
            exactMatchList.UnionWith(text);

            return this;
        }

        public MatchingList AddPrefixMatch(params string[] text)
        {
            prefixMatchList.AddRange(text);

            return this;
        }

        public MatchingList AddSuffixMatch(params string[] text)
        {
            suffixMatchList.AddRange(text);

            return this;
        }

        public MatchingList AddRegexMatch(params Regex[] rx)
        {
            regexMatchList.AddRange(rx);

            return this;
        }

        public MatchingList AddSubstringMatch(params string[] text)
        {
            substringMatchList.AddRange(text);

            return this;
        }

        public bool MatchesAny(IEnumerable<string?> list)
        {
            return list.Any(str => Matches(str));
        }

        /**
         * Returns true if the given string matches to anything within this MatchingList
         */

        public bool Matches(string? str)
        {
            if (null == str)
            {
                return false;
            }
            if (exactMatchList.Contains(str, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (prefixMatchList.Any(prefix => str.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            if (suffixMatchList.Any(suffix => str.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            if (substringMatchList.Any(substr => str.Contains(substr, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return regexMatchList.Any(regex => regex.IsMatch(str));
        }
    }
}
