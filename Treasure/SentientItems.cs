using System;
using System.Collections.Generic;

//Sentient Items (Monsters & Treasure pages 316-318): alignment, will, communication, senses,
//powers and special purpose of an intelligent weapon. The book gives no chance for a weapon
//to be sentient, so the Castle Keeper sets it (ChancePercent, 0 = never).
//Not automated (left to the Castle Keeper): lands and titles, services, unworked metals,
//and the rules for creating, destroying and valuing sentient items.
public static class SentientItems
{
    public static int ChancePercent = 0;

    private struct WillEntry
    {
        public int will;
        public string communication;
        public int maxLanguages; //0 = does not speak, -1 = any language
        public bool telepathy;
        public string senses;
        public int lesserPowers;
        public int greaterPowers;
        public bool readMagic;
        public bool specialPurpose;

        public WillEntry(int will, string communication, int maxLanguages, bool telepathy, string senses, int lesserPowers, int greaterPowers, bool readMagic, bool specialPurpose)
        {
            this.will = will;
            this.communication = communication;
            this.maxLanguages = maxLanguages;
            this.telepathy = telepathy;
            this.senses = senses;
            this.lesserPowers = lesserPowers;
            this.greaterPowers = greaterPowers;
            this.readMagic = readMagic;
            this.specialPurpose = specialPurpose;
        }
    }

    private static readonly Dictionary<(int, int), string> alignmentTable = new Dictionary<(int, int), string>
    {
        { (1, 5), "Chaotic good" },
        { (6, 15), "Chaotic neutral" },
        { (16, 20), "Chaotic evil" },
        { (21, 25), "Neutral evil" },
        { (26, 30), "Lawful evil" },
        { (31, 55), "Lawful good" },
        { (56, 60), "Lawful neutral" },
        { (61, 80), "Neutral good" },
        { (81, 100), "Neutral" },
    };

    private static readonly Dictionary<(int, int), WillEntry> willTable = new Dictionary<(int, int), WillEntry>
    {
        { (1, 50), new WillEntry(17, "Semi-empathy", 0, false, "None", 1, 0, false, false) },
        { (51, 75), new WillEntry(18, "Empathy", 0, false, "None", 2, 0, false, false) },
        { (76, 87), new WillEntry(19, "Empathy", 0, false, "Sight (human)", 3, 0, false, false) },
        { (88, 93), new WillEntry(20, "Speech", 2, false, "Sight and hearing (human)", 3, 1, false, false) },
        { (94, 96), new WillEntry(21, "Speech and telepathy", 4, true, "Sight and hearing (human)", 3, 2, true, false) },
        { (97, 98), new WillEntry(22, "Speech and telepathy", 6, true, "Sight and hearing (elf)", 3, 3, true, false) },
        { (99, 100), new WillEntry(23, "Speech and telepathy", -1, true, "Sight and hearing (elf)", 4, 4, true, true) },
    };

    private static readonly Dictionary<(int, int), string> lesserPowerTable = new Dictionary<(int, int), string>
    {
        { (1, 5), "Bless allies 3/day" },
        { (6, 10), "Faerie fire 3/day" },
        { (11, 13), "Minor image 1/day" },
        { (14, 20), "Cure light wounds 3/day" },
        { (21, 25), "Detect magic at will" },
        { (26, 31), "Determine depth and direction (as dwarf)" },
        { (32, 33), "Resistance to poison (as dwarf)" },
        { (34, 36), "Resistance to arcane magic (as dwarf)" },
        { (37, 40), "Resistance to fear (as dwarf)" },
        { (41, 45), "Stonework (as dwarf)" },
        { (46, 50), "Spot hidden doors (as elf)" },
        { (51, 54), "Spell resistance (as elf)" },
        { (55, 60), "Legend lore 3/day (as bard)" },
        { (61, 66), "Nature lore 3/day (as druid)" },
        { (67, 72), "Deflect missiles 3/day (as monk)" },
        { (73, 77), "Major image 1/day" },
        { (78, 80), "Darkness 3/day" },
        { (81, 83), "Hold person 3/day" },
        { (84, 86), "Detect illusion 3/day" },
        { (87, 89), "Suggestion 3/day" },
        { (90, 95), "Locate object 3/day" },
        { (96, 100), "Cure moderate wounds (2d8+3) on wielder 3/day" },
    };

