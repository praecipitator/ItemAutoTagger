using ItemTagger.Helper;
using ItemTagger.ItemTypeFinder;
using ItemTagger.TaggingConfigs;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.FormKeys.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Exceptions;
using Mutagen.Bethesda.Synthesis;
using Noggog;
using System.Text.RegularExpressions;

namespace ItemTagger
{
    public class TaggingProcessor
    {
        private readonly TaggingConfiguration taggingConfig;

        private readonly TaggerSettings settings;
        private readonly IPatcherState<IFallout4Mod, IFallout4ModGetter> state;

        private readonly ItemTyper itemTyper;

        private static readonly Regex TAG_EXTRACT_REGEX = new(@"^[\[\]()|{}]([^\[\]()|{}]+)[\[\]()|{}].+$", RegexOptions.Compiled);

        private static readonly Regex TAG_STRIP_COMPONENTS = new(@"{{{[^{}]*}}}\s*$", RegexOptions.Compiled);

        private static readonly Regex REMOVE_BRACKETS = new(@"^[\[{(]([^\[{(\]})]+)[\]})]$", RegexOptions.Compiled);

        private readonly HashSet<IInstanceNamingRulesGetter> relevantInnrs = new();

        private readonly HashSet<FormKey> irrelevantInnrs;

        public TaggingProcessor(
            TaggingConfiguration taggingConf,
            TaggerSettings settings,
            IPatcherState<IFallout4Mod, IFallout4ModGetter> state
        )
        {
            this.taggingConfig = taggingConf;
            this.state = state;
            this.settings = settings;

            itemTyper = new ItemTyper(state, settings.ItemTypeConfig);

            // innrTypeMapping = new();
            irrelevantInnrs = new();
        }

        public void Process()
        {
            ProcessMiscs();
            ProcessKeys();
            ProcessAmmo();
            ProcessBooks();
            ProcessHolotapes();
            ProcessAlch();
            // now, equipment
            ProcessWeapons();
            ProcessArmors();
            // NEW: leveled items
            ProcessLeveledItems();
            // finally, INNRs
            ProcessInnrs();
        }

        public static string GetTaggedName(string newTag, string inputName, string suffix = "")
        {
            if (newTag == "")
            {
                return inputName + suffix;
            }

            return newTag + " " + inputName + suffix;
        }

        private void ProcessArmors()
        {
            foreach (var armor in state.LoadOrder.PriorityOrder.Armor().WinningOverrides())
            {
                var prevName = armor.Name?.String;

                if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                {
                    continue;
                }

                var curType = itemTyper.GetArmorType(armor);
                switch (curType)
                {
                    case ItemType.Armor:
                    case ItemType.Clothes:
                    case ItemType.VaultSuit:
                    case ItemType.PowerArmor:
                        ProcessArmorWithINNRs(armor, curType);
                        break;

                    default:
                        ProcessArmorWithoutINNRs(prevName, armor, curType);
                        break;
                }
                // in any case, do template names
                ProcessArmorTemplateNames(armor, curType);
            }
        }

        private void ProcessArmorWithoutINNRs(string prevName, IArmorGetter item, ItemType type)
        {
            if (type == ItemType.None)
            {
                return;
            }

            var prefix = taggingConfig[type];
            if (prefix == "")
            {
                return;
            }

            if (!item.InstanceNaming.IsNull)
            {
                // this should be handled by INNR patching
                return;
            }

            var nameBase = CleanName(prevName);
            if (nameBase.Length == 0)
            {
                return;
            }

            var newItem = state.PatchMod.Armors.GetOrAddAsOverride(item);
            newItem.Name = GetTaggedName(prefix, nameBase);
        }

        private void ProcessInnrs()
        {
            if (!settings.PatchINNRs)
            {
                return;
            }

            foreach (var innr in relevantInnrs)
            {
                var type = itemTyper.GetInnrType(innr);
                if (type != ItemType.None)
                {
                    TagInnr(innr, type);
                }
            }
        }

        private bool IsInnrTagged(IInstanceNamingRulesGetter innr)
        {
            foreach (var set in innr.RuleSets)
            {
                // disregard empty sets until we find a non-empty one
                if (set == null || set.Names == null)
                {
                    continue;
                }

                foreach (var n in set.Names)
                {
                    if (n.Name != null && !n.Name.String.IsNullOrEmpty())
                    {
                        var theString = n.Name.String;
                        // we found a non-empty name, make the stuff depend on this
                        // either this IS a valid tag, or it HAS a valid tag
                        return (IsValidTag(theString) || HasValidTag(theString));
                    }
                }
            }
            return false;
        }

