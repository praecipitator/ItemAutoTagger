using ItemTagger.ItemTypeFinder;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using System.Text.RegularExpressions;

namespace ItemTagger
{
    internal class TaggingProcessor
    {
        private TaggingConfiguration taggingConfig;

        private TaggerSettings settings;
        private IPatcherState<IFallout4Mod, IFallout4ModGetter> state;

        private ItemTyper itemTyper;
        public TaggingProcessor(
            TaggingConfiguration taggingConf,
            TaggerSettings settings,
            IPatcherState<IFallout4Mod, IFallout4ModGetter> state
        )
        {
            this.taggingConfig = taggingConf;
            this.state = state;
            this.settings = settings;

            itemTyper = new ItemTyper(state);
        }

        public void process()
        {
            processMiscs();
        }

        private void processMiscs()
        {
            // process MISCs
            var miscs = state.LoadOrder.PriorityOrder.MiscItem().WinningOverrides();

            foreach (var misc in miscs)
            {
                // before even trying to process this, check if we even should
                var prevName = misc.Name?.String;

                if (prevName == null || !shouldTag(prevName))
                {
                    continue;
                }

                var curType = itemTyper.getMiscType(misc);
                if (curType != ItemType.None)
                {
                    var prefix = taggingConfig[curType];
                    if (prefix != "")
                    {
                        var newMisc = state.PatchMod.MiscItems.GetOrAddAsOverride(misc);
                        if (settings.UseComponentString && misc.Components != null && misc.Components.Count > 0)
                        {
                            // make a component string
                            newMisc.Name = prefix + " " + prevName + getComponentString(misc.Components);
                        }
                        else
                        {
                            newMisc.Name = prefix + " " + prevName;
                        }
                    }
                }
            }
        }

        private string getComponentString(IReadOnlyList<IMiscItemComponentGetter> components)
        {
            var cmpNames = components
                .Select(cmpo => cmpo.Component.TryResolve(state.LinkCache)?.Name?.String)
                .Where(kw => kw != null) ?? new List<string>();

            var joinedStr = string.Join(",", cmpNames);
            if (joinedStr == "")
            {
                return "";
            }

            return "{{{" + joinedStr + "}}}";
        }

        private static string extractTag(string name)
        {
            var r = new Regex(@"^[\[\]\(\){}|]([^\[\]\(\){}|]+)[\[\]\(\){}|]");

            var matches = r.Match(name);

            if (!matches.Success)
            {
                return "";
            }

            return matches.Groups[1].Value;
        }

        private static bool isCharSeparator(char c)
        {
            return c == '[' || c == ']' || c == '(' || c == ')' || c == '{' || c == '}' || c == '|';
        }

        private bool shouldTag(string name)
        {
            var trimmedName = name.Trim();
            // first, if the whole name is basically a tag, like "[my weird holotape]"
            if (isCharSeparator(trimmedName.First()) && isCharSeparator(trimmedName.Last()))
            {
                return true;
            }

            var existingTag = extractTag(trimmedName);
            if (existingTag == "")
            {
                return true;
            }

            return !taggingConfig.isTagValid(existingTag);
        }
    }
}
