using System;
using System.Collections.Generic;

//Tables 4.5-4.5E: Miscellaneous Magic (Monsters & Treasure pages 326-328, descriptions pages 334-348).
//Items whose value or nature depends on a further roll get a subRoll from their description.
public static class MiscMagic
{
    private static readonly Dictionary<(int, int), string> subTables = new Dictionary<(int, int), string>
    {
        { (1, 20), "4.5A" },
        { (21, 40), "4.5B" },
        { (41, 60), "4.5C" },
        { (61, 80), "4.5D" },
        { (81, 100), "4.5E" },
    };

    private static readonly Dictionary<(int, int), Item> tableA = new Dictionary<(int, int), Item>
    {
        { (1, 4), I("Amulet of Health", 0, 0, 334, SubRolls.PlusTwoFourSix, "Enchantment bonus to constitution") },
        { (5, 8), I("Amulet of Mighty Fists", 0, 0, 335, SubRolls.PlusOneToFive, "Bonus on attack and damage with unarmed attacks and natural weapons") },
        { (9, 12), I("Amulet of Natural Armor", 0, 0, 335, SubRolls.PlusOneToFive, "Natural armor bonus") },
        { (13, 16), I("Amulet of the Planes", 20250, 6750, 335) },
        { (17, 20), I("Bag of Holding", 25500, 8500, 335, BagOfHolding) },
        { (21, 24), I("Bag of Tricks", 0, 0, 335, BagOfTricks) },
        { (25, 28), I("Belt of Giant Strength", 0, 1000, 335, SubRolls.GiantStrengthBelt) },
        { (29, 32), I("Blessed Book", 112000, 0, 335, null, "Pages are empty when found. XP: 100 per spell level in the book that the wizard does not already possess") },
        { (33, 36), I("Boat, Folding", 17500, 1750, 335) },
        { (37, 40), I("Boots of Elvenkind", 13500, 3500, 336) },
        { (41, 44), I("Boots of Levitation", 11000, 2500, 336) },
        { (45, 48), I("Boots of Speed", 12750, 1275, 336) },
        { (49, 52), I("Boots of Striding and Springing", 9000, 900, 336) },
        { (53, 56), I("Boots of Teleportation", 20250, 6750, 336) },
        { (57, 60), I("Boots of the Winterlands", 9500, 950, 336) },
        { (61, 64), I("Bowl of Commanding Water Elementals", 25500, 8500, 336) },
        { (65, 68), I("Bracers of Armor", 0, 0, 336, SubRolls.PlusOneToFive, "Armor bonus") },
        { (69, 72), I("Bracers of Deflection", 13500, 4500, 336) },
        { (73, 76), I("Brazier of Commanding Fire Elementals", 25000, 8500, 336) },
        { (77, 80), I("Brooch of Shielding", 11500, 1150, 336) },
        { (81, 84), I("Broom of Flying", 12750, 4250, 336) },
        { (85, 88), I("Candle of Invocation", 22000, 7300, 336) },
        { (89, 92), I("Cape of the Mountebank", 16000, 1600, 336) },
        { (93, 96), I("Carpet of Flying", 17250, 5750, 336) },
        { (97, 100), I("Censer of Controlling Air Elementals", 25000, 8500, 336) },
    };

