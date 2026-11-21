// Page logic for the treasure generator. All rolling happens in the C# treasure code, which is
// compiled to WebAssembly and runs here in the browser (TreasureApi.cs). This file only reads
// the options, calls the C# code and prints the result.

import { dotnet } from "./_framework/dotnet.js";

const MAX_TREASURE_LEVEL = 18;
const SETTINGS_KEY = "cncTreasureSettings";

const form = document.getElementById("treasure-form");
const levelSelect = document.getElementById("level");
const randomBox = document.getElementById("random");
const lairBox = document.getElementById("lair");
const hoardBox = document.getElementById("hoard");
const sentientInput = document.getElementById("sentient");
const showLogBox = document.getElementById("show-log");
const rollButton = document.getElementById("roll-button");
const output = document.getElementById("output");

const ROLL_LABEL = "Roll for Treasure";

let lastResult = null;
let treasureApi = null;

for (let level = 1; level <= MAX_TREASURE_LEVEL; level++) {
    levelSelect.add(new Option(String(level), String(level)));
}
levelSelect.value = "1";

// ---------- Options ----------

function updateDisabledStates() {
    levelSelect.disabled = randomBox.checked;
    document.getElementById("level-field").classList.toggle("disabled", randomBox.checked);

    // A hoard is always in the lair (as in the terminal app)
    hoardBox.disabled = !lairBox.checked;
    if (!lairBox.checked) hoardBox.checked = false;
    hoardBox.closest("label").classList.toggle("disabled", !lairBox.checked);
}

function loadSettings() {
    try {
        const saved = JSON.parse(localStorage.getItem(SETTINGS_KEY));
        if (!saved) return;
        randomBox.checked = !!saved.random;
        if (saved.level !== undefined) levelSelect.value = String(saved.level);
        lairBox.checked = saved.lair !== false;
        hoardBox.checked = !!saved.hoard;
        showLogBox.checked = !!saved.showLog;
        // The sentient chance is not remembered: every visit starts at 0%
    } catch (e) { /* Storage unavailable: keep the defaults */ }
}

function saveSettings(options) {
    const { sentient, ...remembered } = options;
    try { localStorage.setItem(SETTINGS_KEY, JSON.stringify(remembered)); } catch (e) { /* ignore */ }
}

function readOptions() {
    let sentient = parseInt(sentientInput.value, 10);
    if (isNaN(sentient)) sentient = 0;
    sentient = Math.min(100, Math.max(0, sentient));
    sentientInput.value = sentient;
    return {
        random: randomBox.checked,
        level: parseInt(levelSelect.value, 10),
        lair: lairBox.checked,
        hoard: lairBox.checked && hoardBox.checked,
        sentient: sentient,
        showLog: showLogBox.checked,
    };
}

// ---------- Rolling ----------

// Starts the .NET runtime in the browser and finds the exported C# methods
async function loadTreasureApi() {
    try {
        const { getAssemblyExports, getConfig } = await dotnet.create();
        const exports = await getAssemblyExports(getConfig().mainAssemblyName);
        treasureApi = exports.CnCTreasureGenerator.TreasureApi;
        rollButton.disabled = false;
        rollButton.textContent = ROLL_LABEL;
    } catch (error) {
        rollButton.textContent = "The dice could not be found";
        showError("The treasure tables failed to load: " + error.message);
    }
}

function rollTreasure(event) {
    event.preventDefault();
    if (!treasureApi) return;
    const options = readOptions();
    saveSettings(options);

    try {
        const json = treasureApi.Roll(options.level, options.random, options.lair, options.hoard, options.sentient);
        lastResult = JSON.parse(json);
        render(lastResult, options.showLog);
    } catch (error) {
        showError("The dice went astray: " + error.message);
    }
}

function showError(message) {
    output.hidden = false;
    output.replaceChildren(el("p", "error", message));
}

// ---------- Output ----------

