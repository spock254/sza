using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    
    public Item item;

    void Start()
    {
        SpriteRenderer spriteRenderer;
        
        if (TryGetComponent<SpriteRenderer>(out spriteRenderer)) 
        { 
           spriteRenderer.sprite = item.itemSprite;
        }

        StartCoroutine(LateStart(1));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        EventController eventController = Global.Component.GetEventController();
        eventController.OnNewTicEvent.AddListener(ModifyItems);
    }

    void ModifyItems()
    {
        //Item item = cell.GetComponent<ItemCell>().item;

        if (item.itemTimeflowModify.IsTimeFlowModifiable() && item.itemTimeflowModify.tics == 0)
        {
            Item modItem = Instantiate(item.itemTimeflowModify.modifiedItem);
            
            Image image = GetComponent<Image>();
            
            if (image != null) 
            {
                image.sprite = modItem.itemSprite;
            }
            else 
            { 
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = modItem.itemSprite;
            
            }

            item = modItem;
            //item = Object.Instantiate(item.itemTimeflowModify.modifiedItem) as Item;

            //controller.currentHand.GetComponent<ItemCell>().item = item;
            //controller.currentHand.GetComponent<Image>().sprite = item.itemSprite;
            return;
        }

        item.itemTimeflowModify.tics--;
    }
}
