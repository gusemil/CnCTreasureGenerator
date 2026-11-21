using System;
using System.Collections.Generic;

//Table 4.9: Artifacts (Monsters & Treasure page 329, descriptions pages 357-364).
//Artifacts are priceless and give no XP. They count as over any max XP limit on Table 1.0.
public static class Artifacts
{
    private static readonly Dictionary<(int, int), Item> table = new Dictionary<(int, int), Item>
    {
        { (1, 3), A("Aegis", 357) },
        { (4, 5), A("Andvaranaut", 357) },
        { (6, 8), A("Angurvadal", 357) },
        { (9, 10), A("Book of Infinite Spells", 357) },
        { (11, 13), A("Caduceus", 358) },
        { (14, 15), A("Carnwennan", 358) },
        { (16, 18), A("Chentu", 358) },
        { (19, 20), A("Cowl of the Black Fox", 358) },
        { (21, 23), A("Deck of Many Things", 358) },
        { (24, 25), A("Excalibur", 360) },
        { (26, 28), A("Fail-Not Bow of Tristan", 360) },
        { (29, 31), A("Hammer of Thunderbolts", 360) },
        { (32, 35), A("Hand of Glory", 360) },
        { (36, 37), A("Jarrod's Blade", 360) },
        { (38, 40), A("Kladenets", 360) },
        { (41, 42), A("Lantern of Diogenes", 360) },
        { (43, 45), A("Mace of the Dove", 361) },
        { (46, 47), A("Mantle of Lorenzo", 361) },
        { (48, 50), A("Mjolnir", 361) },
        { (51, 53), A("Moaning Diamond", 361) },
        { (54, 55), A("Orbs of Dragonkind", 361) },
        { (56, 58), A("Philosopher's Stone", 361) },
        { (59, 60), A("Ruji Jingu Bang", 362) },
        { (61, 63), A("Saint's Mace", 362) },
        { (64, 65), A("Shadowstaff", 362) },
        { (66, 68), A("Sharanga", 362) },
        { (69, 70), A("Shield of Maccabee", 362) },
        { (71, 73), A("Shield of the Sun", 362) },
        { (74, 76), A("Sphere of Annihilation", 362) },
        { (77, 78), A("Staff of the Magi", 363) }, //Book lists 76-78, overlapping the Sphere of Annihilation
        { (79, 80), A("Sword of Conaire Mor", 363) },
        { (81, 83), A("Sword of Tarnhelm", 363) },
        { (84, 86), A("Talisman of Pure Good", 363) },
        { (87, 89), A("Talisman of the Sphere", 363) },
        { (90, 92), A("Talisman of Ultimate Evil", 364) },
        { (93, 94), A("Tarnhelm", 364) },
        { (95, 97), A("Tome of the Unclean", 364) },
        { (98, 98), A("Tyrfing", 364) },
        { (99, 99), A("Uchide No Kozuchi", 364) },
        { (100, 100), A("Varunastra", 364) },
    };

    public static Item Roll()
    {
        return CommonUtils.RollItem(table, "Table 4.9 Artifacts");
    }

    private static Item A(string name, int page)
    {
        return new Item("'" + name + "' (artifact)", 0, 0, "", page) { isPriceless = true };
    }
}
