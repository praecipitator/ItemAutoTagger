using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTagger.ItemTypeFinder
{
    internal class MatchingFormList<T>: HashSet<FormLinkGetter<T>>
        where T : class, IMajorRecordGetter
    {
        public void Add(FormKey formKey)
        {
            var formLink = formKey.ToLink<T>();
            if(null != formLink)
            {
                base.Add(formLink);
            }
        }

        /// <summary>
        ///     Adds a FormKey based on a string in the 01234567:File.esp format
        /// </summary>
        /// <param name="formLinkStr">String in the 01234567:File.esp format</param>
        public void Add(string formLinkStr)
        {
            Add(FormKey.Factory(formLinkStr));
        }

        public bool ContainsAny(IEnumerable<FormLinkGetter<T>> otherList)
        {
            return otherList.Any(entry => this.Contains(entry));
        }
    }
}
