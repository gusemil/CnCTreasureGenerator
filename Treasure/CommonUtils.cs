using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CommonUtils
{
    private static Random randomInstance = null;

    public static void InitRandom()
    {
        if(RandomInstance == null) RandomInstance = new Random();
    }

    public static void InvalidInput()
    {
        Console.WriteLine("\nInvalid input. Please try again");
    }

    public static int RollNumber(int min, int max, int modifier = 0)
    {
        Random rnd = RandomInstance;
        int roll = rnd.Next(min, (max +1)) + modifier; //+1 to include maximum
        Console.WriteLine("\nRolling " + min + "-" + max + " + modifier: " + modifier + " Result: " + roll);

        return roll;
    }

    public static void PageBreak()
    {
        Console.WriteLine("\n-------------------------------------------------\n");
    }
    public static int RollPercentage()
    {
        Random rnd = RandomInstance;
        int roll = rnd.Next(1, (100 +1)); //+1 to include maximum
        Console.WriteLine("Percentage roll: " + roll);
        return roll;
    }

    public static bool RollPercentageSuccess(int percentageChance)
    {
        bool result = false;
        int roll = RollPercentage();
        if (percentageChance > 0 && roll <= percentageChance)
        {
            result = true;
        }
        return result;
    }

    public static void RollItemTable(Dictionary<(int, int), Item> dict, string itemType)
    {
        Item item = RollItem(dict, itemType);
        item.PrintInfo();
        TreasureReport.AddExtraordinaryItem(item);
    }

    //Rolls a die sized to the table (d% for 1-100, d12 for 1-12...) and picks the matching entry
    public static T RollOnTable<T>(Dictionary<(int, int), T> dict, string tableName)
    {
        int max = dict.Keys.Max(k => k.Item2);
        int roll = max == 100 ? RollPercentage() : RollDie(max);
        T entry = Lookup(dict, roll, tableName);
        Console.WriteLine("Rolled " + roll + " on " + tableName);
        return entry;
    }

    //Rolls on an item table and returns a fresh copy of the entry with all of its own rolls done
    public static T RollItem<T>(Dictionary<(int, int), T> dict, string tableName) where T : Item
    {
        T item = (T)RollOnTable(dict, tableName).Clone();
        item.RollDetails();
        return item;
    }

    public static T Lookup<T>(Dictionary<(int, int), T> dict, int roll, string tableName)
    {
        foreach (KeyValuePair<(int, int), T> entry in dict)
        {
            if (roll >= entry.Key.Item1 && roll <= entry.Key.Item2) return entry.Value;
        }
        throw new InvalidOperationException("No entry for roll " + roll + " on table " + tableName);
    }

    //A single die roll, 1 to sides, without console output
    public static int RollDie(int sides)
    {
        return RandomInstance.Next(1, sides + 1);
    }

    //Picks one option with equal chance (for "determine randomly" lists in item descriptions)
    public static T Pick<T>(params T[] options)
    {
        return options[RandomInstance.Next(options.Length)];
    }

    public static Random RandomInstance { get => randomInstance; private set => randomInstance = value; }
}



