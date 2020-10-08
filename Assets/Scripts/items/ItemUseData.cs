using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemUseData
{
    public enum ItemSize { Small, Middle, Big }
    // не может быть одновременно HandUsable и Openable и Upgradable
    // Upgrate - айтем для абгрейта
    // Upgradable - айтем который можно абрейдить
    //
    public enum ItemType { Head, Face, Body, Arm, Lags, Bag, Card, Packet_left, Packet_right,
                           Unwearable, Untakable, Dragable, HandUsable, Openable, Upgrate, Upgradable, NONE }

    public ItemSize itemSize;
    [SerializeReference]
    public IUse use;
    [SerializeReference]
    public ItemType[] itemTypes;

    public ItemUseData(ItemSize itemSize, IUse use, ItemType[] itemTypes)
    {
        this.itemSize = itemSize;
        this.use = use;
        this.itemTypes = itemTypes;
    }

    public ItemUseData(IUse use) 
    {
        this.use = use;
        this.itemTypes = new ItemType[] { ItemType.NONE };
    }
}