        private void TagInnr(IInstanceNamingRulesGetter innr, ItemType type)
        {
            var prefix = taggingConfig[type];
            if (prefix == "")
            {
                return;
            }
            // it's probably not safe to add more than the default 10 rules, so we have 3 options
            //  - there is still at least one empty Rulesets at the back
            //      -> move all forward by one, put the new one at the front
            //  - there are no empty rulesets at the back, but there is an empty one at the front
            //      -> modify the first-most one to add the tag
            //  - there are no empty rulesets nowhere
            //      -> take the first ruleset, whatever it is. Prefix all the names with the tag.
            //      -> If there isn't one without conditions, append a new one, containing just the tag.

            var firstNotEmpty = -1;
            var lastNotEmpty = -1;
            var i = 0;
            foreach (var set in innr.RuleSets)
            {
                var isEmpty = false;
                if (set.Names == null)
                {
                    isEmpty = true;
                }
                else
                {
                    isEmpty = true;
                    foreach (var n in set.Names)
                    {
                        if (n.Name != null && !n.Name.String.IsNullOrEmpty())
                        {
                            isEmpty = false;
                            break;
                        }
                    }
                }

                if (!isEmpty)
                {
                    if (i > lastNotEmpty)
                    {
                        lastNotEmpty = i;
                    }
                    if (firstNotEmpty < 0)
                    {
                        firstNotEmpty = i;
                    }
                }
                i++;
            }

            var newOverride = state.PatchMod.InstanceNamingRules.GetOrAddAsOverride(innr);

            // we have space to move forward
            if (lastNotEmpty < 9)
            {
                //var newOverride = state.PatchMod.InstanceNamingRules.GetOrAddAsOverride(innr);
                // we can move stuff by one
                newOverride.RuleSets.ShiftByOne();
                newOverride.RuleSets[0] = GetNewNamingRuleSet(prefix);
                return;
            }

            // there is an empty one at the front, reuse it
            if (firstNotEmpty > 0)
            {
                //var newOverride = state.PatchMod.InstanceNamingRules.GetOrAddAsOverride(innr);
                int lastEmpty = firstNotEmpty - 1;

                newOverride.RuleSets[lastEmpty] = GetNewNamingRuleSet(prefix);
                return;
            }

            // finally, take entry 0, prefix all of the strings there
            bool haveUnconditional = false;
            InstanceNamingRuleSet firstRs = newOverride.RuleSets[0];
            firstRs.Names ??= new();// this shouldn't actually be null, the loop above should have caught that
            foreach (var n in firstRs.Names)
            {
                if ((n.Keywords == null || n.Keywords.Count == 0) && (n.Properties == null))
                {
                    haveUnconditional = true;
                }
                string? properStringName = n.Name?.String;

                if (properStringName.IsNullOrEmpty())
                {
                    n.Name = prefix;
                }
                else
                {
                    var nameBase = CleanName(properStringName);

                    if (nameBase.IsNullOrEmpty())
                    {
                        n.Name = prefix;
                    }
                    else
                    {
                        n.Name = GetTaggedName(prefix, nameBase);
                    }
                }
            }
            if (!haveUnconditional)
            {
                // append a new one
                var namingRule = new InstanceNamingRule
                {
                    Index = 10000,
                    Name = prefix
                };

                firstRs.Names.Add(namingRule);
            }

            // Console.WriteLine("Could not autopatch INNR " + innr.ToString() + ": found no empty ruleset.");
        }

        private void ProcessLeveledItems()
        {
            foreach (var lvli in state.LoadOrder.PriorityOrder.LeveledItem().WinningOverrides())
            {
                var name = lvli.OverrideName.ToNonNullString();
                if (name == "" || lvli.Entries == null || HasValidTag(name))
                {
                    continue;
                }

                var type = itemTyper.GetLeveledItemType(lvli);
                TagLeveledItem(lvli, type);
            }
        }

