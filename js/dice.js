"use strict";

// Shared helpers: dice, table lookups, items and the value table.
// (CommonUtils.cs, Roll.cs, Item.cs, MagicItem.cs and ValueTable.cs in the C# version.)
//
// Tables are arrays of [low, high, entry]: the entry is picked when the roll is between low and high.

// ---------- Roll log (the terminal app printed every roll to the console) ----------

const RollLog = {
    lines: [],
    clear() { this.lines = []; },
    text() { return this.lines.join("\n"); },
};

function log(text = "") {
    RollLog.lines.push(String(text));
}

function pageBreak() {
    log("");
    log("-------------------------------------------------");
    log("");
}

// ---------- Dice ----------

// A single die roll, 1 to sides, without logging
function rollDie(sides) {
    return Math.floor(Math.random() * sides) + 1;
}

function rollNumber(min, max, modifier = 0) {
    const roll = min + Math.floor(Math.random() * (max - min + 1)) + modifier;
    log("Rolling " + min + "-" + max + " + modifier: " + modifier + " Result: " + roll);
    return roll;
}

function rollPercentage() {
    const roll = rollDie(100);
    log("Percentage roll: " + roll);
    return roll;
}

function rollPercentageSuccess(percentageChance) {
    const roll = rollPercentage();
    return percentageChance > 0 && roll <= percentageChance;
}

// Picks one option with equal chance (for "determine randomly" lists in item descriptions)
function pick(options) {
    return options[Math.floor(Math.random() * options.length)];
}

// XdY + modifier, times the multiplier
class Roll {
    constructor(x, y, modifier = 0, multiplier = 0) {
        this.x = x;
        this.y = y;
        this.modifier = modifier;
        this.multiplier = multiplier;
    }

    // applyMultiplier = false rolls only XdY + modifier (coins outside a lair, Table 1.0)
    roll(applyMultiplier = true) {
        const multiplier = applyMultiplier ? this.multiplier : 0;
        let roll = 0;
        for (let i = 0; i < this.x; i++) roll += rollDie(this.y);
        const unadjustedRoll = roll;

        roll += this.modifier;
        if (roll < 0) roll = 0;
        if (multiplier > 0 && roll !== 0) roll *= multiplier;

        log("Rolling " + this.x + "d" + this.y + " results in " + unadjustedRoll + " adding modifier " + this.modifier +
            " and multiplier " + multiplier + " Result: " + roll);
        return roll;
    }
}

function dice(x, y, modifier = 0, multiplier = 0) {
    return new Roll(x, y, modifier, multiplier);
}

// ---------- Tables ----------

function tableMax(table) {
    return Math.max(...table.map(entry => entry[1]));
}

function lookup(table, roll, tableName) {
    for (const [low, high, entry] of table) {
        if (roll >= low && roll <= high) return entry;
    }
    throw new Error("No entry for roll " + roll + " on table " + tableName);
}

// Rolls a die sized to the table (d% for 1-100, d12 for 1-12...) and picks the matching entry
function rollOnTable(table, tableName) {
    const max = tableMax(table);
    const roll = max === 100 ? rollPercentage() : rollDie(max);
    const entry = lookup(table, roll, tableName);
    log("Rolled " + roll + " on " + tableName);
    return entry;
}

// Rolls on an item table and returns a fresh copy of the entry with all of its own rolls done
function rollItem(table, tableName) {
    const item = cloneItem(rollOnTable(table, tableName));
    rollDetails(item);
    return item;
}

// ---------- Items ----------

// options: description, page, valueTable (roll the material on the value table), rollGp (Roll added
// to the value per item), rollAmount (Roll for how many), subRoll (function(item) for item-specific
// rolls), priceless, rollBonus (Table 4.3B bonus), optionalTable + optionalTableName
function makeItem(name, gp = 0, xp = 0, options = {}) {
    return {
        name: name,
        gp: gp,
        xp: xp,
        description: options.description || "",
        page: options.page || 0,
        valueTable: options.valueTable || false,
        material: null,
        rollGp: options.rollGp || null,
        amount: 1,
        rollAmount: options.rollAmount || null,
        subRoll: options.subRoll || null,
        priceless: options.priceless || false,
        rollBonus: options.rollBonus || false,
        optionalTable: options.optionalTable || null,
        optionalTableName: options.optionalTableName || "",
        // Extra details rolled for this specific item (type, size, charges, spells...)
        notes: [],
    };
}

