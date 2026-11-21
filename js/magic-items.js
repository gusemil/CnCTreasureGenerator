"use strict";

// Magic items: Table 4.0 item types, Table 4.1 potions, Table 4.2 scrolls and Tables 4.3-4.3E weapons.
// The other magic item tables are in their own files. (MagicItems.cs)

// Optional sub-tables from item descriptions
const BANE_FOES = [
    [1, 1, "Undead"],
    [2, 2, "Creatures able to cast spells"],
    [3, 4, "Orc"],
    [5, 6, "Goblin"],
    [7, 8, "Giant"],
    [9, 10, "Lycanthropes"],
    [11, 11, "Demons/Devils"],
    [12, 12, "Dragons"],
];

const DRAGON_TYPES = [
    [1, 1, "Black"],
    [2, 2, "Blue"],
    [3, 3, "Green"],
    [4, 4, "Red"],
    [5, 5, "White"],
    [6, 6, "Brass"],
    [7, 7, "Bronze"],
    [8, 8, "Copper"],
    [9, 9, "Gold"],
    [10, 10, "Silver"],
];

const MagicItems = {
    // Table 1.0 column per treasure level 1-18: % chance, number of items and max. XP value of each item (0 = no limit)
    levels: [
        { chance: 5, roll: dice(1, 1), maxXp: 100 },
        { chance: 10, roll: dice(1, 1), maxXp: 200 },
        { chance: 15, roll: dice(1, 1), maxXp: 300 },
        { chance: 20, roll: dice(1, 1), maxXp: 500 },
        { chance: 30, roll: dice(1, 2), maxXp: 800 },
        { chance: 40, roll: dice(1, 2), maxXp: 1300 },
        { chance: 50, roll: dice(1, 2), maxXp: 2100 },
        { chance: 60, roll: dice(1, 2), maxXp: 3400 },
        { chance: 70, roll: dice(1, 3), maxXp: 5500 },
        { chance: 80, roll: dice(1, 3), maxXp: 0 },
        { chance: 90, roll: dice(1, 3), maxXp: 0 },
        { chance: 92, roll: dice(1, 3), maxXp: 0 },
        { chance: 94, roll: dice(1, 4), maxXp: 0 },
        { chance: 96, roll: dice(1, 4), maxXp: 0 },
        { chance: 98, roll: dice(1, 4), maxXp: 0 },
        { chance: 99, roll: dice(1, 4), maxXp: 0 },
        { chance: 100, roll: dice(1, 6), maxXp: 0 },
        { chance: 100, roll: dice(1, 6), maxXp: 0 },
    ],

    // Table 4.0 Magic Item Generation
    itemTypes: [
        [1, 15, "Potions"],
        [16, 30, "Scroll"],
        [31, 45, "Weapon"],
        [46, 60, "Armor & Shields"],
        [61, 80, "Miscellaneous Magic"],
        [81, 90, "Rings"],
        [91, 97, "Rods, Staves, Wands"],
        [98, 99, "Cursed Items"],
        [100, 100, "Artifacts"],
    ],

    // Table 4.1 Potions
    potions: [
        [1, 3, makeItem("Aid", 400, 200)],
        [4, 6, makeItem("Bless oil", 300, 100)],
        [7, 9, makeItem("Blur", 400, 200)],
        [10, 12, makeItem("Clairaudience/Clairvoyance", 500, 300)],
        [13, 15, makeItem("Cure Light Wounds", 300, 100)],
        [16, 18, makeItem("Cure Serious Wounds", 500, 300)],
        [19, 21, makeItem("Cure Critical Wounds", 700, 500)],
        [22, 24, makeItem("Delay Poison", 400, 200)],
        [25, 27, makeItem("Endure Elements", 300, 100)],
        [28, 30, makeItem("Fly", 500, 300)],
        [31, 33, makeItem("Gaseous Form", 500, 300)],
        [34, 36, makeItem("Giant Strength", 700, 500, { description: "Strength becomes 19-24 (+3 to +6) for 10 minutes", page: 330, subRoll: SubRolls.giantStrengthPotion })],
        [37, 39, makeItem("Haste", 500, 300)],
        [40, 42, makeItem("Heal", 800, 600)],
        [43, 45, makeItem("Invisibility (potion or oil)", 400, 200)],
        [46, 48, makeItem("Levitation (potion or oil)", 400, 200)],
        [49, 51, makeItem("Longevity", 12000, 1500, { description: "Exilir of youth: 2-12 off age. A small chance to reverse effect and age 1-6 years. CK must determine this chance on campaign and situation" })],
        [52, 54, makeItem("Neutralize Poison", 600, 400)],
        [55, 57, makeItem("Nondetection", 500, 300)],
        [58, 60, makeItem("Pass without Trace", 300, 100)],
        [61, 63, makeItem("Protection from Alignment", 300, 100, { description: "Character gains +2 AC and Saves against alignment of CK's choice. Lasts 2 rounds per level of the creator" })],
        [64, 66, makeItem("Protection from Arrows", 500, 300)],
        [67, 69, makeItem("Remove Blindess/deafness", 500, 300)],
        [70, 72, makeItem("Remove Curse", 500, 300)],
        [73, 75, makeItem("Remove Disease", 500, 300)],
        [76, 78, makeItem("Remove Paralysis", 400, 200)],
        [79, 81, makeItem("Restoration", 700, 400)],
        [82, 84, makeItem("Sanctuary", 300, 100)],
        [85, 87, makeItem("Shield of Faith +2", 300, 100)],
        [88, 90, makeItem("Spider Climb", 300, 100)],
        [91, 93, makeItem("Tongues", 500, 300)],
        [94, 96, makeItem("Water Breathing", 500, 300)],
        [97, 99, makeItem("Water Walk", 900, 700)],
        [100, 100, makeItem("Trap the Soul", 1100, 900, { description: "Greenish, thick liquid, usually held in a crystal jar. Within the jar is a small topaz gem. The potion has no taste. Consuming the potion forces the user into the gem as if by a trap the soul spell" })],
    ],

    // Table 4.2 Scrolls. "N Spell Levels": random spells whose levels add up to N, from the Players Handbook spell lists
    scrolls: (() => {
        const scrollGp = [300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700];
        const scrollRanges = [[1, 8], [9, 16], [17, 24], [25, 32], [33, 40], [41, 45], [46, 50], [51, 55], [56, 60], [61, 65], [66, 68], [69, 71], [72, 74], [75, 77], [78, 80]];
        const table = scrollRanges.map(([low, high], i) => {
            const spellLevels = i + 1;
            const name = spellLevels + (spellLevels === 1 ? " Spell Level" : " Spell Levels");
            return [low, high, makeItem(name, scrollGp[i], scrollGp[i] - 200, { subRoll: scroll => ScrollSpells.fill(scroll, spellLevels) })];
        });
        return table.concat([
            [81, 82, makeItem("Teleport without Error", 900, 700)],
            [83, 84, makeItem("Symbol", 1000, 800)],
            [85, 86, makeItem("Trap the Soul", 1100, 900)],
            [87, 88, makeItem("Time Stop", 1100, 900)],
            [89, 90, makeItem("True Resurrection", 1100, 900)],
            [91, 92, makeItem("Mass Heal", 1000, 800)],
            [93, 94, makeItem("Gate", 1100, 900)],
            [95, 96, makeItem("Create Greater Undead", 1000, 800)],
            [97, 98, makeItem("Shape Change", 1100, 900)],
            [99, 100, makeItem("Clone", 1100, 900)],
        ]);
    })(),

    // Table 4.3 Weapons
    weaponTypes: [
        [1, 40, "Swords"],
        [41, 50, "Special Sword"],
        [51, 90, "Miscellaneous Weapons"],
        [91, 100, "Special Weapon"],
    ],

    // Table 4.3A Swords
    swordTypes: [
        [1, 10, "Bastard Sword"],
        [11, 30, "Broad Sword, Falchion"],
        [31, 50, "Short Sword, Scimitar, Rapier"],
        [51, 90, "Long Sword"],
        [91, 100, "Two Handed Sword"],
    ],

    // Table 4.3D Miscellaneous Weapons
    miscWeaponTypes: [
        [1, 2, "10 Arrow"],
        [3, 8, "Axe, Battle"],
        [9, 12, "Axe, hand/throwing"],
        [13, 16, "Axe, two-handed"],
        [17, 20, "Bardiche"],
        [21, 24, "10 Bolt"],
        [25, 28, "Bow"],
        [29, 32, "Club"],
        [33, 36, "Crossbow"],
        [37, 40, "Crowbill (Lucerne)"],
        [41, 44, "Dagger"],
        [45, 48, "Dart"],
        [49, 52, "Flail"],
        [53, 56, "Halberd"],
        [57, 60, "Hammer"],
        [61, 64, "Javelin"],
        [65, 68, "Lance"],
        [69, 72, "Mace"],
        [73, 76, "Morningstar"],
        [77, 80, "Pole arm"],
        [81, 84, "Sling"],
        [85, 88, "Spear"],
        [89, 92, "Staff"],
        [93, 96, "Trident"],
        [97, 100, "Whip"],
    ],

    // Table 4.3C Special Swords
    specialSwords: [
        [1, 8, makeItem("Bane Sword", 13500, 4500, { page: 330, optionalTable: BANE_FOES, optionalTableName: "Bane foe",
            description: "Normally +1 weapon but against designated foe a +3 weapon with extra 2d6 damage" })],
        [9, 16, makeItem("Sword of Dancing", 12500, 4100, { page: 331, description: "TODO" })],
        [17, 24, makeItem("Sword of Defending +4", 24500, 8100, { page: 331,
            description: "Allows the wielder to transfer all or any of the sword's magical bonus to AC. Any bonus transfered is not added to the attack roll" })],
        [25, 32, makeItem("Dragon Slayer +2/+4", 29000, 7250, { page: 331, optionalTable: DRAGON_TYPES, optionalTableName: "Dragon type",
            description: "Normally +2 weapon, but against dragons of the rolled type +4 BtH and double damage" })],
        [33, 36, makeItem("Featheredged Sword", 31750, 15000, { page: 331,
            description: "+3 to hit and +1 to damage. A non modified roll of 18-20 severs a limb/hand/foot or head. Check table on page 331." })],
        [37, 44, makeItem("Flaming Sword +2", 7750, 2000, { page: 331,
            description: "+2 weapon. Deals +1d6 extra damage on hit, +2d6 extra damage on cold based creatures" })],
        [45, 52, makeItem("Frostbane/Frostbrand +3", 13725, 3000, { page: 331,
            description: "+3 icy weapon. Sheds light as a torch on 0F //TODO insert celcius temperature. 10 points of fire resistance each round. Deals double damage against fire-based creatures" })],
        [53, 56, makeItem("Holy Avenger +2/+5", 55000, 27000, { page: 331,
            description: "+2 weapon iron sword becomes +5 holy sword in the hands of a paladin. Deals double damage on targets of evil alignment. Provides bonus spell resistance equal to paladin's level + 5 to the wielder and anyone adjacent to her. " +
                "enables wielder to use dispel magic (once per round as a normal action) at the class level of the paladin. It inflicts 2d20 points of damage to any evil aligned creature that attempts to wield it" })],
        [57, 60, makeItem("Sword of Life Stealing +2", 17000, 5600, { page: 331,
            description: "+2 ebony iron sword. Upon natural 20 weapon drains a level from its victim and grants 1d6 temporary hitpoints to the wielder. These temporary hit points last one day. Victim who survives " +
                "such a strike from a life stealer must make a CON save for each level lost. If successful, the level will return in 1d4 days or with a restoratio spell. If failed, the level is lost permanently and the victim's experience points are " +
                "moved to the mid point of points requried for the previous level (as if struck by undead)" })],
        [61, 68, makeItem("Luck Blade +2", 21500, 7100, { page: 331, subRoll: SubRolls.luckBladeWishes,
            description: "Grants +1 luck bonus on all saves. The possessor gains the benefit of 'Good Fortune' usable once a day. The extraordinary ability allows any single roll to be re-rolled. Blade has a 5% chance to contain 1-3 wishes. When the last wish " +
                "is used the sword retains all of its other abilities and properties." })],
        [69, 72, makeItem("Nine Lives Stealer +2", 8500, 2800, { page: 331,
            description: "+2 Sword. On a natural 20 to hit, the victim of this blade must make a CON save (CL 2) or be struck dead. It can do this nine times before the ability is lost. At that point the sword becomes a simple +2 Sword (with a hint of evil about it). " +
                "Any good creature who wields the sword suffers a -2 to their BtH. No spell can reverse this penalty. Only abjuring the weapon itself will negate the penalties." })],
        [73, 80, makeItem("Sword of Puncturing +2", 12000, 3000, { page: 331,
            description: "+2 wounding sword allows the wielder to inflict 1d6 CON damage 3 times per day (by draining blood from its victim) with a successful hit." })],
        [81, 88, makeItem("Sylvan Sword +3", 10000, 3000, { page: 331,
            description: "+3 blade whren used outdoors in a woodland climate, inflicts and additional 1d6 damage on a hit. If target is reduced to 0 hitpoints or less by a single strike, the wielder of the sword gains a free attack on another single target within melee range of the sword's wielder." })],
        [89, 92, makeItem("Vorpal Sword +4", 38750, 19000, { page: 331,
            description: "+4 weapon. Has the unique ability to decapitate those it strikes. On a natural 20, the weapon severs the opponent's head (if it has one). Undeads, golems or headless creatures are not affected by decapitation. For vampires and most creatures, decapitation results in an instant death." })],
        [93, 100, makeItem("Sword of Wounding +3", 16000, 5300, { page: 332,
            description: "Every time a creature is struck by this sword, in addition to normal damage the creature loses additional hit points from the blood loss equal to the maximum damage of the sword, including the +3 bonus. " +
                "For example a long sword of wounding inflicts 11 points of blood loss damage. The victim suffers 1 point of damage per round until 11 points of daamge are dealt or a healing spell is cast to stop the loss." })],
    ],

    // Table 4.3E Special Miscellaneous Weapons
    specialMiscWeapons: [
        [1, 8, makeItem("Bane Weapon", 13500, 4500, { page: 330, optionalTable: BANE_FOES, optionalTableName: "Bane foe", subRoll: item => MagicItems.addMiscWeaponType(item),
            description: "Normally +1 weapon but against designated foe a +3 weapon with extra 2d6 damage" })],
        // Bonus from Table 4.3B, value stays as listed
        [9, 11, makeItem("Club of Dagda +X", 10000, 4000, { page: 333, subRoll: club => { club.name = club.name.replace("+X", "+" + SubRolls.rollBonus().bonus); },
            description: "User can declare a critical hit once per day. Can also heal 20 hitpoints a day." })],
        [12, 15, makeItem("Dagger of Venom +1", 4750, 1070, { page: 333, description: "+1 Dagger, can inject poison once per day" })],
        [16, 24, makeItem("Dwarven Thrower +2/+3", 18000, 6000, { page: 333, description: "+2 Warhammer, but +3 in hands of Dwarf. If thrown by a dwarf, the hammer gains the ability to return to its owner. Can be hurled a distance of 30ft. When hurled against giants it deals an extra 2d8 damage or an extra 1d8 against any other target. Unless obstructed, it always returns to its owner." })],
        [25, 28, makeItem("Featheredged Axe +3", 31750, 15000, { page: 333, description: "Has bonus of +3 to hit and +1 damage, on a non-modified roll of 18-20 it severs a limb/hand/foot/head" })],
        [29, 36, makeItem("Javelin of Lightning", 4500, 450, { page: 333, description: "Javelin becomes a 5d6 Lightning Bolt when thrown (Victim makes a dex save for half damage). It is consumed in the attack." })],
        [37, 40, makeItem("Mace (Hammer) of Disruption +3", 25500, 8500, { page: 333, description: "+3 weapon that deals double damage to undead. Upon successfu hit, the undead must make a physical save or be destroyed. Other undead near the target are affected as if turned by a 10th level Cleric (i.e. multiple lower hit dice undead can thus be destroyed with a single strike)" })],
        [41, 48, makeItem("Mace of Smiting +3", 17500, 5800, { page: 333, description: "+3 Heavy Mace has a +5 enchantment bonus against constructs, and any roll of natural 20 dealt to a construct shatters it, completely destroying its form. A natural 20 dealt to an extraplanar creature deals 4x damage." })],
        [49, 56, makeItem("Mace of Terror +2", 8500, 2100, { page: 333, description: "On command this +2 mace causes the wielder's clothes and appearance to transform into an illusion of darkest horror. Living creatures in a 30 foot radius become panicked as if by a cause fear spell unless a successful charisma save is made." })],
        [57, 60, makeItem("Nine Lives Stealer +2", 8500, 2800, { page: 333, description: "Read page 333 description" })],
        [61, 64, makeItem("Oathbow +2/+5", 25000, 5000, { page: 333, description: "Read page 333 description" })],
        [65, 68, makeItem("Slaying Arrow/Bolt", 9500, 2300, { page: 333, description: "+1 arrow or bolt magically attuned to a particular type or subtype of creature. If strikes such a creature target must make a constitution save or die instantly. The CK determines the particular type of creature it slays." })],
        [69, 76, makeItem("Sleep Arrow/Bolt", 1250, 350, { page: 333, description: "+1 Arrow/bolt deals normal damage but converted to non-lethal damage. Also forces the target to make a wisdom save or fall asleep with a duration identical to the sleep spell." })],
        [77, 84, makeItem("Sylvan Weapon +3", 10000, 3000, { page: 333, description: "If used in outdoors in a woodland climate, inflicts an additional 1d6 dmg on hit. If a target is reduced to 0 hitpoints or less by a single strike, wielder of the weapon gains a free attack on another single target within melee range of the weapon's wielder." })],
        [85, 92, makeItem("Trident of Fish Command +1", 5000, 1200, { page: 334, description: "Allows the wielder to charm (as the spell) up to 14 HD of aquatic animals, two of which can't be more than 30ft apart. Can be used 3 times per day. Wielder can communicate with the animals as if Speak with animals spell. Animals making their saving throw are free of control." })],
        [93, 100, makeItem("Weapon of Wounding +3", 16000, 5300, { page: 334, subRoll: item => MagicItems.addMiscWeaponType(item),
            description: "Every time a creature is struck with this weapon, it loses additional hit points due blood loss equal to max damage of the weapon including the +3 bonus. This damage must be healed naturally. A healing spell with stop the blood loss but does not heal the damage." })],
    ],

    // Table 4.0: rolls each magic item, rerolling anything worth more XP than the treasure level's
    // "Max. XP Value of Item" (Table 1.0). Artifacts are priceless, so they never fit under a limit.
    rollItemType(itemsAmount, maxXp = 0) {
        for (let i = 0; i < itemsAmount; i++) {
            log("");
            log("Magic item " + (i + 1) + " of " + itemsAmount);
            const item = MagicItems.rollItemWithinXpLimit(maxXp);
            log("");
            logItem(item);
            TreasureReport.addMagicItem(item);
        }
    },

    rollItemWithinXpLimit(maxXp) {
        const MAX_ATTEMPTS = 100;
        for (let attempt = 1; ; attempt++) {
            const item = MagicItems.rollOneMagicItem();
            const overLimit = maxXp > 0 && (item.priceless || item.xp > maxXp);
            if (!overLimit || attempt === MAX_ATTEMPTS) return item;
            log(item.name + " (" + (item.priceless ? "artifact" : item.xp + " xp") + ") is over the " +
                maxXp + " xp limit for this treasure level. Rerolling...");
            log("");
        }
    },

    rollOneMagicItem() {
        const itemType = rollOnTable(MagicItems.itemTypes, "Table 4.0 Magic Item Generation");
        log("Magic item type is " + itemType);

        switch (itemType) {
            case "Potions": return MagicItems.withPrefix(rollItem(MagicItems.potions, "Table 4.1 Potions"), "Potion of ");
            case "Scroll": return MagicItems.withPrefix(rollItem(MagicItems.scrolls, "Table 4.2 Scrolls"), "Scroll of ");
            case "Weapon": return MagicItems.rollWeapon();
            case "Armor & Shields": return ArmorAndShields.roll();
            case "Miscellaneous Magic": return MiscMagic.roll();
            case "Rings": return Rings.roll();
            case "Rods, Staves, Wands": return RodsStavesWands.roll();
            case "Cursed Items": return CursedItems.roll();
            case "Artifacts": return Artifacts.roll();
            default: throw new Error("Unknown magic item table " + itemType);
        }
    },

    // Potion and scroll table entries are just the effect ("Sanctuary", "9 Spell Levels"), so name the item type
    withPrefix(item, prefix) {
        item.name = prefix + item.name;
        return item;
    },

    // Table 4.3: every magic weapon also gets a size and may be sentient
    rollWeapon() {
        const weaponType = rollOnTable(MagicItems.weaponTypes, "Table 4.3 Weapons");
        let weapon;

        switch (weaponType) {
            case "Swords":
                weapon = MagicItems.rollBonusWeapon(MagicItems.rollSwordType(), 324);
                break;
            case "Special Sword":
                weapon = rollItem(MagicItems.specialSwords, "Table 4.3C Special Swords");
                addNote(weapon, "Sword type: " + MagicItems.rollSwordType());
                break;
            case "Miscellaneous Weapons":
                weapon = MagicItems.rollBonusWeapon(MagicItems.rollMiscWeaponType(), 325);
                break;
            default:
                weapon = rollItem(MagicItems.specialMiscWeapons, "Table 4.3E Special Miscellaneous Weapons");
                break;
        }

        SubRolls.rollSize(weapon);
        SentientItems.maybeMakeSentient(weapon);
        return weapon;
    },

    // Table 4.3A/4.3D weapon with its bonus from Table 4.3B
    rollBonusWeapon(weaponName, page) {
        const weapon = makeItem(weaponName, 0, 0, { page: page, rollBonus: true });
        rollDetails(weapon);
        return weapon;
    },

    rollSwordType() {
        return rollOnTable(MagicItems.swordTypes, "Table 4.3A Swords");
    },

    rollMiscWeaponType() {
        return rollOnTable(MagicItems.miscWeaponTypes, "Table 4.3D Miscellaneous Weapons");
    },

    addMiscWeaponType(item) {
        addNote(item, "Weapon type: " + MagicItems.rollMiscWeaponType());
    },
};
