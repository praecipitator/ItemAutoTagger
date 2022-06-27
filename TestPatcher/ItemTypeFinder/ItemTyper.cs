using Mutagen.Bethesda;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Synthesis;

namespace ItemTagger.ItemTypeFinder
{
    /*
     * Types can overlap
     */
    public enum ItemType
    {
        None, // none/unknown/do not tag
        // MISC types. 
        Shipment,   // Shipments
        Scrap,      // Scrap, MISCs which contain components
        Resource,   // Resources, MISCs which a meant to represent one type of component
        LooseMod,   // Loose modifications
        Collectible,// "Collectible" MISCs
        Quest,      // Quest items
        Currency,   // "Currency", MISCs with zero weight and non-zero value
        Valuable,   // MISCs with more value than weight
        OtherMisc,  // all other MISCs, trash
        // ALCH types.
        GoodChem,   // Cures etc
        BadChem,    // Addictive chems
        Food,       // generic food, selfcooked. Usually radless
        FoodRaw,    // raw food, has rads, has disease risk
        FoodCrop,   // crops, has rads
        FoodPrewar, // prewar packaged, has rads
        Drink,      // generic drinkable
        Liquor,     // Alcoholic beverages
        Nukacola,   // Nuka Cola of any kind
        Syringe,    // Syringer ammo
        Device,     // Consumables which are supposed to be devices instead of something to eat, like the Stealth Boy
        Tool,       // Similar to above, but for more low-tech things. Like SimSettlements Town Meeting Gavel, or the Companion Whistle
        // BOOK
        News,       // Newspaper
        Note,       // Any paper note
        Perkmag,    // Perk Magazine
        // WEAP
        Mine,       // Mine
        Grenade,    // Grenade
        // etc
        Key,                // Key
        KeyCard,            // Keycard
        KeyPassword,        // Password, usually written on a note or holotape

        Ammo,               // Generic Ammo
        Holotape,           // Holotape
        HolotapeGame,       // Game Holotape
        HolotapeSettings,   // Settings Holotape

        // EQUIPMENT
        PipBoy, // player's pip-boy, also the MISC item from Vault88
        Gun,    //A weapon which cannot be renamed, but needs an adjustment of INNRs.
        Armor,  //A piece of armor or clothing which cannot be renamed, and needs an adjustment of INNRs.
    }

    public class ItemTyper
    {
        private IPatcherState<IFallout4Mod, IFallout4ModGetter> patcherState;

        private Dictionary<FormKey, FormKey> looseModToOmodLookup = new Dictionary<FormKey, FormKey>();

        private ItemTypeData itemTypeData = new();

        public ItemTyper(IPatcherState<IFallout4Mod, IFallout4ModGetter> state)
        {
            this.patcherState = state;

            loadDictionaries();
        }

        public ItemType getMiscType(IMiscItemGetter miscItem)
        {
            if(isItemBlacklisted(miscItem))
            {
                return ItemType.None;
            }
            // if scriptname ends with :PipboyMiscItemScript -> pipboy
            if (this.hasScript(miscItem, ":PipboyMiscItemScript", false, true))
            {
                return ItemType.PipBoy;
            }

            IEnumerable<string?> keywordEdids = miscItem.Keywords?
                .Select(kw => kw.TryResolve(patcherState.LinkCache)?.EditorID)
                .Where(kw => kw != null) ?? new List<string>();

            //IKeywordedExt.HasKeyword()


            if (keywordEdids.Contains("ObjectTypeLooseMod") || isLooseMod(miscItem))
            {
                return ItemType.LooseMod;
            }

            var cmpo = miscItem.Components;
            if (cmpo != null && cmpo.Count > 0)
            {
                // shipment, scrap, or resource
                // could theoretically also be a scrappable loose mod

                if (keywordEdids.Contains("ShipmentScript"))
                {
                    return ItemType.Shipment;
                }

                if (cmpo.Count == 1)
                {
                    // maybe this is a resource
                    var comp = cmpo[0];
                    var scrapItem = comp.Component.TryResolve(patcherState.LinkCache)?.ScrapItem;

                    if (scrapItem != null && scrapItem.Equals(miscItem))
                    {
                        return ItemType.Resource;
                    }
                }

                return ItemType.Scrap;
            }

            if (keywordEdids.Contains("FeaturedItem"))
            {
                return ItemType.Collectible;
            }

            if (keywordEdids.Contains("VendorItemNoSale") || keywordEdids.Contains("VendorItemNoSale"))
            {
                return ItemType.Quest;
            }

            if (miscItem.Model?.File?.ToLower() == "props\\pipboymiscitem\\pipboymisc01.nif")
            {
                return ItemType.PipBoy;
            }

            if (miscItem.Weight == 0 && miscItem.Value > 0)
            {
                return ItemType.Currency;
            }

            if (miscItem.Value > 0 && miscItem.Value >= miscItem.Weight)
            {
                return ItemType.Valuable;
            }

            return ItemType.OtherMisc;
        }

