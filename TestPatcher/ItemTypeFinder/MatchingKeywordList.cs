using Mutagen.Bethesda;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTagger.ItemTypeFinder
{
    internal class MatchingKeywordList: HashSet<FormKey>
    {
        public void Add(FormLink<IKeywordGetter> formLink)
        {
            base.Add(formLink.FormKey);
        }

        /*
        public new void Add(FormKey formKey)
        {
            base.Add(formKey);
        }*/

        /// <summary>
        ///     Adds a FormKey based on a string in the 01234567:File.esp format
        /// </summary>
        /// <param name="formLinkStr">String in the 01234567:File.esp format</param>
        public void Add(string formLinkStr)
        {
            Add(FormKey.Factory(formLinkStr));
        }

        /// <summary>
        ///     Returns true if the given item has any of the keywords contained in this list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemHasAny(IKeywordedGetter<IKeywordGetter> item)
        {
            if(item.Keywords == null)
            {
                return false;
            }
            return this.Any(kwKey => item.HasKeyword(kwKey));
        }
    }
}
