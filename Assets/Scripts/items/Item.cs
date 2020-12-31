using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Item")]
public class Item : ScriptableObject
{
    [HideInInspector]
    public int id;
    public string itemName;
    public string itemDescription;
    public int itemPrice;

    public Sprite itemSprite;
    
    public ItemStats stats;
    public ItemFightStats ItemFightStats;
    public ItemUseData itemUseData;
    public ItemAnimationData itemAnimationData;
    public ItemTimeflowModify itemTimeflowModify;
    public ItemEffect itemEffect;
    public ItemBuff itemBuff;
    public ItemOptionData itemOptionData;
    public ItemReviewData itemReviewData;
    public ItemSubstitution itemSubstitution;

    public int capacity;
    [SerializeReference]
    public List<Item> innerItems;

    [Header("on player use settings")]
    public bool isDestroyOnPlayerUse;
    public Item afterOnPlayerUseItem;
    
    public int GenerateId() 
    {

        return itemName.GetHashCode() + 
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
        return this.id == item.id
            && this.itemName == item.itemName
            && this.itemDescription == item.itemDescription
            && this.itemPrice == item.itemPrice;
    }

    /* to delete*/
    public GameObject InstEffect(Transform parent) 
    { 
        return Instantiate(itemEffect.effect, parent);
    }

    public override string ToString()
    {
        return "id " + id.ToString() +
                "\nitemName " + itemName +
                "\nitemPrice " + itemPrice +
                "\nitemSprite " + ((itemSprite == null) ? " - " : " + ") +
                "\nstats " + ((itemSprite == null) ? " - " : stats.ToString()) +
                "\nItemFightStats " + ((ItemFightStats == null) ? " - " : ItemFightStats.ToString()) +
                "\nitemUseData " + ((itemUseData == null) ? " - " : itemUseData.ToString());
            
    }
}