        private void TagLeveledItem(ILeveledItemGetter item, ItemType type)
        {
            if(type == ItemType.None)
            {
                return;
            }

            var prefix = taggingConfig[type];
            if(prefix == "")
            {
                return;
            }

            // otherwise do it
            var newOverride = state.PatchMod.LeveledItems.GetOrAddAsOverride(item);
            //newItem.Name = GetTaggedName(prefix, nameBase);
            var baseName = CleanName(newOverride.OverrideName.ToNonNullString());
            newOverride.OverrideName = GetTaggedName(prefix, baseName);
        }

        private static InstanceNamingRuleSet GetNewNamingRuleSet(string name, ushort index = 10000)
        {
            var newEntry = new InstanceNamingRuleSet();

            newEntry.Names ??= new();

            var namingRule = new InstanceNamingRule
            {
                Index = index,
                Name = name
            };

            newEntry.Names.Add(namingRule);

            return newEntry;
        }

        private void CheckInnrForPatching(IFormLinkNullableGetter<IInstanceNamingRulesGetter> innrGetter, ItemType type)
        {
            if (!settings.PatchINNRs)
            {
                return;
            }
            if (irrelevantInnrs.Contains(innrGetter.FormKey))
            {
                return;
            }

            if (!ShouldPatchINNR(innrGetter, type, out IInstanceNamingRulesGetter? innr) || innr == null)
            {
                irrelevantInnrs.Add(innrGetter.FormKey);
                return;
            }

            relevantInnrs.Add(innr);
        }

        private static bool IsInnrTypeCorrect(IInstanceNamingRulesGetter innr, ItemType type)
        {
            switch (innr.Target)
            {
                case InstanceNamingRules.RuleTarget.Armor:
                    if (!ItemTyper.IsTypeArmor(type))
                    {
                        return false;
                    }
                    break;

                case InstanceNamingRules.RuleTarget.Weapon:
                    if (!ItemTyper.IsTypeWeapon(type))
                    {
                        return false;
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        private bool ShouldPatchINNR(IFormLinkNullableGetter<IInstanceNamingRulesGetter> innrGetter, ItemType type, out IInstanceNamingRulesGetter? innr)
        {
            innr = null;
            if (!settings.PatchINNRs || innrGetter.IsNull)
            {
                return false;
            }

            innr = innrGetter.TryResolve(state.LinkCache);
            if (innr == null)
            {
                Console.WriteLine("Failed to resolve INNR " + innrGetter.ToString());
                return false;
            }

            if (itemTyper.IsBlacklisted(innr))
            {
                return false;
            }

            if (!IsInnrTypeCorrect(innr, type))
            {
                Console.WriteLine("Will not process INNR " + innrGetter.ToString() + " for type " + type.ToString() + ", because it has an incorrect type");
                return false;
            }

            if (IsInnrTagged(innr))
            {
                return false;
            }

            return true;
        }

        private void ProcessArmorWithINNRs(IArmorGetter armor, ItemType armorType)
        {
            // do we even need to process this?
            if (!armor.InstanceNaming.IsNull && armor.ObjectTemplates?.Count > 0)
            {
                // potentially process the INNR here
                CheckInnrForPatching(armor.InstanceNaming, armorType);
                return;
            }

            var newOverride = state.PatchMod.Armors.GetOrAddAsOverride(armor);

            // set an INNR
            if (armor.InstanceNaming.IsNull)
            {
                switch (armorType)
                {
                    case ItemType.VaultSuit:
                        newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_VaultSuit);
                        break;

                    case ItemType.PowerArmor:
                        newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_PowerArmor);
                        break;

                    case ItemType.Clothes:
                        newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_Clothes);
                        break;

                    default:
                        newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_CommonArmor);
                        break;
                }
            }
            else
            {
                // otherwise do something with INNRs
                CheckInnrForPatching(armor.InstanceNaming, armorType);
            }

            if (0 == (armor.ObjectTemplates?.Count ?? 0))
            {
                newOverride.ObjectTemplates ??= new();
                newOverride.ObjectTemplates.Add(new ObjectTemplate<Armor.Property>()
                {
                    Default = true,
                    AddonIndex = -1
                });
            }
        }

        private void ProcessWeapons()
        {
            foreach (var weap in state.LoadOrder.PriorityOrder.Weapon().WinningOverrides())
            {
                var prevName = weap.Name?.String;

                if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                {
                    continue;
                }

                var curType = itemTyper.GetWeaponType(weap);

                switch (curType)
                {
                    case ItemType.WeaponRanged:
                    case ItemType.WeaponMelee:
                        ProcessWeaponWithINNRs(weap, curType);
                        break;

                    default:
                        ProcessWeaponWithoutINNRs(prevName, weap, curType);
                        break;
                }
                // in any case, do template names
                ProcessWeaponTemplateNames(weap, curType);
            }
        }

