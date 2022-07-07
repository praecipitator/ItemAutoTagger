using ItemTagger.ItemTypeFinder;
using System.Text.RegularExpressions;

namespace ItemTagger.TaggingConfigs
{
    public class TaggingConfiguration : Dictionary<ItemType, string>
    {
        private readonly List<string> extraValidTags = new();

        private static readonly Regex TAG_EXTRACT_REGEX = new(@"^[\[\]()|{}]([^\[\]()|{}]+)[\[\]()|{}]$", RegexOptions.Compiled);
        public TaggingConfiguration
            (
                string tagShipment,
                string tagScrap,
                string tagResource,
                string tagLooseMod,
                string tagCollectible,
                string tagQuest,
                string tagCurrency,
                string tagValuable,
                string tagOtherMisc,
                string tagGoodChem,
                string tagBadChem,
                string tagFood,
                string tagFoodRaw,
                string tagFoodCrop,
                string tagFoodPrewar,
                string tagDrink,
                string tagLiquor,
                string tagNukacola,
                string tagSyringe,
                string tagDevice,
                string tagTool,
                string tagNews,
                string tagNote,
                string tagPerkmag,
                string tagMine,
                string tagGrenade,
                string tagKey,
                string tagKeyCard,
                string tagKeyPassword,
                string tagAmmo,
                string tagHolotape,
                string tagHolotapeGame,
                string tagHolotapeSettings,
                string tagPipBoy,
                string tagWeaponRanged,
                string tagWeaponMelee,
                string tagArmor,
                string tagClothes,
                string tagVaultSuit,
                string tagPowerArmor,
                List<string> extraValidTags
            )
        {
            Add(ItemType.Shipment, tagShipment);
            Add(ItemType.Scrap, tagScrap);
            Add(ItemType.Resource, tagResource);
            Add(ItemType.LooseMod, tagLooseMod);
            Add(ItemType.Collectible, tagCollectible);
            Add(ItemType.Quest, tagQuest);
            Add(ItemType.Currency, tagCurrency);
            Add(ItemType.Valuable, tagValuable);
            Add(ItemType.OtherMisc, tagOtherMisc);
            Add(ItemType.GoodChem, tagGoodChem);
            Add(ItemType.BadChem, tagBadChem);
            Add(ItemType.Food, tagFood);
            Add(ItemType.FoodRaw, tagFoodRaw);
            Add(ItemType.FoodCrop, tagFoodCrop);
            Add(ItemType.FoodPrewar, tagFoodPrewar);
            Add(ItemType.Drink, tagDrink);
            Add(ItemType.Liquor, tagLiquor);
            Add(ItemType.Nukacola, tagNukacola);
            Add(ItemType.Syringe, tagSyringe);
            Add(ItemType.Device, tagDevice);
            Add(ItemType.Tool, tagTool);
            Add(ItemType.News, tagNews);
            Add(ItemType.Note, tagNote);
            Add(ItemType.Perkmag, tagPerkmag);
            Add(ItemType.Mine, tagMine);
            Add(ItemType.Grenade, tagGrenade);
            Add(ItemType.Key, tagKey);
            Add(ItemType.KeyCard, tagKeyCard);
            Add(ItemType.KeyPassword, tagKeyPassword);
            Add(ItemType.Ammo, tagAmmo);
            Add(ItemType.Holotape, tagHolotape);
            Add(ItemType.HolotapeGame, tagHolotapeGame);
            Add(ItemType.HolotapeSettings, tagHolotapeSettings);
            Add(ItemType.PipBoy, tagPipBoy);
            Add(ItemType.WeaponRanged, tagWeaponRanged);
            Add(ItemType.WeaponMelee, tagWeaponMelee);
            Add(ItemType.Armor, tagArmor);
            Add(ItemType.Clothes, tagClothes);
            Add(ItemType.VaultSuit, tagVaultSuit);
            Add(ItemType.PowerArmor, tagPowerArmor);

            this.extraValidTags = extraValidTags;

            addExtraTag(tagShipment);
            addExtraTag(tagScrap);
            addExtraTag(tagResource);
            addExtraTag(tagLooseMod);
            addExtraTag(tagCollectible);
            addExtraTag(tagQuest);
            addExtraTag(tagCurrency);
            addExtraTag(tagValuable);
            addExtraTag(tagOtherMisc);
            addExtraTag(tagGoodChem);
            addExtraTag(tagBadChem);
            addExtraTag(tagFood);
            addExtraTag(tagFoodRaw);
            addExtraTag(tagFoodCrop);
            addExtraTag(tagFoodPrewar);
            addExtraTag(tagDrink);
            addExtraTag(tagLiquor);
            addExtraTag(tagNukacola);
            addExtraTag(tagSyringe);
            addExtraTag(tagDevice);
            addExtraTag(tagTool);
            addExtraTag(tagNews);
            addExtraTag(tagNote);
            addExtraTag(tagPerkmag);
            addExtraTag(tagMine);
            addExtraTag(tagGrenade);
            addExtraTag(tagKey);
            addExtraTag(tagKeyCard);
            addExtraTag(tagKeyPassword);
            addExtraTag(tagAmmo);
            addExtraTag(tagHolotape);
            addExtraTag(tagHolotapeGame);
            addExtraTag(tagHolotapeSettings);
            addExtraTag(tagPipBoy);
            addExtraTag(tagWeaponRanged);
            addExtraTag(tagWeaponMelee);
            addExtraTag(tagArmor);
            addExtraTag(tagClothes);
            addExtraTag(tagVaultSuit);
            addExtraTag(tagPowerArmor);
        }

        private void addExtraTag(string fullTag)
        {
            // remove the brackets.
            // TODO maybe just use substring?
            var matches = TAG_EXTRACT_REGEX.Match(fullTag);

            if (matches.Groups.Count < 2)
            {
                return;
            }

            var tag = matches.Groups[1].Value;
            extraValidTags.Add(tag);
        }

        public bool isTagValid(string tag)
        {
            return extraValidTags.Contains(tag, StringComparer.OrdinalIgnoreCase);
        }
    }
}
