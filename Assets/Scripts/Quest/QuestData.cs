using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]


public class QuestData 
{
    //public enum QuestLine { None, HandGather }


    [Header("Requirements")]
    public Stats necessaryStats;
    public FightStats necessaryFightStats;
    [SerializeReference]
    public List<Item> necessaryItems;


    [Header("Rewards")]
    [SerializeReference]
    public List<Item> rewards;

    //public QuestLine questLine;
    //public IGather gather;

}