    private static readonly Dictionary<(int, int), string> greaterPowerTable = new Dictionary<(int, int), string>
    {
        { (1, 6), "Detect opposing alignment at will" },
        { (7, 10), "Detect undead at will" },
        { (11, 13), "Cause fear in an enemy at will" },
        { (14, 18), "Holy word 1/day" },
        { (19, 23), "Dismissal 1/day" },
        { (24, 28), "Lesser globe of invulnerability 1/day" },
        { (29, 33), "Arcane eye 1/day" },
        { (34, 37), "Continuous detect scrying" },
        { (38, 41), "Ring of wall of fire around the wielder 1/day" },
        { (42, 45), "Quench 3/day" },
        { (46, 50), "Prismatic spray 2/day" },
        { (51, 54), "Wall of wind 3/day" },
        { (55, 59), "Clairvoyance 3/day" },
        { (60, 64), "Magic circle against opposing alignment at will" },
        { (65, 68), "Haste (on owner) 3/day" },
        { (69, 73), "Daylight 3/day" },
        { (74, 76), "Mass heal 1/day" },
        { (77, 80), "Teleport 2/day" },
        { (81, 85), "Wall of thorns 3/day" },
        { (86, 91), "Locate creature 3/day" },
        { (92, 97), "Fear 3/day" },
        { (98, 100), "Detect thoughts at will" },
    };

    private static readonly Dictionary<(int, int), string> purposeTable = new Dictionary<(int, int), string>
    {
        { (1, 20), "Overthrow an alignment" },
        { (21, 30), "Champion an alignment" },
        { (31, 40), "Find its creator" },
        { (41, 50), "Destroy its creator" },
        { (51, 55), "Defeat/slay a creature type" },
        { (56, 60), "Defeat/slay a race or kind" },
        { (61, 70), "Defend a race or kind" },
        { (71, 80), "Destroy a deity and/or its followers" },
        { (81, 90), "Defend a deity and/or its followers" },
        { (91, 95), "Destroy the world" },
        { (96, 100), "Castle Keeper's choice" },
    };

    //Rolls the CK's chance for a magic weapon to be sentient and, if it is, rolls its personality
    public static void MaybeMakeSentient(Item weapon)
    {
        if (ChancePercent <= 0) return;
        Console.WriteLine("Checking if the weapon is sentient (" + ChancePercent + "% chance)");
        if (!CommonUtils.RollPercentageSuccess(ChancePercent)) return;
        MakeSentient(weapon);
    }

    public static void MakeSentient(Item weapon)
    {
        string alignment = CommonUtils.RollOnTable(alignmentTable, "Sentient Item Alignment");
        WillEntry will = CommonUtils.RollOnTable(willTable, "Weapon Will");

        weapon.name += " (sentient)";
        weapon.AddNote("Sentient: alignment " + alignment + ", will " + will.will);
        weapon.AddNote("Communication: " + will.communication + Languages(will));
        weapon.AddNote("Senses: " + will.senses);

        foreach (string power in RollPowers(lesserPowerTable, "Lesser Powers", will.lesserPowers))
            weapon.AddNote("Lesser power: " + power);
        foreach (string power in RollPowers(greaterPowerTable, "Greater Powers", will.greaterPowers))
            weapon.AddNote("Greater power: " + power);
        if (will.readMagic) weapon.AddNote("Can read magic");

        //"Many sentient weapons, particularly those with wills greater than 20, have a special purpose"
        if (will.specialPurpose || will.will > 20)
        {
            weapon.AddNote("Special purpose: " + CommonUtils.RollOnTable(purposeTable, "Special Purpose"));
        }
    }

    private static string Languages(WillEntry will)
    {
        if (will.maxLanguages == 0) return "";
        if (will.maxLanguages < 0) return " (Common and any language)";
        int languages = CommonUtils.RollDie(will.maxLanguages);
        return " (Common plus " + languages + " other language" + (languages == 1 ? "" : "s") + ")";
    }

    //Duplicate powers are rerolled
    private static List<string> RollPowers(Dictionary<(int, int), string> table, string tableName, int count)
    {
        var powers = new List<string>();
        while (powers.Count < count)
        {
            string power = CommonUtils.RollOnTable(table, tableName);
            if (!powers.Contains(power)) powers.Add(power);
        }
        return powers;
    }
}
