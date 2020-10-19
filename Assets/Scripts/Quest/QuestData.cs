using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest")]
public class QuestData : ScriptableObject
{
    public enum QuestType { Kill, Combo, Delivery, Gather, Escort, Syntax, Hybrids }

    public string questName;
    public string questDescription;
    public QuestType questType;

    
    [Header("Requirements")]
    public Stats necessaryStats;
    public FightStats necessaryFightStats;
    [SerializeReference]
    public List<Item> necessaryItems;


    [Header("Rewards")]
    [SerializeReference]
    public List<Item> rewards;


}
