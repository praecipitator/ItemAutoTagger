using ItemTagger.ItemTypeFinder;
using ItemTagger.TaggingConfigs;
using ItemTypeFinder.Settings;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Synthesis;

namespace ItemTagger
{
    public class Program
    {
        // main settings
        static Lazy<TaggerSettings> _lazySettings = null!;
        static TaggerSettings Settings => _lazySettings.Value;

        /*
        static Lazy<TyperSettings> _lazyTypeSettings = null!;
        static TyperSettings TyperSettings => _lazyTypeSettings.Value;
        */

        static DefaultTaggingConfigurations defaultTaggingConfigs = new();
        
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<IFallout4Mod, IFallout4ModGetter>(RunPatch)
                .SetAutogeneratedSettings(
                    nickname: "Settings",
                    path: "settings.json",
                    out _lazySettings)
                /*.SetAutogeneratedSettings(
                    nickname: "TyperSettings",
                    path: "typer-settings.json",
                    out _lazyTypeSettings)*/
                .SetTypicalOpen(GameRelease.Fallout4, "ItemTags.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<IFallout4Mod, IFallout4ModGetter> state)
        {
            // get the configured tagging config
            var taggingConfig = getTaggingConfigByType(Settings.TaggingConfig);

            /*
            var processor = new TaggingProcessor(taggingConfig, Settings, TyperSettings, state);
            // actually run the thing
            processor.Process();
            */
        }       


        private static TaggingConfiguration getTaggingConfigByType(TaggingConfigType type)
        {
            if(type != TaggingConfigType.Custom)
            {
                return defaultTaggingConfigs.getConfigByType(type);
            }

            // otherwise, build a new one from custom settings
            var customConfig = new TaggingConfiguration() {
                { ItemType.Shipment,            Settings.CustomConfig.tagShipment },
                { ItemType.Scrap,               Settings.CustomConfig.tagScrap },
                { ItemType.Resource,            Settings.CustomConfig.tagResource },
                { ItemType.LooseMod,            Settings.CustomConfig.tagLooseMod },
                { ItemType.Collectible,         Settings.CustomConfig.tagCollectible },
                { ItemType.Quest,               Settings.CustomConfig.tagQuest },
                { ItemType.Currency,            Settings.CustomConfig.tagCurrency },
                { ItemType.Valuable,            Settings.CustomConfig.tagValuable },
                { ItemType.OtherMisc,           Settings.CustomConfig.tagOtherMisc },
                { ItemType.GoodChem,            Settings.CustomConfig.tagGoodChem },
                { ItemType.BadChem,             Settings.CustomConfig.tagBadChem },
                { ItemType.Food,                Settings.CustomConfig.tagFood },
                { ItemType.FoodRaw,             Settings.CustomConfig.tagFoodRaw },
                { ItemType.FoodCrop,            Settings.CustomConfig.tagFoodCrop },
                { ItemType.FoodPrewar,          Settings.CustomConfig.tagFoodPrewar },
                { ItemType.Drink,               Settings.CustomConfig.tagDrink },
                { ItemType.Liquor,              Settings.CustomConfig.tagLiquor },
                { ItemType.Nukacola,            Settings.CustomConfig.tagNukacola },
                { ItemType.Syringe,             Settings.CustomConfig.tagSyringe },
                { ItemType.Device,              Settings.CustomConfig.tagDevice },
                { ItemType.Tool,                Settings.CustomConfig.tagTool },
                { ItemType.News,                Settings.CustomConfig.tagNews },
                { ItemType.Note,                Settings.CustomConfig.tagNote },
                { ItemType.Perkmag,             Settings.CustomConfig.tagPerkmag },
                { ItemType.Mine,                Settings.CustomConfig.tagMine },
                { ItemType.Grenade,             Settings.CustomConfig.tagGrenade },
                { ItemType.Key,                 Settings.CustomConfig.tagKey },
                { ItemType.KeyCard,             Settings.CustomConfig.tagKeyCard },
                { ItemType.KeyPassword,         Settings.CustomConfig.tagPassword },
                { ItemType.Ammo,                Settings.CustomConfig.tagAmmo },
                { ItemType.Holotape,            Settings.CustomConfig.tagHolotape },
                { ItemType.HolotapeGame,        Settings.CustomConfig.tagHolotapeGame },
                { ItemType.HolotapeSettings,    Settings.CustomConfig.tagHolotapeSettings },
                { ItemType.PipBoy,              Settings.CustomConfig.tagPipBoy },
                { ItemType.WeaponRanged,        Settings.CustomConfig.tagWeaponRanged },
                { ItemType.WeaponMelee,         Settings.CustomConfig.tagWeaponMelee },
                { ItemType.Armor,               Settings.CustomConfig.tagArmor },
                { ItemType.Clothes,             Settings.CustomConfig.tagClothing },
                { ItemType.VaultSuit,           Settings.CustomConfig.tagVaultSuit },
                { ItemType.PowerArmor,          Settings.CustomConfig.tagPowerArmor },
            };
            customConfig.AddExtraTags(Settings.CustomConfig.extraValidTags);

            return customConfig;
        }
    }
}
