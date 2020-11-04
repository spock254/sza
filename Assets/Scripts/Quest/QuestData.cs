using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]


public class QuestData 
{
    //public enum QuestLine { None, HandGather }


    [Header("SetUp")]
    public Stats necessaryStats;
    public FightStats necessaryFightStats;

    [SerializeReference]
    public List<Item> necessaryItems;
    [Header("option event data")]
    public string arg;
    public GameObject pref;
    [Header("Rewards")]
    [SerializeReference]
    public List<Item> rewards;

    //public QuestLine questLine;
    //public IGather gather;

}
