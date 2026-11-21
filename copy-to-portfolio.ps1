# Copies the treasure generator into the GitHub portfolio repository, where it is served at
# https://gusemil.github.io/cnc-treasuregenerator/
# Usage (from this folder):  .\copy-to-portfolio.ps1
# Then commit and push the portfolio repository.

param(
    [string]$PortfolioPath = "$PSScriptRoot\..\..\github-portfolio\gusemil.github.io",
    [string]$FolderName = "cnc-treasuregenerator"
)

$ErrorActionPreference = "Stop"

$portfolio = Resolve-Path $PortfolioPath
$target = Join-Path $portfolio $FolderName

# Replace the old copy completely, so files removed here are removed there too
if (Test-Path $target) { Remove-Item -Recurse -Force $target }
New-Item -ItemType Directory -Path $target | Out-Null
foreach ($file in "index.html", "style.css", "app.js", "js") {
    Copy-Item -Recurse -Path (Join-Path $PSScriptRoot $file) -Destination $target
}

Write-Host "Copied to $target"
Write-Host "Commit and push $portfolio to update https://gusemil.github.io/$FolderName/"
