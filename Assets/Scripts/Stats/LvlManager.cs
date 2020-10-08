using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlManager : MonoBehaviour
{
    NewLvL newLvL; // new lvl init!!!
    public ExpInit exp;
    public FightStatsInit fightStats;
    void Awake()
    {
        newLvL = new NewLvL(new FightStats(10, 10));
    }

    // Update is called once per frame
    void Update()
    {
        if (exp.exp.isLvlUp()) 
        {
            newLvL.LvlUp(ref fightStats.fightStats);
        }
    }
}
