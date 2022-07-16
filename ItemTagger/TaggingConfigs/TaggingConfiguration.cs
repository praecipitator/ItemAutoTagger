using ItemTagger.ItemTypeFinder;
using System.Text.RegularExpressions;

namespace ItemTagger.TaggingConfigs
{
    public class TaggingConfiguration : Dictionary<ItemType, string>
    {
        private readonly HashSet<string> extraValidTags = new();

        private static readonly Regex TAG_EXTRACT_REGEX = new(@"^[\[\]()|{}]([^\[\]()|{}]+)[\[\]()|{}]$", RegexOptions.Compiled);

        public TaggingConfiguration()
        {

        }

        /// <summary>
        /// Adds extra tags to be considered valid
        /// </summary>
        /// <param name="extraTags">The tags must be WITHOUT BRACKETS. They will not be cleanead!</param>
        public void AddExtraTags(IEnumerable<string> extraTags)
        {
            extraValidTags.UnionWith(extraTags);
        }

        public new void Add(ItemType type, string tag)
        {
            base.Add(type, tag);

            AddExtraTag(tag);
        }

        private void AddExtraTag(string fullTag)
        {
            // remove the brackets.
            // TODO maybe just use substring?
            var matches = TAG_EXTRACT_REGEX.Match(fullTag);

            if (matches.Groups.Count < 2)
            {
                return;
            }

            var tag = matches.Groups[1].Value;
            if (!extraValidTags.Contains(tag))
            {
                extraValidTags.Add(tag);
            }
        }

        public bool IsTagValid(string tag)
        {
            return extraValidTags.Contains(tag, StringComparer.OrdinalIgnoreCase);
        }
    }
}
