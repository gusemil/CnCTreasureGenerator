"use strict";

// Shared rolls used by several magic item tables: bonuses, sizes, charges and item-specific
// sub-tables from the item descriptions (Monsters & Treasure pages 332-364). (SubRolls.cs)
const SubRolls = {
    // Table 4.3B Weapon Bonus and Table 4.4E Armor Bonus (identical tables)
    bonusTable: [
        [1, 45, { bonus: 1, gp: 1000, xp: 250 }],
        [46, 75, { bonus: 2, gp: 4000, xp: 750 }],
        [76, 90, { bonus: 3, gp: 9000, xp: 1200 }],
        [91, 98, { bonus: 4, gp: 16000, xp: 1750 }],
        [99, 100, { bonus: 5, gp: 25000, xp: 2500 }],
    ],

    rollBonus(tableName = "Table 4.3B/4.4E Bonus") {
        const entry = rollOnTable(SubRolls.bonusTable, tableName);
        log("Bonus is +" + entry.bonus);
        return entry;
    },

    bonusFor(bonus) {
        const found = SubRolls.bonusTable.find(entry => entry[2].bonus === bonus);
        if (!found) throw new Error("No bonus entry for +" + bonus);
        return found[2];
    },

    // Footnote value scale for miscellaneous magic with a +1 to +6 bonus (Table 4.5A-E)
    miscBonusGp(bonus) {
        return [1000, 4000, 8000, 16000, 25000, 36000][bonus - 1];
    },

    setBonus(item, bonus, gp, xp) {
        item.name = item.name + " +" + bonus;
        item.gp = gp;
        item.xp = xp;
    },

    // Items with "+1 to +5" in their description. The bonus is rolled on Table 4.3B.
    plusOneToFive(item) {
        const bonus = SubRolls.rollBonus().bonus;
        SubRolls.setBonus(item, bonus, SubRolls.miscBonusGp(bonus), 500 * bonus);
    },

    // Items with "+2, +4 or +6" in their description (equal chance)
    plusTwoFourSix(item) {
        const bonus = pick([2, 4, 6]);
        log("Bonus is +" + bonus);
        SubRolls.setBonus(item, bonus, SubRolls.miscBonusGp(bonus), 500 * bonus);
    },

    // Manuals: "+1 to +5", listed at 10,000-60,000 gp
    manual(item) {
        const bonus = SubRolls.rollBonus().bonus;
        SubRolls.setBonus(item, bonus, 10000 * bonus, 500 * bonus);
        addNote(item, "Value scaled at 10,000 gp per bonus point (book lists 10,000-60,000 gp)");
    },

    // "Armor and Weapon Sizes" (page 313): 01-30 small, 31-90 medium, 91-100 large
    rollSize(item) {
        const roll = rollPercentage();
        const size = roll <= 30 ? "Small" : roll <= 90 ? "Medium" : "Large";
        addNote(item, "Size: " + size);
    },

    // Rods, staves and wands have 50 charges when created, "though the number can be far less when actually discovered"
    rollCharges(item) {
        const charges = rollDie(50);
        addNote(item, "Charges remaining: " + charges + " (50 when created)");
    },

    // Potion of Giant Strength (page 330)
    giantStrengthPotion(item) {
        const giants = ["Hill: Str 19 (+3)", "Stone: Str 20 (+4)", "Frost: Str 21 (+4)", "Fire: Str 22 (+5)", "Cloud: Str 23 (+5)", "Storm: Str 24 (+6)"];
        const roll = rollDie(6);
        addNote(item, "Giant type (d6 = " + roll + "): " + giants[roll - 1]);
    },

    // Belt of Giant Strength (page 335). Value 10,000-60,000 gp scales with the giant type.
    giantStrengthBelt(item) {
        const giants = ["Hill: Str 19 (+3)", "Frost: Str 20 (+4)", "Fire: Str 21 (+4)", "Stone: Str 22 (+5)", "Cloud: Str 23 (+5)", "Storm: Str 24 (+6)"];
        const roll = rollDie(6);
        item.gp = 10000 * roll;
        addNote(item, "Giant type (d6 = " + roll + "): " + giants[roll - 1]);
    },

    // Luck Blade: "5% chance to contain 1-3 wishes"
    luckBladeWishes(item) {
        if (rollPercentage() <= 5) addNote(item, "Contains " + rollDie(3) + " wish(es)");
        else addNote(item, "Contains no wishes");
    },
};
