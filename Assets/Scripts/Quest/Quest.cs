using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public List<QuestEvent> questEvents = new List<QuestEvent>();

    public Quest() { }

    public QuestEvent AddQuestEvent(QuestEvent questEvent) 
    {
        questEvents.Add(questEvent);

        return questEvent;
    }

    public void AddPath(string fromQuestEventID, string toQuestEventID) 
    {
        QuestEvent from = FindQuestEvent(fromQuestEventID);
        QuestEvent to = FindQuestEvent(toQuestEventID);

        if (from != null && to != null) 
        {
            QuestPath questPath = new QuestPath(from, to);
            from.pathList.Add(questPath);
        }
    }

    QuestEvent FindQuestEvent(string id) 
    {
        foreach (var questEvent in questEvents)
        {
            if (questEvent.GetId() == id) 
            {
                return questEvent;
            }
        }

        return null;
    }

    public void BFS(string id, int orderNumber = 1) 
    {
        QuestEvent thisEvent = FindQuestEvent(id);
        thisEvent.order = orderNumber;

        foreach (var e in thisEvent.pathList)
        {
            if (e.endEvent.order == -1) 
            {
                BFS(e.endEvent.GetId(), orderNumber + 1);
            }
        }
    }

    public void PrintPath() 
    {
        foreach (var questEvent in questEvents)
        {
            Debug.Log(questEvent.questData.questName + " order: " + questEvent.order);
        }
    }
}
