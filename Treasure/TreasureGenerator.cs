using System;

//The main treasure rolls from the terminal app's Program.cs, without the console prompts.
//The options that were asked for in the terminal are passed in instead.
public class TreasureGenerator
{
    public const int MAX_TREASURE_LEVEL = 18;
    private const int HOARD_GOLD_MULTIPLIER = 100;
    //Also double the amount of rolls in addition to doubling items? It is unclear
    private const int HOARD_ITEM_MULTIPLIER = 2;

    private readonly Coins coins = new Coins();
    private readonly Gems gems = new Gems();
    private readonly ExtraOrdinaryItems extraOrdinaryItems = new ExtraOrdinaryItems();
    private readonly MagicItems magicItems = new MagicItems();

    //Table 1.0 options. The coin multiplier only applies to treasure found in a monster's lair.
    //A hoard (in a lair) multiplies gold by a further 100 and doubles gems, extraordinary and magic items.
    private bool inLair = true;
    private bool isHoard = false;

    public static int RollTreasureLevel() => CommonUtils.RollNumber(1, MAX_TREASURE_LEVEL);

    public void RollTreasure(int treasureLevel, bool inLair, bool isHoard)
    {
        this.inLair = inLair;
        this.isHoard = inLair && isHoard;

        string mode = this.isHoard ? "hoard in lair" : this.inLair ? "in lair" : "not in lair";
        TreasureReport.Begin(treasureLevel, mode);
        CommonUtils.PageBreak();

        if (treasureLevel == 0)
        {
            Console.WriteLine("Treasure type Nil: the monster has no treasure");
            return;
        }

        Console.WriteLine("Treasure level " + treasureLevel + ", " + mode);
        RollCoins(treasureLevel);
        RollGems(treasureLevel);
        RollExtraOrdinaryItems(treasureLevel);
        RollMagicItems(treasureLevel);
    }

    private int HoardCount(int amount, string what)
    {
        if (!isHoard || amount == 0) return amount;
        int doubled = amount * HOARD_ITEM_MULTIPLIER;
        Console.WriteLine("Hoard: " + what + " doubled from " + amount + " to " + doubled);
        return doubled;
    }

    private void RollCoins(int treasureLevel)
    {
        CommonUtils.PageBreak();
        int coinsDictIndex = treasureLevel - 1;
        int chanceToHaveCoins = coins.coinsDictionary[coinsDictIndex].percentageChance;
        bool hasCoins = CommonUtils.RollPercentageSuccess(chanceToHaveCoins);
        Console.WriteLine(chanceToHaveCoins + "% chance of the treasure containing gold coins. Your result: " + hasCoins);
        int coinAmount = 0;
        if (hasCoins)
        {
            coinAmount = coins.coinsDictionary[coinsDictIndex].coinsRoll.RollXdY(applyMultiplier: inLair);
            if (isHoard)
            {
                coinAmount *= HOARD_GOLD_MULTIPLIER;
                Console.WriteLine("Hoard: gold multiplied by " + HOARD_GOLD_MULTIPLIER);
            }
        }
        Console.WriteLine("The treasure with treasure level " + treasureLevel + " contains " + coinAmount + " gold pieces");
        TreasureReport.SetGold(coinAmount);
    }

    private void RollGems(int treasureLevel)
    {
        CommonUtils.PageBreak();
        int gemsDictIndex = treasureLevel - 1;
        int chanceToHaveGems = gems.gemsDictionary[gemsDictIndex].percentageChance;
        bool hasGems = CommonUtils.RollPercentageSuccess(chanceToHaveGems);
        Console.WriteLine(chanceToHaveGems + "% chance of the treasure containing gems. Your result: " + hasGems);
        int gemsAmount = 0;
        if (hasGems)
        {
            gems.InitGemsSubtable();
            gemsAmount = HoardCount(gems.gemsDictionary[gemsDictIndex].gemsRoll.RollXdY(), "gems");
        }
        Console.WriteLine("The treasure with treasure level " + treasureLevel + " contains " + gemsAmount + " gems");

        if (hasGems)
        {
            Console.WriteLine("Rolling gem subtable for " + gemsAmount + " gems");
            gems.RollGemsSubTable(gemsAmount, gems.gemsDictionary[gemsDictIndex].gemValueAdjustment);
        }
    }

    private void RollExtraOrdinaryItems(int treasureLevel)
    {
        CommonUtils.PageBreak();
        int extraOrdinaryItemsDictIndex = treasureLevel - 1;
        int chanceToHaveItems = extraOrdinaryItems.extraOrdinaryItemsDictionary[extraOrdinaryItemsDictIndex].percentageChance;
        bool hasItems = CommonUtils.RollPercentageSuccess(chanceToHaveItems);
        Console.WriteLine(chanceToHaveItems + "% chance of the treasure containing extraordinary items. Your result: " + hasItems);
        if (hasItems)
        {
            int itemsAmount = HoardCount(extraOrdinaryItems.extraOrdinaryItemsDictionary[extraOrdinaryItemsDictIndex].itemsRoll.RollXdY(), "extraordinary items");
            Console.WriteLine("The treasure with treasure level " + treasureLevel + " contains " + itemsAmount + " extraordinary items");
            Console.Write("Rolling extraordinary item type for " + itemsAmount + " items");
            extraOrdinaryItems.RollItemType(itemsAmount);
        }
    }

    private void RollMagicItems(int treasureLevel)
    {
        CommonUtils.PageBreak();
        int magicItemsDictIndex = treasureLevel - 1;
        var magicItemsEntry = magicItems.magicItemsDictionary[magicItemsDictIndex];
        bool hasItems = CommonUtils.RollPercentageSuccess(magicItemsEntry.percentageChance);
        Console.WriteLine(magicItemsEntry.percentageChance + "% chance of the treasure containing magic items. Your result: " + hasItems);
        if (hasItems)
        {
            int itemsAmount = HoardCount(magicItemsEntry.itemTypeRoll.RollXdY(), "magic items");
            Console.WriteLine("The treasure with treasure level " + treasureLevel + " contains " + itemsAmount + " magic items");
            if (magicItemsEntry.maxXpOfItems > 0)
                Console.WriteLine("Max. XP value of each item: " + magicItemsEntry.maxXpOfItems);
            Console.Write("Rolling magic item type for " + itemsAmount + " items");
            magicItems.RollItemType(itemsAmount, magicItemsEntry.maxXpOfItems);
        }
    }
}
