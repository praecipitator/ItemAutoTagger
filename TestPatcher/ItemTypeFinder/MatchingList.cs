using ItemTagger.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public void addExactMatch(string text)
        {
            exactMatchList.Add(text);
        }

        public void addPrefixMatch(string text)
        {
            prefixMatchList.Add(text);
        }

        public void addSuffixMatch(string text)
        {
            suffixMatchList.Add(text);
        }

        public void addRegexMatch(Regex rx)
        {
            regexMatchList.Add(rx);
        }

        public void addSubstringMatch(string text)
        {
            substringMatchList.Add(text);
        }
        
        public bool matchesAny(IEnumerable<string?> list)
        {
            return list.Any(str => this.matches(str));
        }


        /**
         * Returns true if the given string matches to anything within this MatchingList
         */
        public bool matches(string? str)
        {
            if(null == str)
            {
                return false;
            }
            if(exactMatchList.Contains(str, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if(prefixMatchList.Any(prefix => str.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
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
