using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemCell : MonoBehaviour
{
    
    public Item item;

    GameObject currentEfect;
    Transform effectToDestroy;
    //GameObject spawnedEffect;
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
    void OnDestroy()
    {
        if (effectToDestroy != null) 
        {
            Destroy(effectToDestroy.gameObject);
        }
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        EventController eventController = Global.Component.GetEventController();
        eventController.OnNewTicEvent.AddListener(ModifyItems);

        effectToDestroy = item.itemEffect.GetEffect();
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
            item.itemTimeflowModify.tics = 0;

            return;
        }

        if (item.itemTimeflowModify.IsTimeFlowModifiable()) 
        { 
            item.itemTimeflowModify.tics--;
        }
    }

    IEnumerator UpdateEffect() 
    {
        GameObject effectList = Global.Obj.GetEffectListObject();
        ItemEffect currentItemEffect = (item == null) ? null : item.itemEffect;
        currentEfect = (currentItemEffect == null) ? null : currentItemEffect.effect;

        while (true) 
        {
            if (item != null) 
            {
                if (item.itemEffect.effect == null) 
                {
                    if (currentEfect != null) 
                    {
                        foreach (Transform childEffect in effectList.transform)
                        {
                            if (childEffect.name.Contains(currentEfect.name))
                            {
                                Destroy(childEffect.gameObject);
                                currentEfect = null;
                                break;
                            }
                        }
                    }
                }
                else 
                {
                    Transform effectInList = item.itemEffect.GetEffect();
                    
                    if (effectInList == null)
                    {
                        if (item.itemEffect.IsSamePlaceEffect()) 
                        {
                            
                            GameObject spawnedEffect = Instantiate(item.itemEffect.effect, effectList.transform);
                            item.itemEffect.SetEffectName(spawnedEffect);
                            currentEfect = spawnedEffect;

                            if (item.itemEffect.currentCell == null)
                            {
                                StartCoroutine(UpdateEnvItemPosition(spawnedEffect, this.transform.position));
                            }
                            else 
                            { 
                                spawnedEffect.transform.position = effectList.transform.position;
                            }
                        }
                    }
                    else 
                    {
                        if (!item.itemEffect.IsSamePlaceEffect()) 
                        {
                            Destroy(effectInList.gameObject);
                            currentEfect = null;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator UpdateEnvItemPosition(GameObject spawnItem, Vector3 position) 
    {
        while (true) 
        {
            if (spawnItem == null) 
            {
                break;
            }
            spawnItem.transform.position = position;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
