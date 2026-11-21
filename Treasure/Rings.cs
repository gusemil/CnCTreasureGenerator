using System;
using System.Collections.Generic;

//Table 4.6: Rings (Monsters & Treasure page 328, descriptions pages 348-351)
public static class Rings
{
    private static readonly Dictionary<(int, int), Item> ringTable = new Dictionary<(int, int), Item>
    {
        { (1, 4), R("Animal Friendship", 1650, 330, 348) },
        { (5, 8), R("Blinking", 9800, 1960, 348) },
        { (9, 12), R("Chameleon Power", 5800, 1160, 349) },
        { (13, 16), R("Climbing", 2800, 560, 349) },
        { (17, 20), R("Counter Spells", 5050, 1010, 349) },
        { (21, 21), R("Elemental Command", 45000, 9000, 349, item => item.AddNote("Element: " + CommonUtils.Pick("Air", "Earth", "Fire", "Water"))) },
        { (22, 25), R("Energy Resistance", 25000, 5000, 349, item => item.AddNote("Energy type: " + CommonUtils.Pick("Acid", "Cold", "Electricity", "Fire") + ". Minor/major/greater strength is the Castle Keeper's choice")) },
        { (26, 29), R("Evasion", 20000, 5000, 350) },
        { (30, 33), R("Feather Falling", 1550, 310, 350) },
        { (34, 37), R("Force Shield", 5050, 1010, 350) },
        { (38, 41), R("Freedom of Movement", 8300, 1660, 350) },
        { (42, 44), R("Friend Shield", 6050, 1210, 350) },
        { (45, 45), R("Gyges", 5800, 1200, 350) },
        { (46, 46), R("Invisibility", 5800, 1160, 350) },
        { (47, 50), R("Jumping", 1550, 310, 350) },
        { (51, 51), new Item("'Lancelot's Ring'", 8000, 3000, "", 350) },
        { (52, 54), R("Mind Shielding", 5800, 1160, 350) },
        { (55, 58), R("Protection", 0, 0, 350, RingOfProtection) },
        { (59, 62), R("the Ram", 5800, 1160, 350) },
        { (63, 63), R("Regeneration", 24050, 4810, 350) },
        { (64, 67), R("Shooting Stars", 9300, 1860, 350) },
        { (68, 71), R("Spell Storing", 0, 0, 351, SpellStoring) },
        { (72, 75), R("Spell Turning", 41800, 8360, 351) },
        { (76, 79), R("Sustenance", 2800, 560, 351) },
        { (80, 83), R("Swimming", 1300, 260, 351) },
        { (84, 87), R("Telekinesis", 12550, 2510, 351) },
        { (88, 88), R("Three Wishes", 114750, 5000, 351, item => item.AddNote("Rubies (wishes) remaining: " + CommonUtils.RollDie(3))) },
        { (89, 92), R("Water Walking", 5800, 1160, 351) },
        { (93, 96), R("Wizardry", 0, 0, 351, Wizardry) },
        { (97, 100), R("X-Ray Vision", 9300, 930, 351) },
    };

    public static Item Roll()
    {
        return CommonUtils.RollItem(ringTable, "Table 4.6 Rings");
    }

    private static Item R(string name, int gp, int xp, int page, Action<Item> subRoll = null)
    {
        return new Item("'Ring of " + name + "'", gp, xp, "", page) { subRoll = subRoll };
    }

    //Bonus rolled on Table 4.3B. XP is 20% of the value.
    private static void RingOfProtection(Item item)
    {
        int bonus = SubRolls.RollBonus().bonus;
        int[] gp = { 2300, 5300, 11300, 17300, 26300 };
        item.name = item.name.TrimEnd('\'') + " +" + bonus + "'";
        item.gp = gp[bonus - 1];
        item.xp = item.gp / 5;
    }

    private static void SpellStoring(Item item)
    {
        (string type, int gp, string levels)[] rings = { ("Minor", 5050, "3 levels of spells"), ("Standard", 12550, "5 levels of spells"), ("Major", 38000, "10 levels of spells") };
        var ring = CommonUtils.Pick(rings);
        item.name = item.name.Replace("Ring of", ring.type + " Ring of");
        item.gp = ring.gp;
        item.xp = ring.gp / 5;
        item.AddNote(ring.type + " ring: stores up to " + ring.levels);
    }

    private static void Wizardry(Item item)
    {
        (string type, int level, int gp)[] rings = { ("Minor", 1, 20000), ("Major", 2, 40000), ("Greater", 3, 70000), ("Arch", 4, 100000) };
        var ring = CommonUtils.Pick(rings);
        item.name = item.name.TrimEnd('\'') + " (" + ring.type + ")'";
        item.gp = ring.gp;
        item.xp = 1000 * ring.level;
        item.AddNote("Doubles the wearer's level " + ring.level + " spells");
    }
}
