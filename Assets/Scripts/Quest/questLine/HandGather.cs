using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGather : IGather
{
    public bool Gather(Item item, GameObject gatherPoint, int count = -1)
    {
        Item itemInHand = gatherPoint.GetComponent<ItemCell>().item;

        return itemInHand.IsSameItems(item);
    }
}
