using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public QuestData quest1;
    public QuestData quest2;
    public QuestData quest3;
    public QuestData quest4;
    public QuestData quest5;

    public Quest quest = new Quest();

    void Start()
    {
        QuestEvent a = new QuestEvent(quest1);
        QuestEvent b = new QuestEvent(quest2);
        QuestEvent c = new QuestEvent(quest3);
        QuestEvent d = new QuestEvent(quest4);
        QuestEvent e = new QuestEvent(quest5);

        quest.AddQuestEvent(a);
        quest.AddQuestEvent(b);
        quest.AddQuestEvent(c);
        quest.AddQuestEvent(d);
        quest.AddQuestEvent(e);


        quest.AddPath(a.GetId(), b.GetId());
        quest.AddPath(b.GetId(), c.GetId());
        quest.AddPath(b.GetId(), d.GetId());
        quest.AddPath(d.GetId(), e.GetId());
        quest.AddPath(c.GetId(), e.GetId());

        quest.BFS(a.GetId());

        quest.PrintPath();
    }
}
