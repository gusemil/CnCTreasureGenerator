using System;
using System.Collections.Generic;
using System.Linq;

public class MagicItem : Item
{
    public bool isRollItemBonus;
    public Dictionary<(int, int), MagicItems.MagicItemType> optionalTable;
    public string optionalTableName;

    public MagicItem(string name, int gpValue, int xp = 0, string description = "", int pageNumber = 0, bool isRollValueTable = false, Roll rollGpValue = null, Roll rollAmount = null, bool isRollItemBonus = false, Dictionary<(int,int), MagicItems.MagicItemType> optionalTable = null, string optionalTableName = "")
        : base(name, gpValue, xp, description, pageNumber, isRollValueTable, rollGpValue, rollAmount)
    {
        this.isRollItemBonus = isRollItemBonus;
        this.optionalTable = optionalTable;
        this.optionalTableName = optionalTableName;
    }

    public override void RollDetails()
    {
        base.RollDetails();
        RollItemBonus();
        RollOptionalTable();
    }

    //Table 4.3B: adds the bonus to the name ("Long Sword +2", or replaces "+X") and its value and XP
    public void RollItemBonus()
    {
        if (!isRollItemBonus) return;

        SubRolls.BonusEntry itemBonus = SubRolls.RollBonus();
        gp += itemBonus.gp;
        xp += itemBonus.xp;
        if (name.Contains("+X")) name = name.Replace("+X", "+" + itemBonus.bonus);
        else name += " +" + itemBonus.bonus;
    }

    //Optional sub-table from the item description (bane foe d12, dragon type d10)
    public void RollOptionalTable()
    {
        if (optionalTable == null) return;

        int sides = optionalTable.Keys.Max(k => k.Item2);
        int roll = CommonUtils.RollDie(sides);
        MagicItems.MagicItemType entry = CommonUtils.Lookup(optionalTable, roll, optionalTableName);
        AddNote(optionalTableName + " (d" + sides + " = " + roll + "): " + entry.subTableDescription);
    }
}
