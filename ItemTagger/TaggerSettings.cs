using ItemTagger.ItemTypeFinder;
using ItemTagger.TaggingConfigs;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Synthesis.Settings;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace ItemTagger
{
    public class TaggerSettings
    {
        [SynthesisOrder]
        [SynthesisSettingName("Tagging Configuration")]
        public TaggingConfigType TaggingConfig = TaggingConfigType.FIS;
        // public TaggingConfigType TaggingConfig = TaggingConfigType.LWIS;

        [SynthesisOrder]
        [SynthesisSettingName("Append components")]
        [SynthesisTooltip("If checked, component strings like {{{Wood,Steel}}} will be appended to scrappable items.")]
        public bool UseComponentString = true;

        [SynthesisOrder]
        [SynthesisSettingName("Remove brackets around names")]
        [SynthesisTooltip("With some UI configurations, names enclosed in brackets become invisible. This option removes brackets around names.")]
        public bool RemoveBrackets = true;

        [SynthesisOrder]
        [SynthesisSettingName("Patch Instance Naming Rules")]
        [SynthesisTooltip("If checked, the patcher will attempt to add tags to custom instance naming rules for weapons and armor.")]
        public bool PatchINNRs = true;

        [SynthesisOrder]
        [SynthesisSettingName("Override Item Type Settings")]
        [SynthesisTooltip("Here, you can manually configure the types of items. The actual tag depends on the selected Tagging Configuration")]
        public List<GenericFormTypeMapping> ItemTypeConfig = new();

        [SynthesisOrder]
        [SynthesisSettingName("Custom Tagging Configuration")]
        [SynthesisTooltip("If you selected \"Custom\" under Tagging Configuration, you can configure your tags here.")]
        public CustomTaggingConfig CustomConfig = new();
    }

    public class FormTypeMapping<T>
        where T : class, IMajorRecordGetter
    {
        [SynthesisOrder]
        [SynthesisSettingName("Item")]
        public IFormLinkGetter<T> form = FormLink<T>.Null;

        [SynthesisOrder]
        [SynthesisSettingName("Type")]
        [SynthesisTooltip("Select \"None\" to blacklist this item")]
        public ItemType type = ItemType.None;
    }

    public class GenericFormTypeMapping
    {
        [SynthesisOrder]
        [SynthesisSettingName("Item")]
        [FormLinkPickerCustomization(
            typeof(IMiscItem),
            typeof(IIngestible),
            typeof(IWeaponGetter),
            typeof(IArmorGetter),
            typeof(IBook),
            typeof(IHolotape),
            typeof(IAmmunition),
            typeof(IKey),
            typeof(IInstanceNamingRule)
        )]
        public IFormLinkGetter form { get; set; } = FormLinkInformation.Null;

        [SynthesisOrder]
        [SynthesisSettingName("Type")]
        [SynthesisTooltip("Select \"None\" to blacklist this item")]
        public ItemType type = ItemType.None;
    }

    public class CustomTaggingConfig
    {
        [SynthesisOrder]
        [SynthesisSettingName("Shipment")]
        [SynthesisTooltip("Shipment of resources")]
        public string tagShipment = "";

        [SynthesisOrder]
        [SynthesisSettingName("Scrap")]
        [SynthesisTooltip("Scrap, a MISC item which contain components")]
        public string tagScrap = "";

        [SynthesisOrder]
        [SynthesisSettingName("Resource")]
        [SynthesisTooltip("Resource, a MISC which is meant to represent one type of component")]
        public string tagResource = "";

        [SynthesisOrder]
        [SynthesisSettingName("Mod")]
        [SynthesisTooltip("Loose modification")]
        public string tagLooseMod = "";

        [SynthesisOrder]
        [SynthesisSettingName("Collectible")]
        [SynthesisTooltip("\"Collectible\" MISC")]
        public string tagCollectible = "";

        [SynthesisOrder]
        [SynthesisSettingName("Quest")]
        [SynthesisTooltip("Quest Item")]
        public string tagQuest = "";

        [SynthesisOrder]
        [SynthesisSettingName("Currency")]
        [SynthesisTooltip("MISC with zero weight and non-zero value")]
        public string tagCurrency = "";

        [SynthesisOrder]
        [SynthesisSettingName("Valuable")]
        [SynthesisTooltip("MISC with more value than weight")]
        public string tagValuable = "";

        [SynthesisOrder]
        [SynthesisSettingName("Misc")]
        [SynthesisTooltip("Any other MISC, trash")]
        public string tagOtherMisc = "";

        [SynthesisOrder]
        [SynthesisSettingName("Aid")]
        [SynthesisTooltip("Stimpacks, Cures, generally positive, non-addictive chems")]
        public string tagGoodChem = "";

        [SynthesisOrder]
        [SynthesisSettingName("Drug")]
        [SynthesisTooltip("Addictive chems, drugs")]
        public string tagBadChem = "";

        [SynthesisOrder]
        [SynthesisSettingName("Food")]
        [SynthesisTooltip("Generic food, selfcooked food. Usually radless")]
        public string tagFood = "";

        [SynthesisOrder]
        [SynthesisSettingName("Raw Food")]
        [SynthesisTooltip("Raw food, has rads, has disease risk")]
        public string tagFoodRaw = "";

        [SynthesisOrder]
        [SynthesisSettingName("Crop")]
        [SynthesisTooltip("Raw crop, usually with rads")]
        public string tagFoodCrop = "";

        [SynthesisOrder]
        [SynthesisSettingName("Prewar Food")]
        [SynthesisTooltip("Prewar packaged, with rads")]
        public string tagFoodPrewar = "";

        [SynthesisOrder]
        [SynthesisSettingName("Drink")]
        [SynthesisTooltip("Generic drinkable")]
        public string tagDrink = "";

        [SynthesisOrder]
        [SynthesisSettingName("Liquor")]
        [SynthesisTooltip("Alcoholic beverage")]
        public string tagLiquor = "";

        [SynthesisOrder]
        [SynthesisSettingName("Nuka")]
        [SynthesisTooltip("Nuka Cola of any kind")]
        public string tagNukacola = "";

        [SynthesisOrder]
        [SynthesisSettingName("Syringe")]
        [SynthesisTooltip("Syringer ammo")]
        public string tagSyringe = "";

        [SynthesisOrder]
        [SynthesisSettingName("Device")]
        [SynthesisTooltip("A consumable which is supposed to be a device, instead of an actual consumable. Or, a Misc item with a special function, like the Salvage Beacon")]
        public string tagDevice = "";

        [SynthesisOrder]
        [SynthesisSettingName("Stealth Boy")]
        [SynthesisTooltip("Any kind of Stealth Boy, when in doubt, use the same text as for \"Device\"")]
        public string tagStealthBoy = "";

        [SynthesisOrder]
        [SynthesisSettingName("Tool")]
        [SynthesisTooltip("Similar to \"Device\", but for more low-tech things. Like SimSettlements Town Meeting Gavel, or the Companion Whistle")]
        public string tagTool = "";

        [SynthesisOrder]
        [SynthesisSettingName("News")]
        [SynthesisTooltip("Newspaper, mostly Publick Occurences")]
        public string tagNews = "";

        [SynthesisOrder]
        [SynthesisSettingName("Note")]
        [SynthesisTooltip("Any other paper note")]
        public string tagNote = "";

        [SynthesisOrder]
        [SynthesisSettingName("Perk Magazine")]
        [SynthesisTooltip("A magazine, usually one which teaches you perks")]
        public string tagPerkmag = "";

        [SynthesisOrder]
        [SynthesisSettingName("Book")]
        [SynthesisTooltip("A readable book, mostly mod-added. Doesn't affect vanilla \"Overdue Books\"")]
        public string tagBook = "";

        [SynthesisOrder]
        [SynthesisSettingName("Mine")]
        [SynthesisTooltip("A mine or trap")]
        public string tagMine = "";

        [SynthesisOrder]
        [SynthesisSettingName("Grenade")]
        [SynthesisTooltip("Any grenade")]
        public string tagGrenade = "";

        [SynthesisOrder]
        [SynthesisSettingName("Key")]
        [SynthesisTooltip("Any generic key")]
        public string tagKey = "";

        [SynthesisOrder]
        [SynthesisSettingName("Keycard")]
        [SynthesisTooltip("Any card-shaped key")]
        public string tagKeyCard = "";

        [SynthesisOrder]
        [SynthesisSettingName("Password")]
        [SynthesisTooltip("Password, usually written on a note or holotape")]
        public string tagPassword = "";

        [SynthesisOrder]
        [SynthesisSettingName("Ammo")]
        [SynthesisTooltip("Generic ammo")]
        public string tagAmmo = "";

        [SynthesisOrder]
        [SynthesisSettingName("Fusion Core")]
        [SynthesisTooltip("Fusion Core or Fusion Core like ammunition")]
        public string tagFusionCore = "";

        [SynthesisOrder]
        [SynthesisSettingName("Mini Nuke")]
        [SynthesisTooltip("Mini Nuke")]
        public string tagMiniNuke = "";

        [SynthesisOrder]
        [SynthesisSettingName("Holotape")]
        [SynthesisTooltip("Generic holotape")]
        public string tagHolotape = "";

        [SynthesisOrder]
        [SynthesisSettingName("Holotape Game")]
        [SynthesisTooltip("Holotape with a game on it")]
        public string tagHolotapeGame = "";

        [SynthesisOrder]
        [SynthesisSettingName("Holotape Settings")]
        [SynthesisTooltip("Settings holotape")]
        public string tagHolotapeSettings = "";

        [SynthesisOrder]
        [SynthesisSettingName("Pip-Boy")]
        [SynthesisTooltip("The player's Pip-Boy, but also MISCs which you can give to NPCs, like from the VaultTec Workshop")]
        public string tagPipBoy = "";

        [SynthesisOrder]
        [SynthesisSettingName("Ranged Weapon")]
        [SynthesisTooltip("Generic ranged weapon")]
        public string tagWeaponRanged = "";

        [SynthesisOrder]
        [SynthesisSettingName("Melee Weapon")]
        [SynthesisTooltip("Generic melee weapon")]
        public string tagWeaponMelee = "";

        [SynthesisOrder]
        [SynthesisSettingName("Armor")]
        [SynthesisTooltip("Generic piece of armor")]
        public string tagArmor = "";

        [SynthesisOrder]
        [SynthesisSettingName("Clothing")]
        [SynthesisTooltip("Generic piece of non-armored clothing")]
        public string tagClothing = "";

        [SynthesisOrder]
        [SynthesisSettingName("Vault Suit")]
        [SynthesisTooltip("Any vault suit")]
        public string tagVaultSuit = "";

        [SynthesisOrder]
        [SynthesisSettingName("Power Armor")]
        [SynthesisTooltip("Generic piece of power armor")]
        public string tagPowerArmor = "";

        [SynthesisOrder]
        [SynthesisSettingName("Extra Tags")]
        [SynthesisTooltip("Any other tags (without brackets) which should be considered valid and left as-is")]
        public List<string> extraValidTags = new();
    }
}
