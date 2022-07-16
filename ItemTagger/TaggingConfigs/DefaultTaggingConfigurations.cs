using ItemTagger.ItemTypeFinder;

namespace ItemTagger.TaggingConfigs
{


    internal class DefaultTaggingConfigurations
    {
        private readonly IDictionary<TaggingConfigType, TaggingConfiguration> defaultConfigs = new Dictionary<TaggingConfigType, TaggingConfiguration>();

        public DefaultTaggingConfigurations()
        {
            // FIS
            // https://www.nexusmods.com/fallout4/articles/2798

            TaggingConfiguration configSettingFIS = new()
            {
                { ItemType.Shipment,            "[Scrap]" },
                { ItemType.Scrap,               "[Scrap]" },
                { ItemType.Resource,            "[Resource]" },
                { ItemType.LooseMod,            "[Mod]" },
                { ItemType.Collectible,         "[Collectible]" },
                { ItemType.Quest,               "[Quest]" },
                { ItemType.Currency,            "[Currency]" },
                { ItemType.Valuable,            "[Valuable]" },
                { ItemType.OtherMisc,           "[Other]" },
                { ItemType.GoodChem,            "[Aid]" },
                { ItemType.BadChem,             "[Chem]" },
                { ItemType.Food,                "[Food]" },
                { ItemType.FoodRaw,             "[RadFood]" },
                { ItemType.FoodCrop,            "[Vegetables]" },
                { ItemType.FoodPrewar,          "[RadFood]" },
                { ItemType.Drink,               "[Drink]" },
                { ItemType.Liquor,              "[Liquor]" },
                { ItemType.Nukacola,            "[Nuka]" },
                { ItemType.Syringe,             "[SyringerDart]" },
                { ItemType.Device,              "[Device]" },
                { ItemType.Tool,                "[Device]" },
                { ItemType.News,                "[NotePubOcc]" },
                { ItemType.Note,                "[Note]" },
                { ItemType.Perkmag,             "[PerkMag]" },
                { ItemType.Mine,                "[Mine]" },
                { ItemType.Grenade,             "[Grenade]" },
                { ItemType.Key,                 "[Key]" },
                { ItemType.KeyCard,             "[Key]" },
                { ItemType.KeyPassword,         "[Passcard]" },
                { ItemType.Ammo,                "[Ammo]" },
                { ItemType.Holotape,            "[Holotape]" },
                { ItemType.HolotapeGame,        "[Game]" },
                { ItemType.HolotapeSettings,    "[Settings]" },
                { ItemType.PipBoy,              "[Pipboy]" },
                { ItemType.WeaponRanged,        "[Ranged]" },
                { ItemType.WeaponMelee,         "[Melee]" },
                { ItemType.Armor,               "[Armor]" },
                { ItemType.Clothes,             "[Clothes]" },
                { ItemType.VaultSuit,           "[VaultSuit]" },
                { ItemType.PowerArmor,          "[PowerArmor]" },
            };

            configSettingFIS.AddExtraTags(
                new string[] { "Melee", "Unarmed", "MeleeOneHand", "MeleeTwoHand", "BoxingGlove", "Gauntlet", "PowerFist", "Knuckles", "BaseballBat", "SuperSledge", "TireIron", "WalkingCane", "PoolCue", "RollingPin", "LeadPipe", "Baton", "PipeWrench", "Board", "PaddleBall", "CombatKnife", "SwitchBlade", "Shish", "Sword", "RevDword", "Machete", "Ripper", "Axe", "PoleHook", "Ranged", "Pistol", "10mm", "TheDeliverer", "44P", "InstitutePistol", "PipeRevolver", "LaserPistol", "PlasmaPistol", "AlienGun", "FlareGun", "ThirstZapper", "AcidSoaker", "Rifle", "Cryo", "Syringer", "PipeRifle", "Leveraction", "InstituteRifle", "HuntingRifle", "Railway", "GaussRifle", "LaserRifle", "PlasmaRifle", "RadiumRifle", "HarpoonGun", "LaserMusket", "HandmadeRifle", "CombatRifle", "AssaultRifle", "Shotgun", "DoubleShotgun", "SMB", "Minigun", "Gatling", "JunkJet", "Cannon", "Flamethrower", "Rocketlauncher", "Fatman", "Ranged", "Grenade", "GrenadeCryo", "GrenadePlasma", "GrenadePulse", "Molotov", "Mine", "MineCryo", "MinePlasma", "MinePulse", "MineBottlecap", "Trap", "Ammo", "AmmoCaliber", "AmmoShells", "AmmoEnergy", "AmmoCannon", "AmmoMissile", "FusionCore", "NonHuman", "Unknown", "SuperMutant", "Clothes", "Hat", "Cap", "Bandana", "Eyes", "Mask", "GasMask", "FullGasMask", "Neck", "Ring", "Gloves", "Underwear", "FullOutfit", "Dress", "ProtectionSuit", "Armor", "FullArmor", "VaultSuit", "SetRaider", "SetLeather", "SetMetal", "SetMarine", "SetDCGuard", "SetSynth", "SetCombat", "SetCustom", "SetRobot", "FullHelm", "Helm", "ArmL", "ArmR", "Torso", "LegL", "LegR", "Jetpack", "Holster", "Pack", "PowerArmor", "PA_Raider", "T45", "T51", "T60", "X01", "PowerHelm", "PowerArmL", "PowerArmR", "PowerTorso", "PowerLegL", "PowerLegR", "Food", "RadFood", "Soup", "Stew", "Steak", "FoodStick", "Cake", "Boxed", "Canned", "Meat", "Vegetables", "Leaf", "Shroom", "Drink", "Nuka", "Liquor", "Beer", "Aid", "Chem", "Stimpak", "MedPills", "RadX", "MedSyringe", "MedSyringeGreen", "MedSyringeOrange", "MedInhalor", "MedRobot", "MedPackRed", "RadAway", "DrugPills", "DrugPillsBlue", "DrugPillsPurple", "DrugPillsOrange", "DrugSyringe", "DrugSyringeRed", "DrugSyringeOrange", "DrugSyringePurple", "DrugInhalor", "StealthBoy", "SyringerDart", "Camping", "PerkBobblehead", "PerkMag", "Collectible", "Valuable", "Note", "NotePubOcc", "NoteMisc", "Key", "Password", "Passcard", "Holotape", "HolotapeT", "HolotapeV", "HolotapeP", "Game", "Mod", "Settings", "Device", "Pipboy", "Lockpick", "Currency", "Resource", "Scrap", "Bottle", "Other", "OtherALCH", "Trash", "Brotherhood", "Minutemen", "Institute", "Railroad", "VaultTec", "Companion", "Quest", "MainQuest", "DiamondCity", "Goodneighbor", "Cabot", "Robot", "FarHarbor", "Acadia", "Atom", "Harbor", "NukaWorld", "Raid", "Radio", "SilverShroud", "Warning", "Military", "Distress", "Skylane", "Fire", "SkullCowboy", "Atom2", "Anarchy", "Energy", "StarOutline", "Defense2", "Biohazard", "Water", "Water2"}
            );

            defaultConfigs.Add(TaggingConfigType.FIS, configSettingFIS);


            // LWIS
            TaggingConfiguration configSettingLWIS = new()
            {
                { ItemType.Shipment,            "{Shipment}" },
                { ItemType.Scrap,               "{Scrap}" },
                { ItemType.Resource,            "{Resource}" },
                { ItemType.LooseMod,            "{Mod}" },
                { ItemType.Collectible,         "[Collectible]" },
                { ItemType.Quest,               "[Quest]" },
                { ItemType.Currency,            "(Currency)" },
                { ItemType.Valuable,            "(Valuable)" },
                { ItemType.OtherMisc,           "[Misc]" },
                { ItemType.GoodChem,            "(Medicine)" },
                { ItemType.BadChem,             "[Chem]" },
                { ItemType.Food,                "(Food)" },
                { ItemType.FoodRaw,             "[Raw]" },
                { ItemType.FoodCrop,            "[Crop]" },
                { ItemType.FoodPrewar,          "[Prewar]" },
                { ItemType.Drink,               "(Drink)" },
                { ItemType.Liquor,              "[Liquor]" },
                { ItemType.Nukacola,            "[Nuka]" },
                { ItemType.Syringe,             "[Syringe]" },
                { ItemType.Device,              "(Device)" },
                { ItemType.Tool,                "(Tool)" },
                { ItemType.News,                "[News]" },
                { ItemType.Note,                "[Note]" },
                { ItemType.Perkmag,             "[PerkMag]" },
                { ItemType.Mine,                "(Mine)" },
                { ItemType.Grenade,             "(Grenade)" },
                { ItemType.Key,                 "[Key]" },
                { ItemType.KeyCard,             "[Passcard]" },
                { ItemType.KeyPassword,         "[Passcard]" },
                { ItemType.Ammo,                "(Ammo)" },
                { ItemType.Holotape,            "[Holotape]" },
                { ItemType.HolotapeGame,        "[Game]" },
                { ItemType.HolotapeSettings,    "[Settings]" },
                { ItemType.PipBoy,              "[Pipboy]" },
                { ItemType.WeaponRanged,        "[Gun]" },
                { ItemType.WeaponMelee,         "[Melee]" },
                { ItemType.Armor,               "[Armor]" },
                { ItemType.Clothes,             "[Clothing]" },
                { ItemType.VaultSuit,           "[Clothing]" },
                { ItemType.PowerArmor,          "[PArmor]" },
            };

            configSettingLWIS.AddExtraTags(new string[] { "Mutant", "Dog", "Game", "Core", "Book", "Ore", "Trash", "Shroom", "Herb", "Ingredient", "Bobblehead", "Ring", "Unique", "Kremvh", "Deliverer", "Armor", "Gun", "Clothing", "Melee", "PArmor", "ArmorChest", "ArmorArm", "ArmorLeg", "ArmorHead", "ArmorMask", "Hat", "Mask", "Eyes", "Flare", "Pistol", "ARifle", "CRifle", "RRifle", "Rifle", "Revolver", "Syringer", "Shotgun", "Rocket", "Railway", "PRifle", "PPistol", "IRifle", "IPistol", "PipeR", "Pipe", "Lever", "FatMan", "Musket", "LRifle", "LPistol", "JunkJet", "Gauss", "Gatling", "Flamer", "Cryolator", "Cannon", "Minigun", "Gamma", "Alien", "SMG", "Revolver", "Harpoon", "Shishkebab", "Ripper", "MeleeH2H", "Melee1H", "Melee2H", "Bat", "Sword", "Knife", "Sledge", "PArmorLegR", "PArmorLegL", "PArmorArmR", "PArmorArmL", "PArmorChest", "PArmorHead" });

            defaultConfigs.Add(TaggingConfigType.LWIS, configSettingLWIS);

            // VIS
            TaggingConfiguration configSettingVIS = new()
            {
                { ItemType.Shipment,            "{Scrap}" },
                { ItemType.Scrap,               "{Scrap}" },
                { ItemType.Resource,            "{Resource}" },
                { ItemType.LooseMod,            "{Mod}" },
                { ItemType.Collectible,         "[Collectible]" },
                { ItemType.Quest,               "[Quest]" },
                { ItemType.Currency,            "(Currency)" },
                { ItemType.Valuable,            "(Valuable)" },
                { ItemType.OtherMisc,           "[Trash]" },
                { ItemType.GoodChem,            "(Chem)" },
                { ItemType.BadChem,             "[Chem]" },
                { ItemType.Food,                "(Food)" },
                { ItemType.FoodRaw,             "[Raw]" },
                { ItemType.FoodCrop,            "[Raw]" },
                { ItemType.FoodPrewar,          "[Prewar]" },
                { ItemType.Drink,               "(Drink)" },
                { ItemType.Liquor,              "[Liquor]" },
                { ItemType.Nukacola,            "[Nuka]" },
                { ItemType.Syringe,             "[Syringe]" },
                { ItemType.Device,              "(Device)" },
                { ItemType.Tool,                "{Tool}" },
                { ItemType.News,                "[News]" },
                { ItemType.Note,                "[Note]" },
                { ItemType.Perkmag,             "[Perk: Mag]" },
                { ItemType.Mine,                "(Mine)" },
                { ItemType.Grenade,             "(Grenade)" },
                { ItemType.Key,                 "|Key|" },
                { ItemType.KeyCard,             "|Passcard|" },
                { ItemType.KeyPassword,         "|Passcard|" },
                { ItemType.Ammo,                "(Ammo)" },
                { ItemType.Holotape,            "[Holotape]" },
                { ItemType.HolotapeGame,        "[Game]" },
                { ItemType.HolotapeSettings,    "[Settings]" },
                { ItemType.PipBoy,              "[Arm]" },
                { ItemType.WeaponRanged,        "[Gun]" },
                { ItemType.WeaponMelee,         "[Melee]" },
                { ItemType.Armor,               "[Armor]" },
                { ItemType.Clothes,             "[Clothing]" },
                { ItemType.VaultSuit,           "[Clothing]" },
                { ItemType.PowerArmor,          "[PArmor]" },
            };
            configSettingVIS.AddExtraTags(new string[] { "Shipment", "Scrap", "Resource", "Mod", "Collectible", "Quest", "Currency", "Valuable", "Misc", "Medicine", "Chem", "Food", "Raw", "Crop", "Prewar", "Drink", "Nuka", "Liquor", "Syringe", "Device", "Tool", "Note", "Perk: Mag", "News", "Grenade", "Mine", "Trap", "Holotape", "Settings", "Key", "Ammo", "Pipboy", "Mutant", "Dog", "Game", "Core", "Book", "Ore", "Trash", "Shroom", "Herb", "Ingredient", "Bobblehead", "Ring", "Unique", "Kremvh", "Deliverer", "Armor", "Gun", "Clothing", "Melee", "PArmor", "ArmorChest", "ArmorArm", "ArmorLeg", "ArmorHead", "ArmorMask", "Hat", "Mask", "Eyes", "Flare", "Pistol", "ARifle", "CRifle", "RRifle", "Rifle", "Revolver", "Syringer", "Shotgun", "Rocket", "Railway", "PRifle", "PPistol", "IRifle", "IPistol", "PipeR", "Pipe", "Lever", "FatMan", "Musket", "LRifle", "LPistol", "JunkJet", "Gauss", "Gatling", "Flamer", "Cryolator", "Cannon", "Minigun", "Gamma", "Alien", "SMG", "Revolver", "Harpoon", "Shishkebab", "Ripper", "MeleeH2H", "Melee1H", "Melee2H", "Bat", "Sword", "Knife", "Sledge", "PArmorLegR", "PArmorLegL", "PArmorArmR", "PArmorArmL", "PArmorChest", "PArmorHead" });

            defaultConfigs.Add(TaggingConfigType.VIS, configSettingVIS);

            // VIS-G
            TaggingConfiguration configSettingVISG = new()
            {
                { ItemType.Shipment,            "(Resource)" },
                { ItemType.Scrap,               "{Scrap}" },
                { ItemType.Resource,            "(Resource)" },
                { ItemType.LooseMod,            "{Mod}" },
                { ItemType.Collectible,         "[Collectible]" },
                { ItemType.Quest,               "[Quest]" },
                { ItemType.Currency,            "(Currency)" },
                { ItemType.Valuable,            "(Valuable)" },
                { ItemType.OtherMisc,           "[Trash]" },
                { ItemType.GoodChem,            "(Aid)" },
                { ItemType.BadChem,             "[Chem]" },
                { ItemType.Food,                "(Food)" },
                { ItemType.FoodRaw,             "[Raw Meat]" },
                { ItemType.FoodCrop,            "[Produce]" },
                { ItemType.FoodPrewar,          "[Rad Food]" },
                { ItemType.Drink,               "(Drink)" },
                { ItemType.Liquor,              "[Liquor]" },
                { ItemType.Nukacola,            "(Nuka)" },
                { ItemType.Syringe,             "[Syringe]" },
                { ItemType.Device,              "(Device)" },
                { ItemType.Tool,                "[Tool]" },
                { ItemType.News,                "[Note]" },
                { ItemType.Note,                "[Note]" },
                { ItemType.Perkmag,             "[Perk: Mag]" },
                { ItemType.Mine,                "(Mine)" },
                { ItemType.Grenade,             "(Grenade)" },
                { ItemType.Key,                 "|Key|" },
                { ItemType.KeyCard,             "|Passcard|" },
                { ItemType.KeyPassword,         "|Passcard|" },
                { ItemType.Ammo,                "(Ammo)" },
                { ItemType.Holotape,            "[Holotape]" },
                { ItemType.HolotapeGame,        "[Game]" },
                { ItemType.HolotapeSettings,    "[Settings]" },
                { ItemType.PipBoy,              "(Pipboy)" },
                { ItemType.WeaponRanged,        "[Rifle]" },
                { ItemType.WeaponMelee,         "[Rifle]" },// maybe "4jz Melee", but, is that really a tag?
                { ItemType.Armor,               "[Armor]" },
                { ItemType.Clothes,             "[Armor]" },
                { ItemType.VaultSuit,           "[Armor]" },// maybe "1s Vault-Tec"?
                { ItemType.PowerArmor,          "[Armor]" },
            };
            configSettingVISG.AddExtraTags(new string[] { "Aid", "Device", "Drink", "Food", "Chem", "Liquor", "Nuka", "Rad Food", "Beer", "Ingredients", "Raw Meat", "Produce", "Egg", "Plant", "Fungi", "Tea", "Coffee", "Trim", "Hash", "Pollen", "Blunt", "Joint", "Seed", "Buds", "Magazine", "Cigarette", "Cigar", "Canteen", "Ammo", "Fuel", "Bolt", "Fusion", "mFusion", "Syringe", "Flare", "1e Combat", "1h Synth", "1r DC Guard", "1q Leather", "1j Metal", "1b Marine", "1i Robot", "1a Therm Optics", "1p Trapper", "1f Nano", "1c Merc", "1d Gunners", "1o Scavvers", "1g Battle", "1k Disciples", "1l Operators", "1m Pack", "1n Raider", "1s Vault-Tec", "3a Super Mutant", "2a X-03", "2b X-02", "2c X-01", "2d T-60", "2e T-53", "2f T-51c", "2g T-51", "2h T-49", "2i T-45", "2j Raider", "2k Combat PA", "2l Construction", "2m Horse Power", "2ma Cpt. Cosmos", "2n Institute PA", "2o Ironman", "2p Knight", "2q Liberty", "2r Midwest BoS", "2s Navi", "2t Relic Marine", "2u Space Marine", "2v Spartan", "2w Submersible", "2x Teddy Bear", "2y TES-51", "2z Train", "2za Tribal", "2zb Vault-Tec", "1a Silver Shroud", "1b Robot", "1c Biosuit", "1d Suit", "1e Dress", "1f Dapper", "1g Casual", "1h Skimpy", "1i Rugged", "1j Minutemen", "1k Vault-Tec", "1l Atom Cats", "1m BoS", "1n Railroad", "1o Institute", "1p DC", "1q Military", "1r Marine", "1s NCR", "1t Merc", "1ta Gunner", "1tb Scavvers", "1u Forged", "1v Disciples", "1w Operators", "1x Pack", "1y Raider", "1z CoA", "2a Masked Helmet", "2b Helmet", "2c Hat", "2d Cap", "2e Headband", "2f Full-Mask", "2g Eyewear", "2h Mouth", "2i Half-Mask", "2j Bandana", "2k Scarf", "2l Bandana", "2m Gloves", "3a Backpack", "3b Jetpack", "3c Backpack Upper", "3d Backpack Lower", "3e Satchel", "3f Bandolier", "3g Harness", "3h Tac-Vest", "3i Belt", "4a Gun On Hip", "4b Melee On Hip", "4c Gun On Back", "4d Melee On Back", "4e Canteen", "4f Device", "4g Mine", "5a Necklace", "5b Bracelet", "5c Watch", "5d Ring", "5e Earring", "5f Piercing", "5g Fingernails", "6a Top", "6b Bottom", "6c Sneakers", "6d Boots", "6e Jacket", "6f Vest", "6g Cloak", "6h Arm Addon", "6i Leg Addon", "6j Offhand", "7a Dog", "8a Kids Dress", "8b Kids Casual", "8c Kids Rugged", "8d Kids Vault-Tec", "8e Kids BoS", "8f Kids Institute", "8g Kids Military", "8h Kids Cap", "Explosives", "Grenade", "Mine", "Signal", "Beacon", "Nuke Grenade", "Pulse Grenade", "Stun Grenade", "Flash Grenade", "Dynamite", "Holy Grenade", "Molotov", "Thrown", "Resource", "Scrap", "Ammo Scrap", "Resource AGP", "Resource AP", "Currency", "Lockpick", "Settings", "Unique", "Valuable", "Bottle", "Collectibles", "Collectible", "Crafting", "CC Kits", "CC Sets", "Game", "Hack", "Holotape", "Note", "Quest", "Trash", "Compost", "Key", "Password", "Passcard", "Camping", "Pipboy", "Wraps", "Tool", "Casing", "Shell", "Baseball Card", "Marvel Card", "Coffee Cup", "Gnome", "Model Robot", "Cpt. Cosmos", "GI Joe", "Mod", "Perk: Quest", "Perk: Companion", "Faction: Railroad", "Faction: BOS", "Perk: Bobblehead", "Perk: Mag", "Faction: Operator", "Faction: Disciple", "Faction: Pack", "Faction: Atom Cats", "0a Mini Nuke", "0b Missile", "0c RocketsMini", "0c RocketsCluster", "0c Fireworks", "0c RocketsLrg", "0c Kids Rocket", "0d 40mm Grenade", "0e Cannonball", "1a 2mm EC", "1b Alien", "1b ET", "1ba SC", "1c PR", "1d PP", "1e Core", "1f Tesla", "1g Cryo", "1h Flamer", "1i IR", "1j IP", "1k LR", "1l LP", "1m LM", "1n Annie Boom", "1o Gamma", "1p Breeder", "1q Assaultron Head", "2a HMG", "2b LMG", "2c BR", "2d AR", "2da HAR", "2e SAR", "2f SAS", "2g SG", "2h LAR", "2i HMR", "2j CARB", "2k SMG", "2ka CSMG", "2kb HSMG", "2la HR", "2lb REV", "2m HP", "2n SAP", "2o LSAP", "2p HMP", "3a Harpoon", "3b Spike", "3c Bolt", "3d Arrow", "3e Syringer", "3f Baseball", "3g Melon", "3h Junk", "3i Flare", "3j Acid", "3k Zapper", "3l Paintball", "4aa Super Sledge", "4ab Sledgehammer", "4ba Chainsaw", "4bb Buzz Axe", "4bc Buzz Blade", "4bd Ripper", "4c Fire", "4d Sword", "4da Katana", "4db Assaultron Blade", "4dd Jiang", "4de Saber", "4e Axe", "4f Bat", "4fb Pipe Wrench", "4fc Lead Pipe", "4fd Board", "4fe Pole Hook", "4ff Pool Cue", "4fg Baton", "4fh Tire Iron", "4fi Rolling Pin", "4fj Walking Cane", "4fk Club", "4ga Hachet", "4gb Tomahawk", "4h Machete", "4i Knife", "4ia Trench Knife", "4ib Switchblade", "4ic Karambit", "4j Fist", "4ja Power Fist", "4jb Deathclaw Gauntlet", "4jc Meat Hook", "4jd Knuckles", "4je Push Knife", "4jf Boxing Gloves", "4jz Melee", "4k String", "1h 1 Fahrenheit_Flamer", "1i 1 Virgils_Rifle", "1l 1 Survivors_Special", "2e 1 Reba", "2o 1 Deliverer", "4dd 1 Zaos_Sword", "2a 1 Ashmaker", "4f 1 Rockville_Slugger", "2e 1 Tinker_Tom_Special", "1k 1 Old_Faithful", "2k 1 Spray_N_Pray", "2f 1 Justice", "0b 1 PartyStarter", "2m 1 Wastelanders_Friend", "2c 1 Overseers_Guardian", "1c 1 AX90_fury", "1c 1 Experiment_18A", "0a 1 Big_Boy", "1k 1 Good_Intentions", "4h 1 Kremvhs_Tooth", "2f 1 Le_Fusil_Terribles", "4i 1 Pickmans_Blade", "1k 1 Righteous_Authority", "1k 1 Wazer_Wifle", "1c 1 Sentinels_Plasmacaster", "0b 1 Death_from_Above", "1e 1 Final_Judgment", "2lb 1 Kelloggs_Pistol", "2lb 1 The_Gainer", "2lb 1 Eddies_Peace", "4e 1 Grognaks_Axe", "4f 1 2076_World_Series", "1a 1 The_Last_Minute", "4bd 1 Reckoning", "4de 1 Shem_Drowne_Sword", "4fb 1 Big_Jim", "1k 1 Prototype_UP77", "4dd 1 General_Chaos_Revenge", "2c 1 Decembers_Child", "0a 1 The_Striker", "3a 1 Defenders_Harpoon_Gun", "4fe 1 Bloodletter", "4aa 1 Atoms_Judgement", "2j 1 Radical_Conversion", "2h 1 Lucky_Eddy", "4f 1 Fencebuster", "3a 1 Skippers_Last_Stand", "4bd 1 The_Harvester", "3a 1 Admirals_Friend", "4jc 1 Butchers_Hook", "4fe 1 The_Fish_Catcher", "2j 1 Kiloton_Radium_Rifle", "2h 1 Old_Reliable", "1h 1 Sergant_Ash", "2da 1 The_Problem_Solver", "4i 1 Throatslicer", "4dd 1 Sword_of_Wonders", "2da 1 Splattercannon", "4f 1 Citos_Shiny_Slugger", "1e 1 Aeternus", "1b 1 Hubs_Alien_Blaster", "3k 1 Rexs_Prototype", "2ka 1 Silver_Submachinegun", "4ja 1 Swans_Power_Fist", "PA Jetpack", "Module", "1 Ranged Weapons", "2 Melee Weapons", "3 Explosives", "1 Outfits", "2 Hats and Accessories", "3 Armor and Power Armor", "4 Dogs", " Kids and Super Mutants Gear", "1 Food", "2 Drinks", "3 Meds and Chems", "4 Ingredients", "1 Settings Holotapes", "2 Collectibles", "3 Valuables", "4 Story Items", "5 Recyclables" });
            defaultConfigs.Add(TaggingConfigType.VISG, configSettingVISG);

            // Horizon
            TaggingConfiguration configSettingHorizon = new()
            {
                { ItemType.Shipment,            "[  ]" },
                { ItemType.Scrap,               "[Junk]" },
                { ItemType.Resource,            "(_)" },
                { ItemType.LooseMod,            "[Mod]" },
                { ItemType.Collectible,         "[Collectible]" },
                { ItemType.Quest,               "[Quest]" },
                { ItemType.Currency,            "(Currency)" },
                { ItemType.Valuable,            "($)" },
                { ItemType.OtherMisc,           "[Misc]" },
                { ItemType.GoodChem,            "(+)" },
                { ItemType.BadChem,             "(~)" },
                { ItemType.Food,                "[Food]" },
                { ItemType.FoodRaw,             "{Raw}" },
                { ItemType.FoodCrop,            "{Crops}" },
                { ItemType.FoodPrewar,          "{Prewar}" },
                { ItemType.Drink,               "[=]" },
                { ItemType.Liquor,              "{Liquor}" },
                { ItemType.Nukacola,            "[=C]" },
                { ItemType.Syringe,             "[Syringe]" },
                { ItemType.Device,              "(^)" },
                { ItemType.Tool,                "(X)" },
                { ItemType.News,                "[#Note]" },
                { ItemType.Note,                "[#Note]" },
                { ItemType.Perkmag,             "[Mag]" },
                { ItemType.Mine,                "(Mine)" },
                { ItemType.Grenade,             "(Grenade)" },
                { ItemType.Key,                 "[Key]" },
                { ItemType.KeyCard,             "[Key]" },
                { ItemType.KeyPassword,         "[Pass]" },
                { ItemType.Ammo,                "(Ammo)" },
                { ItemType.Holotape,            "[#Holotape]" },
                { ItemType.HolotapeGame,        "[#Holotape]" },
                { ItemType.HolotapeSettings,    "[Settings]" },
                { ItemType.PipBoy,              "|Pip|" },
                { ItemType.WeaponRanged,        "[1gun]" },
                { ItemType.WeaponMelee,         "[1melee]" },
                { ItemType.Armor,               "[Armor]" },
                { ItemType.Clothes,             "[#Cloth]" },
                { ItemType.VaultSuit,           "[Vault]" },
                { ItemType.PowerArmor,          "[1power]" },
            };

            configSettingHorizon.AddExtraTags(new string[] { "Aid", "+", "+T", "++", "Cure", "-", "-X", "-R", "Device", "Buff", "Schematic", "!Sleep", "FM", "!Rest", "^", "^D", "^A", "^T", "^R", "^M", "^Fish", "^S", "$", "$$", "X$", "?", "*", ":", "!", "Can", "!P", "!W", "!Y", "!T", "#W", "#SK", "#C", "#B", "#P", "D-", "#^", "#", "##", "__", "CM", "--", " ", "  ", "_", "_#", "!!", "X", "XX", "Y", "J", "Junk", "JunkP", "Model", "Food", "FoodC", "Meal", "Crops", "Crop", "Chem", "ChemC", "Energy", "~", "~C", "~A", "Liquor", "=L", "=", "==", "=C", "Nuka", "NukaC", "Prewar", "I", "Raw", "Wild", "Ingredients", "Syringe", "Ammo", "XAmmo", "AmmoN", "AmmoM", "Fuel", "Core", "PA-AL", "PA-AR", "PA-C", "PA-H", "PA-LL", "PA-LR", "PA", "ArmL", "ArmR", "Arm", "Gloves", "Chest", "Vault", "LegL", "LegR", "Leg", "Bandolier", "Belt", "Biosuit", "!Hazmat", "Hazmat", "Bottom", "Bracelet", "Cloak", "Dog", "Earring", "Footwear", "Harness", "Hat", "#Hat", "Headband", "#Headband", "Eyewear", "#Eyewear", "Helmet", "#Helmet", "Head", "#Head", "H", "Jacket", "Mouth", "Mask", "#Mask", "#Cloth", "!Mask", "Neck", "Offhand", "Pack", "Piercing", "Rifle", "Ring", "Satchel", "Scarf", "Sidearm", "Super Mutant", "Mutant", "Top", "Vest", "Armor", "Clothing", "A", "C", "Underarmor", "Signal", "Resource", "Scrap", "Tool", "Currency", "Lockpick", "Utility", "KW", "KH", "KR", "KE", "SkillA", "SkillB", "SkillTA", "SkillC", "SkillE1", "SkillE2", "SkillE3", "SkillF", "SkillH1", "SkillH2", "SkillL1", "SkillL2", "SkillM", "SkillS1", "SkillS2", "SkillS3", "SkillS4", "SkillTR", "SkillW1", "SkillW2", "SkillW3", "SkillW4", "SkillW5", "WS", "SkillX", "SkillY", "#Karma1", "#Karma2", "#Karma3", "#HZ", "Skill", "Unique", "Valuable", "Collectible", "Crafting", "Craft", "Mat", "Misc", "Game", "Hack", "Holotape", "#Holotape", "HoloC", "#H", "#Note", "NoteC", "#N", "RE", "#Fn", "#FnM", "#FnB", "#FnR", "#FnI", "ADR", "ADP", "FSA", "FSS", "FSD", "FSM", "FSX", "FSC", "FST", "FSE", "FSI", "STR", "Comp", "#Comp", "BH", "Mag", "MagC", "Quest", "Trash", "Key", "KeyC", "Pass", "Mod", "Mod2", "Mod-A", "Mod-W", "Mod-AG", "WAmmo", "Mod-P", "Mod-R", "Mod-D", "1star", "1aim", "1gun", "1energy", "1rifle", "1melee", "1armor", "1helm", "1skull", "1atom", "!A", "1power", "Grenade", "GrenadeP", "GrenadeE", "GrenadeM", "GrenadeK", "Fire", "Damage", "Rad", "Poison", "Cryo", "Mine", "MineC", "MineP", "MineE", "Trap", "Flare", "FlareS", "FlareG", "FlareV", "FlareA", "BPS", "EL", "EP", "EPF", "EWC", "EWG", "WM", "WF", "BG", "BGA", "BP", "BR", "BS", "SNA", "SNGB", "SNGE", "SNM", "M", "XM", "XM1B", "XM1S", "XM2B", "XM2S", "XMU", "Wrist", "NPC", "Shroud", "Casual", "Dress", "Formal", "Rugged", "Lab", "Fireworks", "Settings", "Check", "1Settings", "Modbook", "Config", "Menu", "Nuke", "VNuke", "SW", "SA", "SC", "SH", "SE", "SM", "SP", "RR", "MM", "Institute", "BOS", "BMarket", "Gear", "!Settler", "Settler", "Farm", "Camp", "Boat", "City", "Outpost", "House", "Store", "Town", "Foundation", "Warehouse", "Factory", "Bunker", "Airship", "Aircraft", "Hall", "Bridge", "Dock", "Tower", "Improvised", "Natural", "Park", "Car", "Radar", "Water", "Defense", "Pipe", "MG", "Laser", "Plasma", "Missile", "Shotgun", "Gauss", "Mining", "Salvaging", "Ore", "Color", "Display", "Size", "Weave", "Pip", "Filter" });

            defaultConfigs.Add(TaggingConfigType.Horizon, configSettingHorizon);
        }

        public TaggingConfiguration getConfigByType(TaggingConfigType type)
        {
            return defaultConfigs[type];
        }
    }
}
