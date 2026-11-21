"use strict";

// Tables 4.4-4.4E: Armor and Shields (Monsters & Treasure pages 325-326, descriptions page 334).
// Values come from the item's bonus on Table 4.4E: Armor Bonus. (ArmorAndShields.cs)
const ArmorAndShields = {
    shieldTable: [
        [1, 40, "Buckler"],
        [41, 70, "Medium Shield"],
        [71, 90, "Large Shield"],
        [91, 99, "Pavis"],
        [100, 100, "Special"], // Roll on Table 4.4B
    ],

    specialShieldTable: [
        [1, 40, "Animated Shield"],
        [41, 70, "Shield of Bashing"],
        [71, 85, "Blinding Shield"],
        [86, 95, "Lion's Shield"],
        [96, 100, "Shield of El Cid"],
    ],

    armorTable: [
        [1, 13, "Padded"],
        [14, 29, "Leather"],
        [30, 36, "Studded"],
        [37, 46, "Ring"],
        [47, 51, "Mail Shirt"],
        [52, 61, "Hide"],
        [62, 71, "Scale Mail"],
        [72, 76, "Chainmail"],
        [77, 85, "Breastplate"],
        [86, 90, "Splint Mail"],
        [91, 95, "Banded Mail"],
        [96, 98, "Plate Mail"],
        [99, 99, "Full Plate"],
        [100, 100, "Special"], // Roll on Table 4.4D
    ],

    specialArmorTable: [
        [1, 9, "Armor of Kavacha"],
        [10, 18, "Armor of Cold/Fire Resistance"],
        [19, 36, "Dwarven Plate"],
        [37, 54, "Elven Chain"],
        [55, 64, "Plate of Etherealness"],
        [65, 82, "Plate Armor of the Deep"],
        [83, 100, "Armor of Spell Resistance"],
    ],

    // Table 4.4: 01-75 shield, 76-00 armor
    roll() {
        const roll = rollPercentage();
        log("Rolled " + roll + " on Table 4.4 Armor and Shields");
        return roll <= 75 ? ArmorAndShields.rollShield() : ArmorAndShields.rollArmor();
    },

    rollShield() {
        const shield = rollOnTable(ArmorAndShields.shieldTable, "Table 4.4A Shield");
        if (shield === "Special") return ArmorAndShields.rollSpecialShield();

        const item = ArmorAndShields.itemWithBonus(shield, SubRolls.rollBonus("Table 4.4E Armor Bonus").bonus, 326);
        SubRolls.rollSize(item);
        return item;
    },

    rollSpecialShield() {
        const shield = rollOnTable(ArmorAndShields.specialShieldTable, "Table 4.4B Random Shield Type");
        let item;
        switch (shield) {
            case "Shield of Bashing":
                item = ArmorAndShields.itemWithBonus(shield, 1, 334, "Bash attack with +1 to hit and damage: 1d4 (buckler/medium) or 1d6 (large)");
                addNote(item, "Shield size: " + ArmorAndShields.rollShieldSize(true));
                break;
            case "Lion's Shield":
                item = ArmorAndShields.itemWithBonus(shield, 2, 334, "Heavy steel shield. Lion's head bites 3/day with the wielder's BtH for 2d6 damage");
                addNote(item, "Shield size: Large Shield");
                break;
            case "Shield of El Cid":
                item = ArmorAndShields.itemWithBonus(shield, 2, 334, "Medium steel shield. Bonus applies against any number of attackers. Grants spell resistance 5");
                addNote(item, "Shield size: Medium Shield");
                break;
            case "Blinding Shield":
                item = ArmorAndShields.itemWithBonus(shield, SubRolls.rollBonus("Table 4.4E Armor Bonus").bonus, 334, "Flashes twice per day: anyone within 20 ft. makes a dexterity save or is blinded 1d4 rounds");
                addNote(item, "Shield size: " + ArmorAndShields.rollShieldSize());
                break;
            default: // Animated Shield
                item = ArmorAndShields.itemWithBonus(shield, SubRolls.rollBonus("Table 4.4E Armor Bonus").bonus, 334, "On command floats within 2 ft. of the wielder and protects them, freeing both hands");
                addNote(item, "Shield size: " + ArmorAndShields.rollShieldSize());
                break;
        }
        SubRolls.rollSize(item);
        return item;
    },

    // Table 4.4A ignoring results of 00 (and pavis for a shield of bashing, which "cannot be used to bash")
    rollShieldSize(rerollPavis = false) {
        while (true) {
            const size = rollOnTable(ArmorAndShields.shieldTable, "Table 4.4A Shield (size)");
            if (size === "Special" || (rerollPavis && size === "Pavis")) continue;
            return size;
        }
    },

    rollArmor() {
        const armor = rollOnTable(ArmorAndShields.armorTable, "Table 4.4C Armor");
        if (armor === "Special") return ArmorAndShields.rollSpecialArmor();

        const item = ArmorAndShields.itemWithBonus(armor, SubRolls.rollBonus("Table 4.4E Armor Bonus").bonus, 326);
        SubRolls.rollSize(item);
        return item;
    },

    rollSpecialArmor() {
        const armor = rollOnTable(ArmorAndShields.specialArmorTable, "Table 4.4D Random Armor Type");
        let item;
        switch (armor) {
            case "Armor of Kavacha":
                item = ArmorAndShields.itemWithBonus(armor, 3, 334, "Gilded silver chainmail (+3 full chain suit). Critical hits against the wearer miss instead");
                addNote(item, "Armor type: Chainmail");
                break;
            case "Armor of Cold/Fire Resistance": {
                const element = pick(["Cold", "Fire"]);
                item = ArmorAndShields.itemWithBonus("Armor of " + element + " Resistance", 2, 334, "Absorbs the first 10 points of " + element.toLowerCase() + " damage per attack");
                addNote(item, "Armor type: " + ArmorAndShields.rollArmorType());
                break;
            }
            case "Dwarven Plate":
                item = ArmorAndShields.itemWithBonus(armor, 4, 334, "Fitted only for small creatures. Always found with helm and shield");
                addNote(item, "Armor type: Plate Mail");
                addNote(item, "Size: Small");
                return item; // Size is fixed by the description
            case "Elven Chain":
                item = ArmorAndShields.itemWithBonus(armor, 5, 334, "Mithral chainmail that does not impede movement. Encumbrance value 0");
                addNote(item, "Armor type: Chainmail");
                break;
            case "Plate of Etherealness":
                item = ArmorAndShields.itemWithBonus(armor, 3, 334, "Wearer can enter the ethereal plane (as ethereal jaunt) and stay as long as they wish, once per day");
                addNote(item, "Armor type: Plate Mail");
                break;
            case "Plate Armor of the Deep":
                item = ArmorAndShields.itemWithBonus(armor, 1, 334, "Treated as unarmored for swimming. Wearer breathes water and can converse with water-breathing creatures");
                addNote(item, "Armor type: Full Plate");
                break;
            default: { // Armor of Spell Resistance: XP is 1,000 x the spell resistance bonus (Table 4.4D note)
                const spellResistance = pick([1, 2, 5, 8]);
                item = ArmorAndShields.itemWithBonus(armor, SubRolls.rollBonus("Table 4.4E Armor Bonus").bonus, 334, "Enhances the wearer's spell resistance by +" + spellResistance);
                item.xp = 1000 * spellResistance;
                addNote(item, "Armor type: " + ArmorAndShields.rollArmorType());
                break;
            }
        }
        SubRolls.rollSize(item);
        return item;
    },

    // Table 4.4C ignoring results of 00
    rollArmorType() {
        while (true) {
            const armor = rollOnTable(ArmorAndShields.armorTable, "Table 4.4C Armor (type)");
            if (armor !== "Special") return armor;
        }
    },

    itemWithBonus(name, bonus, page, description = "") {
        const entry = SubRolls.bonusFor(bonus);
        return makeItem(name + " +" + bonus, entry.gp, entry.xp, { description: description, page: page });
    },
};
