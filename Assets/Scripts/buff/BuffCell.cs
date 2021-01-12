using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCell : MonoBehaviour
{
    public BuffType buffType = BuffType.None;
    EventController eventController = null;
    bool isBuffAvtive = false;
    float buffTimeLeft = float.MaxValue;
    BuffController buffController;
    void Start()
    {
        eventController = Global.Component.GetEventController();
    }

    IEnumerator BuffLifeTime(GameObject cell, Item item)
    {
        isBuffAvtive = true;
        float circleTime = 0.1f;
        float buffActiveTime = 0f;

        Buff buff = item.itemBuff.buff;
        bool isBuff = item.itemBuff.buff.buffType.ToString().EndsWith("Debuff");
        buffController = Global.Component.GetBuffController();

        if (item.itemBuff.buffTime < 0)
        {
            buffTimeLeft = float.MaxValue;

            while (isBuffAvtive == true) 
            {

                yield return new WaitForSeconds(circleTime);
            }
        }
        else 
        { 
            while (buffActiveTime <= item.itemBuff.buffTime) 
            { 
                buffActiveTime += circleTime;
                buffTimeLeft = item.itemBuff.buffTime - buffActiveTime;

                if (buff.debuffToRemove != null) 
                {
                    if (buff.IsBuffed() == false) 
                    {
                        if (buffController.IsBuffExistByTypeName(buff.debuffToRemove.buffType, isBuff) == false) 
                        { 
                            buff.BuffDirty();                     
                        }

                    }
                }

                yield return new WaitForSeconds(circleTime);
            
            }
        }

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

    public void SetBuffActive(bool isActive) 
    {
        this.isBuffAvtive = isActive;
    }

    public float GetBuffTimeLeft() 
    {
        return buffTimeLeft;   
    }
}
