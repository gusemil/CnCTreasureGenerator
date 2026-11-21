"use strict";

// Tables 4.5-4.5E: Miscellaneous Magic (Monsters & Treasure pages 326-328, descriptions pages 334-348).
// Items whose value or nature depends on a further roll get a subRoll from their description. (MiscMagic.cs)

// Sub-rolls from item descriptions
const MiscMagicRolls = {
    bagOfHolding(item) {
        const types = ["Type I: weighs 15 lb., holds 250 lb. (market price 2,500 gp)", "Type II: weighs 25 lb., holds 500 lb. (market price 5,000 gp)",
            "Type III: weighs 35 lb., holds 1,000 lb. (market price 7,400 gp)", "Type IV: weighs 60 lb., holds 1,500 lb. (market price 10,000 gp)"];
        addNote(item, "Bag " + pick(types));
    },

    bagOfTricks(item) {
        const bag = pick([
            { color: "Gray", gp: 1000, xp: 300, animals: "bat, rat, cat, weasel, badger" },
            { color: "Rust", gp: 4000, xp: 1250, animals: "wolverine, wolf, boar, black bear" },
            { color: "Tan", gp: 16000, xp: 3000, animals: "brown bear, lion, heavy warhorse, tiger, rhinoceros" },
        ]);
        item.gp = bag.gp;
        item.xp = bag.xp;
        addNote(item, bag.color + " colored bag (animals: " + bag.animals + ")");
    },

    crystalBall(item) {
        const ball = pick([
            { type: "Crystal ball", gp: 42000 },
            { type: "Crystal ball with see invisibility", gp: 50000 },
            { type: "Crystal ball with detect thoughts", gp: 51000 },
            { type: "Crystal ball with telepathic bond", gp: 70000 },
            { type: "Crystal ball with true seeing", gp: 80000 },
        ]);
        item.gp = ball.gp;
        addNote(item, "Type: " + ball.type + " (market price)");
    },

    figurine(item) {
        const figurine = pick([
            { name: "Bronze Griffon", gp: 10000, xp: 2000 }, { name: "Ebony Fly", gp: 10000, xp: 2000 }, { name: "Golden Lions (pair)", gp: 16500, xp: 3300 },
            { name: "Ivory Goats (three)", gp: 21000, xp: 4000 }, { name: "Marble Elephant", gp: 17000, xp: 3400 }, { name: "Obsidian Steed", gp: 28500, xp: 5700 },
            { name: "Onyx Dog", gp: 15500, xp: 3100 }, { name: "Serpentine Owl", gp: 9100, xp: 1820 }, { name: "Silver Raven", gp: 3800, xp: 760 },
        ]);
        item.gp = figurine.gp;
        item.xp = figurine.xp;
        addNote(item, "Figurine: " + figurine.name);
    },

    // d%: 01-40 silver, 41-75 brass, 76-90 bronze, 91-100 iron. XP 1,250 "doubles with each horn"
    hornOfValhalla(item) {
        const roll = rollPercentage();
        const horn =
            roll <= 40 ? { type: "Silver", xp: 1250, summons: "2d4+2 2nd level barbarians. Prerequisite: none" } :
            roll <= 75 ? { type: "Brass", xp: 2500, summons: "2d4+1 3rd level barbarians. Prerequisite: spellcaster level 1st" } :
            roll <= 90 ? { type: "Bronze", xp: 5000, summons: "2d4 4th level barbarians. Prerequisite: proficiency with all weapons or bardic music ability" } :
                         { type: "Iron", xp: 10000, summons: "1d4+1 5th level barbarians. Prerequisite: proficiency with all weapons or bardic music ability" };
        item.xp = horn.xp;
        addNote(item, horn.type + " horn: summons " + horn.summons);
    },

    iounStone(item) {
        const stone = pick([
            { stone: "Clear spindle: sustains creature without food or water", gp: 4000, xp: 500 },
            { stone: "Dusty rose prism: +1 bonus to AC", gp: 5000, xp: 500 },
            { stone: "Deep red sphere: +2 enchantment bonus to dexterity", gp: 8000, xp: 1000 },
            { stone: "Incandescent blue sphere: +2 enchantment bonus to wisdom", gp: 8000, xp: 1000 },
            { stone: "Pale blue rhomboid: +2 enchantment bonus to strength", gp: 8000, xp: 1000 },
            { stone: "Pink rhomboid: +2 enchantment bonus to constitution", gp: 8000, xp: 1000 },
            { stone: "Pink & green sphere: +2 enchantment bonus to charisma", gp: 8000, xp: 1000 },
            { stone: "Scarlet & blue sphere: +2 enchantment bonus to intelligence", gp: 8000, xp: 1000 },
            { stone: "Dark blue rhomboid: alertness (+2 to wisdom attribute checks)", gp: 10000, xp: 1000 },
            { stone: "Vibrant purple prism: stores three levels of spells, as a ring of spell storing", gp: 36000, xp: 1500 },
            { stone: "Iridescent spindle: sustains creature without air", gp: 18000, xp: 1000 },
            { stone: "Pale lavender ellipsoid: absorbs spells of 4th level or lower", gp: 20000, xp: 2000 },
            { stone: "Pearly white spindle: regenerate 1 point of damage per hour", gp: 20000, xp: 1000 },
        ]);
        item.gp = stone.gp;
        item.xp = stone.xp;
        addNote(item, "Stone: " + stone.stone);
    },

    // Market price is 150 gp per die of damage. The book lists no XP value.
    necklaceOfFireballs(item) {
        const necklace = pick([
            { type: "Type I", gp: 1650, spheres: "1x 5d6, 2x 3d6" },
            { type: "Type II", gp: 2700, spheres: "1x 6d6, 2x 4d6, 2x 2d6" },
            { type: "Type III", gp: 4350, spheres: "1x 7d6, 2x 5d6, 4x 3d6" },
            { type: "Type IV", gp: 5400, spheres: "1x 8d6, 2x 6d6, 2x 4d6, 4x 2d6" },
            { type: "Type V", gp: 5850, spheres: "1x 9d6, 2x 7d6, 2x 5d6, 2x 3d6" },
            { type: "Type VI", gp: 8100, spheres: "1x 10d6, 2x 8d6, 2x 6d6, 4x 4d6" },
            { type: "Type VII", gp: 8700, spheres: "1x 10d6, 2x 9d6, 2x 7d6, 2x 5d6, 2x 3d6" },
        ]);
        item.gp = necklace.gp;
        addNote(item, necklace.type + " spheres: " + necklace.spheres + ". No XP value listed");
    },

    // Value 1,000 gp x spell level squared, XP 1,000 per spell level
    pearlOfPower(item) {
        const level = rollDie(9);
        item.gp = 1000 * level * level;
        item.xp = 1000 * level;
        addNote(item, "Recalls one spell of level " + level + " per day");
    },

    robeOfTheArchmagi(item) {
        const roll = rollPercentage();
        const color = roll <= 45 ? "White (good)" : roll <= 75 ? "Gray (neither good nor evil)" : "Black (evil)";
        addNote(item, "Color: " + color);
    },

    usefulPatches: [
        [1, 8, "Bag of 100 gold pieces"],
        [9, 15, "Coffer, silver (500 gp value)"],
        [16, 22, "Door, iron"],
        [23, 30, "Gems, x10 (100 gp value each)"],
        [31, 44, "Ladder, wooden (24 ft.)"],
        [45, 51, "Mule (with saddle bags)"],
        [52, 59, "Pit, open (10 ft. cube)"],
        [60, 68, "Potion of cure serious wounds"],
        [69, 75, "Row boat (12 ft.)"],
        [76, 83, "Minor scroll of one randomly determined spell"],
        [84, 90, "Dogs, pair (riding dogs)"],
        [91, 96, "Window (2 ft. by 4 ft.)"],
        [97, 100, "Portable ram"],
    ],

    // Two each of the standard patches, plus 4d4 more rolled on the patch table
    robeOfUsefulItems(item) {
        addNote(item, "Standard patches (2 each): dagger, bullseye lantern, mirror, 10 ft. pole, 50 ft. hemp rope, large sack");
        const extra = dice(4, 4).roll();
        const counts = new Map();
        for (let i = 0; i < extra; i++) {
            const patch = rollOnTable(MiscMagicRolls.usefulPatches, "Robe of Useful Items patches");
            counts.set(patch, (counts.get(patch) || 0) + 1);
        }
        const parts = [...counts.keys()].sort((a, b) => a.localeCompare(b)).map(patch => counts.get(patch) + "x " + patch);
        addNote(item, extra + " extra patches: " + parts.join("; "));
    },

    prayerBeads(item) {
        const strand = pick([
            "Lesser strand: bead of blessing, bead of healing",
            "Strand: bead of healing, bead of karma, bead of smiting",
            "Greater strand: bead of healing, bead of karma, bead of summons, bead of wind walking",
        ]);
        addNote(item, strand + ". Value and XP: see text");
    },
};

