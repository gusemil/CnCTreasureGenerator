using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CnCTreasureGenerator;

//Called from wwwroot/app.js as exports.CnCTreasureGenerator.TreasureApi.Roll(...).
//Runs the treasure code copied from the terminal app and returns the result as JSON.
[System.Runtime.Versioning.SupportedOSPlatform("browser")]
public static partial class TreasureApi
{
    [JSExport]
    public static string Roll(int level, bool random, bool lair, bool hoard, int sentient)
    {
        if (!random && (level < 0 || level > TreasureGenerator.MAX_TREASURE_LEVEL))
            throw new ArgumentOutOfRangeException(nameof(level), "Treasure level must be between 0 and " + TreasureGenerator.MAX_TREASURE_LEVEL);
        if (sentient < 0 || sentient > 100)
            throw new ArgumentOutOfRangeException(nameof(sentient), "Sentient chance must be between 0 and 100");

        CommonUtils.InitRandom();

        //The treasure code prints every die roll to the console; capture it as the roll log
        TextWriter originalOut = Console.Out;
        var rollLog = new StringWriter();
        Console.SetOut(rollLog);
        try
        {
            SentientItems.ChancePercent = sentient;
            int treasureLevel = random ? TreasureGenerator.RollTreasureLevel() : level;
            new TreasureGenerator().RollTreasure(treasureLevel, lair, hoard);
            TreasureResult result = TreasureResult.FromReport(treasureLevel, rollLog.ToString());
            return JsonSerializer.Serialize(result, TreasureJsonContext.Default.TreasureResult);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}

//JSON shape returned to the page
public record GemResult(int Count, string Name, int GpEach);
public record ItemResult(int Amount, string Name, int Gp, bool Priceless, int Xp, string Material, List<string> Notes, string Description, int Page);
public record TreasureResult(int Level, string Header, bool Nil, int Gold, int GemTotal, List<GemResult> Gems,
    List<ItemResult> ExtraordinaryItems, List<ItemResult> MagicItems, int TotalValue, bool HasPriceless, string Text, string Log)
{
    public static TreasureResult FromReport(int level, string log) => new TreasureResult(
        level,
        TreasureReport.Header,
        TreasureReport.IsNil,
        TreasureReport.Gold,
        TreasureReport.GemTotal,
        TreasureReport.Gems.Select(g => new GemResult(g.count, g.name, g.gpEach)).ToList(),
        TreasureReport.ExtraordinaryItems.Select(ToResult).ToList(),
        TreasureReport.MagicItems.Select(ToResult).ToList(),
        TreasureReport.TotalValue,
        TreasureReport.HasPriceless,
        TreasureReport.Build(),
        log);

    private static ItemResult ToResult(Item item) => new ItemResult(
        item.amount,
        TreasureReport.CleanName(item.name),
        item.gp,
        item.isPriceless,
        item.xp,
        item.isRollValueTable ? TreasureReport.Unquote(item.material.description) : null,
        new List<string>(item.notes),
        TreasureReport.Unquote(item.description),
        item.pageNumber);
}

//Compile-time JSON serialization, so trimming in a publish build cannot remove what it needs
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(TreasureResult))]
internal partial class TreasureJsonContext : JsonSerializerContext
{
}
