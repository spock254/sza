using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInit : MonoBehaviour
{
    public Dictionary<string, Item> bagDB;

    Sprite[] sprites;
    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>(Global.Path.EQUIPMENT_SPRITES_ROOT);

        bagDB = new Dictionary<string, Item>();

        Item bag = CreateEquipment("bag", 120, 0, 25, "weapons-and-equipment_80",
                                    10, new List<Item>());
        bagDB.Add(bag.itemName, bag);
    }

    Item CreateEquipment(string itemName,
                         int itemPrice,
                         float attack,
                         float defence,
                         string spriteName,
                         int bagCapacity,
                         List<Item> innerItems)
    {
        return new Item
    (
        SetItemStats(),
        new ItemFightStats(attack, defence),
        itemName,
        itemPrice,
        new ItemUseData(ItemUseData.ItemSize.Middle, new BagUse(), 
                        new ItemUseData.ItemType[] 
                        { 
                            ItemUseData.ItemType.Bag,
                            ItemUseData.ItemType.Openable
                        }),
        GetSprite(spriteName),
        bagCapacity,
        innerItems
    );
    }

    ItemStats SetItemStats()
    {
        return new ItemStats(PlayerStats.NONE, 0, 0, 0);
    }

    private Sprite GetSprite(string name)
    {
        //Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        if (sprites == null)
            return null;

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == name)
            {
                return (Sprite)sprites[i];
            }
        }

        return null;
    }
}
