using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.FormKeys.Fallout4;
using Mutagen.Bethesda.Plugins;
using Noggog;
using System.Text.RegularExpressions;

namespace ItemTagger.ItemTypeFinder
{
    internal class ItemTypeData
    {
        // TODO potentially move this to a JSON/other config file, when I figure out how

        // blacklists
        public readonly MatchingList blacklistScript = new();

        public readonly MatchingList blacklistEdid = new();
        public readonly MatchingList blacklistName = new();
        public readonly MatchingFormList<IKeywordGetter> keywordsGlobalBlacklist = new();

        // TODO refactor this. use MatchingListSets
        // whitelists
        // models
        // public readonly MatchingList modelListTool = new();

        //public readonly MatchingList modelListStealthboy = new();
        //public readonly MatchingList modelListDevice = new();


        // public readonly MatchingList modelListCard = new();

        // public readonly MatchingList modelListKey = new();
        // public readonly MatchingList modelListPassword = new();

        // public readonly MatchingList modelListFoodCrop = new();
        // public readonly MatchingList modelListNews = new();
        // public readonly MatchingList modelListPipBoy = new();

        // scripts
        // public readonly MatchingList scriptListPipBoy = new();
        // public readonly MatchingList scriptListPerkMag = new();
        // public readonly MatchingList scriptListNews = new();

        // programs (holotapes only)
        public readonly MatchingList programListGame = new();

        // names
        public readonly MatchingList nameListSettings = new();

        // Matching list sets
        public readonly MatchingListSet matchSetEdid = new();
        public readonly MatchingListSet matchSetScript = new();
        public readonly MatchingListSet matchSetModel = new();


        // keywords
        public readonly MatchingFormListSet<IKeywordGetter> matchSetKeyword = new();
        // special, to tell foods apart
        public readonly MatchingFormListSet<IKeywordGetter> matchSetKeywordFood = new();

        // leave this separate, because this requires the addiction check
        public readonly MatchingFormList<IKeywordGetter> keywordListChem = new();

        // sounds
        public readonly MatchingFormListSet<ISoundDescriptorGetter> matchSetSound = new();
        // leave these 2 separately
        public readonly MatchingFormList<ISoundDescriptorGetter> soundListFood = new();
        public readonly MatchingFormList<ISoundDescriptorGetter> soundListChem = new();

        // public readonly MatchingFormList<ISoundDescriptorGetter> soundListDevice = new();
        // public readonly MatchingFormList<ISoundDescriptorGetter> soundListTool = new();

        public readonly MatchingFormList<IInstanceNamingRulesGetter> innrListSkip = new();