function el(tag, className, text) {
    const node = document.createElement(tag);
    if (className) node.className = className;
    if (text !== undefined && text !== null) node.textContent = text;
    return node;
}

function gp(value) {
    return value.toLocaleString("en-US") + " gp";
}

function gpSpan(value) {
    return el("span", "gp", gp(value));
}

function render(result, showLog) {
    output.hidden = false;
    output.replaceChildren();

    output.append(el("h2", null, result.header));
    output.append(el("p", "rolled-at", "Rolled " + new Date().toLocaleString()));

    if (result.nil) {
        output.append(el("p", "none", "The monster has no treasure."));
    } else {
        renderGold(result);
        renderGems(result);
        renderItems("Extraordinary Items", result.extraordinaryItems);
        renderItems("Magic Items", result.magicItems);

        const total = el("div", "total");
        total.append("Total value: ", gpSpan(result.totalValue));
        if (result.hasPriceless) total.append(" plus priceless artifact(s)");
        output.append(total);
    }

    const actions = el("div", "actions");
    const download = el("button", "small", "Download as Treasure.txt");
    download.type = "button";
    download.addEventListener("click", downloadText);
    actions.append(download);
    output.append(actions);

    const log = el("details", "log");
    log.open = showLog;
    log.append(el("summary", null, "Every die roll"), el("pre", null, result.log.trim()));
    output.append(log);

    output.scrollIntoView({ behavior: "smooth", block: "start" });
}

function renderGold(result) {
    output.append(el("h3", null, "Gold"));
    const line = el("p", "gold-line");
    line.append(gpSpan(result.gold));
    output.append(line);
}

function renderGems(result) {
    const count = result.gems.reduce((sum, gem) => sum + gem.count, 0);
    const heading = el("h3", null, count ? "Gems (" + count + ", total " + gp(result.gemTotal) + ")" : "Gems");
    output.append(heading);
    if (!count) {
        output.append(el("p", "none", "None"));
        return;
    }
    const list = el("ul", "gems");
    for (const gem of result.gems) {
        const li = el("li");
        li.append(gem.count + " × " + gem.name + " (", gpSpan(gem.gpEach), " each)");
        list.append(li);
    }
    output.append(list);
}

function renderItems(title, items) {
    output.append(el("h3", null, items.length ? title + " (" + items.length + ")" : title));
    if (!items.length) {
        output.append(el("p", "none", "None"));
        return;
    }

    const list = el("ol", "items");
    for (const item of items) {
        const li = el("li");
        const name = el("span", "item-name", (item.amount > 1 ? item.amount + " × " : "") + item.name);
        const meta = el("span", "item-meta");
        meta.append(" — ");
        if (item.priceless) meta.append(el("span", "gp", "priceless"));
        else meta.append(gpSpan(item.gp));
        if (item.xp > 0) meta.append(", " + item.xp.toLocaleString("en-US") + " xp");
        li.append(name, meta);

        const details = el("ul", "item-details");
        if (item.material) details.append(el("li", null, "Material: " + item.material));
        for (const note of item.notes) {
            // Notes starting with spaces belong to the note above them (e.g. spells on a scroll)
            const isSub = note.startsWith("  ");
            details.append(el("li", isSub ? "sub" : null, note.trim()));
        }
        if (item.description) details.append(el("li", "desc", item.description));
        if (item.page) details.append(el("li", "desc", "Monsters & Treasure p. " + item.page));
        if (details.children.length) li.append(details);

        list.append(li);
    }
    output.append(list);
}

function downloadText() {
    if (!lastResult) return;
    const blob = new Blob([lastResult.text], { type: "text/plain;charset=utf-8" });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = "Treasure.txt";
    link.click();
    URL.revokeObjectURL(link.href);
}

// ---------- Start ----------

loadSettings();
updateDisabledStates();
randomBox.addEventListener("change", updateDisabledStates);
lairBox.addEventListener("change", updateDisabledStates);
form.addEventListener("submit", rollTreasure);
loadTreasureApi();
