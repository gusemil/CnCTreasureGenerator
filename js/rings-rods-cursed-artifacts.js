"use strict";

// The smaller magic item tables: Table 4.6 Rings, Table 4.7 Rods, Staves and Wands,
// Table 4.8 Cursed Items and Table 4.9 Artifacts.
// (Rings.cs, RodsStavesWands.cs, CursedItems.cs and Artifacts.cs)

// ---------- Table 4.6: Rings (Monsters & Treasure page 328, descriptions pages 348-351) ----------

function ring(name, gp, xp, page, subRoll = null) {
    return makeItem("Ring of " + name, gp, xp, { page: page, subRoll: subRoll });
}

const Rings = {
    table: [
        [1, 4, ring("Animal Friendship", 1650, 330, 348)],
        [5, 8, ring("Blinking", 9800, 1960, 348)],
        [9, 12, ring("Chameleon Power", 5800, 1160, 349)],
        [13, 16, ring("Climbing", 2800, 560, 349)],
        [17, 20, ring("Counter Spells", 5050, 1010, 349)],
        [21, 21, ring("Elemental Command", 45000, 9000, 349, item => addNote(item, "Element: " + pick(["Air", "Earth", "Fire", "Water"])))],
        [22, 25, ring("Energy Resistance", 25000, 5000, 349, item => addNote(item, "Energy type: " + pick(["Acid", "Cold", "Electricity", "Fire"]) + ". Minor/major/greater strength is the Castle Keeper's choice"))],
        [26, 29, ring("Evasion", 20000, 5000, 350)],
        [30, 33, ring("Feather Falling", 1550, 310, 350)],
        [34, 37, ring("Force Shield", 5050, 1010, 350)],
        [38, 41, ring("Freedom of Movement", 8300, 1660, 350)],
        [42, 44, ring("Friend Shield", 6050, 1210, 350)],
        [45, 45, ring("Gyges", 5800, 1200, 350)],
        [46, 46, ring("Invisibility", 5800, 1160, 350)],
        [47, 50, ring("Jumping", 1550, 310, 350)],
        [51, 51, makeItem("Lancelot's Ring", 8000, 3000, { page: 350 })],
        [52, 54, ring("Mind Shielding", 5800, 1160, 350)],
        [55, 58, ring("Protection", 0, 0, 350, item => Rings.ringOfProtection(item))],
        [59, 62, ring("the Ram", 5800, 1160, 350)],
        [63, 63, ring("Regeneration", 24050, 4810, 350)],
        [64, 67, ring("Shooting Stars", 9300, 1860, 350)],
        [68, 71, ring("Spell Storing", 0, 0, 351, item => Rings.spellStoring(item))],
        [72, 75, ring("Spell Turning", 41800, 8360, 351)],
        [76, 79, ring("Sustenance", 2800, 560, 351)],
        [80, 83, ring("Swimming", 1300, 260, 351)],
        [84, 87, ring("Telekinesis", 12550, 2510, 351)],
        [88, 88, ring("Three Wishes", 114750, 5000, 351, item => addNote(item, "Rubies (wishes) remaining: " + rollDie(3)))],
        [89, 92, ring("Water Walking", 5800, 1160, 351)],
        [93, 96, ring("Wizardry", 0, 0, 351, item => Rings.wizardry(item))],
        [97, 100, ring("X-Ray Vision", 9300, 930, 351)],
    ],

    roll() {
        return rollItem(Rings.table, "Table 4.6 Rings");
    },

    // Bonus rolled on Table 4.3B. XP is 20% of the value.
    ringOfProtection(item) {
        const bonus = SubRolls.rollBonus().bonus;
        const gp = [2300, 5300, 11300, 17300, 26300];
        item.name += " +" + bonus;
        item.gp = gp[bonus - 1];
        item.xp = Math.floor(item.gp / 5);
    },

    spellStoring(item) {
        const ring = pick([
            { type: "Minor", gp: 5050, levels: "3 levels of spells" },
            { type: "Standard", gp: 12550, levels: "5 levels of spells" },
            { type: "Major", gp: 38000, levels: "10 levels of spells" },
        ]);
        item.name = item.name.replace("Ring of", ring.type + " Ring of");
        item.gp = ring.gp;
        item.xp = Math.floor(ring.gp / 5);
        addNote(item, ring.type + " ring: stores up to " + ring.levels);
    },

    wizardry(item) {
        const ring = pick([
            { type: "Minor", level: 1, gp: 20000 },
            { type: "Major", level: 2, gp: 40000 },
            { type: "Greater", level: 3, gp: 70000 },
            { type: "Arch", level: 4, gp: 100000 },
        ]);
        item.name += " (" + ring.type + ")";
        item.gp = ring.gp;
        item.xp = 1000 * ring.level;
        addNote(item, "Doubles the wearer's level " + ring.level + " spells");
    },
};

