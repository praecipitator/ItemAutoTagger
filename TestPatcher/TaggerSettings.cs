using Mutagen.Bethesda.Synthesis.Settings;

namespace ItemTagger
{
    public class TaggerSettings
    {
        [SynthesisOrder]
        [SynthesisSettingName("Tagging Configuration")]
        public TaggingConfigType TaggingConfig = TaggingConfigType.FIS;

        [SynthesisOrder]
        [SynthesisSettingName("Append components")]
        public bool UseComponentString = false;

        [SynthesisOrder]
        [SynthesisSettingName("Custom Tagging Configuration")]
        public CustomTaggingConfig CustomConfig = new();
    }

    public class CustomTaggingConfig
    {
        [SynthesisOrder]
        [SynthesisDescription("Shipments")]
        public string tagShipment = "";
        [SynthesisOrder]
        [SynthesisDescription("Scrap, MISCs which contain components")]
        public string tagScrap = "";
        [SynthesisOrder]
        [SynthesisDescription("Resources, MISCs which a meant to represent one type of component")]
        public string tagResource = "";
        [SynthesisOrder]
        [SynthesisDescription("Loose modifications")]
        public string tagLooseMod = "";
        [SynthesisOrder]
        [SynthesisDescription("\"Collectible\" MISCs")]
        public string tagCollectible = "";
        [SynthesisOrder]
        [SynthesisDescription("Quest Items")]
        public string tagQuest = "";
        [SynthesisOrder]
        [SynthesisDescription("\"Currency\", MISCs with zero weight and non-zero value")]
        public string tagCurrency = "";
        [SynthesisOrder]
        [SynthesisDescription("MISCs with more value than weight")]
        public string tagValuable = "";
        [SynthesisOrder]
        [SynthesisDescription("all other MISCs, trash")]
        public string tagOtherMisc = "";
        [SynthesisOrder]
        [SynthesisDescription("Cures etc")]
        public string tagGoodChem = "";
        [SynthesisOrder]
        [SynthesisDescription("Addictive chems")]
        public string tagBadChem = "";
        [SynthesisOrder]
        [SynthesisDescription("generic food, selfcooked. Usually radless")]
        public string tagFood = "";
        [SynthesisOrder]
        [SynthesisDescription("raw food, has rads, has disease risk")]
        public string tagFoodRaw = "";
        [SynthesisOrder]
        [SynthesisDescription("crops, has rads")]
        public string tagFoodCrop = "";
        [SynthesisOrder]
        [SynthesisDescription("prewar packaged, has rads")]
        public string tagFoodPrewar = "";
        [SynthesisOrder]
        [SynthesisDescription("generic drinkable")]
        public string tagDrink = "";
        [SynthesisOrder]
        [SynthesisDescription("alcoholic beverages")]
        public string tagLiquor = "";
        [SynthesisOrder]
        [SynthesisDescription("Nuka Cola of any kind")]
        public string tagNukacola = "";
        [SynthesisOrder]
        [SynthesisDescription("Syringer ammo")]
        public string tagSyringe = "";
        [SynthesisOrder]
        [SynthesisDescription("Consumables which are supposed to be devices instead of something to eat, like the Stealth Boy")]
        public string tagDevice = "";
        [SynthesisOrder]
        [SynthesisDescription("Similar to above, but for more low-tech things. Like SimSettlements Town Meeting Gavel, or the Companion Whistle")]
        public string tagTool = "";
        [SynthesisOrder]
        [SynthesisDescription("Newspaper, mostly Publick Occurences")]
        public string tagNews = "";
        [SynthesisOrder]
        [SynthesisDescription("Any other paper note")]
        public string tagNote = "";
        [SynthesisOrder]
        [SynthesisDescription("Perk Magazine")]
        public string tagPerkmag = "";
        [SynthesisOrder]
        [SynthesisDescription("Mines")]
        public string tagMine = "";
        [SynthesisOrder]
        [SynthesisDescription("Grenades")]
        public string tagGrenade = "";
        [SynthesisOrder]
        [SynthesisDescription("Keys")]
        public string tagKey = "";
        [SynthesisOrder]
        [SynthesisDescription("Generic ammo")]
        public string tagAmmo = "";
        [SynthesisOrder]
        [SynthesisDescription("Generic holotape")]
        public string tagHolotape = "";
        [SynthesisOrder]
        [SynthesisDescription("Game holotape")]
        public string tagHolotapeGame = "";
        [SynthesisOrder]
        [SynthesisDescription("Settings holotape")]
        public string tagHolotapeSettings = "";
        [SynthesisOrder]
        [SynthesisDescription("The player's Pip-Boy, but also MISCs which you can give to NPCs, like from the VaultTec Workshop")]
        public string tagPipBoy = "";
        [SynthesisOrder]
        [SynthesisDescription("Any other tags (without brackets) which should be considered valid and left alone")]
        public List<string> extraValidTags = new();
    }
}
