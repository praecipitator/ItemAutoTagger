using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace ItemTagger.ItemTypeFinder
{
    internal class MatchingFormList<T> : HashSet<FormLinkGetter<T>>
        where T : class, IMajorRecordGetter
    {
        public void Add(params FormKey[] keys)
        {
            var mapped = keys.ToList().Select(x => x.ToLink<T>()).Where(x => null != x);
            UnionWith(mapped);
        }

        public void Add(params FormLink<T>[] formLinks)
        {
            UnionWith(formLinks);
        }

        /// <summary>
        ///     Adds a FormKey based on a string in the 01234567:File.esp format
        /// </summary>
        /// <param name="formLinkStr">String in the 01234567:File.esp format</param>
        public void Add(params string[] formLinkStrings)
        {
            var mapped = formLinkStrings.ToList().Select(x => FormKey.Factory(x).ToLink<T>()).Where(x => null != x);
            if(null == mapped)
            {
                return;
            }
            UnionWith(mapped);
        }

        public bool ContainsAny(IEnumerable<FormLinkGetter<T>> otherList)
        {
            return otherList.Any(entry => this.Contains(entry));
        }
    }
}
