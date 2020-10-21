using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[CreateAssetMenu(fileName = "Data", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    [SerializeReference]
    public Queue<QuestEvent> questEvents;
    
    //QuestEvent currentQuestEvent;

    //public Quest() { }

    public QuestEvent AddQuestEvent(QuestEvent questEvent) 
    {
        //if (questEvents.Count == 1) { currentQuestEvent = questEvents[0]; }

        questEvents.Enqueue(questEvent);

        return questEvent;
    }

    public QuestEvent NextQuestEvent() 
    {
        return (questEvents.Count == 0) ? null : questEvents.Dequeue();
    } 
}
