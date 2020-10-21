using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QuestData 
{   
    [Header("Requirements")]
    public Stats necessaryStats;
    public FightStats necessaryFightStats;
    [SerializeReference]
    public List<Item> necessaryItems;


    [Header("Rewards")]
    [SerializeReference]
    public List<Item> rewards;


}
