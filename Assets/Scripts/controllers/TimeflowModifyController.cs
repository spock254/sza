using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        //eventController.OnNewTicEvent.AddListener(ModifyItems);

    }
    
    void ModifyItems() 
    {
        List<Button> mutableItems = new List<Button>();
        mutableItems.Concat(controller.bagCellList)
                    .Concat(controller.cellList)
                    .Concat(controller.invCellList);

        foreach (var cell in mutableItems)
        {
            Item item = cell.GetComponent<ItemCell>().item;
        
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
}
