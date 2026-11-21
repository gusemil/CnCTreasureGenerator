using System;
using System.Collections.Generic;

public class Item
{
    public string name;
    public int gp;
    public int xp;
    public string description;
    public int pageNumber;
    public bool isRollValueTable;
    public ValueTable.ValueTableEntry material; //if ValueTable applies
    public Roll rollGpValue;
    public int amount = 1;
    public Roll rollAmount;

    //Extra details rolled for this specific item (type, size, charges, spells...). Printed under the item.
    public List<string> notes = new List<string>();
    //Optional extra rolls that run when the item is rolled from a table (e.g. giant type for a belt)
    public Action<Item> subRoll;
    //Artifacts are priceless and count as exceeding any maximum XP limit (Table 1.0)
    public bool isPriceless;

    public Item(string name, int gpValue, int xp = 0, string description = "", int pageNumber = 0, bool isRollValueTable = false, Roll rollGpValue = null, Roll rollAmount = null)
    {
        this.name = name;
        this.gp = gpValue;
        this.xp = xp;
        this.description = description;
        this.pageNumber = pageNumber;
        this.isRollValueTable = isRollValueTable;
        this.rollGpValue = rollGpValue;
        this.rollAmount = rollAmount;
    }

    //Copy of a table entry so rolling never modifies the table itself
    public virtual Item Clone()
    {
        Item copy = (Item)MemberwiseClone();
        copy.notes = new List<string>(notes);
        return copy;
    }

    public void AddNote(string note)
    {
        notes.Add(note);
    }

    public void RollValueTable()
    {
        if (isRollValueTable)
        {
            material = ValueTable.RollValueTable(gp);
            gp += material.gpValue;
        }
    }

    public void RollGpValue()
    {
        if (rollGpValue == null) return;

        for (int i = 0; i < amount; i++)
        {
            int roll = rollGpValue.RollXdY();
            gp += roll;
        }
    }

    public void RollAmount()
    {
        if (this.rollAmount == null) return;

        this.amount = this.rollAmount.RollXdY();
    }

    //Runs all rolls an item needs after it has been picked from a table
    public virtual void RollDetails()
    {
        RollAmount();
        RollGpValue();
        RollValueTable();
        subRoll?.Invoke(this);
    }

    public void PrintInfo()
    {
        string gpText = isPriceless ? "priceless" : gp.ToString();
        Console.WriteLine("Name: " + name + " gp value: " + gpText + " amount: " + amount + " xp: " + xp);
        if (description != "") Console.WriteLine("Description: " + description);
        if (pageNumber != 0) Console.WriteLine("Page number: " + pageNumber);
        if (isRollValueTable) Console.WriteLine("Material: " + material.description);
        foreach (string note in notes)
        {
            //Notes starting with spaces belong to the note above them (e.g. spells on a scroll)
            Console.WriteLine(note.StartsWith("  ") ? "      " + note.TrimStart() : "  - " + note);
        }
    }
}
