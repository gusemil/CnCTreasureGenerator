using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MagicItems
{
    public Dictionary<int, MagicItemsTable> magicItemsDictionary = null;
    private Dictionary<(int, int), MagicItemType> magicItemTypesDictionary = null;
    private Dictionary<(int, int), Item> potionsDictionary = null;
    private Dictionary<(int, int), Item> scrollsDictionary = null;
    private Dictionary<(int, int), MagicItemType> magicWeaponTypeDictionary = null;
    private Dictionary<(int, int), MagicItemType> magicSwordTypeDictionary = null;
    private Dictionary<(int, int), MagicItem> specialSwordDictionary = null;
    private Dictionary<(int, int), MagicItemType> magicMiscWeaponTypeDictionary = null;
    private Dictionary<(int, int), MagicItem> specialMiscWeaponDictionary = null;

    private Dictionary<(int, int), MagicItemType> baneDictionary = null;
    private Dictionary<(int, int), MagicItemType> dragonDictionary = null;

    public MagicItems()
    {
        if (magicItemsDictionary != null) return;
        magicItemsDictionary = new Dictionary<int, MagicItemsTable>();
        magicItemsDictionary.Add(0, new MagicItemsTable(5, new Roll(1, 1), 100));
        magicItemsDictionary.Add(1, new MagicItemsTable(10, new Roll(1, 1), 200));
        magicItemsDictionary.Add(2, new MagicItemsTable(15, new Roll(1, 1), 300));
        magicItemsDictionary.Add(3, new MagicItemsTable(20, new Roll(1, 1), 500));
        magicItemsDictionary.Add(4, new MagicItemsTable(30, new Roll(1, 2), 800));
        magicItemsDictionary.Add(5, new MagicItemsTable(40, new Roll(1, 2), 1300));
        magicItemsDictionary.Add(6, new MagicItemsTable(50, new Roll(1, 2), 2100));
        magicItemsDictionary.Add(7, new MagicItemsTable(60, new Roll(1, 2), 3400));
        magicItemsDictionary.Add(8, new MagicItemsTable(70, new Roll(1, 3), 5500));
        magicItemsDictionary.Add(9, new MagicItemsTable(80, new Roll(1, 3)));
        magicItemsDictionary.Add(10, new MagicItemsTable(90, new Roll(1, 3)));
        magicItemsDictionary.Add(11, new MagicItemsTable(92, new Roll(1, 3)));
        magicItemsDictionary.Add(12, new MagicItemsTable(94, new Roll(1, 4)));
        magicItemsDictionary.Add(13, new MagicItemsTable(96, new Roll(1, 4)));
        magicItemsDictionary.Add(14, new MagicItemsTable(98, new Roll(1, 4)));
        magicItemsDictionary.Add(15, new MagicItemsTable(99, new Roll(1, 4)));
        magicItemsDictionary.Add(16, new MagicItemsTable(100, new Roll(1, 6)));
        magicItemsDictionary.Add(17, new MagicItemsTable(100, new Roll(1, 6)));
    }

    private void InitMagicItems()
    {
        if (magicItemTypesDictionary != null) return;
        magicItemTypesDictionary = new Dictionary<(int, int), MagicItemType>();
        magicItemTypesDictionary.Add((1, 15), new MagicItemType(0, "'Potions'"));
        magicItemTypesDictionary.Add((16, 30), new MagicItemType(1, "'Scroll'"));
        magicItemTypesDictionary.Add((31, 45), new MagicItemType(2, "'Weapon'"));
        magicItemTypesDictionary.Add((46, 60), new MagicItemType(3, "'Armor & Shields'"));
        magicItemTypesDictionary.Add((61, 80), new MagicItemType(4, "'Miscellaneous Magic'"));
        magicItemTypesDictionary.Add((81, 90), new MagicItemType(5, "'Rings'"));
        magicItemTypesDictionary.Add((91, 97), new MagicItemType(6, "'Rods, Staves, Wands'"));
        magicItemTypesDictionary.Add((98, 99), new MagicItemType(7, "'Cursed Items'"));
        magicItemTypesDictionary.Add((100, 100), new MagicItemType(8, "'Artifacts'"));
    }

    private Dictionary<(int, int), Item> InitPotions()
    {
        if (potionsDictionary != null) return potionsDictionary;
        potionsDictionary = new Dictionary<(int, int), Item>();

        potionsDictionary.Add((1, 3), new Item("'Aid'", 400, 200));
        potionsDictionary.Add((4, 6), new Item("'Bless oil'", 300, 100));
        potionsDictionary.Add((7, 9), new Item("'Blur", 400, 200));
        potionsDictionary.Add((10, 12), new Item("'Clairaudience/Clairvoyance'", 500, 300));
        potionsDictionary.Add((13, 15), new Item("'Cure Light Wounds'", 300, 100));
        potionsDictionary.Add((16, 18), new Item("'Cure Serious Wounds'", 500, 300));
        potionsDictionary.Add((19, 21), new Item("'Cure Critical Wounds'", 700, 500));
        potionsDictionary.Add((22, 24), new Item("'Delay Poison'", 400, 200));
        potionsDictionary.Add((25, 27), new Item("'Endure Elements'", 300, 100));
        potionsDictionary.Add((28, 30), new Item("'Fly'", 500, 300));
        potionsDictionary.Add((31, 33), new Item("'Gaseous Form'", 500, 300));
        potionsDictionary.Add((34, 36), new Item("'Giant Strength'", 700, 500, "Strength becomes 19-24 (+3 to +6) for 10 minutes", 330) { subRoll = SubRolls.GiantStrengthPotion });
        potionsDictionary.Add((37, 39), new Item("'Haste'", 500, 300));
        potionsDictionary.Add((40, 42), new Item("'Heal'", 800, 600));
        potionsDictionary.Add((43, 45), new Item("'Invisibility (potion or oil)'", 400, 200));
        potionsDictionary.Add((46, 48), new Item("'Levitation (potion or oil)'", 400, 200));
        potionsDictionary.Add((49, 51), new Item("'Longevity'", 12000, 1500, "Exilir of youth: 2-12 off age. A small chance to reverse effect and age 1-6 years. CK must determine this chance on campaign and situation"));
        potionsDictionary.Add((52, 54), new Item("'Neutralize Poison'", 600, 400));
        potionsDictionary.Add((55, 57), new Item("'Nondetection'", 500, 300));
        potionsDictionary.Add((58, 60), new Item("'Pass without Trace'", 300, 100));
        potionsDictionary.Add((61, 63), new Item("'Protection from Alignment'", 300, 100, "Character gains +2 AC and Saves against alignment of CK's choice. Lasts 2 rounds per level of the creator"));
        potionsDictionary.Add((64, 66), new Item("'Protection from Arrows'", 500, 300));
        potionsDictionary.Add((67, 69), new Item("'Remove Blindess/deafness'", 500, 300));
        potionsDictionary.Add((70, 72), new Item("'Remove Curse'", 500, 300));
        potionsDictionary.Add((73, 75), new Item("'Remove Disease'", 500, 300));
        potionsDictionary.Add((76, 78), new Item("'Remove Paralysis'", 400, 200));
        potionsDictionary.Add((79, 81), new Item("'Restoration'", 700, 400));
        potionsDictionary.Add((82, 84), new Item("'Sanctuary'", 300, 100));
        potionsDictionary.Add((85, 87), new Item("'Shield of Faith +2'", 300, 100));
        potionsDictionary.Add((88, 90), new Item("'Spider Climb'", 300, 100));
        potionsDictionary.Add((91, 93), new Item("'Tongues'", 500, 300));
        potionsDictionary.Add((94, 96), new Item("'Water Breathing'", 500, 300));
        potionsDictionary.Add((97, 99), new Item("'Water Walk'", 900, 700));
        potionsDictionary.Add((100, 100), new Item("'Trap the Soul'", 1100, 900, "Greenish, thick liquid, usually held in a crystal jar. Within the jar is a small topaz gem. The potion has no taste. Consuming the potion forces the user into the gem as if by a trap the soul spell"));

        return potionsDictionary;
    }

    private Dictionary<(int, int), Item> InitScrolls()
    {
        if (scrollsDictionary != null) return scrollsDictionary;
        scrollsDictionary = new Dictionary<(int, int), Item>();

        //"N Spell Levels": random spells whose levels add up to N, from the Players Handbook spell lists
        int[] scrollGp = { 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700 };
        (int, int)[] scrollRanges = { (1, 8), (9, 16), (17, 24), (25, 32), (33, 40), (41, 45), (46, 50), (51, 55), (56, 60), (61, 65), (66, 68), (69, 71), (72, 74), (75, 77), (78, 80) };
        for (int i = 0; i < scrollRanges.Length; i++)
        {
            int spellLevels = i + 1;
            string name = "'" + spellLevels + (spellLevels == 1 ? " Spell Level'" : " Spell Levels'");
            scrollsDictionary.Add(scrollRanges[i], new Item(name, scrollGp[i], scrollGp[i] - 200) { subRoll = scroll => ScrollSpells.Fill(scroll, spellLevels) });
        }
        scrollsDictionary.Add((81, 82), new Item("'Teleport without Error'", 900, 700));
        scrollsDictionary.Add((83, 84), new Item("'Symbol'", 1000, 800));
        scrollsDictionary.Add((85, 86), new Item("'Trap the Soul'", 1100, 900));
        scrollsDictionary.Add((87, 88), new Item("'Time Stop'", 1100, 900));
        scrollsDictionary.Add((89, 90), new Item("'True Resurrection'", 1100, 900));
        scrollsDictionary.Add((91, 92), new Item("'Mass Heal'", 1000, 800));
        scrollsDictionary.Add((93, 94), new Item("'Gate'", 1100, 900));
        scrollsDictionary.Add((95, 96), new Item("'Create Greater Undead'", 1000, 800));
        scrollsDictionary.Add((97, 98), new Item("'Shape Change'", 1100, 900));
        scrollsDictionary.Add((99, 100), new Item("'Clone'", 1100, 900));

        return scrollsDictionary;
    }

    private void InitMagicWeaponType()
    {
        if (magicWeaponTypeDictionary != null) return;
        magicWeaponTypeDictionary = new Dictionary<(int, int), MagicItemType>();

        magicWeaponTypeDictionary.Add((1, 40), new MagicItemType(0, "Swords"));
        magicWeaponTypeDictionary.Add((41, 50), new MagicItemType(1, "Special Sword"));
        magicWeaponTypeDictionary.Add((51, 90), new MagicItemType(2, "Miscellaneous Weapons"));
        magicWeaponTypeDictionary.Add((91, 100), new MagicItemType(3, "Special Weapon"));
    }

    private void InitMagicSwordType()
    {
        if (magicSwordTypeDictionary != null) return;
        magicSwordTypeDictionary = new Dictionary<(int, int), MagicItemType>();

        magicSwordTypeDictionary.Add((1, 10), new MagicItemType(0, "Bastard Sword"));
        magicSwordTypeDictionary.Add((11, 30), new MagicItemType(1, "Broad Sword, Falchion"));
        magicSwordTypeDictionary.Add((31, 50), new MagicItemType(2, "Short Sword, Scimitar, Rapier"));
        magicSwordTypeDictionary.Add((51, 90), new MagicItemType(3, "Long Sword"));
        magicSwordTypeDictionary.Add((91, 100), new MagicItemType(4, "Two Handed Sword"));
    }

        private void InitMagicMiscWeaponType()
    {
        if (magicMiscWeaponTypeDictionary != null) return;
        magicMiscWeaponTypeDictionary = new Dictionary<(int, int), MagicItemType>();

        magicMiscWeaponTypeDictionary.Add((1, 2), new MagicItemType(0, "10 Arrow"));
        magicMiscWeaponTypeDictionary.Add((3, 8), new MagicItemType(1, "Axe, Battle"));
        magicMiscWeaponTypeDictionary.Add((9, 12), new MagicItemType(2, "Axe, hand/throwing"));
        magicMiscWeaponTypeDictionary.Add((13, 16), new MagicItemType(2, "Axe, two-handed"));
        magicMiscWeaponTypeDictionary.Add((17, 20), new MagicItemType(3, "Bardiche"));
        magicMiscWeaponTypeDictionary.Add((21, 24), new MagicItemType(4, "10 Bolt"));
        magicMiscWeaponTypeDictionary.Add((25, 28), new MagicItemType(5, "Bow"));
        magicMiscWeaponTypeDictionary.Add((29, 32), new MagicItemType(6, "Club"));
        magicMiscWeaponTypeDictionary.Add((33, 36), new MagicItemType(7, "Crossbow"));
        magicMiscWeaponTypeDictionary.Add((37, 40), new MagicItemType(8, "Crowbill (Lucerne)"));
        magicMiscWeaponTypeDictionary.Add((41, 44), new MagicItemType(9, "Dagger"));
        magicMiscWeaponTypeDictionary.Add((45, 48), new MagicItemType(10, "Dart"));
        magicMiscWeaponTypeDictionary.Add((49, 52), new MagicItemType(11, "Flail"));
        magicMiscWeaponTypeDictionary.Add((53, 56), new MagicItemType(12, "Halberd"));
        magicMiscWeaponTypeDictionary.Add((57, 60), new MagicItemType(2, "Hammer"));
        magicMiscWeaponTypeDictionary.Add((61, 64), new MagicItemType(2, "Javelin"));
        magicMiscWeaponTypeDictionary.Add((65, 68), new MagicItemType(2, "Lance"));
        magicMiscWeaponTypeDictionary.Add((69, 72), new MagicItemType(2, "Mace"));
        magicMiscWeaponTypeDictionary.Add((73, 76), new MagicItemType(2, "Morningstar"));
        magicMiscWeaponTypeDictionary.Add((77, 80), new MagicItemType(2, "Pole arm"));
        magicMiscWeaponTypeDictionary.Add((81, 84), new MagicItemType(2, "Sling"));
        magicMiscWeaponTypeDictionary.Add((85, 88), new MagicItemType(2, "Spear"));
        magicMiscWeaponTypeDictionary.Add((89, 92), new MagicItemType(2, "Staff"));
        magicMiscWeaponTypeDictionary.Add((93, 96), new MagicItemType(2, "Trident"));
        magicMiscWeaponTypeDictionary.Add((97, 100), new MagicItemType(2, "Whip"));
    }

    private void InitSpecialSword()
    {
        if (specialSwordDictionary != null) return;
        specialSwordDictionary = new Dictionary<(int, int), MagicItem>();

        specialSwordDictionary.Add((1, 8),  new MagicItem("Bane Sword", 13500, 4500,
            "Normally +1 weapon but against designated foe a +3 weapon with extra 2d6 damage", 330, false, null, null, false, InitBaneWeapon(), "Bane foe"));
        specialSwordDictionary.Add((9, 16), new MagicItem("Sword of Dancing", 12500, 4100, "TODO", 331));
        specialSwordDictionary.Add((17, 24), new MagicItem("Sword of Defending +4", 24500, 8100, 
            "Allows the wielder to transfer all or any of the sword's magical bonus to AC. Any bonus transfered is not added to the attack roll", 331));
        specialSwordDictionary.Add((25, 32), new MagicItem("Dragon Slayer +2/+4", 29000, 7250,
            "Normally +2 weapon, but against dragons of the rolled type +4 BtH and double damage", 331, false, null, null, false, InitDragonType(), "Dragon type"));
        specialSwordDictionary.Add((33, 36), new MagicItem("Featheredged Sword", 31750, 15000,
    "+3 to hit and +1 to damage. A non modified roll of 18-20 severs a limb/hand/foot or head. Check table on page 331.", 331));
        specialSwordDictionary.Add((37, 44), new MagicItem("Flaming Sword +2", 7750, 2000,
    "+2 weapon. Deals +1d6 extra damage on hit, +2d6 extra damage on cold based creatures", 331));
        specialSwordDictionary.Add((45, 52), new MagicItem("Frostbane/Frostbrand +3", 13725, 3000,
"+3 icy weapon. Sheds light as a torch on 0F //TODO insert celcius temperature. 10 points of fire resistance each round. Deals double damage against fire-based creatures", 331));
        specialSwordDictionary.Add((53, 56), new MagicItem("Holy Avenger +2/+5", 55000, 27000,
"+2 weapon iron sword becomes +5 holy sword in the hands of a paladin. Deals double damage on targets of evil alignment. Provides bonus spell resistance equal to paladin's level + 5 to the wielder and anyone adjacent to her." +
"enables wielder to use dispel magic (once per round as a normal action) at the class level of the paladin. It inflicts 2d20 points of damage to any evil aligned creature that attempts to wield it", 331));
        specialSwordDictionary.Add((57, 60), new MagicItem("Sword of Life Stealing +2", 17000, 5600,
"+2 ebony iron sword. Upon natural 20 weapon drains a level from its victim and grants 1d6 temporary hitpoints to the wielder. These temporary hit points last one day. Victim who survives" +
"such a strike from a life stealer must make a CON save for each level lost. If successful, the level will return in 1d4 days or with a restoratio spell. If failed, the level is lost permanently and the victim's experience points are" +
"" +
"moved to the mid point of points requried for the previous level (as if struck by undead)", 331));
        specialSwordDictionary.Add((61, 68), new MagicItem("Luck Blade +2", 21500, 7100,
"Grants +1 luck bonus on all saves. The possessor gains the benefit of 'Good Fortune' usable once a day. The extraordinary ability allows any single roll to be re-rolled. Blade has a 5% chance to contain 1-3 wishes. When the last wish" +
"is used the sword retains all of its other abilities and properties.", 331) { subRoll = SubRolls.LuckBladeWishes });
        specialSwordDictionary.Add((69, 72), new MagicItem("Nine Lives Stealer +2", 8500, 2800,
"+2 Sword. On a natural 20 to hit, the victim of this blade must make a CON save (CL 2) or be struck dead. It can do this nine times before the ability is lost. At that point the sword becomes a simple +2 Sword (with a hint of evil about it)." +
"Any good creature who wields the sword suffers a -2 to their BtH. No spell can reverse this penalty. Only abjuring the weapon itself will negate the penalties.", 331));
        specialSwordDictionary.Add((73, 80), new MagicItem("Sword of Puncturing +2", 12000, 3000,
"+2 wounding sword allows the wielder to inflict 1d6 CON damage 3 times per day (by draining blood from its victim) with a successful hit.", 331));
        specialSwordDictionary.Add((81, 88), new MagicItem("Sylvan Sword +3", 10000, 3000,
"+3 blade whren used outdoors in a woodland climate, inflicts and additional 1d6 damage on a hit. If target is reduced to 0 hitpoints or less by a single strike, the wielder of the sword gains a free attack on another single target within melee range of the sword's wielder.", 331));
        specialSwordDictionary.Add((89, 92), new MagicItem("Vorpal Sword +4", 38750, 19000,
"+4 weapon. Has the unique ability to decapitate those it strikes. On a natural 20, the weapon severs the opponent's head (if it has one). Undeads, golems or headless creatures are not affected by decapitation. For vampires and most creatures, decapitation results in an instant death.", 331));
        specialSwordDictionary.Add((93, 100), new MagicItem("Sword of Wounding +3", 16000, 5300,
"Every time a creature is struck by this sword, in addition to normal damage the creature loses additional hit points from the blood loss equal to the maximum damage of the sword, including the +3 bonus." +
"For example a long sword of wounding inflicts 11 points of blood loss damage. The victim suffers 1 point of damage per round until 11 points of daamge are dealt or a healing spell is cast to stop the loss.", 332));
    }

    private void InitSpecialMiscWeapon()
    {
        if (specialMiscWeaponDictionary != null) return;
        specialMiscWeaponDictionary = new Dictionary<(int, int), MagicItem>();

        specialMiscWeaponDictionary.Add((1, 8),  new MagicItem("Bane Weapon", 13500, 4500, 
            "Normally +1 weapon but against designated foe a +3 weapon with extra 2d6 damage", 330, false, null, null, false, InitBaneWeapon(), "Bane foe") { subRoll = AddMiscWeaponType });
        specialMiscWeaponDictionary.Add((9, 11), new MagicItem("Club of Dagda +X", 10000, 4000, "User can declare a critical hit once per day. Can also heal 20 hitpoints a day.", 333)
            { subRoll = club => club.name = club.name.Replace("+X", "+" + SubRolls.RollBonus().bonus) }); //Bonus from Table 4.3B, value stays as listed
        specialMiscWeaponDictionary.Add((12, 15), new MagicItem("Dagger of Venom +1", 4750, 1070, "+1 Dagger, can inject poison once per day", 333));
        specialMiscWeaponDictionary.Add((16, 24), new MagicItem("Dwarven Thrower +2/+3", 18000, 6000, "+2 Warhammer, but +3 in hands of Dwarf. If thrown by a dwarf, the hammer gains the ability to return to its owner. Can be hurled a distance of 30ft. When hurled against giants it deals an extra 2d8 damage or an extra 1d8 against any other target. Unless obstructed, it always returns to its owner.", 333));
        specialMiscWeaponDictionary.Add((25, 28), new MagicItem("Featheredged Axe +3", 31750, 15000, "Has bonus of +3 to hit and +1 damage, on a non-modified roll of 18-20 it severs a limb/hand/foot/head", 333));
        specialMiscWeaponDictionary.Add((29, 36), new MagicItem("Javelin of Lightning", 4500, 450, "Javelin becomes a 5d6 Lightning Bolt when thrown (Victim makes a dex save for half damage). It is consumed in the attack.", 333));
        specialMiscWeaponDictionary.Add((37, 40), new MagicItem("Mace (Hammer) of Disruption +3", 25500, 8500, "+3 weapon that deals double damage to undead. Upon successfu hit, the undead must make a physical save or be destroyed. Other undead near the target are affected as if turned by a 10th level Cleric (i.e. multiple lower hit dice undead can thus be destroyed with a single strike)", 333));
        specialMiscWeaponDictionary.Add((41, 48), new MagicItem("Mace of Smiting +3", 17500, 5800, "+3 Heavy Mace has a +5 enchantment bonus against constructs, and any roll of natural 20 dealt to a construct shatters it, completely destroying its form. A natural 20 dealt to an extraplanar creature deals 4x damage.", 333));
        specialMiscWeaponDictionary.Add((49, 56), new MagicItem("Mace of Terror +2", 8500, 2100, "On command this +2 mace causes the wielder's clothes and appearance to transform into an illusion of darkest horror. Living creatures in a 30 foot radius become panicked as if by a cause fear spell unless a successful charisma save is made.", 333));
        specialMiscWeaponDictionary.Add((57, 60), new MagicItem("Nine Lives Stealer +2", 8500, 2800, "Read page 333 description", 333));
        specialMiscWeaponDictionary.Add((61, 64), new MagicItem("Oathbow +2/+5", 25000, 5000, "Read page 333 description", 333));
        specialMiscWeaponDictionary.Add((65, 68), new MagicItem("Slaying Arrow/Bolt", 9500, 2300, "+1 arrow or bolt magically attuned to a particular type or subtype of creature. If strikes such a creature target must make a constitution save or die instantly. The CK determines the particular type of creature it slays.", 333));
        specialMiscWeaponDictionary.Add((69, 76), new MagicItem("Sleep Arrow/Bolt", 1250, 350, "+1 Arrow/bolt deals normal damage but converted to non-lethal damage. Also forces the target to make a wisdom save or fall asleep with a duration identical to the sleep spell.", 333));
        specialMiscWeaponDictionary.Add((77, 84), new MagicItem("Sylvan Weapon +3", 10000, 3000, "If used in outdoors in a woodland climate, inflicts an additional 1d6 dmg on hit. If a target is reduced to 0 hitpoints or less by a single strike, wielder of the weapon gains a free attack on another single target within melee range of the weapon's wielder.", 333));
        specialMiscWeaponDictionary.Add((85, 92), new MagicItem("Trident of Fish Command +1", 5000, 1200, "Allows the wielder to charm (as the spell) up to 14 HD of aquatic animals, two of which can't be more than 30ft apart. Can be used 3 times per day. Wielder can communicate with the animals as if Speak with animals spell. Animals making their saving throw are free of control.", 334));
        specialMiscWeaponDictionary.Add((93, 100), new MagicItem("Weapon of Wounding +3", 16000, 5300, "Every time a creature is struck with this weapon, it loses additional hit points due blood loss equal to max damage of the weapon including the +3 bonus. This damage must be healed naturally. A healing spell with stop the blood loss but does not heal the damage.", 334) { subRoll = AddMiscWeaponType });
    }
    //Table 4.0: rolls each magic item, rerolling anything worth more XP than the treasure level's
    //"Max. XP Value of Item" (Table 1.0). Artifacts are priceless, so they never fit under a limit.
    public void RollItemType(int itemsAmount, int maxXp = 0)
    {
        InitMagicItems();
        for (int i = 0; i < itemsAmount; i++)
        {
            Console.WriteLine("\nMagic item " + (i + 1) + " of " + itemsAmount);
            Item item = RollItemWithinXpLimit(maxXp);
            Console.WriteLine();
            item.PrintInfo();
            TreasureReport.AddMagicItem(item);
        }
    }

    private Item RollItemWithinXpLimit(int maxXp)
    {
        const int MAX_ATTEMPTS = 100;
        for (int attempt = 1; ; attempt++)
        {
            Item item = RollOneMagicItem();
            bool overLimit = maxXp > 0 && (item.isPriceless || item.xp > maxXp);
            if (!overLimit || attempt == MAX_ATTEMPTS) return item;
            Console.WriteLine(item.name + " (" + (item.isPriceless ? "artifact" : item.xp + " xp") + ") is over the "
                + maxXp + " xp limit for this treasure level. Rerolling...\n");
        }
    }

    public Item RollOneMagicItem()
    {
        InitMagicItems();
        MagicItemType itemType = CommonUtils.RollOnTable(magicItemTypesDictionary, "Table 4.0 Magic Item Generation");
        Console.WriteLine("Magic item type is " + itemType.subTableDescription);

        switch (itemType.subTableIndex)
        {
            case 0: return WithPrefix(CommonUtils.RollItem(InitPotions(), "Table 4.1 Potions"), "Potion of ");
            case 1: return WithPrefix(CommonUtils.RollItem(InitScrolls(), "Table 4.2 Scrolls"), "Scroll of ");
            case 2: return RollWeapon();
            case 3: return ArmorAndShields.Roll();
            case 4: return MiscMagic.Roll();
            case 5: return Rings.Roll();
            case 6: return RodsStavesWands.Roll();
            case 7: return CursedItems.Roll();
            case 8: return Artifacts.Roll();
            default: throw new InvalidOperationException("Unknown magic item table " + itemType.subTableIndex);
        }
    }

    //Potion and scroll table entries are just the effect ("Sanctuary", "9 Spell Levels"), so name the item type
    private static Item WithPrefix(Item item, string prefix)
    {
        item.name = item.name.StartsWith("'") ? "'" + prefix + item.name.Substring(1) : prefix + item.name;
        return item;
    }

    //Table 4.3: every magic weapon also gets a size and may be sentient
    private Item RollWeapon()
    {
        InitMagicWeaponType();
        MagicItemType weaponType = CommonUtils.RollOnTable(magicWeaponTypeDictionary, "Table 4.3 Weapons");
        Item weapon;

        switch (weaponType.subTableIndex)
        {
            case 0:
                weapon = RollBonusWeapon(RollSwordType(), 324);
                break;
            case 1:
                InitSpecialSword();
                weapon = CommonUtils.RollItem(specialSwordDictionary, "Table 4.3C Special Swords");
                weapon.AddNote("Sword type: " + RollSwordType());
                break;
            case 2:
                weapon = RollBonusWeapon(RollMiscWeaponType(), 325);
                break;
            default:
                InitSpecialMiscWeapon();
                weapon = CommonUtils.RollItem(specialMiscWeaponDictionary, "Table 4.3E Special Miscellaneous Weapons");
                break;
        }

        SubRolls.RollSize(weapon);
        SentientItems.MaybeMakeSentient(weapon);
        return weapon;
    }

    //Table 4.3A/4.3D weapon with its bonus from Table 4.3B
    private Item RollBonusWeapon(string weaponName, int pageNumber)
    {
        MagicItem weapon = new MagicItem(weaponName, 0, 0, "", pageNumber, false, null, null, true);
        weapon.RollDetails();
        return weapon;
    }

    private string RollSwordType()
    {
        InitMagicSwordType();
        return CommonUtils.RollOnTable(magicSwordTypeDictionary, "Table 4.3A Swords").subTableDescription;
    }

    private string RollMiscWeaponType()
    {
        InitMagicMiscWeaponType();
        return CommonUtils.RollOnTable(magicMiscWeaponTypeDictionary, "Table 4.3D Miscellaneous Weapons").subTableDescription;
    }

    private void AddMiscWeaponType(Item item)
    {
        item.AddNote("Weapon type: " + RollMiscWeaponType());
    }

    #region InitOptionalTables
    
    private Dictionary<(int,int), MagicItemType> InitBaneWeapon()
    {
        if (baneDictionary != null) return baneDictionary;
        baneDictionary = new Dictionary<(int, int), MagicItemType>();

        baneDictionary.Add((1, 1), new MagicItemType(0, "Undead"));
        baneDictionary.Add((2, 2), new MagicItemType(1, "Creatures able to cast spells"));
        baneDictionary.Add((3, 4), new MagicItemType(2, "Orc"));
        baneDictionary.Add((5, 6), new MagicItemType(3, "Goblin"));
        baneDictionary.Add((7, 8), new MagicItemType(4, "Giant"));
        baneDictionary.Add((9, 10), new MagicItemType(5, "Lycanthropes"));
        baneDictionary.Add((11, 11), new MagicItemType(6, "Demons/Devils"));
        baneDictionary.Add((12, 12), new MagicItemType(7, "Dragons"));

        return baneDictionary;
    }

    private Dictionary<(int, int), MagicItemType> InitDragonType()
    {
        if (dragonDictionary != null) return dragonDictionary;
        dragonDictionary = new Dictionary<(int, int), MagicItemType>();

        dragonDictionary.Add((1, 1), new MagicItemType(0, "Black"));
        dragonDictionary.Add((2, 2), new MagicItemType(1, "Blue"));
        dragonDictionary.Add((3, 3), new MagicItemType(2, "Green"));
        dragonDictionary.Add((4, 4), new MagicItemType(3, "Red"));
        dragonDictionary.Add((5, 5), new MagicItemType(4, "White"));
        dragonDictionary.Add((6, 6), new MagicItemType(5, "Brass"));
        dragonDictionary.Add((7, 7), new MagicItemType(6, "Bronze"));
        dragonDictionary.Add((8, 8), new MagicItemType(7, "Copper"));
        dragonDictionary.Add((9, 9), new MagicItemType(8, "Gold"));
        dragonDictionary.Add((10, 10), new MagicItemType(9, "Silver"));

        return dragonDictionary;
    }
    #endregion
    public struct MagicItemsTable
    {
        public int percentageChance;
        public Roll itemTypeRoll;
        public int maxXpOfItems;

        public MagicItemsTable(int percentageChance, Roll itemTypeRoll, int maxXpOfItems = 0)
        {
            this.percentageChance = percentageChance;
            this.itemTypeRoll = itemTypeRoll;
            this.maxXpOfItems = maxXpOfItems;
        }
    }

    public struct MagicItemType
    {
        public int subTableIndex;
        public string subTableDescription;

        public MagicItemType(int subTableIndex, string subTableDescription)
        {
            this.subTableIndex = subTableIndex;
            this.subTableDescription = subTableDescription;
        }
    }

    private struct MagicItemContainer
    {
        public int subTableIndex;
        public string subTableDescription;
        public MagicItem magicItem;

        public MagicItemContainer(int subTableIndex, string subTableDescription, MagicItem magicItem)
        {
            this.subTableIndex = subTableIndex;
            this.subTableDescription = subTableDescription;
            this.magicItem = magicItem;
        }
    }

}

