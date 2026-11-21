# Castles & Crusades Treasure Generator

A browser version of the CastlesAndCrusadesApp terminal treasure roller, in plain HTML, CSS and
JavaScript. No build step and no server: the files in this folder are the whole site.

Live at https://gusemil.github.io/cnc-treasuregenerator/

## Run locally

Open `index.html` in a browser (straight from disk works).

## Files

| File | Contents | C# original |
|---|---|---|
| `index.html`, `style.css` | The page | |
| `app.js` | Reads the options, rolls, prints the result | `Program.cs` (console prompts) |
| `js/dice.js` | Dice, table lookups, items, value table | `CommonUtils.cs`, `Roll.cs`, `Item.cs`, `MagicItem.cs`, `ValueTable.cs` |
| `js/treasure.js` | Table 1.0 coins and gems, the main roll, the report | `Program.cs`, `Coins.cs`, `Gems.cs`, `TreasureReport.cs` |
| `js/extraordinary-items.js` | Extraordinary items | `ExtraOrdinaryItems.cs` |
| `js/magic-items.js` | Tables 4.0-4.3: item types, potions, scrolls, weapons | `MagicItems.cs` |
| `js/armor-and-shields.js` | Tables 4.4-4.4E | `ArmorAndShields.cs` |
| `js/misc-magic.js` | Tables 4.5-4.5E | `MiscMagic.cs` |
| `js/rings-rods-cursed-artifacts.js` | Tables 4.6-4.9 | `Rings.cs`, `RodsStavesWands.cs`, `CursedItems.cs`, `Artifacts.cs` |
| `js/sub-rolls.js` | Bonuses, sizes, charges and other shared sub-rolls | `SubRolls.cs` |
| `js/sentient-items.js` | Sentient weapons | `SentientItems.cs` |
| `js/scroll-spells.js` | Spells on scrolls | `ScrollSpells.cs` |

Tables are arrays of `[low, high, entry]`. The scripts are loaded in order in `index.html`, because
later files use the earlier ones.

## Publishing to the portfolio site

The site is hosted from the portfolio repository. From this folder:

```
.\copy-to-portfolio.ps1
```

This replaces the `cnc-treasuregenerator` folder in `..\..\github-portfolio\gusemil.github.io` with
the files from here. Then commit and push the portfolio repository.

## History

Earlier versions used the C# classes directly (an ASP.NET Core API, then WebAssembly). The
WebAssembly version is in this repository's first commit. It was replaced because it had to download
the .NET runtime (about 1.8 MB) before it could roll; this version is about 130 KB.
The rules were ported from the terminal app, so a table fix in the terminal app has to be made here as well.
