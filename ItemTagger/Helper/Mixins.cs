using ItemTagger.ItemTypeFinder;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Records;
using Noggog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTagger.Helper
{
    internal static class Mixins
    {
        /// <summary>
        /// Checks if this enumerable contains any entry from the other list.
        /// If possible, call this on a set-like enumerable, where .contains is more efficient
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList">This list, .contains will be called on it</param>
        /// <param name="otherList">Other list, .any will be called on it</param>
        /// <returns></returns>
        public static bool ContainsAny<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList)
        {
            return otherList.Any(entry => thisList.Contains(entry));
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
            if(thisEntry == null)
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
            where TKey: notnull
        {
            foreach(var pair in otherDict)
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
                if(!thisDict.ContainsKey(pair.Key))
                {
                    thisDict[pair.Key] = pair.Value;
                }
            }
        }

        public static string GetDebugString(this IMajorRecordGetter item)
        {
            if(item.EditorID == null)
            {
                return item.FormKey.ToString();
            }

            return item.FormKey.ToString() + " " + item.EditorID;
        }
    }
}
