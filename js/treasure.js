"use strict";

// The main treasure roll (Table 1.0): coins, gems, extraordinary items and magic items, plus the
// report of the results. (Program.cs, Coins.cs, Gems.cs and TreasureReport.cs in the C# version.)

const MAX_TREASURE_LEVEL = 18;
// A hoard (in a lair) multiplies gold by a further 100 and doubles gems, extraordinary and magic items.
const HOARD_GOLD_MULTIPLIER = 100;
// Also double the amount of rolls in addition to doubling items? It is unclear
const HOARD_ITEM_MULTIPLIER = 2;

// Table 1.0 coins per treasure level 1-18: % chance and roll. The multiplier only applies in a lair.
const COINS = [
    { chance: 50, roll: dice(2, 4, 0, 10) },
    { chance: 55, roll: dice(4, 4, 0, 10) },
    { chance: 60, roll: dice(6, 4, 0, 10) },
    { chance: 65, roll: dice(8, 4, 0, 10) },
    { chance: 70, roll: dice(2, 6, 0, 50) },
    { chance: 75, roll: dice(4, 6, 0, 50) },
    { chance: 80, roll: dice(6, 6, 0, 50) },
    { chance: 85, roll: dice(8, 6, 0, 50) },
    { chance: 90, roll: dice(2, 8, 0, 100) },
    { chance: 91, roll: dice(4, 8, 0, 100) },
    { chance: 92, roll: dice(6, 8, 0, 100) },
    { chance: 93, roll: dice(8, 8, 0, 100) },
    { chance: 94, roll: dice(2, 10, 0, 200) },
    { chance: 95, roll: dice(4, 10, 0, 200) },
    { chance: 96, roll: dice(6, 10, 0, 200) },
    { chance: 97, roll: dice(8, 10, 0, 200) },
    { chance: 98, roll: dice(2, 12, 0, 400) },
    { chance: 99, roll: dice(4, 12, 0, 400) },
];

// Table 1.0 gems per treasure level 1-18: % chance, number of gems and adjustment to the gem value roll
const GEMS = [
    { chance: 10, roll: dice(1, 4, 0), valueAdjustment: -10 },
    { chance: 20, roll: dice(1, 4, 1), valueAdjustment: -8 },
    { chance: 30, roll: dice(1, 4, 2), valueAdjustment: -6 },
    { chance: 40, roll: dice(1, 4, 3), valueAdjustment: -4 },
    { chance: 50, roll: dice(1, 6, 2), valueAdjustment: -2 },
    { chance: 60, roll: dice(1, 6, 3), valueAdjustment: 0 },
    { chance: 70, roll: dice(1, 6, 4), valueAdjustment: 0 },
    { chance: 80, roll: dice(1, 6, 5), valueAdjustment: 0 },
    { chance: 90, roll: dice(1, 8, 4), valueAdjustment: 0 },
    { chance: 91, roll: dice(1, 8, 5), valueAdjustment: 0 },
    { chance: 92, roll: dice(1, 8, 6), valueAdjustment: 0 },
    { chance: 93, roll: dice(1, 8, 7), valueAdjustment: 0 },
    { chance: 94, roll: dice(1, 10, 6), valueAdjustment: 0 },
    { chance: 95, roll: dice(1, 10, 7), valueAdjustment: 0 },
    { chance: 96, roll: dice(1, 10, 8), valueAdjustment: 0 },
    { chance: 97, roll: dice(1, 10, 9), valueAdjustment: 0 },
    { chance: 98, roll: dice(1, 12, 8), valueAdjustment: 0 },
    { chance: 99, roll: dice(1, 12, 9), valueAdjustment: 0 },
];

const GEM_VALUES = [
    [1, 10, { description: "Amber, amethyst, jadeite", gp: 5 }],
    [11, 20, { description: "Precious opal, banded eye, malachite", gp: 10 }],
    [21, 40, { description: "Moonstone, pearl, lapis lazuli, tiger eye", gp: 25 }],
    [41, 60, { description: "Bloodstone, white agate, violet-blue sapphire", gp: 50 }],
    [61, 75, { description: "Whitish moonstone, common opal", gp: 100 }],
    [76, 85, { description: "Green nephrite, peridot, amethyst", gp: 250 }],
    [86, 90, { description: "Violet or green garnet, fire opal, topaz", gp: 500 }],
    [91, 94, { description: "Emerald, black opal, tourmaline", gp: 1000 }],
    [95, 98, { description: "Star ruby, sapphire (other than blue)", gp: 2500 }],
    [99, 100, { description: "Diamond, blood red ruby, blue sapphire", gp: 5000 }],
];

// ---------- Report ----------

