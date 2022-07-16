using System.Text.RegularExpressions;
using Mutagen.Bethesda.FormKeys.Fallout4;
using Mutagen.Bethesda.Fallout4;
using Mutagen.Bethesda.Plugins;

namespace ItemTagger.ItemTypeFinder
{
    internal class ItemTypeData
    {
        // TODO potentially move this to a JSON/other config file, when I figure out how

        // blacklists
        public readonly MatchingList blacklistScript = new();
        public readonly MatchingList blacklistEdid = new();
        public readonly MatchingList blacklistName = new();

        // whitelists
        public readonly MatchingList whitelistModelTool = new();
        public readonly MatchingList whitelistModelDevice = new();

        // models
        public readonly MatchingList modelListCard = new();
        public readonly MatchingList modelListKey = new();
        public readonly MatchingList modelListPassword = new();
        public readonly MatchingList modelListFoodCrop = new();

        public readonly MatchingList modelListPipBoy = new();
        public readonly MatchingList scriptListPipBoy = new();
        public readonly MatchingList scriptListPerkMag = new();

        public readonly MatchingList whitelistModelNews = new();
        public readonly MatchingList scriptListNews = new();

        public readonly MatchingList programListGame = new();
        public readonly MatchingList nameListSettings = new();

        // edid lists
        //public readonly MatchingList edidListSettings = new();
        //public readonly MatchingList edidListGoodChem = new();

        public readonly MatchingListSet edidMatchLists = new();

        // keywords
        public readonly MatchingFormList<IKeywordGetter> keywordsWeaponMelee = new();
        public readonly MatchingFormList<IKeywordGetter> keywordsGlobalBlacklist = new();
        public readonly MatchingFormList<IKeywordGetter> keywordsPerkmag = new();
        public readonly MatchingFormList<IKeywordGetter> keywordsQuest = new();
        public readonly MatchingFormList<IKeywordGetter> keywordListDrink = new();
        public readonly MatchingFormList<IKeywordGetter> keywordListFood = new();
        public readonly MatchingFormList<IKeywordGetter> keywordListFoodDisease = new();
        public readonly MatchingFormList<IKeywordGetter> keywordListDevice = new();
        public readonly MatchingFormList<IKeywordGetter> keywordListChem = new();

        // sounds
        public readonly MatchingFormList<ISoundDescriptorGetter> soundListFood = new();
        public readonly MatchingFormList<ISoundDescriptorGetter> soundListChem = new();
        public readonly MatchingFormList<ISoundDescriptorGetter> soundListDevice = new();
        public readonly MatchingFormList<ISoundDescriptorGetter> soundListTool = new();

