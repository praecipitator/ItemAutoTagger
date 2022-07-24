This is a [Mutagen](https://github.com/Mutagen-Modding/Mutagen)-based autopatcher for Fallout 4, intended for use with [Synthesis](https://github.com/Mutagen-Modding/Synthesis).

# Description

Adds tags to item names, for usage with inventory/sorting mods.
Supports FIS, VIS, VIS-G, LWIS, and Horizon (tags only).

You can also define custom overrides on a per-item basis, to specify the "type" of that item. 
These types are then used to determine which tag should be used. The type "none" can be used to blacklist that item.

See a list of all currently supported types below.

## Settings

Most settings should be self-explanatory, or have an explanation mouseover text. But here are some more explanations anyway:

### Tagging Configuration
The most important setting, this defines which set of tags to use. If you select "Custom", you must define your tags under "Custom Tagging Configuration".

### Append Components
If checked, will append strings like `{{{wood,steel,copper}}}` after the item's name. Some UI mods can interpret these, and display a nice list of included components, while others don't.

### Override Item Type Settings
If the heuristics gets the types of certain items wrong (like, fails to classify something as a Quest Item, and thinks it's Scrap), you can manually override them here. You can also exclude them from tagging entirely by picking "None" as the type.

### Custom Tagging Configuration
If you picked "Custom" under "Tagging Configuration", you must define your tags here. Put them into the input fields, including the bracktes. If a field stays empty, this item type will not be tagged.

The **Extra Tags** setting below the individual tag fields can be used to put in additional tags **without brackets**, which, if encountered, will be considered valid and left alone, instead of double-tagging the item.


## Notice
This patcher should run after any other item-modifying patchers, like [MakeModsScrappable](https://github.com/praecipitator/MakeModsScrappable) or [MakeItemsWeightless](https://github.com/praecipitator/MakeItemsWeightless).
Otherwise, it won't be taking the changes done by these patchers into account. 

For example, if you enabled "Append Components", but ItemTagger is running before MakeModsScrappable, it will not append components to loose mods, because they don't have any components at that point.

However, if MakeItemsWeightless with enabled weightless-making of Misc Items is running before ItemTagger, it will classify a lot more items as "Currency" than otherwise, because it considers anything as "Currency" which has value, but no weight.


## Item Types
These are the types into which this patcher will categorize items for tagging:
* None: this item will not be tagged.
* Shipment: A shipment, like the "Shipment of Wood - 50" item.
* Scrap: Misc item which contain components
* Resource: Misc item which is meant to represent one unit of a component. Like, one single piece of Wood.
* LooseMod: Misc item which represents a Weapon/Armor/Robot modification. 
* Collectible: A "Collectible" Misc Item.
* Quest: A quest misc item.
* Currency: Misc item with zero weight and a non-zero value.
* Valuable: Misc item MISCs with more value than weight.
* OtherMisc: Any other misc item, trash
* GoodChem: Cures, Medicine, Aid, etc
* BadChem: Drugs, Addictive chems
* Food: generic food, or selfcooked food. Usually radless.
* FoodRaw: raw food, has rads, has disease risk
* FoodCrop: crops, usually has rads
* FoodPrewar: prewar packaged, has rads
* Drink: generic drinkable
* Liquor: Alcoholic beverages
* Nukacola: Nuka Cola of any kind, potentially other softdrinks.
* Syringe: Syringer ammo
* Device: Consumables which are supposed to be devices instead of something to eat, like the Stealth Boy. Can also be used on a Misc Item.
* Tool: Similar to above, but for more low-tech things. Like SimSettlements Town Meeting Gavel, or the Companion Whistle. Can also be used on a Misc Item.
* News: Newspaper
* Note: Any paper note
* Perkmag: Perk Magazine
* Mine: A weapon which is being used like a mine, also Far Harbor traps
* Grenade: Grenade or anything similar
* Key: any generic key
* KeyCard: a key object, which looks like a card
* KeyPassword: Password, usually written on a note or holotape
* Ammo: Any generic ammo
* Holotape: generic holotapes, or misc items meant to represent a holotape
* HolotapeGame: Game Holotape
* HolotapeSettings: Settings Holotape
* PipBoy: player's pip-boy, the misc item from Vault88, or other misc items meant to represent Pip-Boys
* WeaponRanged: A ranged weapon. Usually NOT used for direct tagging, Instead, these will gainthe  dn_CommonGun instance naming rules.
* WeaponMelee: Like above, but for melee weapons, with the dn_CommonMelee naming rules.
* Armor: A piece of clothing with an armor value. Similar to the weapons, whis will usually gain the dn_CommonArmor naming rules
* Clothes: A piece of clothing without armor value. dn_Clothes will be used
* VaultSuit: Vault Suit or similar, dn_VaultSuit will be used
* PowerArmor: any piece of Power Armor, dn_PowerArmor will be used