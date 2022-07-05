using Mutagen.Bethesda;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.FormKeys.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Exceptions;
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

        // INNRable equipment
        WeaponRanged,   // should have dn_CommonGun
        WeaponMelee,    // should have dn_CommonMelee

        Armor,      // should have dn_CommonArmor
        Clothes,    // should have dn_Clothes
        VaultSuit,  // should have dn_VaultSuit
        PowerArmor  // should have dn_PowerArmor
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

        public ItemType GetWeaponType(IWeaponGetter weapon)
        {
            // mines and grenades are their own things, otherwise "gun"
            if(weapon.Flags.HasFlag(Weapon.Flag.NotPlayable))
            {
                return ItemType.None;
            }

            // ammo?
            if(!weapon.Ammo.IsNull)
            {
                // this thing uses ammo, so not an explosive
                if(itemTypeData.keywordsWeaponMelee.ItemHasAny(weapon))
                {
                    return ItemType.WeaponMelee;
                }
                return ItemType.WeaponRanged;
            }

            var kwEdids = this.GetKeywordEdids(weapon);
            if(kwEdids.Contains("WeaponTypeGrenade"))
            {
                return ItemType.Grenade;
            }

            var equipType = weapon.EquipmentType.TryResolve(patcherState.LinkCache);

            if (equipType?.EditorID == "GrenadeSlot")
            {
                if(kwEdids.Contains("AnimsMine"))
                {
                    return ItemType.Mine;
                }

                // get projectile
                var proj = weapon.ExtraData?.ProjectileOverride.TryResolve(patcherState.LinkCache);
                if(proj == null)
                {
                    return ItemType.None;
                }

                if(proj.ExplosionAltTriggerProximity > 0)
                {
                    return ItemType.Mine;

                }

                return ItemType.Grenade;
            }

            if (itemTypeData.keywordsWeaponMelee.ItemHasAny(weapon))
            {
                return ItemType.WeaponMelee;
            }
            // otherwise, assume generic gun
            return ItemType.WeaponRanged;
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
            if(alch.HasKeyword(Fallout4.Keyword.ObjectTypeSyringerAmmo))
            {
                return ItemType.Syringe;
            }

            if(alch.HasKeyword(Fallout4.Keyword.ObjectTypeNukaCola))
            {
                return ItemType.Nukacola;
            }

            if(alch.HasKeyword(Fallout4.Keyword.ObjectTypeAlcohol))
            {
                return ItemType.Liquor;
            }

            
            // do food first, so that stuff with both drink and food KWs get classified as food
            if(itemTypeData.keywordListFood.ItemHasAny(alch))
            {
                return GetFoodType(alch);
            }

            if(itemTypeData.keywordListDrink.ItemHasAny(alch))
            {
                // generic drink
                return ItemType.Drink; 
            }
            if(itemTypeData.keywordListDevice.ItemHasAny(alch))
            {
                return ItemType.Device;
            }

            // now, check addiction
            var isAddictive = !alch.Addiction.IsNull;

            if(itemTypeData.keywordListChem.ItemHasAny(alch))
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
                    return GetFoodType(alch);
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

        private ItemType GetFoodType(IIngestibleGetter food)
        {
            var hasRads = food.Effects.Any(eff => eff.BaseEffect.TryResolve(patcherState.LinkCache)?.EditorID == "DamageRadiationChem");
            //food.Effects.Select(eff => eff.BaseEffect.TryResolve(patcherState.LinkCache)?.EditorID);
            if(!hasRads)
            {
                return ItemType.Food;
            }
            if(food.HasKeyword(Fallout4.Keyword.FruitOrVegetable))
            {
                return ItemType.FoodCrop;
            }

            if(itemTypeData.keywordListFoodDisease.ItemHasAny(food))
            {
                return ItemType.FoodRaw;
            }

            //prewar doesn't have HC_IgnoreAsFood
            if(!food.HasKeyword(Fallout4.Keyword.HC_IgnoreAsFood))
            {
                return ItemType.FoodPrewar;
            }

            return ItemType.Food;
        }

        public ItemType GetHolotapeType(IHolotapeGetter holotape)
        {
            try
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
            catch(MalformedDataException e)
            {
                Console.WriteLine("Exception while processing " + holotape.FormKey.ToString() + ": " + e.Message + ". This thing can't be processed.");
                return ItemType.None;
            }
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

            //IEnumerable<string> keywordEdids = GetKeywordEdids(book);

            
            if(itemTypeData.keywordsPerkmag.ItemHasAny(book) || HasAnyScript(book, itemTypeData.scriptListPerkMag))
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

            if (miscItem.HasKeyword(Fallout4.Keyword.ObjectTypeLooseMod) || IsLooseMod(miscItem))
            //if (keywordEdids.Contains("ObjectTypeLooseMod") || IsLooseMod(miscItem))
            {
                return ItemType.LooseMod;
            }

            var cmpo = miscItem.Components;
            if (cmpo != null && cmpo.Count > 0)
            {
                // shipment, scrap, or resource
                // could theoretically also be a scrappable loose mod
                if(HasScript(miscItem, "ShipmentScript"))
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
            
            if (miscItem.HasKeyword(Fallout4.Keyword.FeaturedItem))
            {
                return ItemType.Collectible;
            }

            if (itemTypeData.keywordsQuest.ItemHasAny(miscItem))
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

        private static bool HasScript(IHaveVirtualMachineAdapterGetter item, String scriptName)
        {
            if (item?.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            var scripts = item.VirtualMachineAdapter.Scripts;
            foreach (var script in scripts)
            {
                if(script.Name.Equals(scriptName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
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
            return itemTypeData.keywordsGlobalBlacklist.ItemHasAny(item);
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
