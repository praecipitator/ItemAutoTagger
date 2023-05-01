using ItemTagger.ItemTypeFinder;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Strings;
using Noggog;

namespace ItemTagger.Helper
{
    internal static class Mixins
    {
        /// <summary>
        /// Checks if this enumerable contains any entry from the other list.
        /// If possible, call this on a set-like enumerable, where .contains is more efficient
        /// </summary>
        /// <typeparam name="T">anything</typeparam>
        /// <param name="thisList">This list, .Contains will be called on it</param>
        /// <param name="otherList">Other list, .Any will be called on it</param>
        /// <returns>bool</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList)
        {
            return otherList.Any(entry => thisList.Contains(entry));
        }

        /// <summary>
        /// Checks if this enumerable contains all entries from the other list.
        /// If possible, call this on a set-like enumerable, where .contains is more efficient
        /// </summary>
        /// <typeparam name="T">anything</typeparam>
        /// <param name="thisList">This list, .Contains will be called on it</param>
        /// <param name="otherList">Other list, .All will be called on it</param>
        /// <returns>bool</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList)
        {
            return otherList.All(entry => thisList.Contains(entry));
        }

        public static bool HasAnyKeyword(this IKeywordedGetter<IKeywordGetter> thisItem, IEnumerable<IFormLinkGetter<IKeywordGetter>> otherList)
        {
            if (thisItem.Keywords == null)
            {
                return false;
            }

            return otherList.ContainsAny(thisItem.Keywords);
        }

        public static bool IsAnyOf<T>(this T? thisEntry, IEnumerable<T> list)
        {
            if (thisEntry == null)
            {
                return false;
            }

            return list.Contains(thisEntry);
        }

        public static bool HasScript(this IHaveVirtualMachineAdapterGetter item, String scriptName)
        {
            if (item?.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            var scripts = item.VirtualMachineAdapter.Scripts;
            foreach (var script in scripts)
            {
                if (script.Name.Equals(scriptName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasAnyScript(this IHaveVirtualMachineAdapterGetter item, MatchingList list)
        {
            if (item.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            return item.VirtualMachineAdapter.Scripts.Any(script => list.Matches(script.Name));
        }

        public static ItemType? GetMatchingTypeByScript(this IHaveVirtualMachineAdapterGetter item, MatchingListSet listSet)
        {
            var scripts = item.VirtualMachineAdapter?.Scripts;
            if (null == scripts)
            {
                return null;
            }
            foreach (var script in scripts)
            {
                var maybeResult = listSet.GetMatchingType(script.Name);
                if (maybeResult != null)
                {
                    return maybeResult;
                }
            }

            return null;
        }

        public static bool HasEffect(this IIngestibleGetter item, FormLinkGetter<IMagicEffectGetter> effect)
        {
            return item.Effects.Any(entry => entry.BaseEffect.Equals(effect));
        }

        /// <summary>
        /// Update a dictionary with entries from another dictionary, overwriting if necessary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="thisDict"></param>
        /// <param name="otherDict"></param>
        public static void MergeWithOverwrite<TKey, TValue>(this Dictionary<TKey, TValue> thisDict, Dictionary<TKey, TValue> otherDict)
            where TKey : notnull
        {
            foreach (var pair in otherDict)
            {
                thisDict[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Update a dictionary with entries from another dictionary, unless the keys are present already
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="thisDict"></param>
        /// <param name="otherDict"></param>
        public static void MergeWithoutOverwrite<TKey, TValue>(this Dictionary<TKey, TValue> thisDict, Dictionary<TKey, TValue> otherDict)
            where TKey : notnull
        {
            foreach (var pair in otherDict)
            {
                if (!thisDict.ContainsKey(pair.Key))
                {
                    thisDict[pair.Key] = pair.Value;
                }
            }
        }

        public static string GetDebugString(this IMajorRecordGetter item)
        {
            if (item.EditorID == null)
            {
                return item.FormKey.ToString();
            }

            return item.FormKey.ToString() + " " + item.EditorID;
        }

        /// <summary>
        /// Shift elements forward by 1, putting the last element into the first place
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShiftByOne<T>(this ExtendedList<T> list)
        {
            var cnt = list.Count;
            if (cnt == 0)
            {
                return;
            }
            var prevElem = list[0];
            for (var i = 1; i < cnt; i++)
            {
                (prevElem, list[i]) = (list[i], prevElem);
            }

            list[0] = prevElem;
        }

        public static Dictionary<FormKey, ItemType> GetAsDictionary(this List<GenericFormTypeMapping> list)
        {
            Dictionary<FormKey, ItemType> result = new();
            foreach (var entry in list)
            {
                if (entry.form.IsNull)
                {
                    continue;
                }
                result.Set(entry.form.FormKey, entry.type);
            }
            return result;
        }

        /// <summary>
        /// like toString(), but will always return "" instead of null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNonNullString(this ITranslatedStringGetter? str)
        {
            if (str == null)
            {
                return "";
            }

            string? actualStr = str.ToString();
            if (actualStr == null)
            {
                return "";
            }

            return actualStr;
        }

        /// <summary>
        /// like toString(), but will always return "" instead of null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNonNullString(this TranslatedString? str)
        {
            if (str == null)
            {
                return "";
            }

            return str.ToString();
        }
    }
}
