using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightStatsInit : MonoBehaviour
{
    [HideInInspector]
    public FightStats fightStats;
    void Awake()
    {
        fightStats = new FightStats(100, 100);
    }
}