        // regexes? regices? regex objects for matching special stuff
        private static readonly Regex MATCH_MODEL_CARD = new(@"card[^\\]*\.nif$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_MODEL_NOTE = new(@"note[^\\]\.nif$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_MODEL_PIPBOY = new(@"pipboy[^\\]\.nif$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex MATCH_SS2_PLUGINNAME = new(@"^.*SS2.*_PluginName_", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_SS2_CPDESIGNER = new(@"^.*SS2.*_CPDesigner_", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_SS2_TEMPLATE = new(@"^.*SS2.*_Template", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_SS2_WEAPLIST = new(@"^.*SS2.*_WeaponList_", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_SS2_THEMESELECTOR = new(@"^.*SS2.*_ThemeSelector_", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        //"SS2_ThemeSelector_EmpireFlag"
        // hard overrides
        public readonly Dictionary<FormKey, ItemType> hardcodedOverrides = new();

        public ItemTypeData()
        {
            // == Blacklists ==
            blacklistScript.AddExactMatch(
                "simsettlements:simbuildingplan",
                "simsettlements:simstory",
                "simsettlements:cityplan",
                "simsettlements:simplanpath",
                "simsettlements:dynamicflag",
                "simsettlements:leadercard",
                "simsettlements:medicalresearchrecipe",
                "simsettlements:cityplanlayer",
                "simsettlements:simbuildingplanskin",
                "simsettlements:spawnablefoundation",
                "simsettlements:factionunitdata",
                "simsettlementsv2:miscobjects:addonpackconfiguration",
                "simsettlementsv2:miscobjects:buildingplantheme",
                "simsettlementsv2:miscobjects:cameracontrolsequence",
                "simsettlementsv2:miscobjects:characterstatcard",
                "simsettlementsv2:miscobjects:foundation",
                "simsettlementsv2:miscobjects:powerpole",
                "simsettlementsv2:miscobjects:npcpreferences",
                "simsettlementsv2:miscobjects:optionsprofile",
                "simsettlementsv2:miscobjects:plotmessages",
                "simsettlementsv2:miscobjects:settlerlocationdiscovery",
                "simsettlementsv2:miscobjects:soundscapelayer",
                "simsettlementsv2:miscobjects:stageitem",
                "simsettlementsv2:miscobjects:usagerequirements",
                "simsettlementsv2:miscobjects:medicalresearchrecipe",
                "workshopframework:library:objectrefs:preventlooting",
                "workshopplus:objectreferences:blueprint",
                "simsettlementsv2:objectreferences:unitselectorform",
                "SimSettlementsV2:MiscObjects:WorldRepopulationCell",
                "simsettlementsv2:miscobjects:ideologychoice",
                "simsettlementsv2:miscobjects:factionname",
                "SimSettlementsV2:MiscObjects:Disaster_AttackDefinition",
                "VFX:MiscObjects:UniversalUnlockable"
            );

            blacklistScript.AddPrefixMatch(
                "autobuilder:",
                "simsettlementsv2:armors:",
                "simsettlementsv2:weapons:",
                "simsettlementsv2:miscobjects:leadertrait",
                "simsettlementsv2:miscobjects:themeruleset",
                "simsettlementsv2:miscobjects:plotconfiguration",
                "simsettlementsv2:miscobjects:unlockable",
                "SimSettlementsV2:HQ:",
                "SimSettlementsV2:MiscObjects:Unlockable"
            );

            blacklistEdid.AddExactMatch(
                "HM_UnassignedLabel"
            );

            // let's just ignore ALL nameholders
            blacklistEdid.AddSubstringMatch(
                "Nameholder"
            );

            blacklistEdid.AddPrefixMatch(
                "DN015_NoneNameMisc",
                "DummyNoEdit_",
                //"SS2_NameHolder_",
                "SS2_SettlerDiscovery_",
                "SS2_Unlockable_",
                "SS2_LeaderTrait_",
                "SS2_SLCP_",
                "SS2_Skin_",
                "SS2_BP_Randomizer",
                "HC_Cannibal_RavenousHunger",
                "HC_DiseaseEffect_",
                "HC_Effect_",
                "HC_EncumbranceEffect_",
                "HC_AdrenalineEffect",
                "HC_HungerEffect_",
                "HC_SleepEffect_",
                "HC_ThirstEffect_",
                //"WSFW_NameHolder_",
                "kgConq_AssaultQuestVerb_",
                "RECYCLED_MISC_",
                "WSFW_Blank",
                "UnarmedSuperMutant",
                "MS02NukeMissileFar",
                "WorkshopArtilleryWeapon",
                "GasTrapDummy",
                //"SS2C2_Nameholder_",
                "SS2_HQWorkerSelectForm_",
                "kgSIM_TextReplace_",
                "SS2RotC_PluginName_",
                "SS2RotC_CPDesigner_",
                "SS2_Tag_VRWorldspaceConfig_",
                "SS2_DisasterInfestationDefinition_",
                "SS2_DisasterAttackDefinition__",
                "SS2_VRWorldspaceConfig_",
                "VFX-SS2_Template_UnlockableCharacter_",
                "VFX-SS2__UnlockableGenericNew_",
                //"WV_ProductionNameHolder_",
                "SS2_TerritoryTrait_",
                "SS2_LoadoutDescription_",
                "SS2C2_DoNotUse_Obsolete",
                "SS2_C3_MapMarkerHandler_",
                "kgSIM_VIPStory",
                "praLibrary_PrizeDummy_"
             );

            blacklistEdid.AddRegexMatch(
                MATCH_SS2_THEMESELECTOR,
                MATCH_SS2_PLUGINNAME,
                MATCH_SS2_CPDESIGNER,
                MATCH_SS2_TEMPLATE,
                MATCH_SS2_WEAPLIST
            );

            keywordsGlobalBlacklist.Add(
                "01F43E:SS2.esm", // SS2_Tag_PetName
                "01F43F:SS2.esm"  // SS2_Tag_NPCName
            ); 

            // == EDIDs ==

            matchSetEdid
                .GetListForType(ItemType.Holotape)
                .AddPrefixMatch(
                    "praSS2_HolotapeDummy_"
                );

            matchSetEdid
                .GetListForType(ItemType.HolotapeSettings)
                .AddSubstringMatch("setting", "config", "cheat");

            matchSetEdid
                .GetListForType(ItemType.GoodChem)
                .AddPrefixMatch("kgSIM_MedicalResearch_");

            matchSetEdid
                .GetListForType(ItemType.Quest)
                .AddPrefixMatch(
                    "DLC01MQ03RobotPart",
                    "SS2_QuestItem_",
                    "SS2C2_QuestItem_",
                    "kgSIM_EnergySignatureScanner_",
                    "DLC06WorkshopControlBoard",
                    "kgSIM_IndRev_Fuse",
                    "DLC03_Banner_",
                    "SS2C2_NQ01_Inv_BookCrate"
                )
                .AddExactMatch(
                    "kgSIM_MicrofusionSubImage",
                    "SS2_WC_ArchonPlans",
                    "SS2_MQ07_TechnicalDocument",
                    "SS2_MQ12JakesScrewdriver",
                    "kgSIM_MedicalResearch_MixedSample",
                    "kgSIM_IndRev_ActiveCore"
                );

            matchSetEdid
                .GetListForType(ItemType.Collectible)
                .AddPrefixMatch(
                    "SS2_PurchaseableJunkSculpture_",
                    "BotModel",
                    "DLC04ParkMedallion"
                );

            matchSetEdid
                .GetListForType(ItemType.Shipment)
                .AddPrefixMatch(
                    "SS2_PriorityShipment_",
                    "SS2E_PurchaseablePet_",
                    "SS2_PurchaseablePet_",
                    "SS2_PurchaseableFurniture_",
                    "ESM_MISC_"
                );

            matchSetEdid
                .GetListForType(ItemType.OtherMisc)
                .AddPrefixMatch("SS2_LootBox_");

            matchSetEdid
                .GetListForType(ItemType.Device)
                .AddExactMatch(
                    "SS2_ASAM_SensorScanner",
                    "kgSIM_EnergySignatureScanner_Inventory",
                    "SS2_SalvageBeacon",
                    "SS2_Mark1Beacon",
                    "SS2UI_MenuOpener"
                );

            matchSetEdid
                .GetListForType(ItemType.LooseMod)
                .AddPrefixMatch(
                    "pa_comp_T60_" // these things are configured in a really weird way, so, hardcoding them like this
                );

            matchSetEdid
                .GetListForType(ItemType.Book)
                .AddPrefixMatch(
                    "SS2_BookStoreStock_"
                );

            // == Models ==
            matchSetModel.GetListForType(ItemType.Tool).AddExactMatch(
                "autobuildplots\\weapons\\hammer\\hammer.nif",
                "props\\smithingtools\\smithingtoolhammer01a.nif",
                "props\\smithingtools\\smithingtoolhammer01b.nif",
                "props\\smithingtools\\smithingtoolhammer02.nif",
                "props\\smithingtools\\smithingtoolhammer03.nif",
                "props\\smithingtools\\smithingtoolsaw01.nif",
                "props\\smithingtools\\smithingtoolsaw02.nif",
                "props\\smithingtools\\smithingtooltongs01.nif",
                "props\\surgicaltools\\surgicalcutter.nif",
                "props\\surgicaltools\\surgicalscalpel.nif",
                "props\\surgicaltools\\surgicalscissors.nif",
                "props\\blowtorch.nif",
                "props\\blowtorch_rare.nif",
                "props\\clipboard.nif",
                "props\\clipboard_prewar.nif",
                "props\\cuttingboard.nif",
                "props\\oilcan.nif",
                "props\\clothingiron\\clothingiron.nif",
                "props\\fishingreel.nif"
            );

            /*
             * there are LOTS of togglers etc which use the stealthboy nif
            matchSetModel.GetListForType(ItemType.StealthBoy).AddExactMatch(
                "props\\stealthboy01.nif" 
            );
            */

            matchSetModel.GetListForType(ItemType.Device).AddExactMatch(
                "dlc01\\props\\dlc01_robotrepairkit01.nif",
                "props\\stealthboy01.nif",
                "props\\bs101radiotransmittor.nif",
                "props\\bosdistresspulser\\bosdistresspulserglowing.nif",
                "props\\boscerebrofusionadaptor\\boscerebrofusionadaptor.nif",
                "actors\\libertyprime\\characterassets\\agitator.nif",
                "props\\bosdistresspulser\\bosdistresspulser.nif",
                "props\\bosreflexcapacitor\\bosreflexcapacitor.nif",
                "props\\boshapticdrive\\boshapticdrive.nif",
                "props\\bosfluxsensor\\bosfluxsensor.nif",
                "props\\generickeycard01.nif",
                "props\\militarycircuitboard\\militarycircuitboard.nif",
                "props\\prewar_toaster.nif",
                "props\\postwar_toaster.nif",
                "setdressing\\building\\deskfanoffice01.nif",
                "setdressing\\building\\deskfanofficeoff01.nif",
                "dlc01\\props\\dlc01_componentpart.nif",
                "dlc01\\props\\dlc01_componentwhole.nif",
                "dlc01\\props\\dlc01_amplifier01.nif",
                "props\\synthchip\\synthchip.nif",
                "dlc03\\props\\dlc03_fogcondenserpowermodule.nif",
                "props\\ms11powerrelaycoil.nif",
                "props\\component\\component_circuitry.nif",
                "props\\biomedicalscanner\\biomedicalscanner.nif",
                "props\\camera.nif",
                "props\\fuse01.nif",
                "props\\fusionpulsecharge\\fusionpulsecharge.nif",
                "props\\bos_magnet.nif",
                "props\\hotplate.nif",
                "props\\ms11radartransmittor.nif",
                "setdressing\\lightfixtures\\lightbulboff.nif",
                "props\\chipboard.nif",
                "setdressing\\quest\\genprototype01.nif",
                "props\\vacuumtube01.nif",
                "weapons\\grenade\\transmitgrenadeprojectile.nif",
                "props\\pipboymiscitem\\pipboymisc01.nif",
                "dlc06\\props\\pipboymiscitem\\dlc06pipboymisc01.nif",
                "setdressing\\factions\\railroad\\tinkertomsdevice01.nif",
                "SS2\\Props\\ASAMSensor_StandingAnimated.nif",
                "SS2\\Props\\ASAMTop_Moveable.nif"
            );

            matchSetModel.GetListForType(ItemType.News)
                .AddExactMatch(
                    "props\\newspaperpublickoccurenceslowpoly.nif",
                    "SS_IndRev\\Props\\NewspaperNewBugle.nif",
                    "SS2C2\\Props\\Newspaper.nif",
                    "Props\\Newspaper01.nif",
                    "Props\\Newspaper02.nif"
                  );

            matchSetModel.GetListForType(ItemType.Book)
                .AddExactMatch(
                    "RealBooks\\BurntBook01.nif"
                  );

            matchSetModel.GetListForType(ItemType.Perkmag)
                .AddExactMatch(
                    "SS_IndRev\\Props\\Magazine.nif"
                  )
                .AddPrefixMatch(
                    "Props\\GrognakComic\\",
                    "MunkySpunk\\Props\\Comic_"
                );

            matchSetModel.GetListForType(ItemType.Key).AddExactMatch(
                "props\\key01.nif",
                "props\\key02.nif",
                "props\\key02.nif",
                "props\\key_chain01.nif",
                "props\\key_chain02.nif",
                "props\\key_chain03.nif",
                "props\\ms07key.nif"
            );

            matchSetModel.GetListForType(ItemType.KeyCard)
                .AddExactMatch(
                    "props\\generickeycard01.nif",
                    "props\\vaultidcard.nif"
                )
                .AddRegexMatch(MATCH_MODEL_CARD);

            matchSetModel.GetListForType(ItemType.KeyPassword)
                .AddExactMatch(
                    "props\\holotape_prop.nif",
                    "interface\\newspaper\\noteripped.nif",
                    "props\\note_lowpoly.nif"
                )
                .AddRegexMatch(MATCH_MODEL_NOTE);

            matchSetModel.GetListForType(ItemType.FoodCrop)
                .AddExactMatch("Landscape\\Plants\\Ingredients\\WastelandFungusStalkIngredient01.nif");

            matchSetModel.GetListForType(ItemType.PipBoy)
                .AddRegexMatch(MATCH_MODEL_PIPBOY);

            matchSetModel.GetListForType(ItemType.FusionCore)
                .AddExactMatch("Ammo\\FusionCore\\FusionCore.nif");

            matchSetModel.GetListForType(ItemType.MiniNuke)
                .AddExactMatch(
                    "Ammo\\FatMan\\AmmoFatMan.nif",
                    "DLC04\\Ammo\\DLC04AmmoNukaNuke.nif"
                );

            // == Scripts ==
            matchSetScript.GetListForType(ItemType.News)
                .AddExactMatch(
                    "SimSettlements:Newspaper", "SimSettlementsV2:Books:NewsArticle"
                );

            matchSetScript.GetListForType(ItemType.Perkmag)
               .AddExactMatch(
                    "CA_SkillMagazineScript",
                    "SimSettlementsV2:Books:MagazineIssue"
               );

            matchSetScript.GetListForType(ItemType.PipBoy)
               .AddSuffixMatch("PipboyMiscItemScript");

            matchSetScript.GetListForType(ItemType.Tool)
                .AddExactMatch("SimSettlementsV2:ObjectReferences:DeployableCityPlannersDesk");

            matchSetScript
                .GetListForType(ItemType.Holotape)
                .AddExactMatch("praVRF:SimulationData");

            matchSetScript
                .GetListForType(ItemType.Shipment)
                .AddExactMatch(
                    "SimSettlementsV2:MiscObjects:PetStoreCreatureItem",
                    "SimSettlementsV2:MiscObjects:FurnitureStoreItem"
                );

            // == Keywords ==

            matchSetKeyword
                .GetListForType(ItemType.WeaponMelee)
                .Add(
                    Fallout4.Keyword.WeaponTypeMelee1H,
                    Fallout4.Keyword.WeaponTypeMelee2H,
                    Fallout4.Keyword.WeaponTypeUnarmed
                );

            matchSetKeyword
                .GetListForType(ItemType.Perkmag)
                .Add(Fallout4.Keyword.PerkMagKeyword);


            matchSetKeyword.GetListForType(ItemType.Collectible)
                .Add(Fallout4.Keyword.FeaturedItem);

            matchSetKeyword.GetListForType(ItemType.Quest)
                .Add(
                    Fallout4.Keyword.VendorItemNoSale,
                    Fallout4.Keyword.UnscrappableObject
                );

            matchSetKeyword.GetListForType(ItemType.Drink)
                .Add(
                    Fallout4.Keyword.ObjectTypeWater,
                    Fallout4.Keyword.ObjectTypeDrink,
                    Fallout4.Keyword.HC_SustenanceType_QuenchesThirst,
                    Fallout4.Keyword.ObjectTypeCaffeinated
                );

            matchSetKeyword.GetListForType(ItemType.Food)
                .Add(Fallout4.Keyword.FoodEffect,
                    Fallout4.Keyword.FruitOrVegetable,
                    Fallout4.Keyword.ObjectTypeFood
                );

            matchSetKeywordFood.GetListForType(ItemType.FoodRaw)
                .Add(
                    Fallout4.Keyword.HC_DiseaseRisk_FoodHigh,
                    Fallout4.Keyword.HC_DiseaseRisk_FoodStandard
                );
            matchSetKeywordFood.GetListForType(ItemType.FoodCrop)
                .Add(
                    Fallout4.Keyword.FruitOrVegetable
                );

            matchSetKeyword.GetListForType(ItemType.StealthBoy)
                .Add(
                    Fallout4.Keyword.StealthBoyKeyword
                );

            matchSetKeyword.GetListForType(ItemType.Device)
                .Add(
                    Fallout4.Keyword.ChemTypeStealthBoy,
                    Robot.Keyword.DLC01ObjectTypeRepairKit
                );

            matchSetKeyword.GetListForType(ItemType.FusionCore)
                .Add(
                    Fallout4.Keyword.isPowerArmorBattery
                );

            keywordListChem.Add(
                Fallout4.Keyword.ObjectTypeStimpak,
                Fallout4.Keyword.ObjectTypeChem,
                Fallout4.Keyword.CA_ObjType_ChemBad,
                Fallout4.Keyword.HC_DiseaseRiskChem,
                Fallout4.Keyword.HC_CausesImmunodeficiency,
                Fallout4.Keyword.HC_SustenanceType_IncreasesHunger
            );

            // == Sounds ==
            soundListFood.Add(
               Fallout4.SoundDescriptor.NPCHumanEatChewy,
               Fallout4.SoundDescriptor.NPCHumanEatGeneric,
               Fallout4.SoundDescriptor.NPCHumanEatEgg,
               Fallout4.SoundDescriptor.NPCHumanEatSoup,
               Fallout4.SoundDescriptor.NPCHumanEatSoupSlurp
           );

            soundListChem.Add(
                Fallout4.SoundDescriptor.NPCHumanEatMentats,
                Fallout4.SoundDescriptor.NPCHumanChemsPsycho,
                Fallout4.SoundDescriptor.NPCHumanChemsUseJet,
                Fallout4.SoundDescriptor.NPCHumanChemsAddictol
            );

            matchSetSound.GetListForType(ItemType.Device)
                .Add(Fallout4.SoundDescriptor.OBJStealthBoyActivate);// leave it here for now

            matchSetSound.GetListForType(ItemType.Tool)
                .Add(Fallout4.SoundDescriptor.NPCHumanWhistleDog);


            // == Programs == 

            programListGame.AddExactMatch(
                "atomiccommand.swf",
                "grognak.swf",
                "pipfall.swf",
                "zetainvaders.swf",
                "redmenace.swf",
                "automatron\\automatron.swf"
            );

            // == Names == 
            nameListSettings.AddPrefixMatch(" - ");
            nameListSettings.AddSubstringMatch("setting", "config");

            

           
            // == INNRs ==
            innrListSkip.Add(
                Fallout4.InstanceNamingRules.dn_BACKUP.FormKey,
                Fallout4.InstanceNamingRules.dn_Clothes.FormKey,
                Fallout4.InstanceNamingRules.dn_CommonArmor.FormKey,
                Fallout4.InstanceNamingRules.dn_CommonGun.FormKey,
                Fallout4.InstanceNamingRules.dn_CommonMelee.FormKey,
                Fallout4.InstanceNamingRules.dn_PowerArmor.FormKey,
                Fallout4.InstanceNamingRules.dn_VaultSuit.FormKey,
        
                NukaWorld.InstanceNamingRules.dn_DLC04_PowerArmor_NukaCola.FormKey,
                NukaWorld.InstanceNamingRules.dn_DLC04_PowerArmor_Overboss.FormKey,
                NukaWorld.InstanceNamingRules.dn_DLC04_PowerArmor_Quantum.FormKey,
                NukaWorld.InstanceNamingRules.DLC04_dn_CommonArmorUpdate.FormKey,
                NukaWorld.InstanceNamingRules.DLC04_dn_CommonGunUpdate.FormKey,
                NukaWorld.InstanceNamingRules.DLC04_dn_CommonMeleeUpdate.FormKey,
    
                Coast.InstanceNamingRules.DLC03_dn_CommonArmor.FormKey,
                Coast.InstanceNamingRules.DLC03_dn_CommonGun.FormKey,
                Coast.InstanceNamingRules.DLC03_dn_CommonMelee.FormKey,
                Coast.InstanceNamingRules.DLC03_dn_Legendary_Armor.FormKey,
                Coast.InstanceNamingRules.DLC03_dn_Legendary_Weapons.FormKey,
     
                Robot.InstanceNamingRules.DLC01dn_LightningGun.FormKey,
                Robot.InstanceNamingRules.DLC01dn_PowerArmor.FormKey
            );



            // == hard overrides ==
            hardcodedOverrides.Add(Fallout4.Ingestible.StealthBoy.FormKey, ItemType.StealthBoy);
            hardcodedOverrides.Add(Fallout4.Ingestible.RRStealthBoy.FormKey, ItemType.StealthBoy);

            hardcodedOverrides.Add(Fallout4.Ingestible.GroundMolerat.FormKey, ItemType.Food);
            hardcodedOverrides.Add(Fallout4.Ingestible.MS05BEggDeathclaw.FormKey, ItemType.FoodRaw);
            hardcodedOverrides.Add(Fallout4.MiscItem.BoS303BerylliumAgitator.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.RR102CarringtonPrototype.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.BoS301FusionAdapter.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.MQ205_CourserChip.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.MS02DampeningCoil.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.BoS101DeepRangeTransmitter.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.BoS301DistressPulser.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.RR303TinkExplosives.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.BoSR02_Drive.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.RRR05MILAObject.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.COMMacCreadyPreventSample.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.RRR09CarePackage.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.InstR02Sample.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.MS02Warhead.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.InstR03Blueprints01.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.DN053_VirgilsSerum.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.MS17TedCompoundMap.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.InstM01Seeds.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.MQ206SignalInterceptorPlans.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.MS04CallingCard.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.InstR04Transmitter.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Fallout4.MiscItem.BoSBloodSample.FormKey, ItemType.Quest);

            //hardcodedOverrides.Add(Coast.MiscItem.DLC03_Banner_Completed.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03AtomM02FlyerWhole.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03FogCondenserModule.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03AtomM01_MotherIcon.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03_FFNucleus03_PumpRegulator.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03_AcadiaM01_HeadQuestItem.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03DiMAMemoryHardDrive01.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03AtomM02MapHalf1.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Coast.MiscItem.DLC03AtomM02MapHalf2.FormKey, ItemType.Quest);

            hardcodedOverrides.Add(NukaWorld.MiscItem.DLC04_ProjectCobaltSchematics.FormKey, ItemType.Quest);

            hardcodedOverrides.Add(Robot.MiscItem.DLC01MQ01RadarBeacon.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Robot.MiscItem.DLC01MQ03RobotPart01.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Robot.MiscItem.DLC01MQ02Schematics.FormKey, ItemType.Quest);
            hardcodedOverrides.Add(Robot.MiscItem.DLC01MQ04JezebelHead.FormKey, ItemType.Quest);
            //ovrWeapon.Add(Fallout4)

            // blacklist
            hardcodedOverrides.Add(Workshop01.Ingestible.DLC02WorkshopDetectLifeTest.FormKey, ItemType.None);
        }
    }
}
