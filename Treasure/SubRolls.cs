using System;
using System.Collections.Generic;

//Shared rolls used by several magic item tables: bonuses, sizes, charges and item-specific
//sub-tables from the item descriptions (Monsters & Treasure pages 332-364).
public static class SubRolls
{
    public struct BonusEntry
    {
        public int bonus;
        public int gp;
        public int xp;

        public BonusEntry(int bonus, int gp, int xp)
        {
            this.bonus = bonus;
            this.gp = gp;
            this.xp = xp;
        }
    }

    //Table 4.3B Weapon Bonus and Table 4.4E Armor Bonus (identical tables)
    private static readonly Dictionary<(int, int), BonusEntry> bonusTable = new Dictionary<(int, int), BonusEntry>
    {
        { (1, 45), new BonusEntry(1, 1000, 250) },
        { (46, 75), new BonusEntry(2, 4000, 750) },
        { (76, 90), new BonusEntry(3, 9000, 1200) },
        { (91, 98), new BonusEntry(4, 16000, 1750) },
        { (99, 100), new BonusEntry(5, 25000, 2500) },
    };

    public static BonusEntry RollBonus(string tableName = "Table 4.3B/4.4E Bonus")
    {
        BonusEntry entry = CommonUtils.RollOnTable(bonusTable, tableName);
        Console.WriteLine("Bonus is +" + entry.bonus);
        return entry;
    }

    public static BonusEntry BonusFor(int bonus)
    {
        foreach (BonusEntry entry in bonusTable.Values)
        {
            if (entry.bonus == bonus) return entry;
        }
        throw new ArgumentOutOfRangeException(nameof(bonus));
    }

    //Footnote value scale for miscellaneous magic with a +1 to +6 bonus (Table 4.5A-E)
    public static int MiscBonusGp(int bonus)
    {
        int[] gp = { 1000, 4000, 8000, 16000, 25000, 36000 };
        return gp[bonus - 1];
    }

    private static void SetBonus(Item item, int bonus, int gp, int xp)
    {
        item.name = item.name + " +" + bonus;
        item.gp = gp;
        item.xp = xp;
    }

    //Items with "+1 to +5" in their description. The bonus is rolled on Table 4.3B.
    public static void PlusOneToFive(Item item)
    {
        int bonus = RollBonus().bonus;
        SetBonus(item, bonus, MiscBonusGp(bonus), 500 * bonus);
    }

    //Items with "+2, +4 or +6" in their description (equal chance)
    public static void PlusTwoFourSix(Item item)
    {
        int bonus = CommonUtils.Pick(2, 4, 6);
        Console.WriteLine("Bonus is +" + bonus);
        SetBonus(item, bonus, MiscBonusGp(bonus), 500 * bonus);
    }

    //Manuals: "+1 to +5", listed at 10,000-60,000 gp
    public static void Manual(Item item)
    {
        int bonus = RollBonus().bonus;
        SetBonus(item, bonus, 10000 * bonus, 500 * bonus);
        item.AddNote("Value scaled at 10,000 gp per bonus point (book lists 10,000-60,000 gp)");
    }

    //"Armor and Weapon Sizes" (page 313): 01-30 small, 31-90 medium, 91-100 large
    public static void RollSize(Item item)
    {
        int roll = CommonUtils.RollPercentage();
        string size = roll <= 30 ? "Small" : roll <= 90 ? "Medium" : "Large";
        item.AddNote("Size: " + size);
    }

    //Rods, staves and wands have 50 charges when created, "though the number can be far less when actually discovered"
    public static void RollCharges(Item item)
    {
        int charges = CommonUtils.RollDie(50);
        item.AddNote("Charges remaining: " + charges + " (50 when created)");
    }

    //Potion of Giant Strength (page 330)
    public static void GiantStrengthPotion(Item item)
    {
        string[] giants = { "Hill: Str 19 (+3)", "Stone: Str 20 (+4)", "Frost: Str 21 (+4)", "Fire: Str 22 (+5)", "Cloud: Str 23 (+5)", "Storm: Str 24 (+6)" };
        int roll = CommonUtils.RollDie(6);
        item.AddNote("Giant type (d6 = " + roll + "): " + giants[roll - 1]);
    }

    //Belt of Giant Strength (page 335). Value 10,000-60,000 gp scales with the giant type.
    public static void GiantStrengthBelt(Item item)
    {
        string[] giants = { "Hill: Str 19 (+3)", "Frost: Str 20 (+4)", "Fire: Str 21 (+4)", "Stone: Str 22 (+5)", "Cloud: Str 23 (+5)", "Storm: Str 24 (+6)" };
        int roll = CommonUtils.RollDie(6);
        item.gp = 10000 * roll;
        item.AddNote("Giant type (d6 = " + roll + "): " + giants[roll - 1]);
    }

    //Luck Blade: "5% chance to contain 1-3 wishes"
    public static void LuckBladeWishes(Item item)
    {
        if (CommonUtils.RollPercentage() <= 5)
        {
            item.AddNote("Contains " + CommonUtils.RollDie(3) + " wish(es)");
        }
        else
        {
            item.AddNote("Contains no wishes");
        }
    }
}
