using Mutagen.Bethesda;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Synthesis;
using Noggog;

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

            LoadDictionaries();
        }

        private IEnumerable<string> GetKeywordEdids(IKeywordedGetter<IKeywordGetter> item)
        {
            return item.Keywords?
                .Select(kw => kw.TryResolve(patcherState.LinkCache)?.EditorID)
                .NotNull() ?? new List<string>();
        }

        public ItemType GetAlchType(IIngestibleGetter alch)
        {
            if (IsItemBlacklisted(alch))
            {
                return ItemType.None;
            }
            var model = alch.Model?.File;
            if(model.IsNullOrEmpty())
            {
                // so apparently I'm not tagging model-less ALCHs
                return ItemType.None;
            }

            var keywordEdids = this.GetKeywordEdids(alch);
            if(keywordEdids.Contains("ObjectTypeSyringerAmmo"))
            {
                return ItemType.Syringe;
            }

            if (keywordEdids.Contains("ObjectTypeNukaCola"))
            {
                return ItemType.Nukacola;
            }

            if (keywordEdids.Contains("ObjectTypeAlcohol"))
            {
                return ItemType.Liquor;
            }

            
            // do food first, so that stuff with both drink and food KWs get classified as food
            if(itemTypeData.keywordListFood.matchesAny(keywordEdids))
            {
                return GetFoodType(alch, keywordEdids);
            }

            if(itemTypeData.keywordListDrink.matchesAny(keywordEdids))
            {
                // generic drink
                return ItemType.Drink; 
            }
            if(itemTypeData.keywordListDevice.matchesAny(keywordEdids))
            {
                return ItemType.Device;
            }

            // now, check addiction
            var isAddictive = !alch.Addiction.IsNull;

            if(itemTypeData.keywordListChem.matchesAny(keywordEdids))
            {
                if(isAddictive)
                {
                    return ItemType.BadChem;
                }
                return ItemType.GoodChem;
            }

            // model list tool and device
            if (itemTypeData.whitelistModelDevice.matches(model))
            {
                return ItemType.Device;
            }

            if (itemTypeData.whitelistModelTool.matches(model))
            {
                return ItemType.Tool;
            }

            // now try sound
            var soundEdid = alch.ConsumeSound.TryResolve(patcherState.LinkCache)?.EditorID;
            if(soundEdid != null)
            {
                if(soundEdid == "NPCHumanDrinkGeneric")
                {
                    if(isAddictive)
                    {
                        return ItemType.Liquor;
                    }
                    return ItemType.Drink;
                }

                if(itemTypeData.soundListFood.matches(soundEdid))
                {
                    var isFoodItem = alch.Flags.HasFlag(Ingestible.Flag.FoodItem);
                    var isMedicine = alch.Flags.HasFlag(Ingestible.Flag.Medicine);

                    if(!isFoodItem && isMedicine)
                    {
                        if(isAddictive)
                        {
                            return ItemType.BadChem;
                        }
                        return ItemType.GoodChem;
                    }
                    return GetFoodType(alch, keywordEdids);
                }

                if(itemTypeData.soundListChem.matches(soundEdid))
                {
                    if(isAddictive)
                    {
                        return ItemType.BadChem;
                    }
                    return ItemType.GoodChem;
                }

                if(itemTypeData.soundListDevice.matches(soundEdid))
                {
                    return ItemType.Device;
                }

                if (itemTypeData.soundListTool.matches(soundEdid))
                {
                    return ItemType.Tool;
                }
            }

            if(alch.Flags.HasFlag(Ingestible.Flag.Medicine))
            {
                return ItemType.GoodChem;
            }

            return ItemType.None;
        }

        private ItemType GetFoodType(IIngestibleGetter food, IEnumerable<string> kwEdids)
        {
            var hasRads = food.Effects.Any(eff => eff.BaseEffect.TryResolve(patcherState.LinkCache)?.EditorID == "DamageRadiationChem");
            //food.Effects.Select(eff => eff.BaseEffect.TryResolve(patcherState.LinkCache)?.EditorID);
            if(!hasRads)
            {
                return ItemType.Food;
            }

            if(kwEdids.Contains("FruitOrVegetable"))
            {
                return ItemType.FoodCrop;
            }

            if(itemTypeData.keywordListFoodDisease.matchesAny(kwEdids))
            {
                return ItemType.FoodRaw;
            }

            //prewar doesn't have HC_IgnoreAsFood
            if(!kwEdids.Contains("HC_IgnoreAsFood"))
            {
                return ItemType.FoodPrewar;
            }

            return ItemType.Food;
        }

        public ItemType GetHolotapeType(IHolotapeGetter holotape)
        {
            if (IsBlacklistedByName(holotape.Name?.String) ||
                IsBlacklistedByEditorId(holotape.EditorID) ||
                IsBlacklistedByScript(holotape))
            {
                return ItemType.None;
            }

            switch(holotape.Data)
            {
                case IHolotapeVoiceGetter:
                case IHolotapeSoundGetter:
                    // in these cases, consider it to be 100% a generic holotape
                    return ItemType.Holotape;
                case IHolotapeProgramGetter holotapeProgram:
                    if(itemTypeData.programListGame.matches(holotapeProgram.File))
                    {
                        return ItemType.HolotapeGame;
                    }                    
                    break;
            }
            
            // if still alive here, continue with heuristics.
            // check EDID and Name
            if (itemTypeData.nameListSettings.matches(holotape.Name?.String) || itemTypeData.edidListSettings.matches(holotape.EditorID))
            {
                return ItemType.HolotapeSettings;
            }

            return ItemType.Holotape;
        }

        public ItemType GetBookType(IBookGetter book)
        {
            if(IsScriptedItemBlacklisted(book))
            {
                return ItemType.None;
            }
            var modelName = book.Model?.File;
            if (itemTypeData.whitelistModelNews.matches(modelName) || HasAnyScript(book, itemTypeData.scriptListNews))
            {
                return ItemType.News;
            }

            IEnumerable<string> keywordEdids = GetKeywordEdids(book);

            if(itemTypeData.keywordListPerkmag.matchesAny(keywordEdids))
            {
                return ItemType.Perkmag;
            }

            return ItemType.Note;
        }

        public ItemType GetAmmoType(IAmmunitionGetter ammo)
        {
            if(ammo.Flags.HasFlag(Ammunition.Flag.NonPlayable))
            {
                return ItemType.None;
            }

            if (IsItemBlacklisted(ammo))
            {
                return ItemType.None;
            }

            return ItemType.Ammo;
        }

        public ItemType GetKeyType(IKeyGetter keyItem)
        {
            if (IsScriptedItemBlacklisted(keyItem))
            {
                return ItemType.None;
            }

            //var edid = keyItem.EditorID;
            var model = keyItem.Model?.File;

            if (itemTypeData.modelListKey.matches(model))
            {
                return ItemType.Key;
            }

            if (itemTypeData.modelListCard.matches(model))
            {
                return ItemType.KeyCard;
            }

            if (itemTypeData.modelListPassword.matches(model))
            {
                return ItemType.KeyPassword;
            }

            return ItemType.Key;
        }

        public ItemType GetMiscType(IMiscItemGetter miscItem)
        {
            if(IsScriptedItemBlacklisted(miscItem))
            {
                return ItemType.None;
            }


            if (HasAnyScript(miscItem, itemTypeData.scriptListPipBoy)) {
                return ItemType.PipBoy;
            }

            IEnumerable<string> keywordEdids = GetKeywordEdids(miscItem);

            if (keywordEdids.Contains("ObjectTypeLooseMod") || IsLooseMod(miscItem))
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

            if (itemTypeData.keywordListQuest.matchesAny(keywordEdids))
            {
                return ItemType.Quest;
            }

            if (itemTypeData.modelListPipBoy.matches(miscItem.Model?.File))
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

        private void LoadDictionaries()
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

        private bool IsLooseMod(IMiscItemGetter miscItem)
        {
            return looseModToOmodLookup.ContainsKey(miscItem.FormKey);
        }

        private static bool HasAnyScript(IHaveVirtualMachineAdapterGetter item, MatchingList list)
        {
            if (item?.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            var scripts = item.VirtualMachineAdapter.Scripts;
            foreach (var script in scripts)
            {
                if(list.matches(script.Name))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsScriptedItemBlacklisted<T>(T item)
            where T: IHaveVirtualMachineAdapterGetter, ITranslatedNamedRequiredGetter, IMajorRecordGetter, IKeywordedGetter<IKeywordGetter>
        {
            return (
                IsBlacklistedByName(item.Name.String) ||
                IsBlacklistedByEditorId(item.EditorID) || 
                IsBlacklistedByScript(item) ||
                IsBlacklistedByKeyword(item)
            );
        }

        private bool IsItemBlacklisted<T>(T item)
            where T : ITranslatedNamedRequiredGetter, IMajorRecordGetter, IKeywordedGetter<IKeywordGetter>
        {
            return (
                IsBlacklistedByName(item.Name.String) ||
                IsBlacklistedByEditorId(item.EditorID) ||
                IsBlacklistedByKeyword(item)
            );
        }

        private bool IsBlacklistedByKeyword(IKeywordedGetter<IKeywordGetter> item)
        {
            if (item.Keywords == null)
            {
                return false;
            }

            return itemTypeData.blacklistKeyword.matchesAny(GetKeywordEdids(item));
        }

        private bool IsBlacklistedByName(string? name)
        {
            if(null == name)
            {
                return false;
            }

            return itemTypeData.blacklistName.matches(name);
        }

        private bool IsBlacklistedByEditorId(string? edid)
        {
            if(edid == null)
            {
                return false;
            }

            return itemTypeData.blacklistEdid.matches(edid);
        }

        private bool IsBlacklistedByScript(IHaveVirtualMachineAdapterGetter item)
        {
            if(item.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            var scripts = item.VirtualMachineAdapter.Scripts;
            foreach (var script in scripts)
            {
                if(itemTypeData.blacklistScript.matches(script.Name))
                {
                    return true;
                }                
            }

           return false;
        }
    }
}
