using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public InventoryInit inventory;
    public ItemInit item;
    public StatInit stats;
    public UI ui;
    UnityEvent removeItem;

    private void Start()
    {
        if (removeItem == null) 
        {
            removeItem = new UnityEvent();
        }

        //removeItem.AddListener(RemoveFromList);
        //removeItem.AddListener(UpdateInventory);
    }

    private void Update()
    {
        // !! shoud be in UI
        //if (Input.GetKeyDown(KeyCode.F) && ui.isInventoryOpen) 
        //{
        //    ItemModify.UseItem(ui.choosedItem, stats.stats);
        //    InventoryModify.RemoveFromList(ui.choosedItem.id, item.items);
        //    InventoryModify.UpdateInventory(inventory.inventory, item.items);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha0)) 
        //{
        //    //InventoryModify.AddToList(new Item(new ItemStats(PlayerStats.HEALTH, 0, 0.3f, 5),
        //    //        "posion", 200), item.items);
        //    //InventoryModify.UpdateInventory(inventory.inventory, item.items);
        //}
    }

    public void OpenInventoryUI() 
    {
        ui.isInventoryOpen = !ui.isInventoryOpen;
    }

    private void AddToList(Item newItem) 
    {
            item.items.Add(newItem);
    }
    //private void UpdateInventory()
    //{
    //    inventory.inventory.Clear();

    //    foreach (Item item in item.items)
    //    {
    //        bool skip = false;
    //        foreach (var cell in inventory.inventory)
    //        {
    //            if (item.id == cell.item.id)
    //            {
    //                cell.count++;
    //                skip = true;
    //                break;
    //            }
    //        }
    //        if (!skip)
    //        {
    //            inventory.inventory.Add(new InventoryCell(item, 1));

    //        }
    //    }
    //}
}
