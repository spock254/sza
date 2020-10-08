using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public const Item NO_ITEM = null;
    public Item choosedItem = NO_ITEM;
    public Item choosedShopItem = NO_ITEM;
    int chooseItemIndex = 0;
    int chooseShopItemIndex = 0;

    int barlength = 150;

    //get stats after init
    public Stats stats;
    public StatInit statInit;

    public FightStatsInit fightStats;
    public ExpInit exp;
    public GoldInit gold;
    public ShopManager shop;

    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;

    //public InventoryInit item;

    public bool isInventoryOpen = false;
    public bool isShopOpen = false;
    int step = 36;
    private void Start()
    {
        stats = statInit.stats;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isShopOpen = !isShopOpen;
        }

        //if (isInventoryOpen) 
        //{
        //    if (item.inventory.Count > 0)
        //    {
        //        choosedItem = ItemChose();
        //    }
        //}

        //if (isShopOpen)
        //{
        //    if (shop.shopInventory.Count > 0)
        //    {
        //        choosedShopItem = ItemShopChose();
        //    }
        //}
    }
    private void OnGUI()
    {
        DrawStatBar(stats.Age, 0);
        DrawStatBar(stats.Health, 1);
        DrawStatBar(stats.Hunger, 2);
        DrawStatBar(stats.Sleep, 3);
        DrawStatBar(stats.Happiness, 4);
        DrawExpBar(5);
        DrawFightStatBar(fightStats.fightStats.Attack, 6, "attack");
        DrawFightStatBar(fightStats.fightStats.Defence, 7, "defence");

        // inv
        if (isInventoryOpen)
        {
            DrawItemsBar();
        }
        else 
        {
            choosedItem = NO_ITEM;
        }

        //// shop
        //if (isShopOpen) 
        //{
        //    DrawShopBar();
        //}
        //else 
        //{ 
        
        //}
    }
    void DrawStatBar(StatsField statsField, int order)
    {
        GUI.BeginGroup(new Rect(0, step * order, barlength, 32));
        GUI.Box(new Rect(0, 0, barlength, 32), progressBarEmpty);
        GUI.BeginGroup(new Rect(0, 0, statsField.Value / 100 * barlength, 32));
        GUI.Box(new Rect(0, 0, barlength, 32), progressBarFull);
        GUI.EndGroup();
        GUI.EndGroup();
        GUI.Label(new Rect(0, step * order, barlength, 32), statsField.Name + "/" + ((int)statsField.Value).ToString() + "/" + statsField.Duration + " " 
                                                            + statsField.TempDuration + "/" + statsField.BuffTime);
    }

    //void DrawShopBar() 
    //{
    //    GUI.Label(new Rect(Screen.width - barlength / 2, 0, barlength, step), "gold " + gold.gold.GoldAmount.ToString());

    //    int idx = 1;
    //    foreach (InventoryCell item in shop.shopInventory)
    //    {
    //        GUI.Label(new Rect(Screen.width - barlength * 2, idx * 20, barlength, step), item.item.itemName + (idx - 1 == chooseShopItemIndex ? "*" : " ") + " " + item.count + " " + item.item.itemPrice);
    //        idx++;
    //    }
    //}
    void DrawFightStatBar(float fightStats, int order, string statName) 
    {
        GUI.BeginGroup(new Rect(0, step * order, barlength, step));
        GUI.Box(new Rect(0, 0, barlength, step), progressBarEmpty);
        GUI.BeginGroup(new Rect(0, 0, fightStats / 1000 * barlength, step));
        GUI.Box(new Rect(0, 0, barlength, step), progressBarFull);
        GUI.EndGroup();
        GUI.EndGroup();
        GUI.Label(new Rect(0, step * order, barlength, step), statName + " " + fightStats);
    }

    void DrawExpBar(int order)
    {
        GUI.BeginGroup(new Rect(0, step * order, barlength, step));
        GUI.Box(new Rect(0, 0, barlength, step), progressBarEmpty);
        float exp_progressBar = (((float)(exp.exp.Current_exp - exp.exp.lvllt[exp.exp.Lvl]) / exp.exp.lvllt[exp.exp.Lvl + 1]));
        GUI.BeginGroup(new Rect(0, 0, exp_progressBar * barlength, step));
        GUI.Box(new Rect(0, 0, barlength, step), progressBarFull);
        GUI.EndGroup();
        GUI.EndGroup();
        GUI.Label(new Rect(0, step * order, barlength, step), "exp " + exp.exp.Current_exp.ToString() + " / " + exp.exp.lvllt[exp.exp.Lvl + 1].ToString() + " lvl " + exp.exp.Lvl);
    }

    void DrawItemsBar() 
    {
        //GUI.Label(new Rect(Screen.width - barlength / 2, 0, barlength, step), "gold " + gold.gold.GoldAmount.ToString());

        //int idx = 1;
        //foreach (InventoryCell item in item.inventory)
        //{
        //    GUI.Label(new Rect(Screen.width - barlength, idx * 20, barlength, step), item.item.itemName + (idx - 1 == chooseItemIndex ? "*" : " ") + " " +  item.count);
        //    idx++;
        //}
    }

    //private Item ItemChose() 
    //{
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        if (chooseItemIndex > 0) 
    //        {
    //            chooseItemIndex--;
    //        }
    //        else 
    //        {
    //            chooseItemIndex = item.inventory.Count - 1;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        if (chooseItemIndex < item.inventory.Count - 1)
    //        {
    //            chooseItemIndex++;
    //        }
    //        else
    //        {
    //            chooseItemIndex = 0;
    //        }
    //    }

    //    //TODO
    //    if(chooseItemIndex == item.inventory.Count) 
    //    {
    //        chooseItemIndex = item.inventory.Count - 1;
    //    }
    //    return item.inventory[chooseItemIndex].item;
    //}

    //private Item ItemShopChose()
    //{
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        if (chooseShopItemIndex > 0)
    //        {
    //            chooseShopItemIndex--;
    //        }
    //        else
    //        {
    //            chooseShopItemIndex = shop.shopInventory.Count - 1;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        if (chooseShopItemIndex < shop.shopInventory.Count - 1)
    //        {
    //            chooseShopItemIndex++;
    //        }
    //        else
    //        {
    //            chooseShopItemIndex = 0;
    //        }
    //    }

    //    Debug.Log(chooseShopItemIndex);

    //    //TODO
    //    if (chooseShopItemIndex == item.inventory.Count)
    //    {
    //        chooseShopItemIndex = item.inventory.Count - 1;
    //    }
    //    return shop.shopInventory[chooseShopItemIndex].item;
    //}
}
