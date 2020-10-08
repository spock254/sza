using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInit : MonoBehaviour
{
    public List<Item> items;


    public Sprite sprite_helmet;
    public Sprite sprite_bag;
    public Sprite sprite_bag2;
    public Sprite sprite_chest;
    public Sprite sprite_card;
    void Awake()
    {
        items = new List<Item>();

        Item helmet = new Item(new ItemFightStats(0, 5), "helmet", 200,
            new ItemUseData(ItemUseData.ItemSize.Middle, new DummyItemUse(),
                            new ItemUseData.ItemType[] { ItemUseData.ItemType.Head, 
                                                         ItemUseData.ItemType.HandUsable }), 
                            sprite_helmet, Global.Item.MIDDLE_SIZE, null);

        Item chest = new Item(new ItemFightStats(0, 10), "chest", 200,
            new ItemUseData(ItemUseData.ItemSize.Middle, new DummyItemUse(),
                    new ItemUseData.ItemType[] { ItemUseData.ItemType.Body,
                                                         ItemUseData.ItemType.HandUsable }),
                    sprite_chest, Global.Item.MIDDLE_SIZE, null);



        Item bag = new Item(new ItemFightStats(0, 0), "bag", 150,
            new ItemUseData(ItemUseData.ItemSize.Big, new DummyItemUse(),
                            new ItemUseData.ItemType[] { ItemUseData.ItemType.Bag,
                                                        ItemUseData.ItemType.Openable}),
                            sprite_bag, 10, new List<Item>() { chest });


        Item card = new Item(new ItemFightStats(0, 0), "card", 200,
                new ItemUseData(ItemUseData.ItemSize.Small, new DummyItemUse(),
                            new ItemUseData.ItemType[] { ItemUseData.ItemType.Card,
                                                         ItemUseData.ItemType.Packet_left,
                                                         ItemUseData.ItemType.Packet_right}),
            sprite_card, 2, null);

        Item bag2 = new Item(new ItemFightStats(0, 0), "bag2", 150,
            new ItemUseData(ItemUseData.ItemSize.Big, new DummyItemUse(),
                    new ItemUseData.ItemType[] { ItemUseData.ItemType.Bag,
                                                 ItemUseData.ItemType.Openable}),
                    sprite_bag2, 10, new List<Item>() { card, card, card });

        items.Add(helmet);
        items.Add(bag);
        items.Add(bag2);
        items.Add(card);

    }

    
}
