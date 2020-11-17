using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemCounDetection : MonoBehaviour
{
    public int itemCount;
    Controller controller;
    void Start()
    {
        controller = Global.Component.GetController();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (itemCount == 0) 
        {
            CountItems(controller.left_hand_btn);
            CountItems(controller.right_hand_btn);
            CountItems(controller.left_pack_btn);
            CountItems(controller.right_pack_btn);

            if (!controller.IsEmpty(controller.bag_btn))
            {
                itemCount++;
                itemCount += controller.bag_btn.GetComponent<ItemCell>().item.innerItems.Count;
            }
        }
    }

    void CountItems(Button invCell) 
    {
        if (!controller.IsEmpty(invCell)) 
        {
            Item item = invCell.GetComponent<ItemCell>().item;
            
            itemCount++;

            if (item.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Openable)) 
            {
                itemCount += item.innerItems.Count;
            }
        }
    }
}