// Collects the results of one treasure roll
const TreasureReport = {
    header: "",
    nil: false,
    gold: 0,
    gems: [],
    gemTotal: 0,
    extraordinaryItems: [],
    magicItems: [],

    begin(treasureLevel, mode) {
        this.nil = treasureLevel === 0;
        this.header = this.nil ? "Treasure type Nil" : "Treasure level " + treasureLevel + ", " + mode;
        this.gold = 0;
        this.gems = [];
        this.gemTotal = 0;
        this.extraordinaryItems = [];
        this.magicItems = [];
    },

    setGold(amount) { this.gold = amount; },

    addGem(count, name, gpEach) {
        this.gems.push({ count: count, name: name, gpEach: gpEach });
        this.gemTotal += count * gpEach;
    },

    addExtraordinaryItem(item) { this.extraordinaryItems.push(item); },
    addMagicItem(item) { this.magicItems.push(item); },

    totalValue() {
        let total = this.gold + this.gemTotal;
        for (const item of this.extraordinaryItems) total += item.gp;
        for (const item of this.magicItems) if (!item.priceless) total += item.gp;
        return total;
    },

    hasPriceless() {
        return this.magicItems.some(item => item.priceless);
    },

    // Plain data for the page
    result(treasureLevel, rollLog) {
        const toResult = item => ({
            amount: item.amount,
            name: item.name,
            gp: item.gp,
            priceless: item.priceless,
            xp: item.xp,
            material: item.valueTable ? item.material.description : null,
            notes: [...item.notes],
            description: item.description,
            page: item.page,
        });
        return {
            level: treasureLevel,
            header: this.header,
            nil: this.nil,
            gold: this.gold,
            gemTotal: this.gemTotal,
            gems: [...this.gems].sort((a, b) => b.gpEach - a.gpEach), // Most valuable first
            extraordinaryItems: this.extraordinaryItems.map(toResult),
            magicItems: this.magicItems.map(toResult),
            totalValue: this.totalValue(),
            hasPriceless: this.hasPriceless(),
            text: this.buildText(),
            log: rollLog,
        };
    },

    // Same layout the terminal app appended to Treasure.txt
    buildText() {
        const lines = [];
        const line = "=".repeat(60);
        lines.push(line, this.header, "Rolled " + formatDate(new Date()), line);

        if (this.nil) {
            lines.push("No treasure.", "");
            return lines.join("\n") + "\n";
        }

        lines.push("", "GOLD: " + gp(this.gold), "");

        if (this.gems.length === 0) {
            lines.push("GEMS: none");
        } else {
            const count = this.gems.reduce((sum, gem) => sum + gem.count, 0);
            lines.push("GEMS (" + count + ", total " + gp(this.gemTotal) + "):");
            for (const gem of [...this.gems].sort((a, b) => b.gpEach - a.gpEach)) {
                lines.push("  " + gem.count + " x " + gem.name + " (" + gp(gem.gpEach) + " each)");
            }
        }
        lines.push("");

        appendItems(lines, "EXTRAORDINARY ITEMS", this.extraordinaryItems);
        appendItems(lines, "MAGIC ITEMS", this.magicItems);

        lines.push("TOTAL VALUE: " + gp(this.totalValue()) + (this.hasPriceless() ? " plus priceless artifact(s)" : ""), "", "");
        return lines.join("\n") + "\n";
    },
};

function appendItems(lines, title, items) {
    if (items.length === 0) {
        lines.push(title + ": none", "");
        return;
    }

    lines.push(title + " (" + items.length + "):");
    items.forEach((item, i) => {
        const amount = item.amount > 1 ? item.amount + " x " : "";
        const value = item.priceless ? "priceless" : gp(item.gp);
        const xp = item.xp > 0 ? ", " + item.xp.toLocaleString("en-US") + " xp" : "";
        lines.push("  " + (i + 1) + ". " + amount + item.name + " - " + value + xp);

        if (item.valueTable) lines.push("     Material: " + item.material.description);
        for (const note of item.notes) lines.push("     " + note);
        if (item.description) lines.push("     " + item.description);
        if (item.page) lines.push("     (Monsters & Treasure p. " + item.page + ")");
    });
    lines.push("");
}

function gp(value) {
    return value.toLocaleString("en-US") + " gp";
}

function formatDate(date) {
    const pad = n => String(n).padStart(2, "0");
    return date.getFullYear() + "-" + pad(date.getMonth() + 1) + "-" + pad(date.getDate()) + " " + pad(date.getHours()) + ":" + pad(date.getMinutes());
}

// ---------- Generator ----------