// ---------- Table 4.7: Rods, Staves and Wands (page 328, descriptions pages 351-355) ----------
// All of them have 50 charges when created, often fewer when found.

function chargedItem(name, gp, xp, page) {
    return makeItem(name, gp, xp, { page: page, subRoll: SubRolls.rollCharges });
}

const RodsStavesWands = {
    table: [
        [1, 4, chargedItem("Rod of Absorption", 50000, 8300, 351)],
        [5, 5, chargedItem("Rod of Cancellation", 13500, 2250, 352)],
        [6, 6, chargedItem("Rod of Lordly Might", 20500, 3400, 352)],
        [7, 10, chargedItem("Rod of Negation", 9500, 1500, 352)],
        [11, 14, chargedItem("Rod of the Python", 8750, 1400, 352)],
        [15, 18, chargedItem("Rod of Rulership", 20000, 3200, 353)],
        [19, 22, chargedItem("Rod of Thunder & Lightning", 29000, 4800, 353)],
        [23, 26, chargedItem("Rod of Withering", 25000, 4000, 353)],
        [27, 30, chargedItem("Rod of Wonder", 13500, 2250, 353)],
        [31, 34, chargedItem("Staff of Abjuration", 54000, 9000, 354)],
        [35, 38, chargedItem("Staff of Conjuration", 47000, 7800, 354)],
        [39, 42, chargedItem("Staff of Evocation", 47000, 7880, 354)],
        [43, 46, chargedItem("Staff of Fire", 23000, 3800, 354)],
        [47, 50, chargedItem("Staff of Frost", 32500, 5400, 354)],
        [51, 54, chargedItem("Staff of Healing", 31000, 5100, 354)],
        [55, 58, chargedItem("Staff of Power", 72000, 12000, 354)],
        [59, 59, chargedItem("Staff of Resurrection", 41000, 6800, 354)],
        [60, 63, chargedItem("Staff of the Woodlands", 42000, 7000, 354)],
        [64, 67, chargedItem("Wand of Color Spray", 12250, 2000, 355)],
        [68, 71, chargedItem("Wand of Darkness", 12250, 2000, 355)],
        [72, 75, chargedItem("Wand of Dispel Magic", 16750, 2800, 355)],
        [76, 79, chargedItem("Wand of Illusion", 22000, 3600, 355)],
        [80, 80, chargedItem("Wand of Invisibility", 31000, 5000, 355)],
        [81, 84, chargedItem("Wand of Levitation", 26000, 4300, 355)],
        [85, 88, chargedItem("Wand of Magic Missile", 12250, 2000, 355)],
        [89, 92, chargedItem("Wand of Polymorph", 23000, 3800, 355)],
        [93, 96, chargedItem("Wand of Restoration", 18000, 3000, 355)],
        [97, 100, chargedItem("Wand of Suggestion", 14500, 2400, 355)],
    ],

    roll() {
        return rollItem(RodsStavesWands.table, "Table 4.7 Rods, Staves and Wands");
    },
};

// ---------- Table 4.8: Cursed Items (page 329, descriptions pages 355-357). No value or XP. ----------

function cursedItem(name, page) {
    return makeItem(name + " (cursed)", 0, 0, { page: page });
}

