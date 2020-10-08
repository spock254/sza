using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    public int id;
    public Sprite itemSprite;
    public ItemStats stats;
    public ItemFightStats ItemFightStats;
    public string itemName;
    public int itemPrice;
    public ItemUseData itemUseData;

    [SerializeReference]
    public List<Item> innerItems;
    public int capacity;

    public Item(ItemStats stats, ItemFightStats itemFightStats, string itemName, 
        int itemPrice, ItemUseData itemUseData, Sprite itemSprite, int capacity, List<Item> innerItems)
    {
        this.id = GenerateId(itemName, stats);
        this.stats = stats;
        this.ItemFightStats = itemFightStats;
        this.itemName = itemName;
        this.itemPrice = itemPrice;
        this.itemUseData = itemUseData;
        this.itemSprite = itemSprite;
        this.capacity = capacity;
        this.innerItems = innerItems;
    }

    //ctr for non eatable items
    public Item(ItemFightStats itemFightStats, string itemName, int itemPrice, 
        ItemUseData itemUseData, Sprite itemSprite, int capacity, List<Item> innerItems)
    {
        this.stats = new ItemStats(PlayerStats.NONE, 0, 0, 0);
        this.id = GenerateId(itemName, stats);
        this.ItemFightStats = itemFightStats;
        this.itemName = itemName;
        this.itemPrice = itemPrice;
        this.itemUseData = itemUseData;
        this.itemSprite = itemSprite;
        this.capacity = capacity;
        this.innerItems = innerItems;
    }

    // кстр для пустых ячеек инв
    public Item(string itemName, ItemUseData itemUseData, Sprite sprite) 
    {
        this.itemName = itemName;
        this.itemUseData = itemUseData;
        this.itemSprite = sprite;
    }

    protected int GenerateId(string name, ItemStats stats) 
    {

        return name.GetHashCode() + 
            (int)stats.value + 
            (int)stats.playerStats;
    }

    public int GetItemSize(Item item) 
    {
        if (item.itemUseData.itemSize == ItemUseData.ItemSize.Big)
        {
            return Global.Item.BIG_SIZE;
        }
        else if (item.itemUseData.itemSize == ItemUseData.ItemSize.Middle) 
        {
            return Global.Item.MIDDLE_SIZE;
        }

        return Global.Item.SMALL_SIZE;
    }

    public int GetItemSize()
    {
        if (itemUseData.itemSize == ItemUseData.ItemSize.Big)
        {
            return Global.Item.BIG_SIZE;
        }
        else if (itemUseData.itemSize == ItemUseData.ItemSize.Middle)
        {
            return Global.Item.MIDDLE_SIZE;
        }

        return Global.Item.SMALL_SIZE;
    }

    public int CountInnerCapacity() 
    {
        int innerCapacity = 0;

        foreach (var item in innerItems)
        {
            innerCapacity += GetItemSize(item);
        }

        return innerCapacity;
    }

    public bool IsSameItems(Item item) 
    {
        return this.id == item.id;
    }
}
