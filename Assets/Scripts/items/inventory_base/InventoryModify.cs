using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class InventoryModify
{
    public static void RemoveFromList(int id, List<Item> items)
    {
        Item itemToRemove = items.FirstOrDefault(i => i.id == id);

        if (itemToRemove == null)
        {
            Debug.LogError("RemoveFromList itemToRemove == null");
            return;
        }

        items.Remove(itemToRemove);
    }
    public static void CreateInvBaseOnItemList(ref List<InventoryCell> inventory, List<Item> items) 
    {
        //inventory = new List<InventoryCell>();

        //if (items == null) 
        //{
        //    Debug.LogWarning("items == null");
        //    return;
        //}

        //if (items.Count == 0)
        //{
        //    return;
        //}

        //if (inventory == null) 
        //{
        //    Debug.LogWarning("inventory == null");
        //    return;
        //}


        //foreach (Item item in items)
        //{
        //    bool skip = false;
        //    foreach (var cell in inventory)
        //    {
        //        if (item.id == cell.item.id)
        //        {
        //            cell.count++;
        //            skip = true;
        //            break;
        //        }
        //    }
        //    if (!skip)
        //    {
        //        inventory.Add(new InventoryCell(item, 1));

        //    }
        //}
    }

    public static void UpdateInventory(List<InventoryCell> inventory, List<Item> items)
    {
        inventory.Clear();

        foreach (Item item in items)
        {
            bool skip = false;
            foreach (var cell in inventory)
            {
                if (item.id == cell.item.id)
                {
                    cell.count++;
                    skip = true;
                    break;
                }
            }
            if (!skip)
            {
                inventory.Add(new InventoryCell(item, 1));

            }
        }

    }
    public static void AddToList(Item newItem, List<Item> items)
    {
        items.Add(newItem);
    }
}
