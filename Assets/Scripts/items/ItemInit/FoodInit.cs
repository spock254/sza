using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInit : MonoBehaviour
{
    Item apple;

    public Dictionary<string, Item> foodDB { get; private set; }

    //const string RES_PATH = "Images/Items/Food/";

    Sprite[] sprites;
    public void Awake()
    {
        sprites = Resources.LoadAll<Sprite>(Global.Path.FOOD_SPRITES_ROOT);

        foodDB = new Dictionary<string, Item>();

        apple = CreateFood("apple", 23, 5, 5, 5, "food_item_2");
        foodDB.Add(apple.itemName, apple);
    }

    private Item CreateFood(string itemName,
                                    int itemPrice,
                                    float hunger_value, 
                                    float hunger_duration, 
                                    float hunger_time, 
                                    string spriteName) 
    {
        return new Item
    (
        new ItemStats(PlayerStats.HUNGER, hunger_value, hunger_duration, hunger_time),
        SetItemFightStats(),
        itemName,
        itemPrice,
        new ItemUseData(ItemUseData.ItemSize.Small, new FoodUse(),
                        new ItemUseData.ItemType[]
                        {
                                    ItemUseData.ItemType.Packet_left,
                                    ItemUseData.ItemType.Packet_right,
                                    ItemUseData.ItemType.HandUsable
                        }),
        GetSprite(spriteName),
        0,
        null
    );
    }

    private ItemFightStats SetItemFightStats() 
    {
        return new ItemFightStats(0, 0);
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
