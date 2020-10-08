using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Use_DressedUp(FightStats fightStats, Stats stats, Item item);
}
