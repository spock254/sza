using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestType { Kill, Combo, Delivery, Gather, Escort, Syntax, Dialogue, Hybrids, 
    Use, Spawn, FindInScene, FindGameObjectInSceneState, ActivateGameObject, EndQuest }

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest/QuestEvent")]
public class QuestEvent : ScriptableObject
{

    //public string questEventName;

    public string questEventDescription;
    public QuestType questType;

    [SerializeField]
    public QuestData questData;

    //[Header("Optional Data")]
    //public Vector2 position;
}
