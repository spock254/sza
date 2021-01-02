using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCell : MonoBehaviour
{
    public Buff.BuffType buffType = Buff.BuffType.None;
    EventController eventController = null;
    bool isBuffAvtive = false;
    void Start()
    {
        eventController = Global.Component.GetEventController();    
    }

    IEnumerator BuffLifeTime(GameObject cell, Item item)
    {
        isBuffAvtive = true;
        
        yield return new WaitForSeconds(item.itemBuff.buffTime);

        eventController.OnRemoveBuffEvent.Invoke(cell, item);
        
        isBuffAvtive = false;
    }

    public void InitBuff(GameObject cell, Item item) 
    {
        StartCoroutine(BuffLifeTime(cell, item));
    }

    public void RefreshBuff(GameObject cell, Item item) 
    {
        StopAllCoroutines();
        StartCoroutine(BuffLifeTime(cell, item));
    }

    public bool IsBuffActive() 
    {
        return isBuffAvtive;
    }
}
