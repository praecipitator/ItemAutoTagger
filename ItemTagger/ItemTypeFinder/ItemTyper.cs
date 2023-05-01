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
        Scrap,      // Scrap, MISCs which contain components, but are neither Shipments nor Resources
        Resource,   // Resources, MISCs which are meant to represent one type of component
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
            ItemType.Armor,
            ItemType.PowerArmor,
            ItemType.Clothes,
            ItemType.VaultSuit,
            ItemType.PipBoy
        };

        private static readonly ItemType[] TYPES_WEAPON = {
            ItemType.WeaponMelee,
            ItemType.WeaponRanged,
            ItemType.Grenade,
            ItemType.Mine,
            ItemType.Tool,
            ItemType.Device
        };

        private static readonly ItemType[] TYPES_WEAPON_EXPLOSIVE = {
            ItemType.Grenade,
            ItemType.Mine,
        };

        private static readonly ItemType[] TYPES_ALCH = {
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
            ItemType.Ammo
        };

        private static readonly ItemType[] TYPES_HOLOTAPE = {
            ItemType.Holotape,
            ItemType.HolotapeGame,
            ItemType.HolotapeSettings
        };

        private static readonly ItemType[] TYPES_BOOK = {
            ItemType.News,
            ItemType.Note,
            ItemType.Perkmag
        };

        private static readonly ItemType[] TYPES_KEY = {
            ItemType.Key,
            ItemType.KeyCard,
            ItemType.KeyPassword
        };

        private static readonly ItemType[] TYPES_MISC = {
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

        private readonly Dictionary<FormKey, HashSet<FormKey>> innrLookup = new();

        private readonly Dictionary<FormKey, ItemType> itemTypeCache = new();

        public ItemTyper(IPatcherState<IFallout4Mod, IFallout4ModGetter> state, List<GenericFormTypeMapping> itemTypeOverrides)
        {
            this.patcherState = state;

            itemOverrides = itemTypeOverrides.GetAsDictionary();
            itemOverrides.MergeWithoutOverwrite(itemTypeData.hardcodedOverrides);

            LoadDictionaries();
        }

        /// <summary>
        /// Tries to figure out if the given set of item types can be categorized into any of
        /// Armor, Grenade, WeaponRanged, GoodChem, Ammo, Holotape, Note, Key, Misc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ItemType ReduceItemTypeSet(HashSet<ItemType> input)
        {
            if(TYPES_ARMOR.ContainsAll(input))
            {
                return ItemType.Armor;
            }

            if(TYPES_WEAPON_EXPLOSIVE.ContainsAll(input))
            {
                return ItemType.Grenade;
            }

            if(TYPES_WEAPON.ContainsAll(input))
            {
                return ItemType.WeaponRanged;
            }

            if(TYPES_ALCH.ContainsAll(input))
            {
                return ItemType.GoodChem;
            }

            if(TYPES_AMMO.ContainsAll(input))
            {
                return ItemType.Ammo;
            }

            if (TYPES_HOLOTAPE.ContainsAll(input))
            {
                return ItemType.Holotape;
            }

            if(TYPES_BOOK.ContainsAll(input))
            {
                return ItemType.Note;
            }

            if (TYPES_KEY.ContainsAll(input))
            {
                return ItemType.Key;
            }

            if(TYPES_MISC.ContainsAll(input))
            {
                return ItemType.OtherMisc;
            }

            return ItemType.None;
        }

        public static bool IsTypeArmor(ItemType type)
        {
            return (type != ItemType.None && TYPES_ARMOR.Contains(type));
        }

        public static bool IsTypeWeapon(ItemType type)
        {
            return (type != ItemType.None && TYPES_WEAPON.Contains(type));
        }

        public ItemType GetUnknownItemType(IItemGetter item)
        {
            return item switch
            {
                IWeaponGetter weapon => GetWeaponType(weapon),
                IArmorGetter armor => GetArmorType(armor),
                IMiscItemGetter misc => GetMiscType(misc),
                IKeyGetter key => GetKeyType(key),
                IAmmunitionGetter ammo => GetAmmoType(ammo),
                IBookGetter book => GetBookType(book),
                IHolotapeGetter tape => GetHolotapeType(tape),
                IIngestibleGetter alch => GetAlchType(alch),
                ILeveledItemGetter lvli => GetLeveledItemType(lvli),
                _ => ItemType.None,
            };
        }

        public ItemType GetLeveledItemType(ILeveledItemGetter item)
        {
            if (itemTypeCache.TryGetValue(item.FormKey, out var result))
            {
                return result;
            }

            result = GetLeveledItemTypeUncached(item);
            itemTypeCache.Add(item.FormKey, result);
            return result;
        }

        public ItemType GetLeveledItemTypeUncached(ILeveledItemGetter lvli)
        {
            if(lvli.Entries == null)
            {
                return ItemType.None;
            }
            HashSet<ItemType> foundTypes = new();
            foreach (var entry in lvli.Entries)
            {
                if (entry.Data == null)
                {
                    continue;
                }
                //if (patcherState.LinkCache.TryResolve<IWeaponGetter>(item, out var weapon) && weapon != null)
                var item = entry.Data.Reference.TryResolve(patcherState.LinkCache);
                if (item == null)
                {
                    continue;
                }
                var curType = GetUnknownItemType(item);
                foundTypes.Add(curType);
            }
            return foundTypes.Count switch
            {
                0 => ItemType.None,
                1 => foundTypes.First(),
                _ => ReduceItemTypeSet(foundTypes),
            };
        }

        public ItemType GetInnrType(IInstanceNamingRulesGetter innr)
        {
            if (itemTypeCache.TryGetValue(innr.FormKey, out var result))
            {
                return result;
            }

            result = GetInnrTypeUncached(innr);
            itemTypeCache.Add(innr.FormKey, result);
            return result;
        }

        private ItemType GetInnrTypeUncached(IInstanceNamingRulesGetter innr)
        {
            // check overrides
            if (itemOverrides.ContainsKey(innr.FormKey))
            {
                return itemOverrides.GetValueOrDefault(innr.FormKey, ItemType.None);
            }

            return innr.Target switch
            {
                InstanceNamingRules.RuleTarget.Armor => GetArmorInnrType(innr),
                InstanceNamingRules.RuleTarget.Weapon => GetWeaponInnrType(innr),
                _ => ItemType.None,
            };
        }

        private ItemType GetWeaponInnrType(IInstanceNamingRulesGetter innr)
        {
            if (!innrLookup.TryGetValue(innr.FormKey, out var outSet) || outSet == null)
            {
                return ItemType.None;
            }

            HashSet<ItemType> foundTypes = new();

            foreach (var item in outSet)
            {
                if (patcherState.LinkCache.TryResolve<IWeaponGetter>(item, out var weapon) && weapon != null)
                {
                    if (innr.Target != InstanceNamingRules.RuleTarget.Weapon)
                    {
                        Console.WriteLine("WARNING: invalid INNR set for Weapon " + weapon.GetDebugString() + ", INNR type is " + innr.Target);
                        continue;
                    }

                    var weaponType = GetWeaponType(weapon);
                    if (weaponType != ItemType.None)
                    {
                        foundTypes.Add(weaponType);
                    }
                }
            }

            if (foundTypes.Count == 0)
            {
                return ItemType.None;
            }

            if (foundTypes.Count == 1)
            {
                return foundTypes.First();
            }

            if (foundTypes.Contains(ItemType.WeaponRanged))
            {
                return ItemType.WeaponRanged;
            }

            if (foundTypes.Contains(ItemType.WeaponMelee))
            {
                return ItemType.WeaponMelee;
            }

            if (foundTypes.Contains(ItemType.Grenade))
            {
                return ItemType.Grenade;
            }

            if (foundTypes.Contains(ItemType.Mine))
            {
                return ItemType.Mine;
            }

            return ItemType.WeaponRanged;
        }

        private ItemType GetArmorInnrType(IInstanceNamingRulesGetter innr)
        {
            if (!innrLookup.TryGetValue(innr.FormKey, out var outSet) || outSet == null)
            {
                return ItemType.None;
            }

            HashSet<ItemType> foundTypes = new();

            foreach (var item in outSet)
            {
                if (patcherState.LinkCache.TryResolve<IArmorGetter>(item, out var armor) && armor != null)
                {
                    if (innr.Target != InstanceNamingRules.RuleTarget.Armor)
                    {
                        Console.WriteLine("WARNING: invalid INNR set for Armor " + armor.GetDebugString() + ", INNR type is " + innr.Target);
                        continue;
                    }

                    var armorType = GetArmorType(armor);
                    if (armorType != ItemType.None)
                    {
                        foundTypes.Add(armorType);
                    }
                }
            }

            if (foundTypes.Count == 0)
            {
                return ItemType.None;
            }

            if (foundTypes.Count == 1)
            {
                return foundTypes.First();
            }

            // otherwise:
            // { Armor, Clothing, VaultSuit, PowerArmor, PipBoy } -> Armor
            // { Clothing, VaultSuit, PowerArmor, PipBoy } -> Armor
            // { Clothing, VaultSuit, PipBoy } -> Clothing
            // { VaultSuit, PipBoy } -> VaultSuit

            // otherwise, just assume Clothing

            if (foundTypes.Contains(ItemType.Armor) || foundTypes.Contains(ItemType.PowerArmor))
            {
                return ItemType.Armor;
            }

            if (foundTypes.Contains(ItemType.Clothes))
            {
                return ItemType.Clothes;
            }

            if (foundTypes.Contains(ItemType.VaultSuit))
            {
                return ItemType.VaultSuit;
            }

            return ItemType.Clothes;
        }

        public ItemType GetArmorType(IArmorGetter armor)
        {
            if (itemTypeCache.TryGetValue(armor.FormKey, out var result))
            {
                return result;
            }

            result = GetArmorTypeUncached(armor);
            itemTypeCache.Add(armor.FormKey, result);

            return result;
        }

        private ItemType GetArmorTypeUncached(IArmorGetter armor)
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
            if (!armor.Race.Equals(Fallout4.Race.HumanRace))
            {
                if (armor.WorldModel == null || (armor.WorldModel.Male == null && armor.WorldModel.Female == null))
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

            if (armor.HasKeyword(Fallout4.Keyword.ArmorTypePower))
            {
                return ItemType.PowerArmor;
            }

            if (armor.ArmorRating == 0)
            {
                return ItemType.Clothes;
            }

            return ItemType.Armor;
        }

        public ItemType GetWeaponType(IWeaponGetter weapon)
        {
            if (itemTypeCache.TryGetValue(weapon.FormKey, out var result))
            {
                return result;
            }

            result = GetWeaponTypeUncached(weapon);
            itemTypeCache.Add(weapon.FormKey, result);

            return result;
        }

        private ItemType GetWeaponTypeUncached(IWeaponGetter weapon)
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
                if (weapon.HasAnyKeyword(itemTypeData.keywordsWeaponMelee))
                {
                    return ItemType.WeaponMelee;
                }
                return ItemType.WeaponRanged;
            }

            if (weapon.HasKeyword(Fallout4.Keyword.WeaponTypeGrenade))
            {
                return ItemType.Grenade;
            }

            if (weapon.EquipmentType.Equals(Fallout4.EquipType.GrenadeSlot))
            {
                if (weapon.HasKeyword(Fallout4.Keyword.AnimsMine))
                {
                    return ItemType.Mine;
                }

                // get projectile
                var proj = weapon.ExtraData?.ProjectileOverride.TryResolve(patcherState.LinkCache);
                if (proj == null)
                {
                    return ItemType.None;
                }

                if (proj.ExplosionAltTriggerProximity > 0)
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
            if (itemTypeCache.TryGetValue(alch.FormKey, out var result))
            {
                return result;
            }

            result = GetAlchTypeUncached(alch);
            itemTypeCache.Add(alch.FormKey, result);

            return result;
        }

        private ItemType GetAlchTypeUncached(IIngestibleGetter alch)
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
            if (model.IsNullOrEmpty())
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

            if (alch.HasKeyword(Fallout4.Keyword.ObjectTypeNukaCola))
            {
                return ItemType.Nukacola;
            }

            if (alch.HasKeyword(Fallout4.Keyword.ObjectTypeAlcohol))
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
            if (alch.HasKeyword(Fallout4.Keyword.ObjectTypeFood))
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

            if (alch.HasAnyKeyword(itemTypeData.keywordListDrink))
            {
                // generic drink
                return ItemType.Drink;
            }
            if (alch.HasAnyKeyword(itemTypeData.keywordListDevice))
            {
                return ItemType.Device;
            }

            if (alch.HasAnyKeyword(itemTypeData.keywordListChem))
            {
                if (isAddictive)
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

                if (alch.ConsumeSound.IsAnyOf(itemTypeData.soundListFood))
                {
                    var isFoodItem = alch.Flags.HasFlag(Ingestible.Flag.FoodItem);
                    var isMedicine = alch.Flags.HasFlag(Ingestible.Flag.Medicine);

                    if (!isFoodItem && isMedicine)
                    {
                        if (isAddictive)
                        {
                            return ItemType.BadChem;
                        }
                        return ItemType.GoodChem;
                    }
                    return GetFoodType(alch);
                }

                if (alch.ConsumeSound.IsAnyOf(itemTypeData.soundListChem))
                {
                    if (isAddictive)
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

            if (alch.Flags.HasFlag(Ingestible.Flag.Medicine))
            {
                return ItemType.GoodChem;
            }

            // probably better than not tagging it
            return ItemType.GoodChem;
        }

        /// <summary>
        /// helper, if I know that this ALCH is a food, but not which one
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        private ItemType GetFoodType(IIngestibleGetter food)
        {
            var hasRads = food.HasEffect(Fallout4.MagicEffect.DamageRadiationChem);

            if (!hasRads)
            {
                return ItemType.Food;
            }
            if (food.HasKeyword(Fallout4.Keyword.FruitOrVegetable))
            {
                return ItemType.FoodCrop;
            }

            if (food.HasAnyKeyword(itemTypeData.keywordListFoodDisease))
            {
                return ItemType.FoodRaw;
            }

            //prewar doesn't have HC_IgnoreAsFood
            if (!food.HasKeyword(Fallout4.Keyword.HC_IgnoreAsFood))
            {
                return ItemType.FoodPrewar;
            }

            // after keyword matching, match meshes and such
            if (itemTypeData.modelListFoodCrop.Matches(food.Model?.File))
            {
                return ItemType.FoodCrop;
            }

            return ItemType.Food;
        }

        public ItemType GetHolotapeType(IHolotapeGetter holotape)
        {
            if (itemTypeCache.TryGetValue(holotape.FormKey, out var result))
            {
                return result;
            }

            result = GetHolotapeTypeUncached(holotape);
            itemTypeCache.Add(holotape.FormKey, result);

            return result;
        }

        private ItemType GetHolotapeTypeUncached(IHolotapeGetter holotape)
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
                        if (itemTypeData.programListGame.Matches(holotapeProgram.File))
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
            catch (MalformedDataException e)
            {
                Console.WriteLine("Exception while processing " + holotape.FormKey.ToString() + ": " + e.Message + ". This thing can't be processed.");
                return ItemType.None;
            }
        }

        public ItemType GetBookType(IBookGetter book)
        {
            if (itemTypeCache.TryGetValue(book.FormKey, out var result))
            {
                return result;
            }

            result = GetBookTypeUncached(book);
            itemTypeCache.Add(book.FormKey, result);

            return result;
        }

        private ItemType GetBookTypeUncached(IBookGetter book)
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

            if (book.HasAnyKeyword(itemTypeData.keywordsPerkmag) || book.HasAnyScript(itemTypeData.scriptListPerkMag))
            {
                return ItemType.Perkmag;
            }

            return ItemType.Note;
        }

        public ItemType GetAmmoType(IAmmunitionGetter ammo)
        {
            if (itemTypeCache.TryGetValue(ammo.FormKey, out var result))
            {
                return result;
            }

            result = GetAmmoTypeUncached(ammo);
            itemTypeCache.Add(ammo.FormKey, result);

            return result;
        }

        private ItemType GetAmmoTypeUncached(IAmmunitionGetter ammo)
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
            if (itemTypeCache.TryGetValue(keyItem.FormKey, out var result))
            {
                return result;
            }

            result = GetKeyTypeUncached(keyItem);
            itemTypeCache.Add(keyItem.FormKey, result);

            return result;
        }

        private ItemType GetKeyTypeUncached(IKeyGetter keyItem)
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
            if (itemTypeCache.TryGetValue(miscItem.FormKey, out var result))
            {
                return result;
            }

            result = GetMiscTypeUncached(miscItem);
            itemTypeCache.Add(miscItem.FormKey, result);

            return result;
        }

        private ItemType GetMiscTypeUncached(IMiscItemGetter miscItem)
        {
            if (itemOverrides.ContainsKey(miscItem.FormKey))
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

            var scriptType = miscItem.GetMatchingTypeByScript(itemTypeData.scriptMatchLists);
            if (scriptType != null)
            {
                return (ItemType)scriptType;
            }

            if (miscItem.HasAnyScript(itemTypeData.scriptListPipBoy))
            {
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
                if (miscItem.HasScript("ShipmentScript"))
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
            var prioLO = patcherState.LoadOrder.PriorityOrder;

            var omods = prioLO.AObjectModification().WinningOverrides();

            foreach (var omod in omods)
            {
                if (omod.LooseMod != null)
                {
                    looseModToOmodLookup[omod.LooseMod.FormKey] = omod.FormKey;
                }
            }

            // should I do the INNRs here, as well?
            var armors = prioLO.Armor().WinningOverrides();

            foreach (var armor in armors)
            {
                if (armor != null)
                {
                    var curInnr = armor.InstanceNaming;
                    if (!curInnr.IsNull)
                    {
                        if (innrLookup.TryGetValue(curInnr.FormKey, out var curSet))
                        {
                            curSet.Add(armor.FormKey);
                        }
                        else
                        {
                            innrLookup.Add(curInnr.FormKey, new() { armor.FormKey });
                        }
                    }
                }
            }

            var weapons = prioLO.Weapon().WinningOverrides();

            foreach (var weap in weapons)
            {
                if (weap != null)
                {
                    var curInnr = weap.InstanceNaming;
                    if (!curInnr.IsNull)
                    {
                        if (innrLookup.TryGetValue(curInnr.FormKey, out var curSet))
                        {
                            curSet.Add(weap.FormKey);
                        }
                        else
                        {
                            innrLookup.Add(curInnr.FormKey, new() { weap.FormKey });
                        }
                    }
                }
            }
        }

        private bool IsLooseMod(IMiscItemGetter miscItem)
        {
            return looseModToOmodLookup.ContainsKey(miscItem.FormKey);
        }

        // IsBlacklisted versions for different items

        // this one is used by TaggingProcessor
        public bool IsBlacklisted(IInstanceNamingRulesGetter innrGetter)
        {
            return
                itemTypeData.innrListSkip.Contains(innrGetter) ||
                IsBlacklistedByEditorId(innrGetter.EditorID);
        }

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
            where T : IHaveVirtualMachineAdapterGetter, ITranslatedNamedRequiredGetter, IMajorRecordGetter, IKeywordedGetter<IKeywordGetter>
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
            if (null == name)
            {
                return false;
            }

            return itemTypeData.blacklistName.Matches(name);
        }

        private bool IsBlacklistedByEditorId(string? edid)
        {
            if (edid == null)
            {
                return false;
            }

            return itemTypeData.blacklistEdid.Matches(edid);
        }

        private bool IsBlacklistedByScript(IHaveVirtualMachineAdapterGetter item)
        {
            if (item.VirtualMachineAdapter?.Scripts == null)
            {
                return false;
            }

            var scripts = item.VirtualMachineAdapter.Scripts;
            foreach (var script in scripts)
            {
                if (itemTypeData.blacklistScript.Matches(script.Name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