const TreasureGenerator = {
    inLair: true,
    isHoard: false,

    // options: level (0-18, 0 = Nil), random (roll the level 1-18 instead), lair, hoard, sentient (% chance)
    roll(options) {
        const level = options.level;
        const sentient = options.sentient || 0;
        if (!options.random && !(Number.isInteger(level) && level >= 0 && level <= MAX_TREASURE_LEVEL))
            throw new Error("Treasure level must be between 0 and " + MAX_TREASURE_LEVEL);
        if (!(sentient >= 0 && sentient <= 100))
            throw new Error("Sentient chance must be between 0 and 100");

        RollLog.clear();
        SentientItems.chancePercent = sentient;
        const treasureLevel = options.random ? rollNumber(1, MAX_TREASURE_LEVEL) : level;

        this.inLair = options.lair !== false;
        this.isHoard = this.inLair && !!options.hoard;
        const mode = this.isHoard ? "hoard in lair" : this.inLair ? "in lair" : "not in lair";
        TreasureReport.begin(treasureLevel, mode);
        pageBreak();

        if (treasureLevel === 0) {
            log("Treasure type Nil: the monster has no treasure");
        } else {
            log("Treasure level " + treasureLevel + ", " + mode);
            this.rollCoins(treasureLevel);
            this.rollGems(treasureLevel);
            this.rollExtraordinaryItems(treasureLevel);
            this.rollMagicItems(treasureLevel);
        }

        return TreasureReport.result(treasureLevel, RollLog.text());
    },

    hoardCount(amount, what) {
        if (!this.isHoard || amount === 0) return amount;
        const doubled = amount * HOARD_ITEM_MULTIPLIER;
        log("Hoard: " + what + " doubled from " + amount + " to " + doubled);
        return doubled;
    },

    rollCoins(treasureLevel) {
        pageBreak();
        const entry = COINS[treasureLevel - 1];
        const hasCoins = rollPercentageSuccess(entry.chance);
        log(entry.chance + "% chance of the treasure containing gold coins. Your result: " + hasCoins);
        let coinAmount = 0;
        if (hasCoins) {
            coinAmount = entry.roll.roll(this.inLair);
            if (this.isHoard) {
                coinAmount *= HOARD_GOLD_MULTIPLIER;
                log("Hoard: gold multiplied by " + HOARD_GOLD_MULTIPLIER);
            }
        }
        log("The treasure with treasure level " + treasureLevel + " contains " + coinAmount + " gold pieces");
        TreasureReport.setGold(coinAmount);
    },

    rollGems(treasureLevel) {
        pageBreak();
        const entry = GEMS[treasureLevel - 1];
        const hasGems = rollPercentageSuccess(entry.chance);
        log(entry.chance + "% chance of the treasure containing gems. Your result: " + hasGems);
        let gemsAmount = 0;
        if (hasGems) gemsAmount = this.hoardCount(entry.roll.roll(), "gems");
        log("The treasure with treasure level " + treasureLevel + " contains " + gemsAmount + " gems");

        if (hasGems) {
            log("Rolling gem subtable for " + gemsAmount + " gems");
            this.rollGemValues(gemsAmount, entry.valueAdjustment);
        }
    },

    rollGemValues(gemAmount, valueAdjustment) {
        let totalGemValue = 0;
        const counts = new Map(); // description -> { count, gp }, in the order first rolled
        for (let i = 0; i < gemAmount; i++) {
            let roll = rollPercentage() + valueAdjustment;
            if (roll < 1) roll = 1;
            const gem = lookup(GEM_VALUES, roll, "Gem values");
            log(gem.description + " gem has a gold piece value of " + gem.gp);
            const counted = counts.get(gem.description) || { count: 0, gp: gem.gp };
            counted.count++;
            counts.set(gem.description, counted);
            totalGemValue += gem.gp;
        }

        log("");
        log("Gem summary:");
        for (const [description, counted] of counts) {
            log(counted.count + " x " + description);
            TreasureReport.addGem(counted.count, description, counted.gp);
        }
        log("Total value of all " + gemAmount + " gems is: " + totalGemValue);
    },

    rollExtraordinaryItems(treasureLevel) {
        pageBreak();
        const entry = ExtraordinaryItems.levels[treasureLevel - 1];
        const hasItems = rollPercentageSuccess(entry.chance);
        log(entry.chance + "% chance of the treasure containing extraordinary items. Your result: " + hasItems);
        if (hasItems) {
            const itemsAmount = this.hoardCount(entry.roll.roll(), "extraordinary items");
            log("The treasure with treasure level " + treasureLevel + " contains " + itemsAmount + " extraordinary items");
            log("Rolling extraordinary item type for " + itemsAmount + " items");
            ExtraordinaryItems.rollItemType(itemsAmount);
        }
    },

    rollMagicItems(treasureLevel) {
        pageBreak();
        const entry = MagicItems.levels[treasureLevel - 1];
        const hasItems = rollPercentageSuccess(entry.chance);
        log(entry.chance + "% chance of the treasure containing magic items. Your result: " + hasItems);
        if (hasItems) {
            const itemsAmount = this.hoardCount(entry.roll.roll(), "magic items");
            log("The treasure with treasure level " + treasureLevel + " contains " + itemsAmount + " magic items");
            if (entry.maxXp > 0) log("Max. XP value of each item: " + entry.maxXp);
            log("Rolling magic item type for " + itemsAmount + " items");
            MagicItems.rollItemType(itemsAmount, entry.maxXp);
        }
    },
};
