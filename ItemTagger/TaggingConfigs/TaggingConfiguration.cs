using ItemTagger.ItemTypeFinder;
using System.Text.RegularExpressions;

namespace ItemTagger.TaggingConfigs
{
    public class TaggingConfiguration : Dictionary<ItemType, string>
    {
        private readonly HashSet<string> extraValidTags = new();
        private readonly HashSet<string> deprecatedTags = new();

        private static readonly Regex TAG_EXTRACT_REGEX = new(@"^[\[\]()|{}]([^\[\]()|{}]+)[\[\]()|{}]$", RegexOptions.Compiled);

        public TaggingConfiguration()
        {

        }

        /// <summary>
        /// Adds extra tags to be considered valid
        /// </summary>
        /// <param name="extraTags">The tags must be WITHOUT BRACKETS. They will not be cleanead!</param>
        public void AddExtraTags(params string[] extraTags)
        {
            extraValidTags.UnionWith(extraTags);
        }

        /// <summary>
        /// Adds tags which should be definitely stripped away and replaced
        /// </summary>
        /// <param name="extraTags">The tags must be WITHOUT BRACKETS. They will not be cleanead!</param>
        public void AddDeprecatedTags(params string[] extraTags)
        {
            deprecatedTags.UnionWith(extraTags);
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

        public bool IsTagDeprecated(string tag)
        {
            return deprecatedTags.Contains(tag, StringComparer.OrdinalIgnoreCase);
        }

        public bool HasDeprecatedTags()
        {
            return deprecatedTags.Count > 0;
        }
    }
}
