"use strict";

// Extraordinary items: expert weapons, jewelry, worn & ceremonial items, hand crafted items and
// antiquities. (ExtraOrdinaryItems.cs)
const ExtraordinaryItems = {
    // Table 1.0 column per treasure level 1-18: % chance and number of items
    levels: [
        { chance: 10, roll: dice(1, 2, 0) },
        { chance: 20, roll: dice(1, 2, 1) },
        { chance: 30, roll: dice(1, 2, 2) },
        { chance: 40, roll: dice(1, 2, 3) },
        { chance: 50, roll: dice(1, 4, 2) },
        { chance: 60, roll: dice(1, 4, 3) },
        { chance: 70, roll: dice(1, 4, 4) },
        { chance: 80, roll: dice(1, 4, 5) },
        { chance: 90, roll: dice(1, 6, 4) },
        { chance: 91, roll: dice(1, 6, 5) },
        { chance: 92, roll: dice(1, 6, 6) },
        { chance: 93, roll: dice(1, 6, 7) },
        { chance: 94, roll: dice(1, 8, 6) },
        { chance: 95, roll: dice(1, 8, 7) },
        { chance: 96, roll: dice(1, 8, 8) },
        { chance: 97, roll: dice(1, 8, 9) },
        { chance: 98, roll: dice(1, 8, 8) },
        { chance: 99, roll: dice(1, 8, 9) },
    ],

    itemTypes: [
        [1, 4, { table: "expertWeapons", description: "Expert Weapons", tableName: "Extra ordinary weapon" }],
        [5, 8, { table: "jewelry", description: "Jewelry", tableName: "Jewelry" }],
        [9, 12, { table: "wornAndCeremonial", description: "Worn & Ceremonial Items", tableName: "Worn and ceremonial" }],
        [13, 16, { table: "handcrafted", description: "Hand Crafted Items", tableName: "Handcrafted" }],
        [17, 20, { table: "antiquities", description: "Antiquities", tableName: "Antiquities" }],
    ],

    expertWeapons: [
        [1, 5, makeItem("Arrow", 1)],
        [6, 10, makeItem("Battle Axe", 100)],
        [11, 15, makeItem("Bolt", 12, 0, { description: "Gp value in sp" })],
        [16, 20, makeItem("Bow", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [21, 25, makeItem("Crossbow", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [26, 30, makeItem("Dagger", 20)],
        [31, 35, makeItem("Dart", 5)],
        [36, 40, makeItem("Flail", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [41, 45, makeItem("Halberd", 100)],
        [46, 50, makeItem("Hammer", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [51, 55, makeItem("Hand Axe", 40)],
        [56, 60, makeItem("Javelin", 10)],
        [61, 65, makeItem("Lance", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [66, 70, makeItem("Mace", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [71, 75, makeItem("Morningstar", 80)],
        [76, 80, makeItem("Scimitar", 150)],
        [81, 85, makeItem("Spear", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [86, 90, makeItem("Sword", 0, 0, { description: "Type is Castle Keeper's Choice. Value varies" })],
        [91, 95, makeItem("Trident", 100)],
        [96, 100, makeItem("Two-Handed Axe", 300)],
    ],

    // Value of most jewelry comes from the value table
    jewelry: [
        [1, 5, makeItem("Ankle Chain", 0, 0, { valueTable: true })],
        [6, 10, makeItem("Arm Band", 0, 0, { valueTable: true })],
        [11, 15, makeItem("Belt", 0, 0, { valueTable: true })],
        [16, 20, makeItem("Bracelet", 0, 0, { valueTable: true })],
        [21, 25, makeItem("Broach", 0, 0, { valueTable: true })],
        [26, 30, makeItem("Buckle", 0, 0, { valueTable: true })],
        // 1 gp per button, 1d3+1 buttons
        [31, 35, makeItem("Button", 0, 0, { description: "2-4 buttons, each button 1gp worth", rollGp: dice(1, 1), rollAmount: dice(1, 3, 1) })],
        [36, 40, makeItem("Collar", 0, 0, { valueTable: true })],
        [41, 45, makeItem("Choker", 0, 0, { valueTable: true })],
        [46, 50, makeItem("Earrings", 0, 0, { valueTable: true })],
        [51, 55, makeItem("Locket", 0, 0, { valueTable: true })],
        [56, 60, makeItem("Medallion", 0, 0, { valueTable: true })],
        [61, 65, makeItem("Necklace", 0, 0, { valueTable: true })],
        [66, 70, makeItem("Pendant", 0, 0, { valueTable: true })],
        [71, 75, makeItem("Ring", 0, 0, { valueTable: true })],
        [76, 80, makeItem("Stud", 0, 0, { valueTable: true })],
        [81, 85, makeItem("Tiara", 0, 0, { valueTable: true })],
        [86, 90, makeItem("Toe Ring", 0, 0, { valueTable: true })],
        [91, 95, makeItem("Torque", 0, 0, { valueTable: true })],
        [96, 100, makeItem("Waist Chain", 0, 0, { valueTable: true })],
    ],

    wornAndCeremonial: [
        [1, 5, makeItem("Coronet", 0, 0, { valueTable: true })],
        [6, 10, makeItem("Crown", 0, 0, { valueTable: true })],
        [11, 15, makeItem("Orb", 0, 0, { valueTable: true })],
        [16, 20, makeItem("Scepter", 0, 0, { valueTable: true })],
        [21, 25, makeItem("Signet ring", 0, 0, { valueTable: true })],
        [26, 30, makeItem("Holy Symbol", 0, 0, { valueTable: true })],
        [31, 35, makeItem("Holy water", 0, 0, { description: "water container?", valueTable: true })],
        [36, 40, makeItem("Idol", 0, 0, { valueTable: true })],
        [41, 45, makeItem("Relic", 0, 0, { description: "Relics include bones of saints, iconographic statues, or anything else that may be holy to a religious group.", valueTable: true })],
        [46, 50, makeItem("Rune Stones", 0, 0, { valueTable: true })],
        [51, 55, makeItem("Fur coat", 0, 0, { rollGp: dice(2, 10) })],
        [56, 60, makeItem("Hair shirt", 0, 0, { rollGp: dice(1, 10) })],
        [61, 65, makeItem("Leather jerkin", 0, 0, { rollGp: dice(2, 10) })],
        [66, 70, makeItem("Oilskin cloth", 0, 0, { rollGp: dice(2, 10) })],
        [71, 75, makeItem("Silk garment", 0, 0, { rollGp: dice(5, 10) })],
        [76, 80, makeItem("Gown", 0, 0, { rollGp: dice(1, 10) })],
        [81, 85, makeItem("Hood", 0, 0, { rollGp: dice(1, 4) })],
        [86, 90, makeItem("Mantle", 0, 0, { rollGp: dice(1, 4) })],
        [91, 95, makeItem("Surcoat", 0, 0, { rollGp: dice(3, 10) })],
        [96, 100, makeItem("Tabard", 0, 0, { rollGp: dice(2, 10) })],
    ],

    handcrafted: [
        [1, 5, makeItem("Wooden bird cage", 20)],
        [6, 10, makeItem("Ivory pipe", 50)],
        [11, 15, makeItem("Paper, ink & quill", 15, 0, { description: "These should come in a scroll case or box" })],
        [16, 20, makeItem("Silver snuff box", 100)],
        [21, 25, makeItem("Mechnical toy", 0, 0, { rollGp: dice(2, 10) })],
        [26, 30, makeItem("China place settings", 0, 0, { description: "from 1-12 are found, value is per setting", rollGp: dice(2, 6, 0, 10), rollAmount: dice(1, 12) })],
        [31, 35, makeItem("Crystal vase", 0, 0, { rollGp: dice(10, 10) })],
        [36, 40, makeItem("Pewter goblet", 2)],
        [41, 45, makeItem("Trencher, silver plated", 4)],
        [46, 50, makeItem("Wooden gourd", 1)],
        [51, 55, makeItem("Golden harp", 0, 0, { valueTable: true })],
        [56, 60, makeItem("Hunter's horn", 0, 0, { valueTable: true })],
        [61, 65, makeItem("Lute of Vaughn", 120)],
        [66, 70, makeItem("Elven mandolin", 100)],
        [71, 75, makeItem("Dragonclaw panpipes", 500)],
        // 1d991+9 = 10-1000 gp
        [76, 80, makeItem("Animal pelt", 0, 0, { description: "Cured. The value of any pelt ranges from 10 gp to 1000 gp depending on locale and rarity", rollGp: dice(1, 991, 9) })],
        [81, 85, makeItem("Decorative egg", 100)],
        [86, 90, makeItem("Statue", 0, 0, { valueTable: true })],
        [91, 95, makeItem("Carved wood", 0, 0, { valueTable: true })],
        [96, 100, makeItem("Miniature figurine", 0, 0, { valueTable: true })],
    ],

    antiquities: [
        [1, 5, makeItem("Book(s)", 0, 0, { description: "Books, charts and maps can contain anything from histories amd geographical references to treasure maps and nautical charts", rollGp: dice(10, 10) })],
        [6, 10, makeItem("Charts", 0, 0, { description: "Books, charts and maps can contain anything from histories amd geographical references to treasure maps and nautical charts", rollGp: dice(5, 10) })],
        [11, 15, makeItem("Map", 0, 0, { description: "Books, charts and maps can contain anything from histories amd geographical references to treasure maps and nautical charts", rollGp: dice(5, 10) })],
        [16, 20, makeItem("Scroll", 0, 0, { rollGp: dice(10, 10) })],
        [21, 25, makeItem("Stone Tablet", 0, 0, { rollGp: dice(10, 10, 0, 10) })],
        [26, 30, makeItem("Banner", 250)],
        [31, 35, makeItem("Painting", 0, 0, { rollGp: dice(10, 10, 0, 10) })],
        [36, 40, makeItem("Rug", 0, 0, { rollGp: dice(10, 10) })],
        [41, 45, makeItem("Tapestry", 0, 0, { rollGp: dice(10, 10, 0, 100) })],
        [46, 50, makeItem("Trophy", 0, 0, { rollGp: dice(10, 10) })],
        [51, 55, makeItem("Brazier", 0, 0, { valueTable: true })],
        [56, 60, makeItem("Candlelabra", 0, 0, { valueTable: true })],
        [61, 65, makeItem("Coffer", 0, 0, { valueTable: true })],
        [66, 70, makeItem("Mirror", 0, 0, { valueTable: true })],
        [71, 75, makeItem("Urn", 0, 0, { valueTable: true })],
        [76, 80, makeItem("Death Mask", 0, 0, { valueTable: true })],
        [81, 85, makeItem("Hour glass", 0, 0, { valueTable: true })],
        [86, 90, makeItem("Music box", 0, 0, { valueTable: true })],
        [91, 95, makeItem("Wine", 0, 0, { rollGp: dice(2, 10, 0, 10) })],
        [96, 100, makeItem("Troll knuckles", 0, 0, { valueTable: true })],
    ],

    rollItemType(itemsAmount) {
        for (let i = 0; i < itemsAmount; i++) {
            const roll = rollNumber(1, 20);
            const itemType = lookup(ExtraordinaryItems.itemTypes, roll, "Extraordinary item type");
            log("Rolled " + roll + " for extraordinary item type which is " + itemType.description);

            const item = rollItem(ExtraordinaryItems[itemType.table], itemType.tableName);
            logItem(item);
            TreasureReport.addExtraordinaryItem(item);
        }
    },
};
