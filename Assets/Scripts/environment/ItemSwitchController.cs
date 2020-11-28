using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitchController : MonoBehaviour
{
    [SerializeField]
    Item resultItem = null;

    [SerializeField]
    Item needItem = null;

    public void SwitchItem(Item itemToSwitch, Button hand) 
    {
        if (itemToSwitch.IsSameItems(needItem)) 
        {
            Item resultItemClone = Instantiate(resultItem);

            hand.GetComponent<ItemCell>().item = resultItemClone;
            hand.GetComponent<Image>().sprite = resultItemClone.itemSprite;
        }
    }
}
