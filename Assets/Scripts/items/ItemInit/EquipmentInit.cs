using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInit : MonoBehaviour
{
    public Dictionary<string, Item> equipmentDB;

    Sprite[] sprites;
    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>(Global.Path.EQUIPMENT_SPRITES_ROOT);

        equipmentDB = new Dictionary<string, Item>();

        Item head = CreateEquipment("head", 120, 0, 20, "weapons-and-equipment_21", 
                                    new ItemUseData.ItemType[] { ItemUseData.ItemType.Head });
        equipmentDB.Add(head.itemName, head);
    }

    Item CreateEquipment(string itemName, 
                         int itemPrice, 
                         float attack, 
                         float defence, 
                         string spriteName,
                         ItemUseData.ItemType[] itemTypes) 
    {
        return new Item
    (
        SetItemStats(),
        new ItemFightStats(attack, defence),
        itemName,
        itemPrice,
        new ItemUseData(ItemUseData.ItemSize.Middle, new EquipmentUse(), itemTypes),
        GetSprite(spriteName),
        0,
        null
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
