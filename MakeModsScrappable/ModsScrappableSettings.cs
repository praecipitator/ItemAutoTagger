using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.FormKeys.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Synthesis.Settings;

namespace MakeModsScrappable
{
    internal class ModsScrappableSettings
    {
        [SynthesisOrder]
        [SynthesisSettingName("Component return factor")]
        [SynthesisTooltip("This will be multiplied onto the amounts of components.")]
        public double componentLossFactor = 0.75;

        [SynthesisOrder]
        [SynthesisSettingName("Rounding Mode")]
        [SynthesisTooltip("How should amounts be rounded after applying the component return factor. \"Normal\" means regular rounding, \"Up\" and \"Down\" will always round up and down respectively.")]
        public RoundingMode roundMode = RoundingMode.Normal;

        [SynthesisOrder]
        [SynthesisSettingName("Components to exclude")]
        [SynthesisTooltip("These components will be excluded entirely. Should be unrecoverable components, or components which get used up while crafting.")]
        public List<IFormLinkGetter<IComponentGetter>> excludeList = new()
        {
            Fallout4.Component.c_Acid,
            Fallout4.Component.c_Adhesive,
            Fallout4.Component.c_Antiseptic,
            Fallout4.Component.c_Oil,
        };
    }

    public enum RoundingMode
    {
        Up,
        Normal,
        Down
    }    
}
