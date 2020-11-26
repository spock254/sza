using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemCounDetection : MonoBehaviour
{
    public int itemCount;
    Controller controller;
    public DoorController doorController;
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

    void OnTriggerExit2D(Collider2D collision)
    {
        if (doorController != null) 
        {
            doorController.OnDoorClick(null, doorController.transform.position,
                           doorController.gameObject.GetComponent<Collider2D>(), false);
            doorController.isLocked = true;
        }
    }

    void CountItems(Button invCell) 
    {
        if (!controller.IsEmpty(invCell)) 
        {
            Item item = invCell.GetComponent<ItemCell>().item;

            //так как айди все равно надо будет отдать
            if (item.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Card)) 
            {
                return;
            }

            itemCount++;

            if (item.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Openable)) 
            {
                itemCount += item.innerItems.Count;
            }
        }
    }
}
