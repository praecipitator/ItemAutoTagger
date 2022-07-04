using Noggog;
using Synthesis.Bethesda.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ItemTagger.ItemTypeFinder
{
    internal class ItemTypeData
    {
        // TODO potentially move this to a JSON/other config file, when I figure out how

        // blacklists
        public readonly MatchingList blacklistScript = new();
        public readonly MatchingList blacklistEdid = new();
        public readonly MatchingList blacklistKeyword = new();
        public readonly MatchingList blacklistName = new();

        // whitelists
        public readonly MatchingList whitelistModelTool = new();
        public readonly MatchingList whitelistModelDevice = new();

        public readonly MatchingList modelListCard = new();
        public readonly MatchingList modelListKey = new();
        public readonly MatchingList modelListPassword = new();

        public readonly MatchingList modelListPipBoy = new();
        public readonly MatchingList scriptListPipBoy = new();

        public readonly MatchingList keywordListQuest = new();

        public readonly MatchingList whitelistModelNews = new();
        public readonly MatchingList scriptListNews = new();
        public readonly MatchingList keywordListPerkmag = new();

        public readonly MatchingList programListGame = new();
        public readonly MatchingList edidListSettings = new();
        public readonly MatchingList nameListSettings = new();

        public readonly MatchingList keywordListDrink = new();
        public readonly MatchingList keywordListFood = new();
        public readonly MatchingList keywordListFoodDisease = new();

        public readonly MatchingList keywordListDevice = new();
        public readonly MatchingList keywordListChem = new();

        public readonly MatchingList soundListFood = new();
        public readonly MatchingList soundListChem = new();
        public readonly MatchingList soundListDevice = new();
        public readonly MatchingList soundListTool = new();


        // regexes? regices? regex objects for matching special stuff
        private static readonly Regex MATCH_MODEL_MODEL  = new(@"card[^\\]*\.nif$",  RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_MODEL_NOTE   = new(@"note[^\\]\.nif$",   RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex MATCH_MODEL_PIPBOY = new(@"pipboy[^\\]\.nif$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public ItemTypeData()
        {
            blacklistScript.addExactMatch("simsettlements:simbuildingplan");
            blacklistScript.addExactMatch("simsettlements:simstory");
            blacklistScript.addExactMatch("simsettlements:cityplan");
            blacklistScript.addExactMatch("simsettlements:simplanpath");
            blacklistScript.addExactMatch("simsettlements:dynamicflag");
            blacklistScript.addExactMatch("simsettlements:leadercard");
            blacklistScript.addExactMatch("simsettlements:medicalresearchrecipe");
            blacklistScript.addExactMatch("simsettlements:cityplanlayer");
            blacklistScript.addExactMatch("simsettlements:simbuildingplanskin");
            blacklistScript.addExactMatch("simsettlements:spawnablefoundation");
            blacklistScript.addExactMatch("simsettlements:factionunitdata");    
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:addonpackconfiguration");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:buildingplantheme");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:cameracontrolsequence");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:characterstatcard");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:foundation");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:powerpole");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:npcpreferences");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:optionsprofile");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:plotmessages");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:settlerlocationdiscovery");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:soundscapelayer");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:stageitem");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:usagerequirements");
            blacklistScript.addExactMatch("simsettlementsv2:miscobjects:medicalresearchrecipe");
            blacklistScript.addExactMatch("workshopframework:library:objectrefs:preventlooting");
            blacklistScript.addExactMatch("workshopplus:objectreferences:blueprint");
            blacklistScript.addExactMatch("simsettlementsv2:objectreferences:unitselectorform");
            blacklistScript.addExactMatch("SimSettlementsV2:MiscObjects:WorldRepopulationCell");

            blacklistScript.addPrefixMatch("autobuilder:");
            blacklistScript.addPrefixMatch("simsettlementsv2:armors:");
            blacklistScript.addPrefixMatch("simsettlementsv2:weapons:");
            blacklistScript.addPrefixMatch("simsettlementsv2:miscobjects:leadertrait");
            blacklistScript.addPrefixMatch("simsettlementsv2:miscobjects:themeruleset");
            blacklistScript.addPrefixMatch("simsettlementsv2:miscobjects:plotconfiguration");
            blacklistScript.addPrefixMatch("simsettlementsv2:miscobjects:unlockable");
            blacklistScript.addPrefixMatch("SimSettlementsV2:HQ:");
            blacklistScript.addPrefixMatch("SimSettlementsV2:MiscObjects:Unlockable");


            blacklistEdid.addPrefixMatch("DN015_NoneNameMisc");
            blacklistEdid.addPrefixMatch("DummyNoEdit_");
            blacklistEdid.addPrefixMatch("SS2_NameHolder_");
            blacklistEdid.addPrefixMatch("SS2_SettlerDiscovery_");
            blacklistEdid.addPrefixMatch("SS2_Unlockable_");
            blacklistEdid.addPrefixMatch("SS2_LeaderTrait_");
            blacklistEdid.addPrefixMatch("SS2_SLCP_");
            blacklistEdid.addPrefixMatch("SS2_Skin_");
            blacklistEdid.addPrefixMatch("SS2_BP_Randomizer");
            blacklistEdid.addPrefixMatch("HC_Cannibal_RavenousHunger");
            blacklistEdid.addPrefixMatch("HC_DiseaseEffect_");
            blacklistEdid.addPrefixMatch("HC_Effect_");
            blacklistEdid.addPrefixMatch("HC_EncumbranceEffect_");
            blacklistEdid.addPrefixMatch("HC_AdrenalineEffect");
            blacklistEdid.addPrefixMatch("HC_HungerEffect_");
            blacklistEdid.addPrefixMatch("HC_SleepEffect_");
            blacklistEdid.addPrefixMatch("HC_ThirstEffect_");
            blacklistEdid.addPrefixMatch("WSFW_NameHolder_");
            blacklistEdid.addPrefixMatch("kgConq_AssaultQuestVerb_");
            blacklistEdid.addPrefixMatch("RECYCLED_MISC_");    
            blacklistEdid.addPrefixMatch("WSFW_Blank");
            blacklistEdid.addPrefixMatch("UnarmedSuperMutant");
            blacklistEdid.addPrefixMatch("MS02NukeMissileFar");
            blacklistEdid.addPrefixMatch("WorkshopArtilleryWeapon");
            blacklistEdid.addPrefixMatch("GasTrapDummy");
            blacklistEdid.addPrefixMatch("SS2C2_Nameholder_");
            blacklistEdid.addPrefixMatch("SS2_HQWorkerSelectForm_");

            blacklistKeyword.addExactMatch("SS2_Tag_PetName");
            blacklistKeyword.addExactMatch("SS2_Tag_NPCName");

            whitelistModelTool.addExactMatch("autobuildplots\\weapons\\hammer\\hammer.nif");
            whitelistModelTool.addExactMatch("props\\smithingtools\\smithingtoolhammer01a.nif");
            whitelistModelTool.addExactMatch("props\\smithingtools\\smithingtoolhammer01b.nif");
            whitelistModelTool.addExactMatch("props\\smithingtools\\smithingtoolhammer02.nif");
            whitelistModelTool.addExactMatch("props\\smithingtools\\smithingtoolhammer03.nif");
            whitelistModelTool.addExactMatch("props\\smithingtools\\smithingtoolsaw01.nif");
            whitelistModelTool.addExactMatch("props\\smithingtools\\smithingtoolsaw02.nif");
            whitelistModelTool.addExactMatch("props\\smithingtools\\smithingtooltongs01.nif");
            whitelistModelTool.addExactMatch("props\\surgicaltools\\surgicalcutter.nif");
            whitelistModelTool.addExactMatch("props\\surgicaltools\\surgicalscalpel.nif");
            whitelistModelTool.addExactMatch("props\\surgicaltools\\surgicalscissors.nif");
            whitelistModelTool.addExactMatch("props\\blowtorch.nif");
            whitelistModelTool.addExactMatch("props\\blowtorch_rare.nif");
            whitelistModelTool.addExactMatch("props\\clipboard.nif");
            whitelistModelTool.addExactMatch("props\\clipboard_prewar.nif");
            whitelistModelTool.addExactMatch("props\\cuttingboard.nif");
            whitelistModelTool.addExactMatch("props\\oilcan.nif");
            whitelistModelTool.addExactMatch("props\\clothingiron\\clothingiron.nif");
            whitelistModelTool.addExactMatch("props\\fishingreel.nif");


            whitelistModelDevice.addExactMatch("props\\stealthboy01.nif");
            whitelistModelDevice.addExactMatch("dlc01\\props\\dlc01_robotrepairkit01.nif");
            whitelistModelDevice.addExactMatch("props\\bs101radiotransmittor.nif");
            whitelistModelDevice.addExactMatch("props\\bosdistresspulser\\bosdistresspulserglowing.nif");
            whitelistModelDevice.addExactMatch("props\\boscerebrofusionadaptor\\boscerebrofusionadaptor.nif");
            whitelistModelDevice.addExactMatch("actors\\libertyprime\\characterassets\\agitator.nif");
            whitelistModelDevice.addExactMatch("props\\bosdistresspulser\\bosdistresspulser.nif");
            whitelistModelDevice.addExactMatch("props\\bosreflexcapacitor\\bosreflexcapacitor.nif");
            whitelistModelDevice.addExactMatch("props\\boshapticdrive\\boshapticdrive.nif");
            whitelistModelDevice.addExactMatch("props\\bosfluxsensor\\bosfluxsensor.nif");
            whitelistModelDevice.addExactMatch("props\\generickeycard01.nif");
            whitelistModelDevice.addExactMatch("props\\militarycircuitboard\\militarycircuitboard.nif");
            whitelistModelDevice.addExactMatch("props\\prewar_toaster.nif");
            whitelistModelDevice.addExactMatch("props\\postwar_toaster.nif");
            whitelistModelDevice.addExactMatch("setdressing\\building\\deskfanoffice01.nif");
            whitelistModelDevice.addExactMatch("setdressing\\building\\deskfanofficeoff01.nif");
            whitelistModelDevice.addExactMatch("dlc01\\props\\dlc01_componentpart.nif");
            whitelistModelDevice.addExactMatch("dlc01\\props\\dlc01_componentwhole.nif");
            whitelistModelDevice.addExactMatch("dlc01\\props\\dlc01_amplifier01.nif");
            whitelistModelDevice.addExactMatch("props\\synthchip\\synthchip.nif");
            whitelistModelDevice.addExactMatch("dlc03\\props\\dlc03_fogcondenserpowermodule.nif");
            whitelistModelDevice.addExactMatch("props\\ms11powerrelaycoil.nif");
            whitelistModelDevice.addExactMatch("props\\component\\component_circuitry.nif");
            whitelistModelDevice.addExactMatch("props\\biomedicalscanner\\biomedicalscanner.nif");
            whitelistModelDevice.addExactMatch("props\\camera.nif");
            whitelistModelDevice.addExactMatch("props\\fuse01.nif");
            whitelistModelDevice.addExactMatch("props\\fusionpulsecharge\\fusionpulsecharge.nif");
            whitelistModelDevice.addExactMatch("props\\bos_magnet.nif");
            whitelistModelDevice.addExactMatch("props\\hotplate.nif");
            whitelistModelDevice.addExactMatch("props\\ms11radartransmittor.nif");
            whitelistModelDevice.addExactMatch("setdressing\\lightfixtures\\lightbulboff.nif");
            whitelistModelDevice.addExactMatch("props\\chipboard.nif");
            whitelistModelDevice.addExactMatch("setdressing\\quest\\genprototype01.nif");
            whitelistModelDevice.addExactMatch("props\\vacuumtube01.nif");
            whitelistModelDevice.addExactMatch("props\\stealthboy01.nif");
            whitelistModelDevice.addExactMatch("weapons\\grenade\\transmitgrenadeprojectile.nif");
            whitelistModelDevice.addExactMatch("props\\pipboymiscitem\\pipboymisc01.nif");
            whitelistModelDevice.addExactMatch("dlc06\\props\\pipboymiscitem\\dlc06pipboymisc01.nif");
            whitelistModelDevice.addExactMatch("setdressing\\factions\\railroad\\tinkertomsdevice01.nif");
            whitelistModelDevice.addExactMatch("SS2\\Props\\ASAMSensor_StandingAnimated.nif");
            whitelistModelDevice.addExactMatch("SS2\\Props\\ASAMTop_Moveable.nif");

            whitelistModelNews.addExactMatch("props\\newspaperpublickoccurenceslowpoly.nif");
            whitelistModelNews.addExactMatch("SS_IndRev\\Props\\NewspaperNewBugle.nif");

            scriptListNews.addExactMatch("SimSettlements:Newspaper");
            scriptListNews.addExactMatch("SimSettlementsV2:Books:NewsArticle");

            keywordListPerkmag.addExactMatch("PerkMagKeyword");
            keywordListPerkmag.addExactMatch("CA_SkillMagazineScript");

            // keys
            modelListKey.addExactMatch("props\\key01.nif");
            modelListKey.addExactMatch("props\\key02.nif");
            modelListKey.addExactMatch("props\\key02.nif");
            modelListKey.addExactMatch("props\\key_chain01.nif");
            modelListKey.addExactMatch("props\\key_chain02.nif");
            modelListKey.addExactMatch("props\\key_chain03.nif");
            modelListKey.addExactMatch("props\\ms07key.nif");

            modelListCard.addExactMatch("props\\generickeycard01.nif");
            modelListCard.addExactMatch("props\\vaultidcard.nif");
            modelListCard.addRegexMatch(MATCH_MODEL_MODEL);

            modelListPassword.addExactMatch("props\\holotape_prop.nif");
            modelListPassword.addExactMatch("interface\\newspaper\\noteripped.nif");
            modelListPassword.addExactMatch("props\\note_lowpoly.nif");
            modelListPassword.addRegexMatch(MATCH_MODEL_NOTE);

            // stuff
            modelListPipBoy.addRegexMatch(MATCH_MODEL_PIPBOY);
            scriptListPipBoy.addSuffixMatch("PipboyMiscItemScript");
            keywordListQuest.addExactMatch("VendorItemNoSale");
            keywordListQuest.addExactMatch("UnscrappableObject");

            programListGame.addExactMatch("atomiccommand.swf");
            programListGame.addExactMatch("grognak.swf");
            programListGame.addExactMatch("pipfall.swf");
            programListGame.addExactMatch("zetainvaders.swf");
            programListGame.addExactMatch("redmenace.swf");
            programListGame.addExactMatch("automatron\\automatron.swf");

            edidListSettings.addSubstringMatch("setting");
            edidListSettings.addSubstringMatch("config");
            edidListSettings.addSubstringMatch("cheat");

            nameListSettings.addPrefixMatch(" - ");
            nameListSettings.addSubstringMatch("setting");
            nameListSettings.addSubstringMatch("config");

            keywordListDrink.addExactMatch("ObjectTypeWater");
            keywordListDrink.addExactMatch("ObjectTypeDrink");
            keywordListDrink.addExactMatch("HC_SustenanceType_QuenchesThirst");
            keywordListDrink.addExactMatch("ObjectTypeCaffeinated");

            keywordListFood.addExactMatch("FoodEffect");
            keywordListFood.addExactMatch("HC_DiseaseRisk_FoodVeryHigh");
            keywordListFood.addExactMatch("HC_DiseaseRisk_FoodLow");
            keywordListFood.addExactMatch("HC_DiseaseRisk_FoodHigh");
            keywordListFood.addExactMatch("HC_DiseaseRisk_FoodStandard");
            keywordListFood.addExactMatch("FruitOrVegetable");
            keywordListFood.addExactMatch("ObjectTypeFood");

            keywordListFoodDisease.addExactMatch("HC_DiseaseRisk_FoodVeryHigh");
            keywordListFoodDisease.addExactMatch("HC_DiseaseRisk_FoodLow");
            keywordListFoodDisease.addExactMatch("HC_DiseaseRisk_FoodHigh");
            keywordListFoodDisease.addExactMatch("HC_DiseaseRisk_FoodStandard");

            keywordListDevice.addExactMatch("ChemTypeStealthBoy");
            keywordListDevice.addExactMatch("StealthBoyKeyword");
            keywordListDevice.addExactMatch("DLC01ObjectTypeRepairKit");

            keywordListChem.addExactMatch("ObjectTypeStimpak");
            keywordListChem.addExactMatch("ObjectTypeChem");
            keywordListChem.addExactMatch("CA_ObjType_ChemBad");
            keywordListChem.addExactMatch("HC_CausesImmunodeficiency");
            keywordListChem.addExactMatch("HC_SustenanceType_IncreasesHunger");

            soundListFood.addExactMatch("NPCHumanEatChewy");
            soundListFood.addExactMatch("NPCHumanEatGeneric");
            soundListFood.addExactMatch("NPCHumanEatEgg");
            soundListFood.addExactMatch("NPCHumanEatSoup");
            soundListFood.addExactMatch("NPCHumanEatSoupSlurp");

            soundListChem.addExactMatch("NPCHumanEatMentats");
            soundListChem.addExactMatch("NPCHumanChemsPsycho");
            soundListChem.addExactMatch("NPCHumanChemsUseJet");
            soundListChem.addExactMatch("NPCHumanChemsAddictol");

            soundListDevice.addExactMatch("OBJStealthBoyActivate");

            soundListTool.addExactMatch("NPCHumanWhistleDog");
        }
    }
}
