using ItemTagger.Helper;
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
    public enum ItemType
    {
        None, // none/do not tag
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
        GoodChem,   // Cures, Medicine, Aid, etc
        BadChem,    // Drugs, Addictive chems
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
        private static readonly ItemType[] TYPES_ARMOR = {
            ItemType.None,
            ItemType.Armor,
            ItemType.PowerArmor,
            ItemType.Clothes,
            ItemType.VaultSuit,
            ItemType.PipBoy
        };

        private static readonly ItemType[] TYPES_WEAPON = {
            ItemType.None,
            ItemType.WeaponMelee,
            ItemType.WeaponRanged,
            ItemType.Grenade,
            ItemType.Mine,
            ItemType.Tool,
            ItemType.Device
        };

        private static readonly ItemType[] TYPES_ALCH = {
            ItemType.None,
            ItemType.BadChem,
            ItemType.Device,
            ItemType.Drink,
            ItemType.Food,
            ItemType.FoodCrop,
            ItemType.FoodPrewar,
            ItemType.FoodRaw,
            ItemType.GoodChem,
            ItemType.Liquor,
            ItemType.Nukacola,
            ItemType.Syringe,
            ItemType.Tool
        };

        private static readonly ItemType[] TYPES_AMMO = {
            ItemType.None,
            ItemType.Ammo
        };

        private static readonly ItemType[] TYPES_HOLOTAPE = {
            ItemType.None,
            ItemType.Holotape,
            ItemType.HolotapeGame,
            ItemType.HolotapeSettings
        };

        private static readonly ItemType[] TYPES_BOOK = {
            ItemType.None,
            ItemType.News,
            ItemType.Note,
            ItemType.Perkmag
        };

        private static readonly ItemType[] TYPES_KEY = {
            ItemType.None,
            ItemType.Key,
            ItemType.KeyCard,
            ItemType.KeyPassword
        };

        private static readonly ItemType[] TYPES_MISC = { 
            ItemType.None,
            ItemType.Ammo,
            ItemType.Collectible,
            ItemType.Currency,
            ItemType.Device,
            ItemType.Holotape,
            ItemType.LooseMod,
            ItemType.OtherMisc,
            ItemType.PipBoy,
            ItemType.Quest,
            ItemType.Resource,
            ItemType.Scrap,
            ItemType.Shipment,
            ItemType.Tool,
            ItemType.Valuable
        };
        

        private readonly IPatcherState<IFallout4Mod, IFallout4ModGetter> patcherState;

        private readonly Dictionary<FormKey, FormKey> looseModToOmodLookup = new();

        private readonly ItemTypeData itemTypeData = new();

        private readonly Dictionary<FormKey, ItemType> itemOverrides;

        public ItemTyper(IPatcherState<IFallout4Mod, IFallout4ModGetter> state, ItemTypeOverrides itemTypeOverrides)
        {
            this.patcherState = state;

            itemOverrides = itemTypeOverrides.GetMergedOverrides();
            itemOverrides.MergeWithoutOverwrite(itemTypeData.hardcodedOverrides);

            LoadDictionaries();
        }

        public ItemType GetArmorType(IArmorGetter armor)
        {
            if (itemOverrides.ContainsKey(armor.FormKey))
            {
                return itemOverrides.GetValueOrDefault(armor.FormKey, ItemType.None);
            }

            if (IsBlacklisted(armor))
            {
                return ItemType.None;
            }

            // check the race.
            // if the race is human, all is fine.
            // if not, it can still be a usable, but not always
            if(!armor.Race.Equals(Fallout4.Race.HumanRace))
            {
                if(armor.WorldModel == null || (armor.WorldModel.Male == null && armor.WorldModel.Female == null))
                {
                    // it *seems* that the various "skins" and "tans" never have these models, but regular items
                    // have at least one
                    return ItemType.None;
                }
            }

            // check this BEFORE non-playable
            if (armor.BipedBodyTemplate != null && armor.BipedBodyTemplate.FirstPersonFlags == BipedObjectFlag.Pipboy)
            {
                return ItemType.PipBoy;
            }

            if (armor.MajorFlags.HasFlag(Armor.MajorFlag.NonPlayable))
            {
                return ItemType.None;
            }

            var edidType = itemTypeData.edidMatchLists.GetMatchingType(armor.EditorID, TYPES_ARMOR);
            if (edidType != null)
            {
                return (ItemType)edidType;
            }

            // otherwise, this is one of the 4 armor types
            if (armor.HasKeyword(Fallout4.Keyword.VaultSuitKeyword))
            {
                return ItemType.VaultSuit;
            }

            if(armor.HasKeyword(Fallout4.Keyword.ArmorTypePower))
            {
                return ItemType.PowerArmor;
            }

            if(armor.ArmorRating == 0)
            {
                return ItemType.Clothes;
            }

            return ItemType.Armor;
        }

        public ItemType GetWeaponType(IWeaponGetter weapon)
        {
            if (itemOverrides.ContainsKey(weapon.FormKey))
            {
                return itemOverrides.GetValueOrDefault(weapon.FormKey, ItemType.None);
            }

            // mines and grenades are their own things, otherwise "gun"
            if (weapon.Flags.HasFlag(Weapon.Flag.NotPlayable))
            {
                return ItemType.None;
            }

            if (IsBlacklisted(weapon))
            {
                return ItemType.None;
            }

            var edidType = itemTypeData.edidMatchLists.GetMatchingType(weapon.EditorID, TYPES_WEAPON);
            if (edidType != null)
            {
                return (ItemType)edidType;
            }

            // ammo?
            if (!weapon.Ammo.IsNull)
            {
                // this thing uses ammo, so not an explosive
                if(weapon.HasAnyKeyword(itemTypeData.keywordsWeaponMelee)) 
                {
                    return ItemType.WeaponMelee;
                }
                return ItemType.WeaponRanged;
            }

            if(weapon.HasKeyword(Fallout4.Keyword.WeaponTypeGrenade))
            {
                return ItemType.Grenade;
            }

            if(weapon.EquipmentType.Equals(Fallout4.EquipType.GrenadeSlot))
            {
                if(weapon.HasKeyword(Fallout4.Keyword.AnimsMine))
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

            if (weapon.HasAnyKeyword(itemTypeData.keywordsWeaponMelee))
            {
                return ItemType.WeaponMelee;
            }
            // otherwise, assume generic gun
            return ItemType.WeaponRanged;
        }

        public ItemType GetAlchType(IIngestibleGetter alch)
        {
            if (itemOverrides.ContainsKey(alch.FormKey))
            {
                return itemOverrides.GetValueOrDefault(alch.FormKey, ItemType.None);
            }

            if (IsBlacklisted(alch))
            {
                return ItemType.None;
            }

            var model = alch.Model?.File;
            if(model.IsNullOrEmpty())
            {
                // so apparently I'm not tagging model-less ALCHs
                return ItemType.None;
            }

            var edidType = itemTypeData.edidMatchLists.GetMatchingType(alch.EditorID, TYPES_ALCH);
            if (edidType != null)
            {
                return (ItemType)edidType;
            }

            // check the ObjectType* keywords first
            if (alch.HasKeyword(Fallout4.Keyword.ObjectTypeSyringerAmmo))
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

            var isAddictive = !alch.Addiction.IsNull;
            if (alch.HasKeyword(Fallout4.Keyword.ObjectTypeChem))
            {
                if (isAddictive)
                {
                    return ItemType.BadChem;
                }
                return ItemType.GoodChem;
            }

            // food has prio over drink
            if(alch.HasKeyword(Fallout4.Keyword.ObjectTypeFood))
            {
                return GetFoodType(alch);
            }

            if (alch.HasKeyword(Fallout4.Keyword.ObjectTypeDrink))
            {
                return ItemType.Drink;
            }

            // continue with heuristics
            if (alch.HasAnyKeyword(itemTypeData.keywordListFood))
            {
                return GetFoodType(alch);
            }

            if(alch.HasAnyKeyword(itemTypeData.keywordListDrink))
            {
                // generic drink
                return ItemType.Drink; 
            }
            if(alch.HasAnyKeyword(itemTypeData.keywordListDevice))
            {
                return ItemType.Device;
            }            

            if(alch.HasAnyKeyword(itemTypeData.keywordListChem))
            {
                if(isAddictive)
                {
                    return ItemType.BadChem;
                }
                return ItemType.GoodChem;
            }

            // model list tool and device
            if (itemTypeData.whitelistModelDevice.Matches(model))
            {
                return ItemType.Device;
            }

            if (itemTypeData.whitelistModelTool.Matches(model))
            {
                return ItemType.Tool;
            }

            // now try sound
            if (!alch.ConsumeSound.IsNull)
            {
                // this seems to work
                if (alch.ConsumeSound.Equals(Fallout4.SoundDescriptor.NPCHumanDrinkGeneric))
                {
                    if (isAddictive)
                    {
                        return ItemType.Liquor;
                    }
                    return ItemType.Drink;
                }

                if(alch.ConsumeSound.IsAnyOf(itemTypeData.soundListFood))
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

                if (alch.ConsumeSound.IsAnyOf(itemTypeData.soundListChem))
                {
                    if(isAddictive)
                    {
                        return ItemType.BadChem;
                    }
                    return ItemType.GoodChem;
                }

                if (alch.ConsumeSound.IsAnyOf(itemTypeData.soundListDevice))
                {
                    return ItemType.Device;
                }

                if (alch.ConsumeSound.IsAnyOf(itemTypeData.soundListTool))
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

        /// <summary>
        /// helper, if I know that this ALCH is a food, but not which one
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        private ItemType GetFoodType(IIngestibleGetter food)
        {
            var hasRads = food.HasEffect(Fallout4.MagicEffect.DamageRadiationChem);

            if(!hasRads)
            {
                return ItemType.Food;
            }
            if(food.HasKeyword(Fallout4.Keyword.FruitOrVegetable))
            {
                return ItemType.FoodCrop;
            }

            if(food.HasAnyKeyword(itemTypeData.keywordListFoodDisease))
            {
                return ItemType.FoodRaw;
            }

            //prewar doesn't have HC_IgnoreAsFood
            if(!food.HasKeyword(Fallout4.Keyword.HC_IgnoreAsFood))
            {
                return ItemType.FoodPrewar;
            }

            // after keyword matching, match meshes and such
            if(itemTypeData.modelListFoodCrop.Matches(food.Model?.File))
            {
                return ItemType.FoodCrop;
            }

            return ItemType.Food;
        }

        public ItemType GetHolotapeType(IHolotapeGetter holotape)
        {
            if (itemOverrides.ContainsKey(holotape.FormKey))
            {
                return itemOverrides.GetValueOrDefault(holotape.FormKey, ItemType.None);
            }

            try
            {
                if (IsBlacklisted(holotape))
                {
                    return ItemType.None;
                }

                var edidType = itemTypeData.edidMatchLists.GetMatchingType(holotape.EditorID, TYPES_HOLOTAPE);
                if (edidType != null)
                {
                    return (ItemType)edidType;
                }

                switch (holotape.Data)
                {
                    case IHolotapeVoiceGetter:
                    case IHolotapeSoundGetter:
                        // in these cases, consider it to be 100% a generic holotape
                        return ItemType.Holotape;
                    case IHolotapeProgramGetter holotapeProgram:
                        if(itemTypeData.programListGame.Matches(holotapeProgram.File))
                        {
                            return ItemType.HolotapeGame;
                        }                    
                        break;
                }
            
                // if still alive here, continue with heuristics.
                // check EDID and Name
                if (itemTypeData.nameListSettings.Matches(holotape.Name?.String))
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
            if (itemOverrides.ContainsKey(book.FormKey))
            {
                return itemOverrides.GetValueOrDefault(book.FormKey, ItemType.None);
            }

            if (IsBlacklisted(book))
            {
                return ItemType.None;
            }

            var edidType = itemTypeData.edidMatchLists.GetMatchingType(book.EditorID, TYPES_BOOK);
            if (edidType != null)
            {
                return (ItemType)edidType;
            }

            var modelName = book.Model?.File;
            if (itemTypeData.whitelistModelNews.Matches(modelName) || book.HasAnyScript(itemTypeData.scriptListNews))
            {
                return ItemType.News;
            }
            
            if(book.HasAnyKeyword(itemTypeData.keywordsPerkmag) || book.HasAnyScript(itemTypeData.scriptListPerkMag))
            {
                return ItemType.Perkmag;
            }

            return ItemType.Note;
        }

        public ItemType GetAmmoType(IAmmunitionGetter ammo)
        {
            if (itemOverrides.ContainsKey(ammo.FormKey))
            {
                return itemOverrides.GetValueOrDefault(ammo.FormKey, ItemType.None);
            }
            
            if (ammo.Flags.HasFlag(Ammunition.Flag.NonPlayable))
            {
                return ItemType.None;
            }
            
            if (IsBlacklisted(ammo))
            {
                return ItemType.None;
            }

            var edidType = itemTypeData.edidMatchLists.GetMatchingType(ammo.EditorID, TYPES_AMMO);
            if (edidType != null)
            {
                return (ItemType)edidType;
            }

            return ItemType.Ammo;
        }

        public ItemType GetKeyType(IKeyGetter keyItem)
        {
            if (itemOverrides.ContainsKey(keyItem.FormKey))
            {
                return itemOverrides.GetValueOrDefault(keyItem.FormKey, ItemType.None);
            }

            if (IsBlacklisted(keyItem))
            {
                return ItemType.None;
            }

            var edidType = itemTypeData.edidMatchLists.GetMatchingType(keyItem.EditorID, TYPES_KEY);
            if (edidType != null)
            {
                return (ItemType)edidType;
            }

            //var edid = keyItem.EditorID;
            var model = keyItem.Model?.File;

            if (itemTypeData.modelListKey.Matches(model))
            {
                return ItemType.Key;
            }

            if (itemTypeData.modelListCard.Matches(model))
            {
                return ItemType.KeyCard;
            }

            if (itemTypeData.modelListPassword.Matches(model))
            {
                return ItemType.KeyPassword;
            }

            return ItemType.Key;
        }

        public ItemType GetMiscType(IMiscItemGetter miscItem)
        {
            if(itemOverrides.ContainsKey(miscItem.FormKey))
            {
                return itemOverrides.GetValueOrDefault(miscItem.FormKey, ItemType.None);
            }

            if (IsBlacklisted(miscItem))
            {
                return ItemType.None;
            }

            var edidType = itemTypeData.edidMatchLists.GetMatchingType(miscItem.EditorID, TYPES_MISC);
            if (edidType != null)
            {
                return (ItemType)edidType;
            }

            if (miscItem.HasAnyScript(itemTypeData.scriptListPipBoy)) {
                return ItemType.PipBoy;
            }

            if (miscItem.HasKeyword(Fallout4.Keyword.ObjectTypeLooseMod) || IsLooseMod(miscItem))
            {
                return ItemType.LooseMod;
            }

            var cmpo = miscItem.Components;
            if (cmpo != null && cmpo.Count > 0)
            {
                // shipment, scrap, or resource
                // could theoretically also be a scrappable loose mod
                if(miscItem.HasScript("ShipmentScript"))
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

            if (miscItem.HasAnyKeyword(itemTypeData.keywordsQuest))
            {
                return ItemType.Quest;
            }

            if (itemTypeData.modelListPipBoy.Matches(miscItem.Model?.File))
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

        // IsBlacklisted versions for different items
        private bool IsBlacklisted(IHolotapeGetter holotape)
        {
            return IsBlacklistedByName(holotape.Name?.String) ||
                    IsBlacklistedByEditorId(holotape.EditorID) ||
                    IsBlacklistedByScript(holotape);
        }

        private bool IsBlacklisted(IIngestibleGetter item)
        {
            return (
                IsBlacklistedByName(item.Name?.String) ||
                IsBlacklistedByEditorId(item.EditorID) ||
                IsBlacklistedByKeyword(item)
            );
        }

        private bool IsBlacklisted(IAmmunitionGetter item)
        {
            return (
                IsBlacklistedByName(item.Name?.String) ||
                IsBlacklistedByEditorId(item.EditorID) ||
                IsBlacklistedByKeyword(item)
            );
        }

        private bool IsBlacklisted<T>(T item)
            where T: IHaveVirtualMachineAdapterGetter, ITranslatedNamedRequiredGetter, IMajorRecordGetter, IKeywordedGetter<IKeywordGetter>
        {
            return (
                IsBlacklistedByName(item.Name.String) ||
                IsBlacklistedByEditorId(item.EditorID) || 
                IsBlacklistedByScript(item) ||
                IsBlacklistedByKeyword(item)
            );
        }

        // specialized blacklist helpers
        private bool IsBlacklistedByKeyword(IKeywordedGetter<IKeywordGetter> item)
        {
            return item.HasAnyKeyword(itemTypeData.keywordsGlobalBlacklist);
        }

        private bool IsBlacklistedByName(string? name)
        {
            if(null == name)
            {
                return false;
            }

            return itemTypeData.blacklistName.Matches(name);
        }

        private bool IsBlacklistedByEditorId(string? edid)
        {
            if(edid == null)
            {
                return false;
            }

            return itemTypeData.blacklistEdid.Matches(edid);
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
                if(itemTypeData.blacklistScript.Matches(script.Name))
                {
                    return true;
                }                
            }

           return false;
        }
    }
}
