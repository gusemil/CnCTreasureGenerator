# Castles & Crusades Treasure Generator

A browser version of the CastlesAndCrusadesApp terminal treasure roller.

The treasure rules are the C# classes from the terminal app (copied into `Treasure/`), compiled to
WebAssembly so they run in the visitor's browser. The page in `wwwroot` is plain HTML, CSS and
JavaScript and calls the C# through `TreasureApi.Roll` (`TreasureApi.cs`). There is no server, so the
site is just static files and can be hosted on GitHub Pages.

## Run locally

```
dotnet run
```

Opens http://localhost:5180. (Opening `wwwroot/index.html` straight from disk does not work: the
browser only loads the WebAssembly files over http.)

## Build the static site

```
dotnet publish -c Release
```

The site is in `bin/Release/net10.0/publish/wwwroot`. Upload that folder's contents to any static host.
It keeps working under a sub-path such as `https://<user>.github.io/<repo>/`.

## GitHub Pages

1. Put this folder in its own GitHub repository (this folder is the repository root) and push to `main`.
2. On GitHub: Settings > Pages > Build and deployment > Source: **GitHub Actions**.
3. `.github/workflows/deploy-pages.yml` then builds and publishes the site on every push to `main`.

If you upload the published files by hand instead (e.g. to a `gh-pages` branch), keep the `.nojekyll`
file: without it GitHub Pages hides the `_framework` folder that holds the .NET files.

## Keeping the rules in sync

`Treasure/` is a copy of the terminal app's classes, apart from `TreasureReport.cs` (returns results
instead of writing Treasure.txt) and `TreasureGenerator.cs` (the terminal `Program.cs` rolls without
the console prompts). A table fix in the terminal app has to be copied here as well.