    private static readonly Dictionary<(int, int), Item> tableB = new Dictionary<(int, int), Item>
    {
        { (1, 4), I("Chime of Interruption", 13500, 1350, 337) },
        { (5, 8), I("Chime of Opening", 10500, 1050, 337) },
        { (9, 12), I("Circlet of Persuasion", 11250, 1125, 337) },
        { (13, 16), I("Cloak of Arachnida", 17500, 1750, 337) },
        { (17, 20), I("Cloak of the Bat", 13500, 3000, 337) },
        { (21, 24), I("Cloak of Charisma", 0, 0, 337, SubRolls.PlusTwoFourSix, "Bonus to all charisma based checks") },
        { (25, 28), I("Cloak of Displacement", 17000, 1700, 337) },
        { (29, 32), I("Cloak of Elvenkind", 15500, 3000, 337) },
        { (33, 36), I("Cloak of Etherealness", 11250, 2500, 337) },
        { (37, 40), I("Cloak of the Manta Ray", 9000, 2500, 337) },
        { (41, 44), I("Cloak of Resistance", 0, 0, 337, SubRolls.PlusOneToFive, "Enchantment bonus on all saving throws") },
        { (45, 46), I("Coin, Sampo", 10000, 2750, 337) },
        { (47, 48), I("Crystal Ball", 0, 5000, 337, CrystalBall) },
        { (49, 52), I("Cube of Force", 47250, 4000, 338) },
        { (53, 56), I("Cube of Frost Resistance", 12750, 1270, 338) },
        { (57, 60), I("Cubic Gate", 24750, 4500, 338) },
        { (61, 64), I("Decanter of Endless Water", 13500, 1350, 338) },
        { (65, 66), I("Dragon's Teeth", 7000, 12000, 338) },
        { (67, 68), I("Drums of Panic", 4750, 475, 338) },
        { (69, 72), I("Dust of Appearance", 8500, 850, 338) },
        { (73, 76), I("Dust of Disappearance", 5500, 550, 338) },
        { (77, 80), I("Dust of Illusion", 5500, 550, 339) },
        { (81, 84), I("Efficient Quiver", 8500, 850, 339) },
        { (85, 88), I("Efreeti Bottle", 47250, 4700, 339) },
        { (89, 92), I("Elemental Gem", 20250, 2000, 339, item => item.AddNote("Elemental type: " + CommonUtils.Pick("Air (transparent)", "Earth (light brown)", "Fire (reddish orange)", "Water (blue-green)"))) },
        { (93, 96), I("Eversmoking Bottle", 4250, 425, 339) },
        { (97, 100), I("Eyes of Doom", 0, 7, 339, null, "Value: free round at the House of Sludge (as listed in the book)") },
    };

    private static readonly Dictionary<(int, int), Item> tableC = new Dictionary<(int, int), Item>
    {
        { (1, 3), I("Eyes of the Eagle", 4250, 425, 339) },
        { (4, 7), I("Figurines of Wondrous Power", 0, 0, 339, Figurine) },
        { (8, 11), I("Feather Token", 4500, 450, 339, item => item.AddNote("Token: " + CommonUtils.Pick("Anchor", "Bird", "Fan", "Swan Boat", "Tree", "Whip"))) },
        { (12, 13), I("Fruit of the Lotus Tree", 10000, 5000, 340) },
        { (14, 16), I("Gauntlets of Ogre Power", 9500, 950, 340) },
        { (17, 20), I("Gem of Brightness", 17750, 2000, 340) },
        { (21, 24), I("Gem of Seeing", 15250, 1500, 341) },
        { (25, 28), I("Gloves of Dexterity", 0, 0, 341, SubRolls.PlusOneToFive, "Enchantment bonus to dexterity") },
        { (29, 32), I("Gloves of Swimming and Climbing", 9000, 900, 341) },
        { (33, 36), I("Handy Haversack", 12250, 1225, 341) },
        { (37, 40), I("Harp of Charming", 7750, 2000, 341) },
        { (41, 44), I("Hat of Disguise", 5500, 550, 341) },
        { (45, 48), I("Helm of Brilliance", 34750, 3000, 341) },
        { (49, 52), I("Helm of Comprehend Language & Read Magic", 4250, 425, 341) },
        { (53, 56), I("Helm of Telepathy", 17250, 3450, 341) },
        { (57, 60), I("Helm of Teleportation", 20250, 2250, 341) },
        { (61, 64), I("Helm of Underwater Action", 4500, 1125, 341) },
        { (65, 68), I("Horn of Blasting", 17500, 1750, 341) },
        { (69, 72), I("Horn of Fog", 4250, 425, 342) },
        { (73, 76), I("Horn of Goodness/Evil", 4250, 1050, 342) },
        { (77, 80), I("Horn of the Merfolk", 12500, 2500, 342) },
        { (81, 84), I("Horn of Valhalla", 32000, 0, 342, HornOfValhalla) },
        { (85, 88), I("Horseshoes of Speed", 7750, 775, 342) },
        { (89, 92), I("Horseshoes of the Zephyr", 9750, 975, 342) },
        { (93, 96), I("Instant Fortress", 22500, 2250, 342) },
        { (97, 100), I("Ioun Stone", 0, 0, 343, IounStone) },
    };

