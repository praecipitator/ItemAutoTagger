using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTagger.ItemTypeFinder
{
    internal class MatchingListSet: Dictionary<ItemType, MatchingList>
    {
        /// <summary>
        /// Returns the first type in the given subset for which there is a MatchingList which machtes for the given string
        /// </summary>
        /// <param name="input">string to check</param>
        /// <param name="subset">only check these item types</param>
        /// <returns>An ItemType if anything matched, null otherwise</returns>
        public ItemType? GetMatchingType(string? input, params ItemType[] subset)
        {
            if(input == null)
            {
                return null;
            }
            foreach(var checkType in subset)
            {
                var curList = this.GetValueOrDefault(checkType);
                if(null != curList)
                {
                    if(curList.Matches(input))
                    {
                        return checkType;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first type present in this set, where the corresponding MatchingList matches for the given string
        /// </summary>
        /// <param name="input">string to check</param>
        /// <returns>An ItemType if anything matched, null otherwise</returns>
        public ItemType? GetMatchingType(string? input)
        {
            if(input == null)
            {
                return null;
            }
            foreach (var pair in this)
            {
                if(pair.Value.Matches(input))
                {
                    return pair.Key;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first type present in this set, where the corresponding MatchingList which matches for any of the given input strings
        /// </summary>
        /// <param name="inputs">list of strings to check</param>
        /// <returns>An ItemType if anything matched, null otherwise</returns>
        public ItemType? GetMatchingType(List<string> inputs)
        {
            foreach (var pair in this)
            {
                if (pair.Value.MatchesAny(inputs))
                {
                    return pair.Key;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first type in the given subset for which there is a MatchingList which matches for any of the given input strings
        /// </summary>
        /// <param name="inputs">list of strings to check</param>
        /// <param name="subset">only check these item types</param>
        /// <returns>An ItemType if anything matched, null otherwise</returns>
        public ItemType? GetMatchingType(List<string> inputs, params ItemType[] subset)
        {
            foreach (var checkType in subset)
            {
                var curList = this.GetValueOrDefault(checkType);
                if (null != curList)
                {
                    if (curList.MatchesAny(inputs))
                    {
                        return checkType;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returs the MatchingList for the given type, creating it if it's not yet present
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MatchingList GetListForType(ItemType type)
        {
            var curList = this.GetValueOrDefault(type);
            if (null == curList)
            {
                curList = new MatchingList();
                this.Add(type, curList);
            }
            return curList;
        }
    }
}
