using ItemTagger.Helper;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Records;

namespace ItemTagger.ItemTypeFinder
{
    internal class MatchingFormListSet<T> : Dictionary<ItemType, MatchingFormList<T>>
        where T : class, IMajorRecordGetter
    {
        /// <summary>
        /// Returs the MatchingList for the given type, creating it if it's not yet present
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MatchingFormList<T> GetListForType(ItemType type)
        {
            // for now, only make this
            var curList = this.GetValueOrDefault(type);
            if (null == curList)
            {
                curList = new MatchingFormList<T>();
                this.Add(type, curList);
            }
            return curList;
        }

        public ItemType? GetMatchingType(IFormLinkGetter<T> input)
        {
            foreach(var pair in this)
            {
                if(input.IsAnyOf(pair.Value)) 
                {
                    return pair.Key;
                }
            }
            return null;
        }

        public ItemType? GetMatchingType(IFormLinkGetter<T> input, params ItemType[] subset)
        {
            foreach (var type in subset)
            {
                if(input.IsAnyOf(GetListForType(type)))
                {
                    return type;
                }
            }
            return null;
        }

    }

}
