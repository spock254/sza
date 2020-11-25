using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

        StartCoroutine(ItemAnimate());
        StartCoroutine(UpdateEffect());
        StartCoroutine(LateStart(1));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        EventController eventController = Global.Component.GetEventController();
        eventController.OnNewTicEvent.AddListener(ModifyItems);
    }

    IEnumerator ItemAnimate()
    {
        while (true) 
        {
            if (item != null && item.itemAnimationData.itemSpriteFrames.Count > 0)
            {
                Sprite itemFrame = item.itemAnimationData.GetNextFrameSprite();
                Image cellImg = GetComponent<Image>();

                if (cellImg != null)
                {
                    cellImg.sprite = itemFrame;
                }
                else 
                {
                    GetComponent<SpriteRenderer>().sprite = itemFrame;
                }
            }
            
            yield return new WaitForSeconds(0.3f);
        }
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
            return;
        }

        item.itemTimeflowModify.tics--;
    }

    IEnumerator UpdateEffect() 
    {
        GameObject effectList = Global.Obj.GetEffectListObject();
        
        while (true) 
        {
            if (item.itemEffect.effect != null && item.itemEffect.effectCells.Contains(item.itemEffect.currentCell)) 
            { 
                if (!effectList.transform.Find(item.itemEffect.effect.name)) 
                {
                    GameObject newEffect = Instantiate(item.itemEffect.effect, effectList.transform);
                    newEffect.transform.position = effectList.transform.position;
                }
            
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
