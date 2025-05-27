using UnityEngine;

public class InventoryItem
{
    public Rune Rune { get; private set; }
    public int Quantity { get; private set; }

    public InventoryItem(Rune rune, int quantity = 1)
    {
        Rune = rune;
        Quantity = quantity;
    }

    public void AddQuantity(int amount)
    {
        Quantity += amount;
    }

    public void RemoveQuantity(int amount)
    {
        Quantity = Mathf.Max(0, Quantity - amount);
    }

    public bool IsEmpty()
    {
        return Quantity <= 0;
    }
} 