        private void ProcessWeaponWithoutINNRs(string prevName, IWeaponGetter item, ItemType type)
        {
            if (type == ItemType.None)
            {
                return;
            }

            var prefix = taggingConfig[type];
            if (prefix == "")
            {
                return;
            }

            // this shouldn't actually happen...
            if (!item.InstanceNaming.IsNull)
            {
                //item.FormKey.ToString
                Console.WriteLine("Not tagging " + item.GetDebugString() + " with " + prefix + ", because it has INNRs");
                return;
            }

            var nameBase = CleanName(prevName);
            if (nameBase.Length == 0)
            {
                return;
            }
            var newItem = state.PatchMod.Weapons.GetOrAddAsOverride(item);

            newItem.Name = GetTaggedName(prefix, nameBase);
        }

        private void ProcessWeaponWithINNRs(IWeaponGetter weapon, ItemType type)
        {
            // do we even need to process this?
            if (!weapon.InstanceNaming.IsNull && weapon.ObjectTemplates?.Count > 0)
            {
                CheckInnrForPatching(weapon.InstanceNaming, type);
                return;
            }

            var newOverride = state.PatchMod.Weapons.GetOrAddAsOverride(weapon);

            // set an INNR
            if (weapon.InstanceNaming.IsNull)
            {
                if (type == ItemType.WeaponMelee)
                {
                    newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_CommonMelee);
                }
                else
                {
                    newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_CommonGun);
                }
            }
            else
            {
                CheckInnrForPatching(weapon.InstanceNaming, type);
            }

            if ((weapon.ObjectTemplates?.Count ?? 0) == 0)
            {
                newOverride.ObjectTemplates ??= new();
                newOverride.ObjectTemplates.Add(new ObjectTemplate<Weapon.Property>()
                {
                    Default = true,
                    AddonIndex = -1
                });
            }
        }

        private bool ShouldProcessTemplates<T>(IReadOnlyList<IObjectTemplateGetter<T>> templates) where T : struct, Enum
        {
            foreach (var template in templates)
            {
                if (!template.IsEditorOnly)
                {
                    var name = template.Name.ToNonNullString();
                    if(name != "" && !HasValidTag(name))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ProcessArmorTemplateNames(IArmorGetter armor, ItemType type)
        {
            if (type == ItemType.None)
            {
                return;
            }
            var srcTemplates = armor.ObjectTemplates;
            if (srcTemplates == null || srcTemplates.Count <= 0 || !ShouldProcessTemplates(srcTemplates))
            {
                return;
            }

            var prefix = taggingConfig[type];
            if (prefix == "")
            {
                return;
            }

            // need override here
            var newOverride = state.PatchMod.Armors.GetOrAddAsOverride(armor);
            ExtendedList<ObjectTemplate<Armor.Property>>? newTemplates = newOverride.ObjectTemplates;
            if (newTemplates == null)
            {
                // this shouldn't happen
                return;
            }

            TagTemplateNames(newTemplates, prefix);
        }

        private void ProcessWeaponTemplateNames(IWeaponGetter weapon, ItemType type)
        {
            if (type == ItemType.None)
            {
                return;
            }
            var srcTemplates = weapon.ObjectTemplates;
            if (srcTemplates == null || srcTemplates.Count <= 0 || !ShouldProcessTemplates(srcTemplates))
            {
                return;
            }

            var prefix = taggingConfig[type];
            if (prefix == "")
            {
                return;
            }

            // need override here
            var newOverride = state.PatchMod.Weapons.GetOrAddAsOverride(weapon);
            ExtendedList<ObjectTemplate<Weapon.Property>>? newTemplates = newOverride.ObjectTemplates;
            if(newTemplates == null)
            {
                // this shouldn't happen
                return;
            }

            TagTemplateNames(newTemplates, prefix);
        }

        private void TagTemplateNames<T>(ExtendedList<ObjectTemplate<T>> newTemplates, string prefix) where T : struct, Enum
        {
            foreach (var template in newTemplates)
            {
                string curName = template.Name.ToNonNullString();
                if (!template.IsEditorOnly && curName != "" && !HasValidTag(curName))
                {
                    template.Name = GetTaggedName(prefix, curName);
                }
            }
        }

        private void ProcessAlch()
        {
            foreach (var alch in state.LoadOrder.PriorityOrder.Ingestible().WinningOverrides())
            {
                try
                {
                    var prevName = alch.Name?.String;

                    if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                    {
                        continue;
                    }

                    var curType = itemTyper.GetAlchType(alch);

                    TagItem(prevName, alch, curType, state.PatchMod.Ingestibles);
                }
                catch (Exception ex)
                {
                    throw RecordException.Enrich(ex, alch);
                }
            }
        }

        private void ProcessHolotapes()
        {
            foreach (var tape in state.LoadOrder.PriorityOrder.Holotape().WinningOverrides())
            {
                try
                {
                    var prevName = tape.Name?.String;

                    if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                    {
                        continue;
                    }

                    var curType = itemTyper.GetHolotapeType(tape);

                    TagItem(prevName, tape, curType, state.PatchMod.Holotapes);
                }
                catch (Exception ex)
                {
                    throw RecordException.Enrich(ex, tape);
                }
            }
        }

        private void ProcessBooks()
        {
            foreach (var book in state.LoadOrder.PriorityOrder.Book().WinningOverrides())
            {
                try
                {
                    var prevName = book.Name?.String;

                    if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                    {
                        continue;
                    }

                    var curType = itemTyper.GetBookType(book);

                    TagItem(prevName, book, curType, state.PatchMod.Books);
                }
                catch (Exception ex)
                {
                    throw RecordException.Enrich(ex, book);
                }
            }
        }

        private void ProcessAmmo()
        {
            var ammos = state.LoadOrder.PriorityOrder.Ammunition().WinningOverrides();
            foreach (var ammo in ammos)
            {
                try
                {
                    var prevName = ammo.Name?.String;
                    if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                    {
                        continue;
                    }

                    var curType = itemTyper.GetAmmoType(ammo);
                    TagItem(prevName, ammo, curType, state.PatchMod.Ammunitions);
                }
                catch (Exception ex)
                {
                    throw RecordException.Enrich(ex, ammo);
                }
            }
        }

        private void ProcessKeys()
        {
            var keys = state.LoadOrder.PriorityOrder.Key().WinningOverrides();
            foreach (var key in keys)
            {
                try
                {
                    var prevName = key.Name?.String;

                    if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                    {
                        continue;
                    }

                    var curType = itemTyper.GetKeyType(key);

                    TagItem(prevName, key, curType, state.PatchMod.Keys);
                }
                catch (Exception ex)
                {
                    throw RecordException.Enrich(ex, key);
                }
            }
        }

        private void ProcessMiscs()
        {
            var miscs = state.LoadOrder.PriorityOrder.MiscItem().WinningOverrides();

            foreach (var misc in miscs)
            {
                try
                {
                    ProcessMisc(misc);
                }
                /*catch(ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Exception while processing "+misc.FormKey.ToString()+": "+e.Message+". This might be a bug in Synthesis...");
                }*/
                catch (Exception e)
                {
                    throw RecordException.Enrich(e, misc);
                }
            }
        }

        private void ProcessMisc(IMiscItemGetter misc)
        {
            // before even trying to process this, check if we even should
            var prevName = misc.Name?.String;

            // for MISCs, even those with a valid tag might have to be processed
            if (prevName.IsNullOrEmpty())
            {
                return;
            }

            ItemType curType;
            if (HasValidTag(prevName))
            {
                // special case: check if we have components
                if (misc.Components != null && misc.Components.Count > 0)
                {
                    curType = itemTyper.GetMiscType(misc);
                    if (curType != ItemType.None)
                    {
                        var nameBase = CleanName(prevName);
                        if (nameBase.Length == 0)
                        {
                            return;
                        }

                        var newItem = state.PatchMod.MiscItems.GetOrAddAsOverride(misc);
                        var suffix = GetComponentString(misc.Components);

                        newItem.Name = GetTaggedName("", nameBase, suffix);
                    }
                }
                return;
            }

            curType = itemTyper.GetMiscType(misc);
            if (curType != ItemType.None)
            {
                // do not add component tags for Resource or Shipment, because it should be obvious anyway
                if (settings.UseComponentString && (curType != ItemType.Resource && curType != ItemType.Shipment) && misc.Components != null && misc.Components.Count > 0)
                {
                    TagItem(prevName, misc, curType, state.PatchMod.MiscItems, misc.Components);
                }
                else
                {
                    TagItem(prevName, misc, curType, state.PatchMod.MiscItems);
                }
            }
        }

        private string CleanName(string name, bool stripAllTags = false)
        {
            return GetCleanedName(name, taggingConfig, settings.RemoveBrackets, stripAllTags);
        }

        public static string GetCleanedName(string name, TaggingConfiguration taggingConfig, bool removeBrackets, bool stripAllTags)
        {
            name = TAG_STRIP_COMPONENTS.Replace(name, "").Trim();
            if (name.Length == 0)
            {
                return "";
            }

            if (stripAllTags)
            {
                var existingTag = ExtractTag(name);
                if (existingTag != "")
                {
                    name = name[(existingTag.Length + 3)..];
                }
            }
            else
            {
                if (taggingConfig.HasDeprecatedTags())
                {
                    var existingTag = ExtractTag(name);
                    if (existingTag.Length > 0 && taggingConfig.IsTagDeprecated(existingTag))
                    {
                        // strip existing tag
                        // should be the length of existingTag+3
                        name = name[(existingTag.Length + 3)..];
                    }
                }
            }

            var firstPart = name[..1];
            var lastPart = name[^1..];

            if (removeBrackets)
            {
                var matches = REMOVE_BRACKETS.Match(name);
                if (matches.Groups.Count >= 2)
                {
                    name = matches.Groups[1].Value.Trim();
                }
            }

            if (firstPart == "-" && lastPart != "-")
            {
                // strip the leading - off
                name = name[1..].Trim();
            }

            return name;
        }

        private void TagItem<T>(string prevName, IFallout4MajorRecordGetter item, ItemType type, Fallout4Group<T> group, IReadOnlyList<IMiscItemComponentGetter>? components)
            where T : Fallout4MajorRecord, ITranslatedNamedRequired
        {
            if (type == ItemType.None)
            {
                return;
            }

            var prefix = taggingConfig[type];
            if (prefix == "")
            {
                return;
            }

            var nameBase = CleanName(prevName);
            if (nameBase.Length == 0)
            {
                return;
            }

            var newItem = group.GetOrAddAsOverride(item);
            var suffix = "";
            if (null != components && components.Count > 0)
            {
                suffix = GetComponentString(components);
            }
            newItem.Name = GetTaggedName(prefix, nameBase, suffix);
        }

        private void TagItem<T>(string prevName, IFallout4MajorRecordGetter item, ItemType type, Fallout4Group<T> group)
            where T : Fallout4MajorRecord, ITranslatedNamedRequired
        {
            TagItem(prevName, item, type, group, null);
        }

        private string GetComponentString(IReadOnlyList<IMiscItemComponentGetter> components)
        {
            var cmpNames = components
                .Select(cmpo => cmpo.Component.TryResolve(state.LinkCache)?.Name?.String)
                .NotNull()
                .Select(kwName => kwName == null ? "" : CleanName(kwName, true))
                .Where(kwName => kwName != "") ?? new List<string>();

            if (!cmpNames.Any())
            {
                return "";
            }

            var joinedStr = string.Join(",", cmpNames);
            if (joinedStr == "")
            {
                return "";
            }

            return "{{{" + joinedStr + "}}}";
        }

        private static string ExtractTag(string name)
        {
            var matches = TAG_EXTRACT_REGEX.Match(name);

            if (matches.Groups.Count < 2)
            {
                return "";
            }

            return matches.Groups[1].Value;
        }

        private bool HasValidTag(string name, out string existingTag)
        {
            var trimmedName = name.Trim();
            existingTag = "";
            if (trimmedName.IsNullOrEmpty())
            {
                // this shouldn't actually happen
                return false;
            }

            existingTag = ExtractTag(trimmedName);

            if (existingTag == "")
            {
                return false;
            }

            return taggingConfig.IsTagValid(existingTag);
        }

        private bool HasValidTag(string name)
        {
            return HasValidTag(name, out _);
        }

        private bool IsValidTag(string name, bool onlyWithBrackets = true)
        {
            var matches = REMOVE_BRACKETS.Match(name);
            if (matches.Groups.Count >= 2)
            {
                name = matches.Groups[1].Value.Trim();
            }
            else
            {
                if (!onlyWithBrackets)
                {
                    // no bracket removal matches, so this is
                    return false;
                }
            }

            return taggingConfig.IsTagValid(name);
        }
    }
}