        // regexes? regices? regex objects for matching special stuff
        private static readonly Regex MATCH_MODEL_MODEL  = new(@"card[^\\]*\.nif$",  RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_MODEL_NOTE   = new(@"note[^\\]\.nif$",   RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_MODEL_PIPBOY = new(@"pipboy[^\\]\.nif$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        // hard overrides
        public readonly Dictionary<FormKey, ItemType> hardcodedOverrides = new();


        public ItemTypeData()
        {
            keywordsWeaponMelee.Add(Fallout4.Keyword.WeaponTypeMelee1H);
            keywordsWeaponMelee.Add(Fallout4.Keyword.WeaponTypeMelee2H);
            keywordsWeaponMelee.Add(Fallout4.Keyword.WeaponTypeUnarmed);

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
                "SimSettlementsV2:MiscObjects:WorldRepopulationCell"
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

            blacklistEdid.AddPrefixMatch(
                "DN015_NoneNameMisc",
                "DummyNoEdit_",
                "SS2_NameHolder_",
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
                "WSFW_NameHolder_",
                "kgConq_AssaultQuestVerb_",
                "RECYCLED_MISC_",
                "WSFW_Blank",
                "UnarmedSuperMutant",
                "MS02NukeMissileFar",
                "WorkshopArtilleryWeapon",
                "GasTrapDummy",
                "SS2C2_Nameholder_",
                "SS2_HQWorkerSelectForm_",
                "kgSIM_TextReplace_"
             );

            whitelistModelTool.AddExactMatch(
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

            whitelistModelDevice.AddExactMatch(
                "props\\stealthboy01.nif",
                "dlc01\\props\\dlc01_robotrepairkit01.nif",
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
                "props\\stealthboy01.nif",
                "weapons\\grenade\\transmitgrenadeprojectile.nif",
                "props\\pipboymiscitem\\pipboymisc01.nif",
                "dlc06\\props\\pipboymiscitem\\dlc06pipboymisc01.nif",
                "setdressing\\factions\\railroad\\tinkertomsdevice01.nif",
                "SS2\\Props\\ASAMSensor_StandingAnimated.nif",
                "SS2\\Props\\ASAMTop_Moveable.nif"
            );

            whitelistModelNews.AddExactMatch(
                "props\\newspaperpublickoccurenceslowpoly.nif", 
                "SS_IndRev\\Props\\NewspaperNewBugle.nif"
              );

            scriptListNews.AddExactMatch(
                "SimSettlements:Newspaper", "SimSettlementsV2:Books:NewsArticle"
            );

            keywordsGlobalBlacklist.Add("01F43E:SS2.esm"); // SS2_Tag_PetName
            keywordsGlobalBlacklist.Add("01F43F:SS2.esm"); // SS2_Tag_NPCName

            keywordsPerkmag.Add(Fallout4.Keyword.PerkMagKeyword);
            scriptListPerkMag.AddExactMatch("CA_SkillMagazineScript");

            // keys
            modelListKey.AddExactMatch("props\\key01.nif",
                "props\\key02.nif",
                "props\\key02.nif",
                "props\\key_chain01.nif",
                "props\\key_chain02.nif",
                "props\\key_chain03.nif",
                "props\\ms07key.nif"
            );

            modelListCard.AddExactMatch(
                "props\\generickeycard01.nif", 
                "props\\vaultidcard.nif"
            );

            modelListCard.AddRegexMatch(MATCH_MODEL_MODEL);

            modelListPassword.AddExactMatch(
                "props\\holotape_prop.nif", 
                "interface\\newspaper\\noteripped.nif",
                "props\\note_lowpoly.nif"
            );
            modelListPassword.AddRegexMatch(MATCH_MODEL_NOTE);

            modelListFoodCrop.AddExactMatch("Landscape\\Plants\\Ingredients\\WastelandFungusStalkIngredient01.nif");

            // stuff
            modelListPipBoy.AddRegexMatch(MATCH_MODEL_PIPBOY);
            scriptListPipBoy.AddSuffixMatch("PipboyMiscItemScript");

            keywordsQuest.Add(Fallout4.Keyword.VendorItemNoSale);
            keywordsQuest.Add(Fallout4.Keyword.UnscrappableObject);

            programListGame.AddExactMatch(
                "atomiccommand.swf",
                "grognak.swf",
                "pipfall.swf",
                "zetainvaders.swf",
                "redmenace.swf",
                "automatron\\automatron.swf"
            );

            

            nameListSettings.AddPrefixMatch(" - ");
            nameListSettings.AddSubstringMatch("setting", "config");

            keywordListDrink.Add(Fallout4.Keyword.ObjectTypeWater);
            keywordListDrink.Add(Fallout4.Keyword.ObjectTypeDrink);
            keywordListDrink.Add(Fallout4.Keyword.HC_SustenanceType_QuenchesThirst);
            keywordListDrink.Add(Fallout4.Keyword.ObjectTypeCaffeinated);

            keywordListFood.Add(Fallout4.Keyword.FoodEffect);

            // these are unreliable, more than just food has these
            //keywordListFood.Add(Fallout4.Keyword.HC_DiseaseRisk_FoodHigh);
            //keywordListFood.Add(Fallout4.Keyword.HC_DiseaseRisk_FoodStandard);

            keywordListFood.Add(Fallout4.Keyword.FruitOrVegetable);
            keywordListFood.Add(Fallout4.Keyword.ObjectTypeFood);


            keywordListFoodDisease.Add(Fallout4.Keyword.HC_DiseaseRisk_FoodHigh);
            keywordListFoodDisease.Add(Fallout4.Keyword.HC_DiseaseRisk_FoodStandard);

            keywordListDevice.Add(Fallout4.Keyword.ChemTypeStealthBoy);
            keywordListDevice.Add(Fallout4.Keyword.StealthBoyKeyword);

            keywordListDevice.Add(Robot.Keyword.DLC01ObjectTypeRepairKit);

            keywordListChem.Add(Fallout4.Keyword.ObjectTypeStimpak);
            keywordListChem.Add(Fallout4.Keyword.ObjectTypeChem);
            keywordListChem.Add(Fallout4.Keyword.CA_ObjType_ChemBad);
            keywordListChem.Add(Fallout4.Keyword.HC_DiseaseRiskChem);
            keywordListChem.Add(Fallout4.Keyword.HC_CausesImmunodeficiency);
            keywordListChem.Add(Fallout4.Keyword.HC_SustenanceType_IncreasesHunger);

            soundListFood.Add(Fallout4.SoundDescriptor.NPCHumanEatChewy);
            soundListFood.Add(Fallout4.SoundDescriptor.NPCHumanEatGeneric);
            soundListFood.Add(Fallout4.SoundDescriptor.NPCHumanEatEgg);
            soundListFood.Add(Fallout4.SoundDescriptor.NPCHumanEatSoup);
            soundListFood.Add(Fallout4.SoundDescriptor.NPCHumanEatSoupSlurp);


            soundListChem.Add(Fallout4.SoundDescriptor.NPCHumanEatMentats);
            soundListChem.Add(Fallout4.SoundDescriptor.NPCHumanChemsPsycho);
            soundListChem.Add(Fallout4.SoundDescriptor.NPCHumanChemsUseJet);
            soundListChem.Add(Fallout4.SoundDescriptor.NPCHumanChemsAddictol);

            soundListDevice.Add(Fallout4.SoundDescriptor.OBJStealthBoyActivate);
            soundListTool.Add(Fallout4.SoundDescriptor.NPCHumanWhistleDog);

            // TODO: 
            // SimSettlementsV2:ObjectReferences:DeployableCityPlannersDesk -> city planner's desk -> tool? device?

            // edid matches

            edidMatchLists
                .GetListForType(ItemType.HolotapeSettings)
                .AddSubstringMatch("setting", "config", "cheat");

            edidMatchLists
                .GetListForType(ItemType.GoodChem)
                .AddPrefixMatch("kgSIM_MedicalResearch_");


            edidMatchLists
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

            edidMatchLists
                .GetListForType(ItemType.Collectible)
                .AddPrefixMatch(
                    "SS2_PurchaseableJunkSculpture_",
                    "BotModel",
                    "DLC04ParkMedallion"
                );


            edidMatchLists
                .GetListForType(ItemType.Shipment)
                .AddPrefixMatch(
                    "SS2_PriorityShipment_",
                    "SS2E_PurchaseablePet_",
                    "SS2_PurchaseablePet_",
                    "SS2_PurchaseableFurniture_"
                );

            edidMatchLists
                .GetListForType(ItemType.OtherMisc)
                .AddPrefixMatch("SS2_LootBox_");

            edidMatchLists
                .GetListForType(ItemType.Device)
                .AddExactMatch(
                    "SS2_ASAM_SensorScanner",
                    "kgSIM_EnergySignatureScanner_Inventory",
                    "SS2_SalvageBeacon",
                    "SS2_Mark1Beacon"
                );

            edidMatchLists
                .GetListForType(ItemType.LooseMod)
                .AddPrefixMatch(
                    "pa_comp_T60_" // these things are configured in a really weird way, so, hardcoding them like this
                );

            // hard overrides
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
