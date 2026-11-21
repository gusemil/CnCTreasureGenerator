using System;
using System.Collections.Generic;

//Table 4.7: Rods, Staves and Wands (Monsters & Treasure page 328, descriptions pages 351-355).
//All of them have 50 charges when created, often fewer when found.
public static class RodsStavesWands
{
    private static readonly Dictionary<(int, int), Item> table = new Dictionary<(int, int), Item>
    {
        { (1, 4), I("Rod of Absorption", 50000, 8300, 351) },
        { (5, 5), I("Rod of Cancellation", 13500, 2250, 352) },
        { (6, 6), I("Rod of Lordly Might", 20500, 3400, 352) },
        { (7, 10), I("Rod of Negation", 9500, 1500, 352) },
        { (11, 14), I("Rod of the Python", 8750, 1400, 352) },
        { (15, 18), I("Rod of Rulership", 20000, 3200, 353) },
        { (19, 22), I("Rod of Thunder & Lightning", 29000, 4800, 353) },
        { (23, 26), I("Rod of Withering", 25000, 4000, 353) },
        { (27, 30), I("Rod of Wonder", 13500, 2250, 353) },
        { (31, 34), I("Staff of Abjuration", 54000, 9000, 354) },
        { (35, 38), I("Staff of Conjuration", 47000, 7800, 354) },
        { (39, 42), I("Staff of Evocation", 47000, 7880, 354) },
        { (43, 46), I("Staff of Fire", 23000, 3800, 354) },
        { (47, 50), I("Staff of Frost", 32500, 5400, 354) },
        { (51, 54), I("Staff of Healing", 31000, 5100, 354) },
        { (55, 58), I("Staff of Power", 72000, 12000, 354) },
        { (59, 59), I("Staff of Resurrection", 41000, 6800, 354) },
        { (60, 63), I("Staff of the Woodlands", 42000, 7000, 354) },
        { (64, 67), I("Wand of Color Spray", 12250, 2000, 355) },
        { (68, 71), I("Wand of Darkness", 12250, 2000, 355) },
        { (72, 75), I("Wand of Dispel Magic", 16750, 2800, 355) },
        { (76, 79), I("Wand of Illusion", 22000, 3600, 355) },
        { (80, 80), I("Wand of Invisibility", 31000, 5000, 355) },
        { (81, 84), I("Wand of Levitation", 26000, 4300, 355) },
        { (85, 88), I("Wand of Magic Missile", 12250, 2000, 355) },
        { (89, 92), I("Wand of Polymorph", 23000, 3800, 355) },
        { (93, 96), I("Wand of Restoration", 18000, 3000, 355) },
        { (97, 100), I("Wand of Suggestion", 14500, 2400, 355) },
    };

    public static Item Roll()
    {
        return CommonUtils.RollItem(table, "Table 4.7 Rods, Staves and Wands");
    }

    private static Item I(string name, int gp, int xp, int page)
    {
        return new Item("'" + name + "'", gp, xp, "", page) { subRoll = SubRolls.RollCharges };
    }
}
