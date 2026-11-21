"use strict";

// Sentient Items (Monsters & Treasure pages 316-318): alignment, will, communication, senses,
// powers and special purpose of an intelligent weapon. The book gives no chance for a weapon
// to be sentient, so the Castle Keeper sets it (chancePercent, 0 = never). (SentientItems.cs)
// Not automated (left to the Castle Keeper): lands and titles, services, unworked metals,
// and the rules for creating, destroying and valuing sentient items.
const SentientItems = {
    chancePercent: 0,

    alignmentTable: [
        [1, 5, "Chaotic good"],
        [6, 15, "Chaotic neutral"],
        [16, 20, "Chaotic evil"],
        [21, 25, "Neutral evil"],
        [26, 30, "Lawful evil"],
        [31, 55, "Lawful good"],
        [56, 60, "Lawful neutral"],
        [61, 80, "Neutral good"],
        [81, 100, "Neutral"],
    ],

    // maxLanguages: 0 = does not speak, -1 = any language
    willTable: [
        [1, 50, { will: 17, communication: "Semi-empathy", maxLanguages: 0, telepathy: false, senses: "None", lesserPowers: 1, greaterPowers: 0, readMagic: false, specialPurpose: false }],
        [51, 75, { will: 18, communication: "Empathy", maxLanguages: 0, telepathy: false, senses: "None", lesserPowers: 2, greaterPowers: 0, readMagic: false, specialPurpose: false }],
        [76, 87, { will: 19, communication: "Empathy", maxLanguages: 0, telepathy: false, senses: "Sight (human)", lesserPowers: 3, greaterPowers: 0, readMagic: false, specialPurpose: false }],
        [88, 93, { will: 20, communication: "Speech", maxLanguages: 2, telepathy: false, senses: "Sight and hearing (human)", lesserPowers: 3, greaterPowers: 1, readMagic: false, specialPurpose: false }],
        [94, 96, { will: 21, communication: "Speech and telepathy", maxLanguages: 4, telepathy: true, senses: "Sight and hearing (human)", lesserPowers: 3, greaterPowers: 2, readMagic: true, specialPurpose: false }],
        [97, 98, { will: 22, communication: "Speech and telepathy", maxLanguages: 6, telepathy: true, senses: "Sight and hearing (elf)", lesserPowers: 3, greaterPowers: 3, readMagic: true, specialPurpose: false }],
        [99, 100, { will: 23, communication: "Speech and telepathy", maxLanguages: -1, telepathy: true, senses: "Sight and hearing (elf)", lesserPowers: 4, greaterPowers: 4, readMagic: true, specialPurpose: true }],
    ],

    lesserPowerTable: [
        [1, 5, "Bless allies 3/day"],
        [6, 10, "Faerie fire 3/day"],
        [11, 13, "Minor image 1/day"],
        [14, 20, "Cure light wounds 3/day"],
        [21, 25, "Detect magic at will"],
        [26, 31, "Determine depth and direction (as dwarf)"],
        [32, 33, "Resistance to poison (as dwarf)"],
        [34, 36, "Resistance to arcane magic (as dwarf)"],
        [37, 40, "Resistance to fear (as dwarf)"],
        [41, 45, "Stonework (as dwarf)"],
        [46, 50, "Spot hidden doors (as elf)"],
        [51, 54, "Spell resistance (as elf)"],
        [55, 60, "Legend lore 3/day (as bard)"],
        [61, 66, "Nature lore 3/day (as druid)"],
        [67, 72, "Deflect missiles 3/day (as monk)"],
        [73, 77, "Major image 1/day"],
        [78, 80, "Darkness 3/day"],
        [81, 83, "Hold person 3/day"],
        [84, 86, "Detect illusion 3/day"],
        [87, 89, "Suggestion 3/day"],
        [90, 95, "Locate object 3/day"],
        [96, 100, "Cure moderate wounds (2d8+3) on wielder 3/day"],
    ],

    greaterPowerTable: [
        [1, 6, "Detect opposing alignment at will"],
        [7, 10, "Detect undead at will"],
        [11, 13, "Cause fear in an enemy at will"],
        [14, 18, "Holy word 1/day"],
        [19, 23, "Dismissal 1/day"],
        [24, 28, "Lesser globe of invulnerability 1/day"],
        [29, 33, "Arcane eye 1/day"],
        [34, 37, "Continuous detect scrying"],
        [38, 41, "Ring of wall of fire around the wielder 1/day"],
        [42, 45, "Quench 3/day"],
        [46, 50, "Prismatic spray 2/day"],
        [51, 54, "Wall of wind 3/day"],
        [55, 59, "Clairvoyance 3/day"],
        [60, 64, "Magic circle against opposing alignment at will"],
        [65, 68, "Haste (on owner) 3/day"],
        [69, 73, "Daylight 3/day"],
        [74, 76, "Mass heal 1/day"],
        [77, 80, "Teleport 2/day"],
        [81, 85, "Wall of thorns 3/day"],
        [86, 91, "Locate creature 3/day"],
        [92, 97, "Fear 3/day"],
        [98, 100, "Detect thoughts at will"],
    ],

    purposeTable: [
        [1, 20, "Overthrow an alignment"],
        [21, 30, "Champion an alignment"],
        [31, 40, "Find its creator"],
        [41, 50, "Destroy its creator"],
        [51, 55, "Defeat/slay a creature type"],
        [56, 60, "Defeat/slay a race or kind"],
        [61, 70, "Defend a race or kind"],
        [71, 80, "Destroy a deity and/or its followers"],
        [81, 90, "Defend a deity and/or its followers"],
        [91, 95, "Destroy the world"],
        [96, 100, "Castle Keeper's choice"],
    ],

    // Rolls the CK's chance for a magic weapon to be sentient and, if it is, rolls its personality
    maybeMakeSentient(weapon) {
        if (SentientItems.chancePercent <= 0) return;
        log("Checking if the weapon is sentient (" + SentientItems.chancePercent + "% chance)");
        if (!rollPercentageSuccess(SentientItems.chancePercent)) return;
        SentientItems.makeSentient(weapon);
    },

    makeSentient(weapon) {
        const alignment = rollOnTable(SentientItems.alignmentTable, "Sentient Item Alignment");
        const will = rollOnTable(SentientItems.willTable, "Weapon Will");

        weapon.name += " (sentient)";
        addNote(weapon, "Sentient: alignment " + alignment + ", will " + will.will);
        addNote(weapon, "Communication: " + will.communication + SentientItems.languages(will));
        addNote(weapon, "Senses: " + will.senses);

        for (const power of SentientItems.rollPowers(SentientItems.lesserPowerTable, "Lesser Powers", will.lesserPowers))
            addNote(weapon, "Lesser power: " + power);
        for (const power of SentientItems.rollPowers(SentientItems.greaterPowerTable, "Greater Powers", will.greaterPowers))
            addNote(weapon, "Greater power: " + power);
        if (will.readMagic) addNote(weapon, "Can read magic");

        // "Many sentient weapons, particularly those with wills greater than 20, have a special purpose"
        if (will.specialPurpose || will.will > 20) {
            addNote(weapon, "Special purpose: " + rollOnTable(SentientItems.purposeTable, "Special Purpose"));
        }
    },

    languages(will) {
        if (will.maxLanguages === 0) return "";
        if (will.maxLanguages < 0) return " (Common and any language)";
        const languages = rollDie(will.maxLanguages);
        return " (Common plus " + languages + " other language" + (languages === 1 ? "" : "s") + ")";
    },

    // Duplicate powers are rerolled
    rollPowers(table, tableName, count) {
        const powers = [];
        while (powers.length < count) {
            const power = rollOnTable(table, tableName);
            if (!powers.includes(power)) powers.push(power);
        }
        return powers;
    },
};
