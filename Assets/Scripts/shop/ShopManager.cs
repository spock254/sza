using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //public List<InventoryCell> shopInventory;
    //public List<Item> items;
    //public UI ui;
    //public GoldInit gold;
    //public InventoryInit inventory;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    items = new List<Item>();

    //    items.Add(GlobalItems.GetApple());
    //    items.Add(GlobalItems.GetApple());
    //    items.Add(GlobalItems.GetApple());
    //    items.Add(GlobalItems.GetApple());

    //    items.Add(GlobalItems.GetCoffe());
    //    items.Add(GlobalItems.GetCoffe());
    //    items.Add(GlobalItems.GetCoffe());
    //    items.Add(GlobalItems.GetCoffe());
    //    items.Add(GlobalItems.GetCoffe());
    //    items.Add(GlobalItems.GetCoffe());
    //    items.Add(GlobalItems.GetCoffe());
    //    items.Add(GlobalItems.GetCoffe());

    //    InventoryModify.CreateInvBaseOnItemList(ref shopInventory, items);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F) 
    //        && ui.isShopOpen 
    //        && gold.gold.GoldAmount >= ui.choosedShopItem.itemPrice)
    //    {
    //        //ItemModify.UseItem(ui.choosedItem, stats.stats);
    //        ItemModify.UseShopItem(ui.choosedShopItem, gold.gold);

    //        InventoryModify.RemoveFromList(ui.choosedShopItem.id, items);
    //        InventoryModify.AddToList(ui.choosedShopItem, inventory.itemInit.items);
    //        InventoryModify.UpdateInventory(shopInventory, items);
    //        InventoryModify.UpdateInventory(inventory.inventory, inventory.itemInit.items);
    //        //UpdateInventory();
    //    }
    //}

    //public void OpenShopUI() 
    //{
    //    ui.isShopOpen = !ui.isShopOpen;
    //}
}
