using ItemTagger.ItemTypeFinder;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Synthesis.Settings;
using Noggog;

namespace ItemTypeFinder.Settings
{
    public class TyperSettings
    {
        [SynthesisOrder]
        [SynthesisSettingName("Override Item Type Settings")]
        [SynthesisTooltip("Here, you can manually configure the types of items. The actual tag depends on the selected Tagging Configuration")]
        public ItemTypeOverrides ItemTypeConfig = new();
    }


    public class ItemTypeOverrides
    {
        [SynthesisOrder]
        [SynthesisSettingName("Misc Items")]
        public List<FormTypeMapping<IMiscItemGetter>> MiscItems = new();

        [SynthesisOrder]
        [SynthesisSettingName("Ingestibles")]
        public List<FormTypeMapping<IIngestibleGetter>> Alch = new();

        [SynthesisOrder]
        [SynthesisSettingName("Weapons")]
        public List<FormTypeMapping<IWeaponGetter>> Weaps = new();

        [SynthesisOrder]
        [SynthesisSettingName("Armor and Clothing")]
        public List<FormTypeMapping<IArmorGetter>> Armors = new();

        [SynthesisOrder]
        [SynthesisSettingName("Notes and Magazines")]
        public List<FormTypeMapping<IBookGetter>> Books = new();

        [SynthesisOrder]
        [SynthesisSettingName("Holotapes")]
        public List<FormTypeMapping<IHolotapeGetter>> Holotapes = new();

        [SynthesisOrder]
        [SynthesisSettingName("Ammunition")]
        public List<FormTypeMapping<IAmmunitionGetter>> Ammo = new();

        [SynthesisOrder]
        [SynthesisSettingName("Keys")]
        public List<FormTypeMapping<IKeyGetter>> Keys = new();

        public Dictionary<FormKey, ItemType> GetMiscOverrides()
        {
            return GetAsDictionary(MiscItems);
        }

        public Dictionary<FormKey, ItemType> GetAlchOverrides()
        {
            return GetAsDictionary(Alch);
        }

        public Dictionary<FormKey, ItemType> GetWeapOverrides()
        {
            return GetAsDictionary(Weaps);
        }

        public Dictionary<FormKey, ItemType> GetArmorOverrides()
        {
            return GetAsDictionary(Armors);
        }

        public Dictionary<FormKey, ItemType> GetBookOverrides()
        {
            return GetAsDictionary(Books);
        }

        public Dictionary<FormKey, ItemType> GetHolotapeOverrides()
        {
            return GetAsDictionary(Holotapes);
        }

        public Dictionary<FormKey, ItemType> GetAmmoOverrides()
        {
            return GetAsDictionary(Ammo);
        }

        public Dictionary<FormKey, ItemType> GetKeysOverrides()
        {
            return GetAsDictionary(Keys);
        }

        public Dictionary<FormKey, ItemType> GetMergedOverrides()
        {
            Dictionary<FormKey, ItemType> result = new();

            MergeIntoDictionary(result, MiscItems);
            MergeIntoDictionary(result, Alch);
            MergeIntoDictionary(result, Weaps);
            MergeIntoDictionary(result, Armors);
            MergeIntoDictionary(result, Books);
            MergeIntoDictionary(result, Holotapes);
            MergeIntoDictionary(result, Ammo);
            MergeIntoDictionary(result, Keys);

            return result;
        }

        private static void MergeIntoDictionary<T>(Dictionary<FormKey, ItemType> result, List<FormTypeMapping<T>> list)
            where T : class, IMajorRecordGetter
        {
            foreach (var entry in list)
            {
                if (entry.form.IsNull)
                {
                    continue;
                }
                result.Set(entry.form.FormKey, entry.type);
            }
        }

        private static Dictionary<FormKey, ItemType> GetAsDictionary<T>(List<FormTypeMapping<T>> list)
            where T : class, IMajorRecordGetter
        {
            Dictionary<FormKey, ItemType> result = new();
            foreach (var entry in list)
            {
                if (entry.form.IsNull)
                {
                    continue;
                }
                result.Set(entry.form.FormKey, entry.type);
            }
            return result;
        }
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
}
