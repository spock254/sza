using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;

    public List<QuestEvent> questEvents;

    public int currentEventIndex = 0;

    public QuestEvent NextQuestEvent()
    {
        QuestEvent eventToReturn = null;

        if (currentEventIndex < questEvents.Count) 
        {
            eventToReturn = questEvents[currentEventIndex];
            currentEventIndex++;
        }

        return eventToReturn;
    }
}
