using ItemTagger.ItemTypeFinder;

namespace ItemTagger
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
                string tagAmmo,
                string tagHolotape,
                string tagHolotapeGame,
                string tagHolotapeSettings,
                string tagPipBoy,
                List<string> extraValidTags
            )
        {
            this.Add(ItemType.Shipment, tagShipment);
            this.Add(ItemType.Scrap, tagScrap);
            this.Add(ItemType.Resource, tagResource);
            this.Add(ItemType.LooseMod, tagLooseMod);
            this.Add(ItemType.Collectible, tagCollectible);
            this.Add(ItemType.Quest, tagQuest);
            this.Add(ItemType.Currency, tagCurrency);
            this.Add(ItemType.Valuable, tagValuable);
            this.Add(ItemType.OtherMisc, tagOtherMisc);
            this.Add(ItemType.GoodChem, tagGoodChem);
            this.Add(ItemType.BadChem, tagBadChem);
            this.Add(ItemType.Food, tagFood);
            this.Add(ItemType.FoodRaw, tagFoodRaw);
            this.Add(ItemType.FoodCrop, tagFoodCrop);
            this.Add(ItemType.FoodPrewar, tagFoodPrewar);
            this.Add(ItemType.Drink, tagDrink);
            this.Add(ItemType.Liquor, tagLiquor);
            this.Add(ItemType.Nukacola, tagNukacola);
            this.Add(ItemType.Syringe, tagSyringe);
            this.Add(ItemType.Device, tagDevice);
            this.Add(ItemType.Tool, tagTool);
            this.Add(ItemType.News, tagNews);
            this.Add(ItemType.Note, tagNote);
            this.Add(ItemType.Perkmag, tagPerkmag);
            this.Add(ItemType.Mine, tagMine);
            this.Add(ItemType.Grenade, tagGrenade);
            this.Add(ItemType.Key, tagKey);
            this.Add(ItemType.Ammo, tagAmmo);
            this.Add(ItemType.Holotape, tagHolotape);
            this.Add(ItemType.HolotapeGame, tagHolotapeGame);
            this.Add(ItemType.HolotapeSettings, tagHolotapeSettings);
            this.Add(ItemType.PipBoy, tagPipBoy);

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
            this.extraValidTags.Add(tagAmmo);
            this.extraValidTags.Add(tagHolotape);
            this.extraValidTags.Add(tagHolotapeGame);
            this.extraValidTags.Add(tagHolotapeSettings);
            this.extraValidTags.Add(tagPipBoy);
        }

        public bool isTagValid(string tag)
        {
            return extraValidTags.Contains(tag);
        }
    }
}
