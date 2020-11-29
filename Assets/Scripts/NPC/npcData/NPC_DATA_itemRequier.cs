using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_itemRequier : NPC_BaseData
{
    [SerializeField]
    List<Item> items = null;

    int itemIndex = 0;

    //[HideInInspector]
    //public List<Item> savedItems = new List<Item>();

    public Transform table;

    public override void ResetState()
    {
        base.ResetState();

        savedItems.Clear();
        itemIndex = 0;
    }

    public Item GetNextItem() 
    {
        Item itemToReturn = null;

        if (items.Count > itemIndex)
        {
            itemToReturn = items[itemIndex];
            itemIndex++;
        }

        return itemToReturn;
    }

    public bool isLastRequierdItem() 
    {
        return itemIndex == items.Count;
    }

    //public void MoveAndRejectItem(Item item, Vector3 position) 
    //{ 
    //}
}
