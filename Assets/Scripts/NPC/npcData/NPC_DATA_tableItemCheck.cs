using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_tableItemCheck : NPC_BaseData
{
    public GameObject itemPref;
    public Transform table;
    public Transform rejectTable;
    public List<Item> restrictItems;

    public void InstItem(Item item) 
    {
        itemPref.GetComponent<ItemCell>().item = item;
        itemPref = Instantiate(itemPref, rejectTable.position, Quaternion.identity);
        itemPref.name = Global.UNPICKABLE_ITEM + item.name;
        itemPref.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
    }
}
