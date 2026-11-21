using System;
using System.Collections.Generic;

//Tables 4.4-4.4E: Armor and Shields (Monsters & Treasure pages 325-326, descriptions page 334).
//Values come from the item's bonus on Table 4.4E: Armor Bonus.
public static class ArmorAndShields
{
    private static readonly Dictionary<(int, int), string> shieldTable = new Dictionary<(int, int), string>
    {
        { (1, 40), "Buckler" },
        { (41, 70), "Medium Shield" },
        { (71, 90), "Large Shield" },
        { (91, 99), "Pavis" },
        { (100, 100), "Special" }, //Roll on Table 4.4B
    };

    private static readonly Dictionary<(int, int), string> specialShieldTable = new Dictionary<(int, int), string>
    {
        { (1, 40), "Animated Shield" },
        { (41, 70), "Shield of Bashing" },
        { (71, 85), "Blinding Shield" },
        { (86, 95), "Lion's Shield" },
        { (96, 100), "Shield of El Cid" },
    };

    private static readonly Dictionary<(int, int), string> armorTable = new Dictionary<(int, int), string>
    {
        { (1, 13), "Padded" },
        { (14, 29), "Leather" },
        { (30, 36), "Studded" },
        { (37, 46), "Ring" },
        { (47, 51), "Mail Shirt" },
        { (52, 61), "Hide" },
        { (62, 71), "Scale Mail" },
        { (72, 76), "Chainmail" },
        { (77, 85), "Breastplate" },
        { (86, 90), "Splint Mail" },
        { (91, 95), "Banded Mail" },
        { (96, 98), "Plate Mail" },
        { (99, 99), "Full Plate" },
        { (100, 100), "Special" }, //Roll on Table 4.4D
    };

    private static readonly Dictionary<(int, int), string> specialArmorTable = new Dictionary<(int, int), string>
    {
        { (1, 9), "Armor of Kavacha" },
        { (10, 18), "Armor of Cold/Fire Resistance" },
        { (19, 36), "Dwarven Plate" },
        { (37, 54), "Elven Chain" },
        { (55, 64), "Plate of Etherealness" },
        { (65, 82), "Plate Armor of the Deep" },
        { (83, 100), "Armor of Spell Resistance" },
    };

    //Table 4.4: 01-75 shield, 76-00 armor
    public static Item Roll()
    {
        int roll = CommonUtils.RollPercentage();
        Console.WriteLine("Rolled " + roll + " on Table 4.4 Armor and Shields");
        return roll <= 75 ? RollShield() : RollArmor();
    }

    private static Item RollShield()
    {
        string shield = CommonUtils.RollOnTable(shieldTable, "Table 4.4A Shield");
        if (shield == "Special") return RollSpecialShield();

        Item item = ItemWithBonus(shield, SubRolls.RollBonus("Table 4.4E Armor Bonus").bonus, 326);
        SubRolls.RollSize(item);
        return item;
    }

    private static Item RollSpecialShield()
    {
        string shield = CommonUtils.RollOnTable(specialShieldTable, "Table 4.4B Random Shield Type");
        Item item;
        switch (shield)
        {
            case "Shield of Bashing":
                item = ItemWithBonus(shield, 1, 334, "Bash attack with +1 to hit and damage: 1d4 (buckler/medium) or 1d6 (large)");
                item.AddNote("Shield size: " + RollShieldSize(rerollPavis: true));
                break;
            case "Lion's Shield":
                item = ItemWithBonus(shield, 2, 334, "Heavy steel shield. Lion's head bites 3/day with the wielder's BtH for 2d6 damage");
                item.AddNote("Shield size: Large Shield");
                break;
            case "Shield of El Cid":
                item = ItemWithBonus(shield, 2, 334, "Medium steel shield. Bonus applies against any number of attackers. Grants spell resistance 5");
                item.AddNote("Shield size: Medium Shield");
                break;
            case "Blinding Shield":
                item = ItemWithBonus(shield, SubRolls.RollBonus("Table 4.4E Armor Bonus").bonus, 334, "Flashes twice per day: anyone within 20 ft. makes a dexterity save or is blinded 1d4 rounds");
                item.AddNote("Shield size: " + RollShieldSize());
                break;
            default: //Animated Shield
                item = ItemWithBonus(shield, SubRolls.RollBonus("Table 4.4E Armor Bonus").bonus, 334, "On command floats within 2 ft. of the wielder and protects them, freeing both hands");
                item.AddNote("Shield size: " + RollShieldSize());
                break;
        }
        SubRolls.RollSize(item);
        return item;
    }