const CursedItems = {
    table: [
        [1, 4, cursedItem("Amulet of Inescapable Location", 355)],
        [5, 8, cursedItem("Armor of Rage", 355)],
        [9, 12, cursedItem("Bag of Devouring", 355)],
        [13, 16, cursedItem("Boots of Dancing", 355)],
        [17, 20, cursedItem("Bracers of Defenselessness", 355)],
        [21, 24, cursedItem("Flask of Curses", 355)],
        [25, 28, cursedItem("Gauntlets of Fumbling", 355)],
        [29, 32, cursedItem("Helm of Opposite Alignment", 355)],
        [33, 36, cursedItem("Incense of Obsession", 356)],
        [37, 40, cursedItem("Mace of Blood", 356)],
        [41, 44, cursedItem("Medallion of Thought Projection", 356)],
        [45, 48, cursedItem("Necklace of Strangulation", 356)],
        [49, 52, cursedItem("Net of Snaring", 356)],
        [53, 56, cursedItem("Periapt of Foul Rotting", 356)],
        [57, 60, cursedItem("Potion of Poison", 356)],
        [61, 64, cursedItem("Robe of Powerlessness", 356)],
        [65, 68, cursedItem("Robe of Vermin", 356)],
        [69, 72, cursedItem("Ring of Clumsiness", 356)],
        [73, 76, cursedItem("Scarab of Death", 357)],
        [77, 80, cursedItem("Spear, Cursed Backbiter", 357)],
        [81, 84, cursedItem("Stone of Weight", 357)],
        [85, 88, cursedItem("-2 Cursed Sword", 357)],
        [89, 92, cursedItem("Sword, Berserking", 357)],
        [93, 96, cursedItem("Vacuous Grimoire", 357)],
        [97, 100, cursedItem("Zane's Ire", 357)],
    ],

    roll() {
        return rollItem(CursedItems.table, "Table 4.8 Cursed Items");
    },
};

// ---------- Table 4.9: Artifacts (page 329, descriptions pages 357-364) ----------
// Artifacts are priceless and give no XP. They count as over any max XP limit on Table 1.0.

function artifact(name, page) {
    return makeItem(name + " (artifact)", 0, 0, { page: page, priceless: true });
}

const Artifacts = {
    table: [
        [1, 3, artifact("Aegis", 357)],
        [4, 5, artifact("Andvaranaut", 357)],
        [6, 8, artifact("Angurvadal", 357)],
        [9, 10, artifact("Book of Infinite Spells", 357)],
        [11, 13, artifact("Caduceus", 358)],
        [14, 15, artifact("Carnwennan", 358)],
        [16, 18, artifact("Chentu", 358)],
        [19, 20, artifact("Cowl of the Black Fox", 358)],
        [21, 23, artifact("Deck of Many Things", 358)],
        [24, 25, artifact("Excalibur", 360)],
        [26, 28, artifact("Fail-Not Bow of Tristan", 360)],
        [29, 31, artifact("Hammer of Thunderbolts", 360)],
        [32, 35, artifact("Hand of Glory", 360)],
        [36, 37, artifact("Jarrod's Blade", 360)],
        [38, 40, artifact("Kladenets", 360)],
        [41, 42, artifact("Lantern of Diogenes", 360)],
        [43, 45, artifact("Mace of the Dove", 361)],
        [46, 47, artifact("Mantle of Lorenzo", 361)],
        [48, 50, artifact("Mjolnir", 361)],
        [51, 53, artifact("Moaning Diamond", 361)],
        [54, 55, artifact("Orbs of Dragonkind", 361)],
        [56, 58, artifact("Philosopher's Stone", 361)],
        [59, 60, artifact("Ruji Jingu Bang", 362)],
        [61, 63, artifact("Saint's Mace", 362)],
        [64, 65, artifact("Shadowstaff", 362)],
        [66, 68, artifact("Sharanga", 362)],
        [69, 70, artifact("Shield of Maccabee", 362)],
        [71, 73, artifact("Shield of the Sun", 362)],
        [74, 76, artifact("Sphere of Annihilation", 362)],
        [77, 78, artifact("Staff of the Magi", 363)], // Book lists 76-78, overlapping the Sphere of Annihilation
        [79, 80, artifact("Sword of Conaire Mor", 363)],
        [81, 83, artifact("Sword of Tarnhelm", 363)],
        [84, 86, artifact("Talisman of Pure Good", 363)],
        [87, 89, artifact("Talisman of the Sphere", 363)],
        [90, 92, artifact("Talisman of Ultimate Evil", 364)],
        [93, 94, artifact("Tarnhelm", 364)],
        [95, 97, artifact("Tome of the Unclean", 364)],
        [98, 98, artifact("Tyrfing", 364)],
        [99, 99, artifact("Uchide No Kozuchi", 364)],
        [100, 100, artifact("Varunastra", 364)],
    ],

    roll() {
        return rollItem(Artifacts.table, "Table 4.9 Artifacts");
    },
};
