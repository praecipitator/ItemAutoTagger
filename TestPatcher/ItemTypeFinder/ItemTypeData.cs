using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTagger.ItemTypeFinder
{
    internal class ItemTypeData
    {
        // TODO potentially move this to a JSON/other config file, when I figure out how
        public readonly List<string> blacklistedScripts = new();
        public readonly List<string> blacklistedScriptPrefixes = new();

        public readonly List<string> blacklistedEdidPrefixes = new();

        public readonly List<string> blacklistedNameSubstrings = new();

        public readonly List<string> blacklistedKeywords = new();

        public readonly List<string> modelListTool = new();
        public readonly List<string> modelListDevice = new();
        public readonly List<string> modelListNewspaper = new();
        public ItemTypeData()
        {
            blacklistedScripts.Add("simsettlements:simbuildingplan");
            blacklistedScripts.Add("simsettlements:simstory");
            blacklistedScripts.Add("simsettlements:cityplan");
            blacklistedScripts.Add("simsettlements:simplanpath");
            blacklistedScripts.Add("simsettlements:dynamicflag");
            blacklistedScripts.Add("simsettlements:leadercard");
            blacklistedScripts.Add("simsettlements:medicalresearchrecipe");
            blacklistedScripts.Add("simsettlements:cityplanlayer");
            blacklistedScripts.Add("simsettlements:simbuildingplanskin");
            blacklistedScripts.Add("simsettlements:spawnablefoundation");
            blacklistedScripts.Add("simsettlements:factionunitdata");    
            blacklistedScripts.Add("simsettlementsv2:miscobjects:addonpackconfiguration");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:buildingplantheme");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:cameracontrolsequence");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:characterstatcard");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:foundation");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:powerpole");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:npcpreferences");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:optionsprofile");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:plotmessages");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:settlerlocationdiscovery");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:soundscapelayer");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:stageitem");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:usagerequirements");
            blacklistedScripts.Add("simsettlementsv2:miscobjects:medicalresearchrecipe");
            blacklistedScripts.Add("workshopframework:library:objectrefs:preventlooting");
            blacklistedScripts.Add("workshopplus:objectreferences:blueprint");

            blacklistedScriptPrefixes.Add("autobuilder:");
            blacklistedScriptPrefixes.Add("simsettlementsv2:armors:");
            blacklistedScriptPrefixes.Add("simsettlementsv2:weapons:");
            blacklistedScriptPrefixes.Add("simsettlementsv2:miscobjects:leadertrait");
            blacklistedScriptPrefixes.Add("simsettlementsv2:miscobjects:themeruleset");
            blacklistedScriptPrefixes.Add("simsettlementsv2:miscobjects:plotconfiguration");
            blacklistedScriptPrefixes.Add("simsettlementsv2:miscobjects:unlockable");

            blacklistedEdidPrefixes.Add("DN015_NoneNameMisc");
            blacklistedEdidPrefixes.Add("SS2_NameHolder_");
            blacklistedEdidPrefixes.Add("SS2_SettlerDiscovery_");
            blacklistedEdidPrefixes.Add("SS2_Unlockable_");
            blacklistedEdidPrefixes.Add("SS2_LeaderTrait_");
            blacklistedEdidPrefixes.Add("SS2_SLCP_");
            blacklistedEdidPrefixes.Add("SS2_Skin_");
            blacklistedEdidPrefixes.Add("SS2_BP_Randomizer");
            blacklistedEdidPrefixes.Add("HC_Cannibal_RavenousHunger");
            blacklistedEdidPrefixes.Add("HC_DiseaseEffect_");
            blacklistedEdidPrefixes.Add("HC_Effect_");
            blacklistedEdidPrefixes.Add("HC_EncumbranceEffect_");
            blacklistedEdidPrefixes.Add("HC_AdrenalineEffect");
            blacklistedEdidPrefixes.Add("HC_HungerEffect_");
            blacklistedEdidPrefixes.Add("HC_SleepEffect_");
            blacklistedEdidPrefixes.Add("HC_ThirstEffect_");
            blacklistedEdidPrefixes.Add("WSFW_NameHolder_");
            blacklistedEdidPrefixes.Add("kgConq_AssaultQuestVerb_");
            blacklistedEdidPrefixes.Add("RECYCLED_MISC_");    
            blacklistedEdidPrefixes.Add("WSFW_Blank");
            blacklistedEdidPrefixes.Add("UnarmedSuperMutant");
            blacklistedEdidPrefixes.Add("MS02NukeMissileFar");
            blacklistedEdidPrefixes.Add("WorkshopArtilleryWeapon");
            blacklistedEdidPrefixes.Add("GasTrapDummy");

            blacklistedKeywords.Add("SS2_Tag_PetName");
            blacklistedKeywords.Add("SS2_Tag_NPCName");

            modelListTool.Add("autobuildplots\\weapons\\hammer\\hammer.nif");
            modelListTool.Add("props\\smithingtools\\smithingtoolhammer01a.nif");
            modelListTool.Add("props\\smithingtools\\smithingtoolhammer01b.nif");
            modelListTool.Add("props\\smithingtools\\smithingtoolhammer02.nif");
            modelListTool.Add("props\\smithingtools\\smithingtoolhammer03.nif");
            modelListTool.Add("props\\smithingtools\\smithingtoolsaw01.nif");
            modelListTool.Add("props\\smithingtools\\smithingtoolsaw02.nif");
            modelListTool.Add("props\\smithingtools\\smithingtooltongs01.nif");
            modelListTool.Add("props\\surgicaltools\\surgicalcutter.nif");
            modelListTool.Add("props\\surgicaltools\\surgicalscalpel.nif");
            modelListTool.Add("props\\surgicaltools\\surgicalscissors.nif");
            modelListTool.Add("props\\blowtorch.nif");
            modelListTool.Add("props\\blowtorch_rare.nif");
            modelListTool.Add("props\\clipboard.nif");
            modelListTool.Add("props\\clipboard_prewar.nif");
            modelListTool.Add("props\\cuttingboard.nif");
            modelListTool.Add("props\\oilcan.nif");
            modelListTool.Add("props\\clothingiron\\clothingiron.nif");
            modelListTool.Add("props\\fishingreel.nif");


            modelListDevice.Add("props\\stealthboy01.nif");
            modelListDevice.Add("dlc01\\props\\dlc01_robotrepairkit01.nif");
            modelListDevice.Add("props\\bs101radiotransmittor.nif");
            modelListDevice.Add("props\\bosdistresspulser\\bosdistresspulserglowing.nif");
            modelListDevice.Add("props\\boscerebrofusionadaptor\\boscerebrofusionadaptor.nif");
            modelListDevice.Add("actors\\libertyprime\\characterassets\\agitator.nif");
            modelListDevice.Add("props\\bosdistresspulser\\bosdistresspulser.nif");
            modelListDevice.Add("props\\bosreflexcapacitor\\bosreflexcapacitor.nif");
            modelListDevice.Add("props\\boshapticdrive\\boshapticdrive.nif");
            modelListDevice.Add("props\\bosfluxsensor\\bosfluxsensor.nif");
            modelListDevice.Add("props\\generickeycard01.nif");
            modelListDevice.Add("props\\militarycircuitboard\\militarycircuitboard.nif");
            modelListDevice.Add("props\\prewar_toaster.nif");
            modelListDevice.Add("props\\postwar_toaster.nif");
            modelListDevice.Add("setdressing\\building\\deskfanoffice01.nif");
            modelListDevice.Add("setdressing\\building\\deskfanofficeoff01.nif");
            modelListDevice.Add("dlc01\\props\\dlc01_componentpart.nif");
            modelListDevice.Add("dlc01\\props\\dlc01_componentwhole.nif");
            modelListDevice.Add("dlc01\\props\\dlc01_amplifier01.nif");
            modelListDevice.Add("props\\synthchip\\synthchip.nif");
            modelListDevice.Add("dlc03\\props\\dlc03_fogcondenserpowermodule.nif");
            modelListDevice.Add("props\\ms11powerrelaycoil.nif");
            modelListDevice.Add("props\\component\\component_circuitry.nif");
            modelListDevice.Add("props\\biomedicalscanner\\biomedicalscanner.nif");
            modelListDevice.Add("props\\camera.nif");
            modelListDevice.Add("props\\fuse01.nif");
            modelListDevice.Add("props\\fusionpulsecharge\\fusionpulsecharge.nif");
            modelListDevice.Add("props\\bos_magnet.nif");
            modelListDevice.Add("props\\hotplate.nif");
            modelListDevice.Add("props\\ms11radartransmittor.nif");
            modelListDevice.Add("setdressing\\lightfixtures\\lightbulboff.nif");
            modelListDevice.Add("props\\chipboard.nif");
            modelListDevice.Add("setdressing\\quest\\genprototype01.nif");
            modelListDevice.Add("props\\vacuumtube01.nif");
            modelListDevice.Add("props\\stealthboy01.nif");
            modelListDevice.Add("weapons\\grenade\\transmitgrenadeprojectile.nif");
            modelListDevice.Add("props\\pipboymiscitem\\pipboymisc01.nif");
            modelListDevice.Add("dlc06\\props\\pipboymiscitem\\dlc06pipboymisc01.nif");
            modelListDevice.Add("setdressing\\factions\\railroad\\tinkertomsdevice01.nif");
            modelListDevice.Add("SS2\\Props\\ASAMSensor_StandingAnimated.nif");
            modelListDevice.Add("SS2\\Props\\ASAMTop_Moveable.nif");

            modelListNewspaper.Add("props\\newspaperpublickoccurenceslowpoly.nif");
            modelListNewspaper.Add("SS_IndRev\\Props\\NewspaperNewBugle.nif");
        }
    }
}
