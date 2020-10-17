using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class MobUse : IUse
{
    public void Use_DressedUp(FightStats fightStats, Stats stats, Item item)
    {
        
    }

    public void Use_In_Hands(Stats stats, Item item)
    {
        
    }

    public void Use_On_Env(RaycastHit2D[] hits, Vector2 mousePos, Button btn_itemInHand, Button btn_tool)
    {

        //Tilemap base2 = GameObject.FindGameObjectWithTag("base2").GetComponent<Tilemap>();
        //Tilemap base3 = GameObject.FindGameObjectWithTag("base3").GetComponent<Tilemap>();

        Tilemap base2 = Global.TileMaps.GetTileMap(Global.TileMaps.BASE_2);
        Tilemap base3 = Global.TileMaps.GetTileMap(Global.TileMaps.BASE_3);

        Vector3Int base2_cell = base2.WorldToCell(mousePos);
        Vector3Int base3_cell = base3.WorldToCell(mousePos);
        
        foreach (var hit in hits)
        {
            if (hit.collider.tag == "base3") 
            {
                base3.SetTile(base3_cell, null);
                return;
            }
        }

        foreach (var hit in hits)
        {
            if (hit.collider.tag == "base2")
            {
                base2.SetTile(base2_cell, null);
                return;
            }
        }
    }

    public void Use_On_Player(Stats stats, Item item)
    {
        
    }

    public void Use_To_Drop(Transform prefab, Transform position, Item item)
    {
        
    }

    public void Use_To_Open(Stats stats, Item item)
    {
        
    }

    public void Use_To_TakeOff(FightStats fightStats, Stats stats, Item item)
    {
        
    }

    public void Use_To_Ware(FightStats fightStats, Stats stats, Item item)
    {
       
    }

    public void Use_When_Ware(FightStats fightStats, Stats stats, Item item)
    {
        
    }
}