        private void loadDictionaries()
        {
            var omods = this.patcherState.LoadOrder.PriorityOrder.AObjectModification().WinningOverrides();

            foreach (var omod in omods)
            {
                if (omod.LooseMod != null)
                {
                    looseModToOmodLookup[omod.LooseMod.FormKey] = omod.FormKey;
                }
            }
        }

        private bool isLooseMod(IMiscItemGetter miscItem)
        {
            return looseModToOmodLookup.ContainsKey(miscItem.FormKey);
        }

        private bool hasKeyword(IMiscItemGetter miscItem, String keywordEdid)
        {
            // IKeywordedExt
            if (miscItem.Keywords == null)
            {
                return false;
            }
            foreach (var kw in miscItem.Keywords)
            {
                var resolvedKw = kw.TryResolve(patcherState.LinkCache);
                if (null != resolvedKw && resolvedKw.EditorID == keywordEdid)
                {
                    return true;
                }

            }
            return false;
        }

        private bool hasScript(IHaveVirtualMachineAdapterGetter miscItem, String scriptName, bool matchPrefix = false, bool matchSuffix = false)
        {
            if (miscItem?.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            var scripts = miscItem.VirtualMachineAdapter.Scripts;

            foreach (var script in scripts)
            {
                if (script.Name == scriptName)
                {
                    return true;
                }
                if (matchPrefix && script.Name.StartsWith(scriptName))
                {
                    return true;
                }
                if (matchSuffix && script.Name.EndsWith(scriptName))
                {
                    return true;
                }
            }

            return false;
        }

        private bool isItemBlacklisted<T>(T item)
            where T: IHaveVirtualMachineAdapterGetter, INamedGetter, IMajorRecordGetter, IKeywordedGetter<IKeywordGetter>
        {
            return (
                isBlacklistedByName(item.Name) ||
                isBlacklistedByEditorId(item.EditorID) || 
                isBlacklistedByScript(item) ||
                isBlacklistedByKeyword(item)
            );
        }

        private bool isBlacklistedByKeyword(IKeywordedGetter<IKeywordGetter> item)
        {
            if (item.Keywords == null)
            {
                return false;
            }

            // now, dark magic
            var kwEdids = item.Keywords
                .Select(kw => kw.TryResolve(patcherState.LinkCache)?.EditorID)
                .Where(kw => kw != null) ?? new List<string>();

            return kwEdids.Any(edid => itemTypeData.blacklistedKeywords.Contains(edid, StringComparer.OrdinalIgnoreCase));
        }

        private bool isBlacklistedByName(string? name)
        {
            if(null == name)
            {
                return false;
            }

            itemTypeData.blacklistedNameSubstrings.Any(substr => name.Contains(substr, StringComparison.OrdinalIgnoreCase));

            return false;
        }

        private bool isBlacklistedByEditorId(string? edid)
        {
            if(edid == null)
            {
                return false;
            }

            return itemTypeData.blacklistedEdidPrefixes.Any(prefix => edid.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }

        private bool isBlacklistedByScript(IHaveVirtualMachineAdapterGetter item)
        {
            if(item.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            var scripts = item.VirtualMachineAdapter.Scripts;
            foreach (var script in scripts)
            {
                if(itemTypeData.blacklistedScripts.Contains(script.Name, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }

                // now prefixes
                if(itemTypeData.blacklistedScriptPrefixes.Any(
                        scriptPrefix => script.Name.StartsWith(scriptPrefix, StringComparison.OrdinalIgnoreCase)
                    ))
                {
                    return true;
                }
                
            }

           return false;
        }
    }
}
