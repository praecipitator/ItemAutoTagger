using ItemTagger.ItemTypeFinder;

namespace ItemTagger.TaggingConfigs
{
    public class TaggingConfiguration : Dictionary<ItemType, string>
    {
        private readonly List<string> extraValidTags = new();

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
                string tagGun,
                string tagArmor,
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
            Add(ItemType.Gun, tagGun);
            Add(ItemType.Armor, tagArmor);

            this.extraValidTags = extraValidTags;

            this.extraValidTags.Add(tagShipment);
            this.extraValidTags.Add(tagScrap);
            this.extraValidTags.Add(tagResource);
            this.extraValidTags.Add(tagLooseMod);
            this.extraValidTags.Add(tagCollectible);
            this.extraValidTags.Add(tagQuest);
            this.extraValidTags.Add(tagCurrency);
            this.extraValidTags.Add(tagValuable);
            this.extraValidTags.Add(tagOtherMisc);
            this.extraValidTags.Add(tagGoodChem);
            this.extraValidTags.Add(tagBadChem);
            this.extraValidTags.Add(tagFood);
            this.extraValidTags.Add(tagFoodRaw);
            this.extraValidTags.Add(tagFoodCrop);
            this.extraValidTags.Add(tagFoodPrewar);
            this.extraValidTags.Add(tagDrink);
            this.extraValidTags.Add(tagLiquor);
            this.extraValidTags.Add(tagNukacola);
            this.extraValidTags.Add(tagSyringe);
            this.extraValidTags.Add(tagDevice);
            this.extraValidTags.Add(tagTool);
            this.extraValidTags.Add(tagNews);
            this.extraValidTags.Add(tagNote);
            this.extraValidTags.Add(tagPerkmag);
            this.extraValidTags.Add(tagMine);
            this.extraValidTags.Add(tagGrenade);
            this.extraValidTags.Add(tagKey);
            this.extraValidTags.Add(tagKeyCard);
            this.extraValidTags.Add(tagKeyPassword);
            this.extraValidTags.Add(tagAmmo);
            this.extraValidTags.Add(tagHolotape);
            this.extraValidTags.Add(tagHolotapeGame);
            this.extraValidTags.Add(tagHolotapeSettings);
            this.extraValidTags.Add(tagPipBoy);
            this.extraValidTags.Add(tagGun);
            this.extraValidTags.Add(tagArmor);
        }

        public bool isTagValid(string tag)
        {
            return extraValidTags.Contains(tag, StringComparer.OrdinalIgnoreCase);
        }
    }
}
