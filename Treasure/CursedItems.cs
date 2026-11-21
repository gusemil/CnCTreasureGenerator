using System;
using System.Collections.Generic;

//Table 4.8: Cursed Items (Monsters & Treasure page 329, descriptions pages 355-357). No value or XP.
public static class CursedItems
{
    private static readonly Dictionary<(int, int), Item> table = new Dictionary<(int, int), Item>
    {
        { (1, 4), I("Amulet of Inescapable Location", 355) },
        { (5, 8), I("Armor of Rage", 355) },
        { (9, 12), I("Bag of Devouring", 355) },
        { (13, 16), I("Boots of Dancing", 355) },
        { (17, 20), I("Bracers of Defenselessness", 355) },
        { (21, 24), I("Flask of Curses", 355) },
        { (25, 28), I("Gauntlets of Fumbling", 355) },
        { (29, 32), I("Helm of Opposite Alignment", 355) },
        { (33, 36), I("Incense of Obsession", 356) },
        { (37, 40), I("Mace of Blood", 356) },
        { (41, 44), I("Medallion of Thought Projection", 356) },
        { (45, 48), I("Necklace of Strangulation", 356) },
        { (49, 52), I("Net of Snaring", 356) },
        { (53, 56), I("Periapt of Foul Rotting", 356) },
        { (57, 60), I("Potion of Poison", 356) },
        { (61, 64), I("Robe of Powerlessness", 356) },
        { (65, 68), I("Robe of Vermin", 356) },
        { (69, 72), I("Ring of Clumsiness", 356) },
        { (73, 76), I("Scarab of Death", 357) },
        { (77, 80), I("Spear, Cursed Backbiter", 357) },
        { (81, 84), I("Stone of Weight", 357) },
        { (85, 88), I("-2 Cursed Sword", 357) },
        { (89, 92), I("Sword, Berserking", 357) },
        { (93, 96), I("Vacuous Grimoire", 357) },
        { (97, 100), I("Zane's Ire", 357) },
    };

    public static Item Roll()
    {
        return CommonUtils.RollItem(table, "Table 4.8 Cursed Items");
    }

    private static Item I(string name, int page)
    {
        return new Item("'" + name + "' (cursed)", 0, 0, "", page);
    }
}
