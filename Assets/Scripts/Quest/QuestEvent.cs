using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestType { Kill, Combo, Delivery, Gather, Escort, Syntax, Hybrids, Use, Spawn }

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest/QuestEvent")]
public class QuestEvent : ScriptableObject
{

    //public string questEventName;

    public string questEventDescription;
    public QuestType questType;

    [SerializeField]
    public QuestData questData;

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