    private static readonly Dictionary<(int, int), Item> tableD = new Dictionary<(int, int), Item>
    {
        { (1, 4), I("Iron Bands of Binding", 17500, 1750, 343) },
        { (5, 7), I("Iron Flask", 0, 0, 343, null, "No value or XP listed") },
        { (8, 9), I("Lady of the Lake's Gift", 15000, 7500, 343) },
        { (10, 11), I("Lyre of Building", 13500, 2000, 343) },
        { (12, 13), I("Mantle of Barb-E-Bayan", 6000, 2500, 343) },
        { (14, 16), I("Mantle of Spell Resistance", 200000, 20000, 343) },
        { (17, 20), I("Manual of Bodily Health", 0, 0, 343, SubRolls.Manual, "Permanent bonus to constitution once read") },
        { (21, 24), I("Manual of Gainful Exercise", 0, 0, 344, SubRolls.Manual, "Permanent bonus to strength once read") },
        { (25, 28), I("Manual of Quickness of Action", 0, 0, 344, SubRolls.Manual, "Permanent bonus to dexterity once read") },
        { (29, 32), I("Mattock of the Titans", 11500, 1150, 344) },
        { (33, 36), I("Maul of the Titans", 13500, 1350, 344) },
        { (37, 38), I("Mead of Stuttungr", 5000, 2500, 344, null, "XP listed as 2,500 per level") },
        { (39, 41), I("Medallion of Thoughts", 5500, 1500, 344) },
        { (42, 44), I("Mirror of Life Trapping", 39000, 2300, 344) },
        { (45, 48), I("Mirror of Mental Prowess", 37250, 3720, 344) },
        { (49, 52), I("Mirror of Opposition", 25600, 2560, 344) },
        { (53, 56), I("Necklace of Adaptation", 4500, 450, 344) },
        { (57, 60), I("Necklace of Fireballs", 0, 0, 344, NecklaceOfFireballs) },
        { (61, 64), I("Orb of Storms", 49500, 2500, 345) },
        { (65, 68), I("Pearl of Power", 0, 0, 345, PearlOfPower) },
        { (69, 72), I("Pearl of the Sirens", 13500, 1350, 345) },
        { (73, 76), I("Periapt of Health", 9500, 950, 345) },
        { (77, 80), I("Periapt of Proof Against Poison", 9500, 950, 345) },
        { (81, 84), I("Periapt of Wisdom", 0, 0, 345, SubRolls.PlusTwoFourSix, "Enchantment bonus to wisdom while worn") },
        { (85, 88), I("Periapt of Wound Closure", 17000, 1000, 345) },
        { (89, 92), I("Phylactery of Faithfulness", 5500, 1250, 345) },
        { (93, 96), I("Phylactery of Undead Turning", 16000, 1600, 345) },
        { (97, 100), I("Pipes of the Sewers", 7500, 750, 345) },
    };

