using System;
using System.Collections.Generic;
using System.Linq;

//Spells for scrolls (Table 4.2: "Spell Scroll, N levels"). The spell lists come from the
//Castles & Crusades Players Handbook class spell lists. A scroll belongs to one caster class and
//holds random spells of level 1-9 whose levels add up to the scroll's total spell levels.
public static class ScrollSpells
{
    //Class name -> spells by level (index 0 = 0-level spells, which are not put on scrolls)
    private static readonly Dictionary<string, string[][]> spellLists = new Dictionary<string, string[][]>
    {
        { "Cleric", new string[][]
            {
                new[] { "Create Water", "Detect Alignment", "Detect Magic", "Detect Poison", "Endure Elements", "First Aid", "Light", "Purify Food and Drink" }, //level 0
                new[] { "Bless", "Bless Water", "Command", "Cure Light Wounds", "Detect Secret Doors", "Detect Undead", "Invisibility to Undead", "Protection from Alignment", "Remove Fear", "Resist Elements", "Sanctuary", "Shield of Faith", "Sound Burst" }, //level 1
                new[] { "Aid", "Augury", "Consecrate", "Darkness", "Delay Poison", "Detect Traps", "Hold Person", "Lesser Restoration", "Remove Paralysis", "Silence", "Speak with Dead", "Spiritual Weapon" }, //level 2
                new[] { "Animate Dead", "Continual Flame", "Create Food and Water", "Cure Serious Wounds", "Dispel Magic", "Glyph of Warding", "Locate Object", "Magic Circle against Alignment", "Prayer", "Remove Blindness or Deafness", "Remove Curse", "Remove Disease" }, //level 3
                new[] { "Air/Water Walk", "Control Water", "Discern Lies", "Dismissal", "Divination", "Freedom of Movement", "Hallow", "Healing Circle", "Neutralize Poison", "Restoration", "Sending", "Tongues" }, //level 4
                new[] { "Atonement", "Commune", "Cure Critical Wounds", "Death Ward", "Dispel Alignment", "Ethereal Jaunt", "Flame Strike", "Insect Plague", "Plane Shift", "Raise Dead", "Scrying", "True Seeing" }, //level 5
                new[] { "Banishment", "Blade Barrier", "Create Undead", "Find the Path", "Geas", "Heal", "Wind Walk", "Word of Recall" }, //level 6
                new[] { "Control Weather", "Greater Restoration", "Greater Scrying", "Holy Word", "Refuge", "Regenerate", "Repulsion", "Resurrection" }, //level 7
                new[] { "Create Greater Undead", "Discern Location", "Earthquake", "Fire Storm", "Holy Aura", "Mass Heal", "Summon Planar Ally", "Symbol" }, //level 8
                new[] { "Antipathy", "Astral Projection", "Energy Drain", "Gate", "Mind Blank", "Soul Bind", "Trap the Soul", "True Resurrection" }, //level 9
            }
        },
        { "Druid", new string[][]
            {
                new[] { "Create Water", "Detect Neutrality", "Detect Poison", "Endure Elements", "First Aid", "Know Direction", "Light", "Purify Food and Drink" }, //level 0
                new[] { "Alarm", "Animal Friendship", "Calm Animals", "Detect Snares and Pits", "Entangle", "Faerie Fire", "Goodberry", "Invisibility to Animals", "Magic Stones", "Obscuring Mist", "Pass Without Trace", "Shillelagh" }, //level 1
                new[] { "Animal Messenger", "Barkskin", "Charm Person or Animal", "Cure Light Wounds", "Delay Poison", "Fire Trap", "Heat Metal", "Hold Animal", "Produce Flame", "Speak with Animals", "Summon Swarm", "Warp Wood" }, //level 2
                new[] { "Call Lightning", "Meld Into Stone", "Neutralize Poison", "Plant Growth", "Protection from Elements", "Pyrotechnics", "Remove Disease", "Shape Stone or Wood", "Snare", "Speak with Plants", "Wall of Wind", "Water Breathing" }, //level 3
                new[] { "Anti-Plant Shell", "Control Plants", "Cure Serious Wounds", "Dispel Magic", "Freedom of Movement", "Quench", "Reincarnate", "Repel Vermin", "Scrying", "Sleet Storm", "Spike Stones", "Summon Animals" }, //level 4
                new[] { "Animal Growth", "Awaken", "Commune with Nature", "Control Winds", "Cure Critical Wounds", "Death Ward", "Ice Storm", "Insect Plague", "Summon Beasts or Plants", "Transmute Mud and Rock", "Wall of Fire", "Wall of Thorns" }, //level 5
                new[] { "Anti-Life Shell", "Fire Seeds", "Ironwood", "Repel Wood", "Stone Tell", "Summon Elemental", "Transport Via Plants", "Wall of Stone" }, //level 6
                new[] { "Changestaff", "Control Weather", "Creeping Doom", "Fire Storm", "Greater Scrying", "Summon Magical Beasts or Fey", "Transmute Metal to Wood", "Wind Walk" }, //level 7
                new[] { "Animal Shapes", "Command Plants", "Finger of Death", "Regenerate", "Repel Metal or Stone", "Sunburst", "Whirlwind", "Word of Recall" }, //level 8
                new[] { "Antipathy", "Astral Projection", "Earthquake", "Heal", "Prismatic Wall", "Shapechange", "Storm of Vengeance", "Summon Elemental Swarm" }, //level 9
            }
        },
        { "Wizard", new string[][]
            {
                new[] { "Arcane Mark", "Dancing Lights", "Detect Magic", "Detect Poison", "Endure Elements", "Ghost Sound", "Light", "Mage Hand", "Mending", "Message", "Open/Close", "Prestidigitation" }, //level 0
                new[] { "Alter Size", "Burning Hands", "Change Self", "Charm Person", "Comprehend Languages", "Erase", "Feather Fall", "Floating Disk", "Hold Portal", "Identify", "Jump", "Magic Missile", "Protection from Alignment", "Read Magic", "Shield", "Shocking Grasp", "Sleep", "Spider Climb", "Summon Familiar", "Unseen Servant" }, //level 1
                new[] { "Acid Arrow", "Continual Flame", "Darkness", "Detect Thoughts", "Enhance Attribute", "Fog Cloud", "Invisibility", "Knock", "Levitate", "Locate Object", "Magic Mouth", "Mirror Image", "Protection from Arrows", "Pyrotechnics", "Ray of Enfeeblement", "Rope Trick", "Scare", "See Invisibility", "Shatter", "Web" }, //level 2
                new[] { "Blink", "Clairaudience/Clairvoyance", "Dispel Magic", "Explosive Runes", "Fireball", "Fly", "Gaseous Form", "Gust of Wind", "Haste", "Hold Person", "Invisibility Sphere", "Lightning Bolt", "Magic Circle against Alignment", "Nondetection", "Stinking Cloud", "Suggestion", "Summon Lesser Monster", "Tiny Hut", "Tongues", "Water Breathing" }, //level 3
                new[] { "Arcane Eye", "Charm Monster", "Confusion", "Detect Scrying", "Dimension Door", "Fear", "Fire Shield", "Fire Trap", "Hallucinatory Terrain", "Ice Storm", "Locate Creature", "Minor Globe of Invulnerability", "Mnemonic Enhancer", "Polymorph", "Remove Curse", "Resilient Sphere", "Scrying", "Shout", "Wall of Fire", "Wall of Ice" }, //level 4
                new[] { "Animate Dead", "Bind Elemental", "Cloudkill", "Cone of Cold", "Contact Other Plane", "Faithful Hound", "Feeblemind", "Hold Monster", "Magic Jar", "Passwall", "Permanency", "Secret Chest", "Summon Monster", "Telekinesis", "Telepathic Bond", "Teleport", "Transmute Mud and Rock", "Wall of Force", "Wall of Iron", "Wall of Stone" }, //level 5
                new[] { "Anti-Magic Shell", "Chain Lightning", "Control Weather", "Disintegrate", "Geas", "Globe of Invulnerability", "Guards and Wards", "Legend Lore", "Mass Suggestion", "Move Earth", "Project Image", "Transmute Flesh and Stone" }, //level 6
                new[] { "Delayed Blast Fireball", "Finger of Death", "Greater Scrying", "Instant Summons", "Limited Wish", "Mass Invisibility", "Phase Door", "Power Word Stun", "Sequester", "Summon Greater Monster", "Teleport Without Error", "Vanish" }, //level 7
                new[] { "Antipathy", "Binding", "Clone", "Incendiary Cloud", "Mass Charm", "Maze", "Mind Blank", "Polymorph Any Object", "Power Word Blind", "Symbol", "Teleportation Circle", "Trap the Soul" }, //level 8
                new[] { "Astral Projection", "Disjunction", "Gate", "Imprisonment", "Meteor Swarm", "Power Word Kill", "Prismatic Sphere", "Refuge", "Shapechange", "Temporal Stasis", "Time Stop", "Wish" }, //level 9
            }
        },
        { "Illusionist", new string[][]
            {
                new[] { "Arcane Mark", "Dancing Lights", "Detect Illusion", "Dragon Mark", "First Aid", "Ghost Sound", "Influence", "Light", "Magical Aura", "Message", "Mending", "Prestidigitation" }, //level 0
                new[] { "Change Self", "Charm Person", "Color Spray", "Darkness", "Daze", "Dragon Armor", "Dragon Image", "Erase", "Faerie's Glamour", "Head Fog", "Hypnotism", "Illusionary Hounds", "Minor Dark Chaos", "Obscuring Mist", "Read Magic", "See Invisibility", "Silent Image", "Undetectable Aura", "Ventriloquism", "Ward's Temporary Strength" }, //level 1
                new[] { "Alter Self", "Angelic Image", "Blur", "Cure Light Wounds", "Dark Chaos", "Detect Magic", "Detect Thoughts", "Dragon Bite", "Eyes of Fire", "False Trap", "Fog Cloud", "Hypnotic Pattern", "Invisibility", "Magic Mouth", "Minor Image", "Mirror Image", "Misdirection", "Pyrotechnics", "Remove Blindness/Deafness", "Ward's Temporary Invisibility" }, //level 2
                new[] { "Blink", "Continual Flame", "Dispel Magic", "Dispel Illusion", "Displaced Image", "Doubled Treasure", "Dragon Mount", "Explosive Runes", "Hallucinatory Terrain", "Hold Person", "Illusionary Help", "Illusory Script", "Invisibility Sphere", "Major Image", "Nondetection", "Rope Trick", "Scare", "Secret Page", "Suggestion", "Tongues" }, //level 3
                new[] { "Charm Monster", "Confusion", "Cure Serious Wounds", "Dragon Scales", "Emotion", "Fear", "Idol of Death", "Illusory Wall", "Improved Invisibility", "Major Dark Chaos", "Minor Creation", "Mirage Arcana", "Phantasmal Killer", "Rainbow Pattern", "Secure Shelter", "Seeming", "Shadow Conjuration", "Solid Fog", "Treasure Hoard", "Ward's Illusionary Portal" }, //level 4
                new[] { "Dragon Breath", "Dragon Shadow", "Dream", "Faithful Hound", "False Vision", "Greater Shadow Conjuration", "Guards and Wards", "Hold Monster", "Humanoid Finding", "Major Creation", "Mass Suggestion", "Mirror Wall", "Neutralize Poison", "Nightmare", "Persistent Image", "Project Image", "Secret Chest", "Shadow Evocation", "True Seeing", "Ward's Extended Invisibility" }, //level 5
                new[] { "Anti-Illusion Shield", "Cloak of Dark Chaos", "Cure Critical Wounds", "Feeble Mind", "Geas", "Greater Shadow Evocation", "Illusionary Lions", "Mislead", "Permanent Image", "Programmed Image", "Shades", "Veil" }, //level 6
                new[] { "Awe", "Insanity", "Mass Invisibility", "Maze", "Power Word Stun", "Prismatic Spray", "Restoration", "Sequester", "Shadow Walk", "Simulacrium", "Telepathic Bond", "Vision" }, //level 7
                new[] { "Antipathy", "Distort Reality", "Find the Path", "Incendiary Cloud", "Mass Charm", "Polymorph", "Power Word Blind", "Prismatic Wall", "Screen", "Sunburst", "Trap the Soul", "Wind Walk" }, //level 8
                new[] { "Astral Projection", "Binding", "Clone", "Dreaming", "Heal", "Mind Blank", "Polymorph Any Object", "Power Word Kill", "Prismatic Sphere", "Regenerate", "Symbol", "Weird" }, //level 9
            }
        },
    };

    public static void Fill(Item scroll, int totalLevels)
    {
        string casterClass = CommonUtils.Pick(spellLists.Keys.ToArray());
        string[][] spells = spellLists[casterClass];
        var chosen = new List<(int level, string name)>();

        int remaining = totalLevels;
        while (remaining > 0)
        {
            int level = CommonUtils.RollDie(Math.Min(9, remaining));
            string[] options = spells[level].Where(s => !chosen.Contains((level, s))).ToArray();
            if (options.Length == 0) continue;
            chosen.Add((level, CommonUtils.Pick(options)));
            remaining -= level;
        }

        scroll.AddNote(casterClass + " scroll with " + chosen.Count + " spell" + (chosen.Count == 1 ? "" : "s") + ":");
        foreach (var spell in chosen.OrderBy(s => s.level).ThenBy(s => s.name))
        {
            scroll.AddNote("  " + spell.name + " (level " + spell.level + ")"); //Leading spaces = sub-note
        }
    }
}