// Copy of a table entry so rolling never modifies the table itself
function cloneItem(item) {
    return { ...item, notes: [...item.notes] };
}

function addNote(item, note) {
    item.notes.push(note);
}

// Runs all rolls an item needs after it has been picked from a table
function rollDetails(item) {
    if (item.rollAmount) item.amount = item.rollAmount.roll();

    if (item.rollGp) {
        for (let i = 0; i < item.amount; i++) item.gp += item.rollGp.roll();
    }

    if (item.valueTable) {
        item.material = rollValueTable();
        item.gp += item.material.gp;
    }

    if (item.subRoll) item.subRoll(item);

    // Table 4.3B: adds the bonus to the name ("Long Sword +2", or replaces "+X") and its value and XP
    if (item.rollBonus) {
        const bonus = SubRolls.rollBonus();
        item.gp += bonus.gp;
        item.xp += bonus.xp;
        if (item.name.includes("+X")) item.name = item.name.replace("+X", "+" + bonus.bonus);
        else item.name += " +" + bonus.bonus;
    }

    // Optional sub-table from the item description (bane foe d12, dragon type d10)
    if (item.optionalTable) {
        const sides = tableMax(item.optionalTable);
        const roll = rollDie(sides);
        const entry = lookup(item.optionalTable, roll, item.optionalTableName);
        addNote(item, item.optionalTableName + " (d" + sides + " = " + roll + "): " + entry);
    }
}

function logItem(item) {
    const gpText = item.priceless ? "priceless" : item.gp;
    log("Name: " + item.name + " gp value: " + gpText + " amount: " + item.amount + " xp: " + item.xp);
    if (item.description) log("Description: " + item.description);
    if (item.page) log("Page number: " + item.page);
    if (item.valueTable) log("Material: " + item.material.description);
    for (const note of item.notes) {
        // Notes starting with spaces belong to the note above them (e.g. spells on a scroll)
        log(note.startsWith("  ") ? "      " + note.trimStart() : "  - " + note);
    }
}

// ---------- Value table (materials of jewelry, ceremonial items...) ----------

const VALUE_TABLE = [
    [1, 2, { description: "Clay", gp: 10 }],
    [3, 8, { description: "Wood", gp: 50 }],
    [9, 11, { description: "Wood with silver inlay", gp: 100 }],
    [12, 13, { description: "Wood with gold inlay", gp: 250 }],
    [14, 14, { description: "Wood with gemstones", gp: 500 }],
    [15, 19, { description: "Stone", gp: 100 }],
    [20, 23, { description: "Stone with gemstones", gp: 500 }],
    [24, 24, { description: "Bone with jewels", gp: 1000 }],
    [25, 34, { description: "Silver", gp: 250 }],
    [35, 39, { description: "Silver with gold", gp: 500 }],
    [40, 44, { description: "Silver with platinum", gp: 750 }],
    [45, 50, { description: "Silver with gemstones", gp: 1250 }],
    [51, 53, { description: "Ivory", gp: 500 }],
    [54, 56, { description: "Ivory with silver", gp: 750 }],
    [57, 58, { description: "Ivory with gold", gp: 1000 }],
    [59, 60, { description: "Ivory with gemstones", gp: 3000 }],
    [61, 63, { description: "Jade (or other precious stone)", gp: 750 }],
    [64, 67, { description: "Jade (or other precious stone) with ivory", gp: 1000 }],
    [68, 70, { description: "Jade (or other precious stone) with silver or gold", gp: 1250 }],
    [71, 72, { description: "Jade (or other precious stone) with platinum", gp: 2000 }],
    [73, 74, { description: "Jade (or other precious stone) with gemstones", gp: 5000 }],
    [75, 86, { description: "Gold", gp: 1000 }],
    [87, 89, { description: "Gold wtih platinum", gp: 3500 }],
    [90, 93, { description: "Gold with gemstones", gp: 7500 }],
    [94, 96, { description: "Platinum", gp: 10000 }],
    [97, 98, { description: "Platinum with gemstones", gp: 15000 }],
    [99, 99, { description: "Platinum with mithril", gp: 20000 }],
    [100, 100, { description: "Mithril", gp: 50000 }],
];

function rollValueTable() {
    const roll = rollPercentage();
    const entry = lookup(VALUE_TABLE, roll, "Value table");
    log("Rolled value table: " + roll + " which equals: " + entry.description + " which has an average gold value of " + entry.gp);
    return { ...entry };
}