    private static readonly Dictionary<(int, int), Item> tableE = new Dictionary<(int, int), Item>
    {
        { (1, 4), I("Portable Hole", 29250, 2950, 345) },
        { (5, 8), I("Restorative Ointment", 5500, 550, 345) },
        { (9, 12), I("Ring Gates", 30500, 3050, 346, null, "Value is for both rings found together. A single ring is valued at collector's market value") },
        { (13, 16), I("Robe of the Arch-magi", 75000, 7500, 346, RobeOfTheArchmagi) },
        { (17, 20), I("Robe of Blending", 23000, 2300, 346) },
        { (21, 24), I("Robe of Eyes", 17500, 1750, 346) },
        { (25, 28), I("Robe of Scintillating Colors", 13500, 1350, 346) },
        { (29, 32), I("Robe of Useful Items", 14000, 1400, 346, RobeOfUsefulItems) },
        { (33, 36), I("Rope of Climbing", 5750, 575, 346) },
        { (37, 40), I("Rope of Entanglement", 7750, 775, 346) },
        { (41, 44), I("Scabbard of Sharpness", 17500, 1700, 347) },
        { (45, 48), I("Scarab of Protection", 34000, 1200, 347) },
        { (49, 52), I("Shrouds of Disintegration", 1650, 165, 347) },
        { (53, 56), I("Slippers of Spider Climbing", 9000, 900, 347) },
        { (57, 60), I("Stone of Alarm", 1400, 140, 347) },
        { (61, 64), I("Stone of Controlling Earth Elementals", 0, 8500, 347, null, "Value listed as 10,000-60,000 gp with no rule given: Castle Keeper's call") },
        { (65, 68), I("Stone of Good Luck", 4250, 425, 347) },
        { (69, 72), I("Stone Horse", 8500, 850, 347, item => item.AddNote("Type: " + CommonUtils.Pick("Courser (riding horse)", "Destrier (heavy warhorse)"))) },
        { (73, 75), I("Strand of Prayer Beads", 0, 0, 347, PrayerBeads) },
        { (76, 77), I("Thunderstone", 2000, 1000, 347) },
        { (78, 80), I("Tome of Clear Thought", 0, 0, 348, SubRolls.PlusOneToFive, "Permanent bonus to intelligence once read") },
        { (81, 83), I("Tome of Leadership and Influence", 0, 0, 348, SubRolls.PlusOneToFive, "Permanent bonus to charisma once read") },
        { (84, 85), I("Tome of Raudskinna", 12000, 8000, 348) },
        { (86, 88), I("Tome of Understanding", 0, 0, 348, SubRolls.PlusOneToFive, "Permanent bonus to wisdom once read") },
        { (89, 92), I("Well of Many Worlds", 40450, 2000, 348) },
        { (93, 96), I("Wind Fan", 5500, 550, 348) },
        { (97, 100), I("Wings of Flying", 15750, 1575, 348) },
    };

    public static Item Roll()
    {
        string subTable = CommonUtils.RollOnTable(subTables, "Table 4.5 Miscellaneous Magic");
        switch (subTable)
        {
            case "4.5A": return CommonUtils.RollItem(tableA, "Table 4.5A");
            case "4.5B": return CommonUtils.RollItem(tableB, "Table 4.5B");
            case "4.5C": return CommonUtils.RollItem(tableC, "Table 4.5C");
            case "4.5D": return CommonUtils.RollItem(tableD, "Table 4.5D");
            default: return CommonUtils.RollItem(tableE, "Table 4.5E");
        }
    }

    private static Item I(string name, int gp, int xp, int page, Action<Item> subRoll = null, string description = "")
    {
        return new Item("'" + name + "'", gp, xp, description, page) { subRoll = subRoll };
    }

    #region Sub-rolls from item descriptions

    private static void BagOfHolding(Item item)
    {
        string[] types = { "Type I: weighs 15 lb., holds 250 lb. (market price 2,500 gp)", "Type II: weighs 25 lb., holds 500 lb. (market price 5,000 gp)",
            "Type III: weighs 35 lb., holds 1,000 lb. (market price 7,400 gp)", "Type IV: weighs 60 lb., holds 1,500 lb. (market price 10,000 gp)" };
        item.AddNote("Bag " + CommonUtils.Pick(types));
    }

