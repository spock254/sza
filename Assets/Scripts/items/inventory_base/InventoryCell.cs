using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCell
{
    public Item item;
    public int count;

    public InventoryCell(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }
}
