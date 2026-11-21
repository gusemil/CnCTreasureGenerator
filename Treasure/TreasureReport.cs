using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

//Collects the results of one treasure roll. The web version returns them as JSON and as the
//same plain-text layout the terminal app wrote to Treasure.txt (offered as a download).
public static class TreasureReport
{
    public const string FILE_NAME = "Treasure.txt";

    private static string header;
    private static int gold;
    private static readonly List<(int count, string name, int gpEach)> gems = new List<(int, string, int)>();
    private static int gemTotal;
    private static readonly List<Item> extraordinaryItems = new List<Item>();
    private static readonly List<Item> magicItems = new List<Item>();

    public static string Header => header;
    public static bool IsNil => header == "Treasure type Nil";
    public static int Gold => gold;
    public static int GemTotal => gemTotal;
    public static IReadOnlyList<Item> ExtraordinaryItems => extraordinaryItems;
    public static IReadOnlyList<Item> MagicItems => magicItems;

    //Most valuable first
    public static List<(int count, string name, int gpEach)> Gems
    {
        get
        {
            var sorted = new List<(int count, string name, int gpEach)>(gems);
            sorted.Sort((a, b) => b.gpEach.CompareTo(a.gpEach));
            return sorted;
        }
    }

    public static int TotalValue
    {
        get
        {
            int total = gold + gemTotal;
            foreach (Item item in extraordinaryItems) total += item.gp;
            foreach (Item item in magicItems) if (!item.isPriceless) total += item.gp;
            return total;
        }
    }

    public static bool HasPriceless => magicItems.Exists(item => item.isPriceless);

    public static void Begin(int treasureLevel, string mode)
    {
        header = treasureLevel == 0 ? "Treasure type Nil" : "Treasure level " + treasureLevel + ", " + mode;
        gold = 0;
        gems.Clear();
        gemTotal = 0;
        extraordinaryItems.Clear();
        magicItems.Clear();
    }

    public static void SetGold(int amount) => gold = amount;

    public static void AddGem(int count, string name, int gpEach)
    {
        gems.Add((count, CleanName(name), gpEach));
        gemTotal += count * gpEach;
    }

    public static void AddExtraordinaryItem(Item item) => extraordinaryItems.Add(item);
    public static void AddMagicItem(Item item) => magicItems.Add(item);

    //Same layout the terminal app appended to Treasure.txt
    public static string Build()
    {
        var sb = new StringBuilder();
        string line = new string('=', 60);
        sb.AppendLine(line);
        sb.AppendLine(header);
        sb.AppendLine("Rolled " + DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
        sb.AppendLine(line);

        if (IsNil)
        {
            sb.AppendLine("No treasure.");
            sb.AppendLine();
            return sb.ToString();
        }

        sb.AppendLine();
        sb.AppendLine("GOLD: " + Gp(gold));
        sb.AppendLine();

        if (gems.Count == 0)
        {
            sb.AppendLine("GEMS: none");
        }
        else
        {
            int count = 0;
            foreach (var gem in gems) count += gem.count;
            sb.AppendLine("GEMS (" + count + ", total " + Gp(gemTotal) + "):");
            foreach (var gem in Gems)
            {
                sb.AppendLine("  " + gem.count + " x " + gem.name + " (" + Gp(gem.gpEach) + " each)");
            }
        }
        sb.AppendLine();

        AppendItems(sb, "EXTRAORDINARY ITEMS", extraordinaryItems);
        AppendItems(sb, "MAGIC ITEMS", magicItems);

        sb.AppendLine("TOTAL VALUE: " + Gp(TotalValue) + (HasPriceless ? " plus priceless artifact(s)" : ""));
        sb.AppendLine();
        sb.AppendLine();
        return sb.ToString();
    }

    private static void AppendItems(StringBuilder sb, string title, List<Item> items)
    {
        if (items.Count == 0)
        {
            sb.AppendLine(title + ": none");
            sb.AppendLine();
            return;
        }

        sb.AppendLine(title + " (" + items.Count + "):");
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            string amount = item.amount > 1 ? item.amount + " x " : "";
            string value = item.isPriceless ? "priceless" : Gp(item.gp);
            string xp = item.xp > 0 ? ", " + item.xp.ToString("N0", CultureInfo.InvariantCulture) + " xp" : "";
            sb.AppendLine("  " + (i + 1) + ". " + amount + CleanName(item.name) + " - " + value + xp);

            if (item.isRollValueTable) sb.AppendLine("     Material: " + Unquote(item.material.description));
            foreach (string note in item.notes) sb.AppendLine("     " + note);
            if (item.description != "") sb.AppendLine("     " + Unquote(item.description));
            if (item.pageNumber != 0) sb.AppendLine("     (Monsters & Treasure p. " + item.pageNumber + ")");
        }
        sb.AppendLine();
    }

    //Some table texts are written as 'text'
    public static string Unquote(string text)
    {
        return text.Length >= 2 && text.StartsWith("'") && text.EndsWith("'") ? text.Substring(1, text.Length - 2) : text;
    }

    public static string Gp(int value) => value.ToString("N0", CultureInfo.InvariantCulture) + " gp";

    //Table names are written as 'Name' or 'Name' +2. Drop the quote marks but keep apostrophes like Dragon's.
    public static string CleanName(string name)
    {
        if (!name.StartsWith("'")) return name;
        for (int i = 1; i < name.Length; i++)
        {
            if (name[i] == '\'' && (i == name.Length - 1 || name[i + 1] == ' '))
            {
                return name.Substring(1, i - 1) + name.Substring(i + 1);
            }
        }
        return name.Substring(1);
    }
}
