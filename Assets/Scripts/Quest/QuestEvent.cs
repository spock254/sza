using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestType { Kill, Combo, Delivery, Gather, Escort, Syntax, Hybrids, Use }

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest/QuestEvent")]
public class QuestEvent : ScriptableObject, IComparable<QuestEvent>
{

    //public string questEventName;
    public int order;
    public string questEventDescription;
    public QuestType questType;

    [SerializeField]
    public QuestData questData;

    public int CompareTo(QuestEvent other)
    {
        return order - other.order;
    }



    //public QuestEvent(QuestData questData) 
    //{
    //    this.id = Guid.NewGuid().ToString();
    //    this.questData = questData;
    //}

    //public string GetId() 
    //{
    //    return id;
    //}
}