    private static void BagOfTricks(Item item)
    {
        (string color, int gp, int xp, string animals)[] bags =
        {
            ("Gray", 1000, 300, "bat, rat, cat, weasel, badger"),
            ("Rust", 4000, 1250, "wolverine, wolf, boar, black bear"),
            ("Tan", 16000, 3000, "brown bear, lion, heavy warhorse, tiger, rhinoceros"),
        };
        var bag = CommonUtils.Pick(bags);
        item.gp = bag.gp;
        item.xp = bag.xp;
        item.AddNote(bag.color + " colored bag (animals: " + bag.animals + ")");
    }

    private static void CrystalBall(Item item)
    {
        (string type, int gp)[] balls = { ("Crystal ball", 42000), ("Crystal ball with see invisibility", 50000), ("Crystal ball with detect thoughts", 51000),
            ("Crystal ball with telepathic bond", 70000), ("Crystal ball with true seeing", 80000) };
        var ball = CommonUtils.Pick(balls);
        item.gp = ball.gp;
        item.AddNote("Type: " + ball.type + " (market price)");
    }

    private static void Figurine(Item item)
    {
        (string name, int gp, int xp)[] figurines = { ("Bronze Griffon", 10000, 2000), ("Ebony Fly", 10000, 2000), ("Golden Lions (pair)", 16500, 3300),
            ("Ivory Goats (three)", 21000, 4000), ("Marble Elephant", 17000, 3400), ("Obsidian Steed", 28500, 5700), ("Onyx Dog", 15500, 3100),
            ("Serpentine Owl", 9100, 1820), ("Silver Raven", 3800, 760) };
        var figurine = CommonUtils.Pick(figurines);
        item.gp = figurine.gp;
        item.xp = figurine.xp;
        item.AddNote("Figurine: " + figurine.name);
    }

    //d%: 01-40 silver, 41-75 brass, 76-90 bronze, 91-100 iron. XP 1,250 "doubles with each horn"
    private static void HornOfValhalla(Item item)
    {
        int roll = CommonUtils.RollPercentage();
        (string type, int xp, string summons) horn =
            roll <= 40 ? ("Silver", 1250, "2d4+2 2nd level barbarians. Prerequisite: none") :
            roll <= 75 ? ("Brass", 2500, "2d4+1 3rd level barbarians. Prerequisite: spellcaster level 1st") :
            roll <= 90 ? ("Bronze", 5000, "2d4 4th level barbarians. Prerequisite: proficiency with all weapons or bardic music ability") :
                         ("Iron", 10000, "1d4+1 5th level barbarians. Prerequisite: proficiency with all weapons or bardic music ability");
        item.xp = horn.xp;
        item.AddNote(horn.type + " horn: summons " + horn.summons);
    }

    private static void IounStone(Item item)
    {
        (string stone, int gp, int xp)[] stones =
        {
            ("Clear spindle: sustains creature without food or water", 4000, 500),
            ("Dusty rose prism: +1 bonus to AC", 5000, 500),
            ("Deep red sphere: +2 enchantment bonus to dexterity", 8000, 1000),
            ("Incandescent blue sphere: +2 enchantment bonus to wisdom", 8000, 1000),
            ("Pale blue rhomboid: +2 enchantment bonus to strength", 8000, 1000),
            ("Pink rhomboid: +2 enchantment bonus to constitution", 8000, 1000),
            ("Pink & green sphere: +2 enchantment bonus to charisma", 8000, 1000),
            ("Scarlet & blue sphere: +2 enchantment bonus to intelligence", 8000, 1000),
            ("Dark blue rhomboid: alertness (+2 to wisdom attribute checks)", 10000, 1000),
            ("Vibrant purple prism: stores three levels of spells, as a ring of spell storing", 36000, 1500),
            ("Iridescent spindle: sustains creature without air", 18000, 1000),
            ("Pale lavender ellipsoid: absorbs spells of 4th level or lower", 20000, 2000),
            ("Pearly white spindle: regenerate 1 point of damage per hour", 20000, 1000),
        };
        var stone = CommonUtils.Pick(stones);
        item.gp = stone.gp;
        item.xp = stone.xp;
        item.AddNote("Stone: " + stone.stone);
    }

