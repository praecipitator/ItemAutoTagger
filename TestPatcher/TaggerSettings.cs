using ItemTagger.TaggingConfigs;
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
        [SynthesisTooltip("If checked, component strings like {{{Wood,Steel}}} will be appended to scrappable items.")]
        public bool UseComponentString = false;

        [SynthesisOrder]
        [SynthesisSettingName("Custom Tagging Configuration")]
        [SynthesisTooltip("If you selected \"Custom\" under Tagging Configuration, you can configure your tags here.")]
        public CustomTaggingConfig CustomConfig = new();
    }

    public class CustomTaggingConfig
    {
        [SynthesisOrder]
        [SynthesisTooltip("Shipments of resources")]
        public string tagShipment = "";
        [SynthesisOrder]
        [SynthesisTooltip("Scrap, MISCs which contain components")]
        public string tagScrap = "";
        [SynthesisOrder]
        [SynthesisTooltip("Resources, MISCs which a meant to represent one type of component")]
        public string tagResource = "";
        [SynthesisOrder]
        [SynthesisTooltip("Loose modifications")]
        public string tagLooseMod = "";
        [SynthesisOrder]
        [SynthesisTooltip("\"Collectible\" MISCs")]
        public string tagCollectible = "";
        [SynthesisOrder]
        [SynthesisTooltip("Quest Items")]
        public string tagQuest = "";
        [SynthesisOrder]
        [SynthesisTooltip("MISCs with zero weight and non-zero value")]
        public string tagCurrency = "";
        [SynthesisOrder]
        [SynthesisTooltip("MISCs with more value than weight")]
        public string tagValuable = "";
        [SynthesisOrder]
        [SynthesisTooltip("All other MISCs, trash")]
        public string tagOtherMisc = "";
        [SynthesisOrder]
        [SynthesisTooltip("Stimpacks, Cures, generally positive, non-addictive chems")]
        public string tagGoodChem = "";
        [SynthesisOrder]
        [SynthesisTooltip("Addictive chems")]
        public string tagBadChem = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic food, selfcooked food. Usually radless")]
        public string tagFood = "";
        [SynthesisOrder]
        [SynthesisTooltip("Raw food, has rads, has disease risk")]
        public string tagFoodRaw = "";
        [SynthesisOrder]
        [SynthesisTooltip("Raw crops, usually have rads")]
        public string tagFoodCrop = "";
        [SynthesisOrder]
        [SynthesisTooltip("Prewar packaged, have rads")]
        public string tagFoodPrewar = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic drinkable")]
        public string tagDrink = "";
        [SynthesisOrder]
        [SynthesisTooltip("Alcoholic beverages")]
        public string tagLiquor = "";
        [SynthesisOrder]
        [SynthesisTooltip("Nuka Cola of any kind")]
        public string tagNukacola = "";
        [SynthesisOrder]
        [SynthesisTooltip("Syringer ammo")]
        public string tagSyringe = "";
        [SynthesisOrder]
        [SynthesisTooltip("Consumables which are supposed to be devices instead of something to eat, like the Stealth Boy")]
        public string tagDevice = "";
        [SynthesisOrder]
        [SynthesisTooltip("Similar to \"Device\", but for more low-tech things. Like SimSettlements Town Meeting Gavel, or the Companion Whistle")]
        public string tagTool = "";
        [SynthesisOrder]
        [SynthesisTooltip("Newspaper, mostly Publick Occurences")]
        public string tagNews = "";
        [SynthesisOrder]
        [SynthesisTooltip("Any other paper note")]
        public string tagNote = "";
        [SynthesisOrder]
        [SynthesisTooltip("Perk Magazine")]
        public string tagPerkmag = "";
        [SynthesisOrder]
        [SynthesisTooltip("Mines")]
        public string tagMine = "";
        [SynthesisOrder]
        [SynthesisTooltip("Grenades")]
        public string tagGrenade = "";
        [SynthesisOrder]
        [SynthesisTooltip("Keys")]
        public string tagKey = "";
        [SynthesisOrder]
        [SynthesisTooltip("Keycards")]
        public string tagKeyCard = "";
        [SynthesisOrder]
        [SynthesisTooltip("Password, usually written on a note or holotape")]
        public string tagPassword = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic ammo")]
        public string tagAmmo = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic holotape")]
        public string tagHolotape = "";
        [SynthesisOrder]
        [SynthesisTooltip("Game holotape")]
        public string tagHolotapeGame = "";
        [SynthesisOrder]
        [SynthesisTooltip("Settings holotape")]
        public string tagHolotapeSettings = "";
        [SynthesisOrder]
        [SynthesisTooltip("The player's Pip-Boy, but also MISCs which you can give to NPCs, like from the VaultTec Workshop")]
        public string tagPipBoy = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic ranged weapon")]
        public string tagWeaponRanged = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic melee weapon")]
        public string tagWeaponMelee = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic piece of armor")]
        public string tagArmor = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic piece of non-armored clothing")]
        public string tagClothing = "";
        [SynthesisOrder]
        [SynthesisTooltip("Any vault suit")]
        public string tagVaultSuit = "";
        [SynthesisOrder]
        [SynthesisTooltip("Generic piece of power armor")]
        public string tagPowerArmor = "";

        [SynthesisOrder]
        [SynthesisTooltip("Any other tags (without brackets) which should be considered valid and left as-is")]
        public List<string> extraValidTags = new();
    }
}