function miscItem(name, gp, xp, page, subRoll = null, description = "") {
    return makeItem(name, gp, xp, { page: page, subRoll: subRoll, description: description });
}

const MiscMagic = {
    subTables: [
        [1, 20, "tableA"],
        [21, 40, "tableB"],
        [41, 60, "tableC"],
        [61, 80, "tableD"],
        [81, 100, "tableE"],
    ],

    tableA: [
        [1, 4, miscItem("Amulet of Health", 0, 0, 334, SubRolls.plusTwoFourSix, "Enchantment bonus to constitution")],
        [5, 8, miscItem("Amulet of Mighty Fists", 0, 0, 335, SubRolls.plusOneToFive, "Bonus on attack and damage with unarmed attacks and natural weapons")],
        [9, 12, miscItem("Amulet of Natural Armor", 0, 0, 335, SubRolls.plusOneToFive, "Natural armor bonus")],
        [13, 16, miscItem("Amulet of the Planes", 20250, 6750, 335)],
        [17, 20, miscItem("Bag of Holding", 25500, 8500, 335, MiscMagicRolls.bagOfHolding)],
        [21, 24, miscItem("Bag of Tricks", 0, 0, 335, MiscMagicRolls.bagOfTricks)],
        [25, 28, miscItem("Belt of Giant Strength", 0, 1000, 335, SubRolls.giantStrengthBelt)],
        [29, 32, miscItem("Blessed Book", 112000, 0, 335, null, "Pages are empty when found. XP: 100 per spell level in the book that the wizard does not already possess")],
        [33, 36, miscItem("Boat, Folding", 17500, 1750, 335)],
        [37, 40, miscItem("Boots of Elvenkind", 13500, 3500, 336)],
        [41, 44, miscItem("Boots of Levitation", 11000, 2500, 336)],
        [45, 48, miscItem("Boots of Speed", 12750, 1275, 336)],
        [49, 52, miscItem("Boots of Striding and Springing", 9000, 900, 336)],
        [53, 56, miscItem("Boots of Teleportation", 20250, 6750, 336)],
        [57, 60, miscItem("Boots of the Winterlands", 9500, 950, 336)],
        [61, 64, miscItem("Bowl of Commanding Water Elementals", 25500, 8500, 336)],
        [65, 68, miscItem("Bracers of Armor", 0, 0, 336, SubRolls.plusOneToFive, "Armor bonus")],
        [69, 72, miscItem("Bracers of Deflection", 13500, 4500, 336)],
        [73, 76, miscItem("Brazier of Commanding Fire Elementals", 25000, 8500, 336)],
        [77, 80, miscItem("Brooch of Shielding", 11500, 1150, 336)],
        [81, 84, miscItem("Broom of Flying", 12750, 4250, 336)],
        [85, 88, miscItem("Candle of Invocation", 22000, 7300, 336)],
        [89, 92, miscItem("Cape of the Mountebank", 16000, 1600, 336)],
        [93, 96, miscItem("Carpet of Flying", 17250, 5750, 336)],
        [97, 100, miscItem("Censer of Controlling Air Elementals", 25000, 8500, 336)],
    ],

    tableB: [
        [1, 4, miscItem("Chime of Interruption", 13500, 1350, 337)],
        [5, 8, miscItem("Chime of Opening", 10500, 1050, 337)],
        [9, 12, miscItem("Circlet of Persuasion", 11250, 1125, 337)],
        [13, 16, miscItem("Cloak of Arachnida", 17500, 1750, 337)],
        [17, 20, miscItem("Cloak of the Bat", 13500, 3000, 337)],
        [21, 24, miscItem("Cloak of Charisma", 0, 0, 337, SubRolls.plusTwoFourSix, "Bonus to all charisma based checks")],
        [25, 28, miscItem("Cloak of Displacement", 17000, 1700, 337)],
        [29, 32, miscItem("Cloak of Elvenkind", 15500, 3000, 337)],
        [33, 36, miscItem("Cloak of Etherealness", 11250, 2500, 337)],
        [37, 40, miscItem("Cloak of the Manta Ray", 9000, 2500, 337)],
        [41, 44, miscItem("Cloak of Resistance", 0, 0, 337, SubRolls.plusOneToFive, "Enchantment bonus on all saving throws")],
        [45, 46, miscItem("Coin, Sampo", 10000, 2750, 337)],
        [47, 48, miscItem("Crystal Ball", 0, 5000, 337, MiscMagicRolls.crystalBall)],
        [49, 52, miscItem("Cube of Force", 47250, 4000, 338)],
        [53, 56, miscItem("Cube of Frost Resistance", 12750, 1270, 338)],
        [57, 60, miscItem("Cubic Gate", 24750, 4500, 338)],
        [61, 64, miscItem("Decanter of Endless Water", 13500, 1350, 338)],
        [65, 66, miscItem("Dragon's Teeth", 7000, 12000, 338)],
        [67, 68, miscItem("Drums of Panic", 4750, 475, 338)],
        [69, 72, miscItem("Dust of Appearance", 8500, 850, 338)],
        [73, 76, miscItem("Dust of Disappearance", 5500, 550, 338)],
        [77, 80, miscItem("Dust of Illusion", 5500, 550, 339)],
        [81, 84, miscItem("Efficient Quiver", 8500, 850, 339)],
        [85, 88, miscItem("Efreeti Bottle", 47250, 4700, 339)],
        [89, 92, miscItem("Elemental Gem", 20250, 2000, 339, item => addNote(item, "Elemental type: " + pick(["Air (transparent)", "Earth (light brown)", "Fire (reddish orange)", "Water (blue-green)"])))],
        [93, 96, miscItem("Eversmoking Bottle", 4250, 425, 339)],
        [97, 100, miscItem("Eyes of Doom", 0, 7, 339, null, "Value: free round at the House of Sludge (as listed in the book)")],
    ],

    tableC: [
        [1, 3, miscItem("Eyes of the Eagle", 4250, 425, 339)],
        [4, 7, miscItem("Figurines of Wondrous Power", 0, 0, 339, MiscMagicRolls.figurine)],
        [8, 11, miscItem("Feather Token", 4500, 450, 339, item => addNote(item, "Token: " + pick(["Anchor", "Bird", "Fan", "Swan Boat", "Tree", "Whip"])))],
        [12, 13, miscItem("Fruit of the Lotus Tree", 10000, 5000, 340)],
        [14, 16, miscItem("Gauntlets of Ogre Power", 9500, 950, 340)],
        [17, 20, miscItem("Gem of Brightness", 17750, 2000, 340)],
        [21, 24, miscItem("Gem of Seeing", 15250, 1500, 341)],
        [25, 28, miscItem("Gloves of Dexterity", 0, 0, 341, SubRolls.plusOneToFive, "Enchantment bonus to dexterity")],
        [29, 32, miscItem("Gloves of Swimming and Climbing", 9000, 900, 341)],
        [33, 36, miscItem("Handy Haversack", 12250, 1225, 341)],
        [37, 40, miscItem("Harp of Charming", 7750, 2000, 341)],
        [41, 44, miscItem("Hat of Disguise", 5500, 550, 341)],
        [45, 48, miscItem("Helm of Brilliance", 34750, 3000, 341)],
        [49, 52, miscItem("Helm of Comprehend Language & Read Magic", 4250, 425, 341)],
        [53, 56, miscItem("Helm of Telepathy", 17250, 3450, 341)],
        [57, 60, miscItem("Helm of Teleportation", 20250, 2250, 341)],
        [61, 64, miscItem("Helm of Underwater Action", 4500, 1125, 341)],
        [65, 68, miscItem("Horn of Blasting", 17500, 1750, 341)],
        [69, 72, miscItem("Horn of Fog", 4250, 425, 342)],
        [73, 76, miscItem("Horn of Goodness/Evil", 4250, 1050, 342)],
        [77, 80, miscItem("Horn of the Merfolk", 12500, 2500, 342)],
        [81, 84, miscItem("Horn of Valhalla", 32000, 0, 342, MiscMagicRolls.hornOfValhalla)],
        [85, 88, miscItem("Horseshoes of Speed", 7750, 775, 342)],
        [89, 92, miscItem("Horseshoes of the Zephyr", 9750, 975, 342)],
        [93, 96, miscItem("Instant Fortress", 22500, 2250, 342)],
        [97, 100, miscItem("Ioun Stone", 0, 0, 343, MiscMagicRolls.iounStone)],
    ],

    tableD: [
        [1, 4, miscItem("Iron Bands of Binding", 17500, 1750, 343)],
        [5, 7, miscItem("Iron Flask", 0, 0, 343, null, "No value or XP listed")],
        [8, 9, miscItem("Lady of the Lake's Gift", 15000, 7500, 343)],
        [10, 11, miscItem("Lyre of Building", 13500, 2000, 343)],
        [12, 13, miscItem("Mantle of Barb-E-Bayan", 6000, 2500, 343)],
        [14, 16, miscItem("Mantle of Spell Resistance", 200000, 20000, 343)],
        [17, 20, miscItem("Manual of Bodily Health", 0, 0, 343, SubRolls.manual, "Permanent bonus to constitution once read")],
        [21, 24, miscItem("Manual of Gainful Exercise", 0, 0, 344, SubRolls.manual, "Permanent bonus to strength once read")],
        [25, 28, miscItem("Manual of Quickness of Action", 0, 0, 344, SubRolls.manual, "Permanent bonus to dexterity once read")],
        [29, 32, miscItem("Mattock of the Titans", 11500, 1150, 344)],
        [33, 36, miscItem("Maul of the Titans", 13500, 1350, 344)],
        [37, 38, miscItem("Mead of Stuttungr", 5000, 2500, 344, null, "XP listed as 2,500 per level")],
        [39, 41, miscItem("Medallion of Thoughts", 5500, 1500, 344)],
        [42, 44, miscItem("Mirror of Life Trapping", 39000, 2300, 344)],
        [45, 48, miscItem("Mirror of Mental Prowess", 37250, 3720, 344)],
        [49, 52, miscItem("Mirror of Opposition", 25600, 2560, 344)],
        [53, 56, miscItem("Necklace of Adaptation", 4500, 450, 344)],
        [57, 60, miscItem("Necklace of Fireballs", 0, 0, 344, MiscMagicRolls.necklaceOfFireballs)],
        [61, 64, miscItem("Orb of Storms", 49500, 2500, 345)],
        [65, 68, miscItem("Pearl of Power", 0, 0, 345, MiscMagicRolls.pearlOfPower)],
        [69, 72, miscItem("Pearl of the Sirens", 13500, 1350, 345)],
        [73, 76, miscItem("Periapt of Health", 9500, 950, 345)],
        [77, 80, miscItem("Periapt of Proof Against Poison", 9500, 950, 345)],
        [81, 84, miscItem("Periapt of Wisdom", 0, 0, 345, SubRolls.plusTwoFourSix, "Enchantment bonus to wisdom while worn")],
        [85, 88, miscItem("Periapt of Wound Closure", 17000, 1000, 345)],
        [89, 92, miscItem("Phylactery of Faithfulness", 5500, 1250, 345)],
        [93, 96, miscItem("Phylactery of Undead Turning", 16000, 1600, 345)],
        [97, 100, miscItem("Pipes of the Sewers", 7500, 750, 345)],
    ],

    tableE: [
        [1, 4, miscItem("Portable Hole", 29250, 2950, 345)],
        [5, 8, miscItem("Restorative Ointment", 5500, 550, 345)],
        [9, 12, miscItem("Ring Gates", 30500, 3050, 346, null, "Value is for both rings found together. A single ring is valued at collector's market value")],
        [13, 16, miscItem("Robe of the Arch-magi", 75000, 7500, 346, MiscMagicRolls.robeOfTheArchmagi)],
        [17, 20, miscItem("Robe of Blending", 23000, 2300, 346)],
        [21, 24, miscItem("Robe of Eyes", 17500, 1750, 346)],
        [25, 28, miscItem("Robe of Scintillating Colors", 13500, 1350, 346)],
        [29, 32, miscItem("Robe of Useful Items", 14000, 1400, 346, MiscMagicRolls.robeOfUsefulItems)],
        [33, 36, miscItem("Rope of Climbing", 5750, 575, 346)],
        [37, 40, miscItem("Rope of Entanglement", 7750, 775, 346)],
        [41, 44, miscItem("Scabbard of Sharpness", 17500, 1700, 347)],
        [45, 48, miscItem("Scarab of Protection", 34000, 1200, 347)],
        [49, 52, miscItem("Shrouds of Disintegration", 1650, 165, 347)],
        [53, 56, miscItem("Slippers of Spider Climbing", 9000, 900, 347)],
        [57, 60, miscItem("Stone of Alarm", 1400, 140, 347)],
        [61, 64, miscItem("Stone of Controlling Earth Elementals", 0, 8500, 347, null, "Value listed as 10,000-60,000 gp with no rule given: Castle Keeper's call")],
        [65, 68, miscItem("Stone of Good Luck", 4250, 425, 347)],
        [69, 72, miscItem("Stone Horse", 8500, 850, 347, item => addNote(item, "Type: " + pick(["Courser (riding horse)", "Destrier (heavy warhorse)"])))],
        [73, 75, miscItem("Strand of Prayer Beads", 0, 0, 347, MiscMagicRolls.prayerBeads)],
        [76, 77, miscItem("Thunderstone", 2000, 1000, 347)],
        [78, 80, miscItem("Tome of Clear Thought", 0, 0, 348, SubRolls.plusOneToFive, "Permanent bonus to intelligence once read")],
        [81, 83, miscItem("Tome of Leadership and Influence", 0, 0, 348, SubRolls.plusOneToFive, "Permanent bonus to charisma once read")],
        [84, 85, miscItem("Tome of Raudskinna", 12000, 8000, 348)],
        [86, 88, miscItem("Tome of Understanding", 0, 0, 348, SubRolls.plusOneToFive, "Permanent bonus to wisdom once read")],
        [89, 92, miscItem("Well of Many Worlds", 40450, 2000, 348)],
        [93, 96, miscItem("Wind Fan", 5500, 550, 348)],
        [97, 100, miscItem("Wings of Flying", 15750, 1575, 348)],
    ],

    roll() {
        const subTable = rollOnTable(MiscMagic.subTables, "Table 4.5 Miscellaneous Magic");
        const tableName = "Table 4.5" + subTable.slice(-1); // tableA -> Table 4.5A
        return rollItem(MiscMagic[subTable], tableName);
    },
};