    //Market price is 150 gp per die of damage. The book lists no XP value.
    private static void NecklaceOfFireballs(Item item)
    {
        (string type, int gp, string spheres)[] necklaces =
        {
            ("Type I", 1650, "1x 5d6, 2x 3d6"),
            ("Type II", 2700, "1x 6d6, 2x 4d6, 2x 2d6"),
            ("Type III", 4350, "1x 7d6, 2x 5d6, 4x 3d6"),
            ("Type IV", 5400, "1x 8d6, 2x 6d6, 2x 4d6, 4x 2d6"),
            ("Type V", 5850, "1x 9d6, 2x 7d6, 2x 5d6, 2x 3d6"),
            ("Type VI", 8100, "1x 10d6, 2x 8d6, 2x 6d6, 4x 4d6"),
            ("Type VII", 8700, "1x 10d6, 2x 9d6, 2x 7d6, 2x 5d6, 2x 3d6"),
        };
        var necklace = CommonUtils.Pick(necklaces);
        item.gp = necklace.gp;
        item.AddNote(necklace.type + " spheres: " + necklace.spheres + ". No XP value listed");
    }

    //Value 1,000 gp x spell level squared, XP 1,000 per spell level
    private static void PearlOfPower(Item item)
    {
        int level = CommonUtils.RollDie(9);
        item.gp = 1000 * level * level;
        item.xp = 1000 * level;
        item.AddNote("Recalls one spell of level " + level + " per day");
    }

    private static void RobeOfTheArchmagi(Item item)
    {
        int roll = CommonUtils.RollPercentage();
        string color = roll <= 45 ? "White (good)" : roll <= 75 ? "Gray (neither good nor evil)" : "Black (evil)";
        item.AddNote("Color: " + color);
    }

    private static readonly Dictionary<(int, int), string> usefulPatches = new Dictionary<(int, int), string>
    {
        { (1, 8), "Bag of 100 gold pieces" },
        { (9, 15), "Coffer, silver (500 gp value)" },
        { (16, 22), "Door, iron" },
        { (23, 30), "Gems, x10 (100 gp value each)" },
        { (31, 44), "Ladder, wooden (24 ft.)" },
        { (45, 51), "Mule (with saddle bags)" },
        { (52, 59), "Pit, open (10 ft. cube)" },
        { (60, 68), "Potion of cure serious wounds" },
        { (69, 75), "Row boat (12 ft.)" },
        { (76, 83), "Minor scroll of one randomly determined spell" },
        { (84, 90), "Dogs, pair (riding dogs)" },
        { (91, 96), "Window (2 ft. by 4 ft.)" },
        { (97, 100), "Portable ram" },
    };

    //Two each of the standard patches, plus 4d4 more rolled on the patch table
    private static void RobeOfUsefulItems(Item item)
    {
        item.AddNote("Standard patches (2 each): dagger, bullseye lantern, mirror, 10 ft. pole, 50 ft. hemp rope, large sack");
        int extra = new Roll(4, 4).RollXdY();
        var counts = new SortedDictionary<string, int>();
        for (int i = 0; i < extra; i++)
        {
            string patch = CommonUtils.RollOnTable(usefulPatches, "Robe of Useful Items patches");
            counts[patch] = counts.TryGetValue(patch, out int n) ? n + 1 : 1;
        }
        var parts = new List<string>();
        foreach (var patch in counts) parts.Add(patch.Value + "x " + patch.Key);
        item.AddNote(extra + " extra patches: " + string.Join("; ", parts));
    }

    private static void PrayerBeads(Item item)
    {
        string strand = CommonUtils.Pick(
            "Lesser strand: bead of blessing, bead of healing",
            "Strand: bead of healing, bead of karma, bead of smiting",
            "Greater strand: bead of healing, bead of karma, bead of summons, bead of wind walking");
        item.AddNote(strand + ". Value and XP: see text");
    }

    #endregion
}
