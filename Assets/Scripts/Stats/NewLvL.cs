using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLvL
{
    public FightStats lvlUpFightStats;
    public NewLvL(FightStats lvlUpFightStats)
    {
        this.lvlUpFightStats = lvlUpFightStats;
    }

    public void LvlUp(ref FightStats fightStats) 
    {
        fightStats.Attack += lvlUpFightStats.Attack;
        fightStats.Defence += lvlUpFightStats.Defence;
    }
}
