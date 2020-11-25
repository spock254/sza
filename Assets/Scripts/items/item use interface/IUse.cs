using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUse
{
    void Use_On_Player(Stats stats, Item item);

    void Use_When_Ware(FightStats fightStats, Stats stats, Item item);

    void Use_In_Hands(Stats stats, Item item);

    void Use_To_Ware(FightStats fightStats, Stats stats, Item item);

    void Use_To_TakeOff(FightStats fightStats, Stats stats, Item item);

    // для сумок открыть инвентарь для других айтемов использовать как ключ
    void Use_To_Open(Stats stats, Item item);

    void Use_To_Drop(Transform prefab, Transform position, Item item);

    // когда айтем уже одет на играока использование пустой рукой
    void Use_DressedUp(Button cellToDress, Item item);

    void Use_On_Env(RaycastHit2D[] rigidbody2Ds, Vector2 mousePos, Button btn_itemInHand, Button btn_tool);
}
