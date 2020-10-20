using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvent
{
    public enum EventStatus { Waiting, Current, Done}

    public string id;
    public QuestData questData;
    public EventStatus eventStatus;
    public int order = -1;

    public List<QuestPath> pathList = new List<QuestPath>();

    public QuestEvent(QuestData questData) 
    {
        this.id = Guid.NewGuid().ToString();
        this.questData = questData;
        this.eventStatus = EventStatus.Waiting;
    }

    public void UpdateQuestEvent(EventStatus eventStatus) 
    {
        this.eventStatus = eventStatus;
    }

    public string GetId() 
    {
        return id;
    }
}
