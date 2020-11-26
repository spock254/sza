using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemCell : MonoBehaviour
{
    
    public Item item;

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
        ItemEffect currentItemEffect = (item == null) ? null : item.itemEffect;
        GameObject currentEfect = (currentItemEffect == null) ? null : currentItemEffect.effect;

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
                            currentEfect = item.itemEffect.effect;
                            
                            GameObject spawnedEffect = Instantiate(currentEfect, effectList.transform);
                            item.itemEffect.SetEffectName(spawnedEffect);
                            spawnedEffect.transform.position = effectList.transform.position;
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
                //if (item.itemEffect.effect == null)
                //{
                //    if (currentEfect != null) 
                //    {
                //        foreach (Transform childEffect in effectList.transform)
                //        {
                //            if (childEffect.name.Contains(item.itemEffect.effect.name))
                //            {
                //                Destroy(childEffect.gameObject);
                //                currentEfect = null;
                //                break;
                //            }
                //        }
                //    }
                //}
                //else 
                //{
                //    if (!item.itemEffect.IsEffectExist() && item.itemEffect.IsSamePlaceEffect())
                //    {
                //        currentEfect = item.itemEffect.effect;
                //        GameObject spawnedEffect = Instantiate(currentEfect, effectList.transform);

                    //        // если айтем во внешном мире
                    //        if (item.itemEffect.currentCell == null)
                    //        {
                    //            spawnedEffect.name += ItemEffect.EffectUsePlace.environment.ToString() + item.itemEffect.envPosition.ToString();
                    //            StartCoroutine(UpdateEnvItemPosition(spawnedEffect, this.transform.position));
                    //        }
                    //        else 
                    //        {
                    //            spawnedEffect.name += item.itemEffect.currentCell.name;
                    //            spawnedEffect.transform.position = effectList.transform.position;
                    //        }

                    //        //spawnedEffect.transform.position = (item.itemEffect.currentCell == null) ? 
                    //        //                                item.itemEffect.envPosition 
                    //        //                              : effectList.transform.position;
                    //    }
                    //    else if (item.itemEffect.IsEffectExist() && !item.itemEffect.IsSamePlaceEffect())
                    //    {
                    //        //foreach (Transform childEffect in effectList.transform)
                    //        //{
                    //        //    if (childEffect.name.Contains(item.itemEffect.effect.name) 
                    //        //            && childEffect.name.Contains((item.itemEffect.currentCell == null) ? 
                    //        //            ItemEffect.EffectUsePlace.environment.ToString() + item.itemEffect.envPosition.ToString() 
                    //        //            : item.itemEffect.currentCell.ToString()))
                    //        //    {
                    //        //        Destroy(childEffect.gameObject);
                    //        //        currentEfect = null;
                    //        //        break;
                    //        //    }
                    //        //}
                    //        Destroy(item.itemEffect.IsEffectExist().gameObject);
                    //        currentEfect = null;

                    //    }
                    //}
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
    void Update()
    {
        //if (spawnedEffect != null && item.itemEffect.effectCells.Contains(ItemEffect.EffectUsePlace.environment)
        //    && item.itemEffect.currentCell == null)
        //{
        //    spawnedEffect.transform.position = this.transform.position;
        //}
    }
}
