using Mutagen.Bethesda.Synthesis.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeItemsWeightless
{
    internal class WeightSettings
    {
        [SynthesisOrder]
        [SynthesisSettingName("Misc Items")]
        [SynthesisTooltip("Make Misc Items weightless")]
        public bool useMisc = true;

        [SynthesisOrder]
        [SynthesisSettingName("Ammo")]
        [SynthesisTooltip("Make Ammunition weightless")]
        public bool useAmmo = true;

        [SynthesisOrder]
        [SynthesisSettingName("Holotapes")]
        [SynthesisTooltip("Make Holotapes weightless")]
        public bool useHolotape = true;

        [SynthesisOrder]
        [SynthesisSettingName("Keys")]
        [SynthesisTooltip("Make Keys weightless")]
        public bool useKeys = true;

        [SynthesisOrder]
        [SynthesisSettingName("Notes")]
        [SynthesisTooltip("Make Notes/Books/Magazines weightless")]
        public bool useBook = true;

        [SynthesisOrder]
        [SynthesisSettingName("Ingestibles")]
        [SynthesisTooltip("Make Ingestibles weightless")]
        public bool useAlch = false;

        [SynthesisOrder]
        [SynthesisSettingName("Weapons")]
        [SynthesisTooltip("Make Weapons weightless. Object Modifications might still increase the weight")]
        public bool useWeap = false;

        [SynthesisOrder]
        [SynthesisSettingName("Armor & Clothing")]
        [SynthesisTooltip("Make Armors and Clothing weightless. Object Modifications might still increase the weight")]
        public bool useArmo = false;

    }
}
