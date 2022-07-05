using ItemTagger.ItemTypeFinder;
using ItemTagger.TaggingConfigs;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Synthesis;
using System.Text.RegularExpressions;

namespace ItemTagger
{
    public class Program
    {
        static Lazy<TaggerSettings> _lazySettings = null!;
        static TaggerSettings Settings => _lazySettings.Value;

        static DefaultTaggingConfigurations defaultTaggingConfigs = new();

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<IFallout4Mod, IFallout4ModGetter>(RunPatch)
                .SetAutogeneratedSettings(
                    nickname: "Settings",
                    path: "settings.json",
                    out _lazySettings)
                .SetTypicalOpen(GameRelease.Fallout4, "ItemTags.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<IFallout4Mod, IFallout4ModGetter> state)
        {
            // get the configured tagging config
            var taggingConfig = getTaggingConfigByType(Settings.TaggingConfig);

            var processor = new TaggingProcessor(taggingConfig, Settings, state);
            // actually run the thing
            processor.Process();
        }       


        private static TaggingConfiguration getTaggingConfigByType(TaggingConfigType type)
        {
            if(type != TaggingConfigType.Custom)
            {
                return defaultTaggingConfigs.getConfigByType(type);
            }

            // otherwise, build a new one from custom settings
            var customConfig = new TaggingConfiguration(
                Settings.CustomConfig.tagShipment,
                Settings.CustomConfig.tagScrap,
                Settings.CustomConfig.tagResource,
                Settings.CustomConfig.tagLooseMod,
                Settings.CustomConfig.tagCollectible,
                Settings.CustomConfig.tagQuest,
                Settings.CustomConfig.tagCurrency,
                Settings.CustomConfig.tagValuable,
                Settings.CustomConfig.tagOtherMisc,
                Settings.CustomConfig.tagGoodChem,
                Settings.CustomConfig.tagBadChem,
                Settings.CustomConfig.tagFood,
                Settings.CustomConfig.tagFoodRaw,
                Settings.CustomConfig.tagFoodCrop,
                Settings.CustomConfig.tagFoodPrewar,
                Settings.CustomConfig.tagDrink,
                Settings.CustomConfig.tagLiquor,
                Settings.CustomConfig.tagNukacola,
                Settings.CustomConfig.tagSyringe,
                Settings.CustomConfig.tagDevice,
                Settings.CustomConfig.tagTool,
                Settings.CustomConfig.tagNews,
                Settings.CustomConfig.tagNote,
                Settings.CustomConfig.tagPerkmag,
                Settings.CustomConfig.tagMine,
                Settings.CustomConfig.tagGrenade,
                Settings.CustomConfig.tagKey,
                Settings.CustomConfig.tagKeyCard,
                Settings.CustomConfig.tagPassword,
                Settings.CustomConfig.tagAmmo,
                Settings.CustomConfig.tagHolotape,
                Settings.CustomConfig.tagHolotapeGame,
                Settings.CustomConfig.tagHolotapeSettings,
                Settings.CustomConfig.tagPipBoy,
                Settings.CustomConfig.tagWeaponRanged,
                Settings.CustomConfig.tagWeaponMelee,
                Settings.CustomConfig.tagArmor,
                Settings.CustomConfig.tagClothing,
                Settings.CustomConfig.tagVaultSuit,
                Settings.CustomConfig.tagPowerArmor,
                Settings.CustomConfig.extraValidTags
            );

            return customConfig;
        }
    }
}
