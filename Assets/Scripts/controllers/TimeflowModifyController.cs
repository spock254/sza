using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeflowModifyController : MonoBehaviour
{
    Controller controller;
    EventController eventController;
    void Awake()
    {
        controller = Global.Component.GetController();
        eventController = Global.Component.GetEventController();

        StartCoroutine(LateStart(1));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    //    eventController.OnNewTicEvent.AddListener(FindMutableItems);
        eventController.OnNewTicEvent.AddListener(ModifyItems);

    }
    List<Item> mutableItems = new List<Item>();
    
    void ModifyItems() 
    {
        Item item = controller.GetItemInHand(controller.currentHand);
        
        if (item.itemTimeflowModify.IsTimeFlowModifiable() && item.itemTimeflowModify.tics == 0) 
        {
            item = Instantiate(item.itemTimeflowModify.modifiedItem);
            //item = Object.Instantiate(item.itemTimeflowModify.modifiedItem) as Item;

            controller.currentHand.GetComponent<ItemCell>().item = item;
            controller.currentHand.GetComponent<Image>().sprite = item.itemSprite;
            return;
        }

        item.itemTimeflowModify.tics--;
    }
}
