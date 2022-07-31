using ItemTagger.ItemTypeFinder;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using System.Text.RegularExpressions;
using ItemTagger.TaggingConfigs;
using Mutagen.Bethesda.Plugins.Aspects;
using Noggog;
using Mutagen.Bethesda.Plugins.Exceptions;
using Mutagen.Bethesda.FormKeys.Fallout4;
using ItemTagger.Helper;

namespace ItemTagger
{
    internal class TaggingProcessor
    {
        private readonly TaggingConfiguration taggingConfig;

        private readonly TaggerSettings settings;
        private IPatcherState<IFallout4Mod, IFallout4ModGetter> state;

        private readonly ItemTyper itemTyper;

        private static readonly Regex TAG_EXTRACT_REGEX = new(@"^[\[\]()|{}]([^\[\]()|{}]+)[\[\]()|{}].+$", RegexOptions.Compiled);

        private static readonly Regex TAG_STRIP_COMPONENTS = new(@"{{{[^{}]*}}}\s*$", RegexOptions.Compiled);

        private static readonly Regex REMOVE_BRACKETS = new(@"^[\[{(](.+)[\]})]$", RegexOptions.Compiled);
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
        }

        private static string GetTaggedName(string newTag, string inputName, string suffix = "")
        {
            if(newTag == "")
            {
                return inputName + suffix;
            }

            return newTag + " " + inputName + suffix;
        }

        private void ProcessArmors()
        {
            foreach(var armor in state.LoadOrder.PriorityOrder.Armor().WinningOverrides())
            {
                var prevName = armor.Name?.String;

                if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                {
                    continue;
                }

                var curType = itemTyper.GetArmorType(armor);
                switch(curType)
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

            // now, special case: if this thing has INNRs, don't actually prefix it. Assume it's INNRs are correct.
            if (!item.InstanceNaming.IsNull)
            {
                //item.FormKey.ToString
                Console.WriteLine("Not tagging " + item.GetDebugString() + " with " + prefix + ", because it has INNRs");
                return;
            }

            var nameBase = CleanName(prevName);

            var newItem = state.PatchMod.Armors.GetOrAddAsOverride(item);
            newItem.Name = GetTaggedName(prefix, nameBase);
        }

        private void ProcessArmorWithINNRs(IArmorGetter armor, ItemType armorType)
        {
            // TODO: check this armor's INNR, if it has any.
            // do we even need to process this?
            if (!armor.InstanceNaming.IsNull && armor.ObjectTemplates?.Count > 0)
            {
                return;
            }

            var newOverride = state.PatchMod.Armors.GetOrAddAsOverride(armor);

            // set an INNR
            if (armor.InstanceNaming.IsNull)
            {
                switch(armorType)
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
            foreach(var weap in state.LoadOrder.PriorityOrder.Weapon().WinningOverrides())
            {
                var prevName = weap.Name?.String;

                if (prevName.IsNullOrEmpty() || HasValidTag(prevName))
                {
                    continue;
                }

                var curType = itemTyper.GetWeaponType(weap);

                switch(curType)
                {
                    case ItemType.WeaponRanged:
                    case ItemType.WeaponMelee:
                        ProcessWeaponWithINNRs(weap, curType);
                        break;
                    default:
                        ProcessWeaponWithoutINNRs(prevName, weap, curType);
                        break;
                }
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

            // now, special case: if this thing has INNRs, don't actually prefix it. Assume it's INNRs are correct.
            if(!item.InstanceNaming.IsNull)
            {
                //item.FormKey.ToString
                Console.WriteLine("Not tagging " + item.GetDebugString() + " with " + prefix + ", because it has INNRs");
                return;
            }

            var newItem = state.PatchMod.Weapons.GetOrAddAsOverride(item);
            var nameBase = CleanName(prevName);
            
            newItem.Name = GetTaggedName(prefix, nameBase);
        }

        private void ProcessWeaponWithINNRs(IWeaponGetter weapon, ItemType type)
        {
            // TODO: check this weapon's INNR, if it has any.
            // do we even need to process this?
            if(!weapon.InstanceNaming.IsNull && weapon.ObjectTemplates?.Count > 0)
            {
                return;
            }

            var newOverride = state.PatchMod.Weapons.GetOrAddAsOverride(weapon);

            // set an INNR
            if (weapon.InstanceNaming.IsNull)
            {
                if(type == ItemType.WeaponMelee)
                {
                    newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_CommonMelee);
                } 
                else
                {
                    newOverride.InstanceNaming.SetTo(Fallout4.InstanceNamingRules.dn_CommonGun);
                }

            }

            if((weapon.ObjectTemplates?.Count ?? 0) == 0)
            {
                newOverride.ObjectTemplates ??= new();
                newOverride.ObjectTemplates.Add(new ObjectTemplate<Weapon.Property>()
                {
                    Default = true,
                    AddonIndex = -1
                });
            }
         
        }

        private void ProcessAlch()
        {
            foreach(var alch in state.LoadOrder.PriorityOrder.Ingestible().WinningOverrides())
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
            foreach(var tape in state.LoadOrder.PriorityOrder.Holotape().WinningOverrides())
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
            foreach(var book in state.LoadOrder.PriorityOrder.Book().WinningOverrides())
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
            foreach(var ammo in ammos)
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
            foreach(var key in keys)
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
                catch(ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Exception while processing "+misc.FormKey.ToString()+": "+e.Message+". This might be a bug in Synthesis...");
                }
                catch(Exception e)
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
            name = TAG_STRIP_COMPONENTS.Replace(name, "").Trim();

            if(stripAllTags)
            {
                var existingTag = ExtractTag(name);
                name = name[(existingTag.Length + 3)..];
            }
            else
            {
                if (taggingConfig.HasDeprecatedTags())
                {
                    var existingTag = ExtractTag(name);
                    if (taggingConfig.IsTagDeprecated(existingTag))
                    {
                        // strip existing tag
                        // should be the length of existingTag+3
                        name = name[(existingTag.Length + 3)..];
                    }
                }
            }

            var firstPart = name[..1];
            var lastPart = name[^1..];

            if (settings.RemoveBrackets)
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
                .Select(kwName => CleanName(kwName, true)) ?? new List<string>();

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
    }
}
