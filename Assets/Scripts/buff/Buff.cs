using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Buff/Buff")]
public class Buff : ScriptableObject
{
    public enum BuffType { FoodBuff, None }

    public Sprite buffSprite;
    public BuffType buffType = BuffType.None;

    EventController eventController = null;
    BuffController buffController = null;
    public void BuffActivate(Item item) 
    {
        if (eventController == null) 
        { 
            eventController = Global.Component.GetEventController();
        }

        if (buffController == null) 
        {
            buffController = Global.Component.GetBuffController();
        }

        if (buffController.IsBuffExist(item) == false) 
        { 
            Type type = Type.GetType(buffType.ToString());
            IBuff buff = (IBuff) Activator.CreateInstance(type);
            buff.Buff();
        }

        eventController.OnAddBuffEvent.Invoke(item);
    }

    public void BuffDiactivate() 
    {
        //if (eventController == null)
        //{
        //    eventController = Global.Component.GetEventController();
        //}

        Type type = Type.GetType(buffType.ToString());
        IBuff buff = (IBuff)Activator.CreateInstance(type);
        buff.Debuff();

        //eventController.OnRemoveBuffEvent.Invoke(item);
    }
}
