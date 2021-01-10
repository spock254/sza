﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUse : UseMassage, IUse
{
    public void Use_DressedUp(Button cellToDress, Item item)
    {
        //if (item.itemEffect.effect != null) 
        //{
        //    GameObject effectList = Global.Obj.GetEffectListObject();
        //    GameObject effect = item.InstEffect(effectList.transform);
        //    effect.transform.position = effectList.transform.position;
        //}
    }

    public void Use_In_Hands(Stats stats, Item item)
    {
       // Debug.Log("Use In Habds" + item.itemName);
        
    }

    public void Use_On_Env(RaycastHit2D[] rigidbody2Ds, Vector2 mousePos, Button btn_itemInHand, Button btn_tool)
    {

    }

    public void Use_On_Player(Stats stats, Item item)
    {
        
    }

    public void Use_To_Drop(Transform prefab, Transform position, Item item)
    {
        //Debug.Log("Drop");
        //if (item.itemEffect != null) 
        //{
        //    item.itemEffect.transform.position = position.position;
        //}
    }

    public void Use_To_Open(Stats stats, Item item)
    {
        
    }

    public void Use_To_TakeOff(FightStats fightStats, Stats stats, Item item)
    {
        fightStats.Attack -= item.ItemFightStats.Attack;
        fightStats.Defence -= item.ItemFightStats.Defence;

        BuffController buffController = Global.Component.GetBuffController();
        buffController.DiactivateBuff(item);
    }

    public void Use_To_Ware(FightStats fightStats, Stats stats, Item item)
    {
        //Debug.Log(item.ToString());

        fightStats.Attack += item.ItemFightStats.Attack;
        fightStats.Defence += item.ItemFightStats.Defence;

        BuffController buffController = Global.Component.GetBuffController();
        buffController.ActivateBuff(item);
    }

    public void Use_When_Ware(FightStats fightStats, Stats stats, Item item)
    {
        //Debug.Log("WARERERE " + item.itemName);
    }
}