    //Table 4.4A ignoring results of 00 (and pavis for a shield of bashing, which "cannot be used to bash")
    private static string RollShieldSize(bool rerollPavis = false)
    {
        while (true)
        {
            string size = CommonUtils.RollOnTable(shieldTable, "Table 4.4A Shield (size)");
            if (size == "Special" || (rerollPavis && size == "Pavis")) continue;
            return size;
        }
    }

    private static Item RollArmor()
    {
        string armor = CommonUtils.RollOnTable(armorTable, "Table 4.4C Armor");
        if (armor == "Special") return RollSpecialArmor();

        Item item = ItemWithBonus(armor, SubRolls.RollBonus("Table 4.4E Armor Bonus").bonus, 326);
        SubRolls.RollSize(item);
        return item;
    }

    private static Item RollSpecialArmor()
    {
        string armor = CommonUtils.RollOnTable(specialArmorTable, "Table 4.4D Random Armor Type");
        Item item;
        switch (armor)
        {
            case "Armor of Kavacha":
                item = ItemWithBonus(armor, 3, 334, "Gilded silver chainmail (+3 full chain suit). Critical hits against the wearer miss instead");
                item.AddNote("Armor type: Chainmail");
                break;
            case "Armor of Cold/Fire Resistance":
                string element = CommonUtils.Pick("Cold", "Fire");
                item = ItemWithBonus("Armor of " + element + " Resistance", 2, 334, "Absorbs the first 10 points of " + element.ToLower() + " damage per attack");
                item.AddNote("Armor type: " + RollArmorType());
                break;
            case "Dwarven Plate":
                item = ItemWithBonus(armor, 4, 334, "Fitted only for small creatures. Always found with helm and shield");
                item.AddNote("Armor type: Plate Mail");
                item.AddNote("Size: Small");
                return item; //Size is fixed by the description
            case "Elven Chain":
                item = ItemWithBonus(armor, 5, 334, "Mithral chainmail that does not impede movement. Encumbrance value 0");
                item.AddNote("Armor type: Chainmail");
                break;
            case "Plate of Etherealness":
                item = ItemWithBonus(armor, 3, 334, "Wearer can enter the ethereal plane (as ethereal jaunt) and stay as long as they wish, once per day");
                item.AddNote("Armor type: Plate Mail");
                break;
            case "Plate Armor of the Deep":
                item = ItemWithBonus(armor, 1, 334, "Treated as unarmored for swimming. Wearer breathes water and can converse with water-breathing creatures");
                item.AddNote("Armor type: Full Plate");
                break;
            default: //Armor of Spell Resistance: XP is 1,000 x the spell resistance bonus (Table 4.4D note)
                int spellResistance = CommonUtils.Pick(1, 2, 5, 8);
                item = ItemWithBonus(armor, SubRolls.RollBonus("Table 4.4E Armor Bonus").bonus, 334, "Enhances the wearer's spell resistance by +" + spellResistance);
                item.xp = 1000 * spellResistance;
                item.AddNote("Armor type: " + RollArmorType());
                break;
        }
        SubRolls.RollSize(item);
        return item;
    }

    //Table 4.4C ignoring results of 00
    private static string RollArmorType()
    {
        while (true)
        {
            string armor = CommonUtils.RollOnTable(armorTable, "Table 4.4C Armor (type)");
            if (armor != "Special") return armor;
        }
    }

    private static Item ItemWithBonus(string name, int bonus, int pageNumber, string description = "")
    {
        SubRolls.BonusEntry entry = SubRolls.BonusFor(bonus);
        return new Item(name + " +" + bonus, entry.gp, entry.xp, description, pageNumber);
    }
}
