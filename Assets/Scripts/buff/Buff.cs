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

    public void BuffActivate(Item item) 
    {
        EventController eventController = Global.Component.GetEventController();

        Type type = Type.GetType(buffType.ToString());
        IBuff buff = (IBuff) Activator.CreateInstance(type);
        buff.Buff();
        Debug.Log("qweqweqweqweqwe");
        eventController.OnAddBuffEvent.Invoke(item);
    }
}
