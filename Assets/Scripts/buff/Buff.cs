using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuffType { WalkSpeedBuff, WalkSpeedDebuff, TestBuff, None }

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Buff/Buff")]
public class Buff : ScriptableObject
{
    public enum BuffMode { BUFF, DEBUFF }

    public string buffDescription = string.Empty;
    public Sprite buffSprite;
    public BuffMode buffMode = BuffMode.BUFF;
    public BuffType buffType = BuffType.None;
    public Buff debuffToRemove = null;

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

    // игнорировать если айтем существует
    public void BuffDirty() 
    {
        if (eventController == null)
        {
            eventController = Global.Component.GetEventController();
        }

        if (buffController == null)
        {
            buffController = Global.Component.GetBuffController();
        }

        Type type = Type.GetType(buffType.ToString());
        IBuff buff = (IBuff) Activator.CreateInstance(type);
        buff.Buff();
    }

    public void DeBuffDirty()
    {
        if (eventController == null)
        {
            eventController = Global.Component.GetEventController();
        }

        if (buffController == null)
        {
            buffController = Global.Component.GetBuffController();
        }

        Type type = Type.GetType(buffType.ToString());
        IBuff buff = (IBuff) Activator.CreateInstance(type);
        buff.Debuff();

    }

    public bool IsBuffed() 
    {
        Type type = Type.GetType(buffType.ToString());
        IBuff buff = (IBuff)Activator.CreateInstance(type);

        return buff.IsBuffed();
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
