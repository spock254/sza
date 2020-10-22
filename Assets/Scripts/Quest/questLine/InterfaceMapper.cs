using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceMapper : MonoBehaviour
{
    QuestSystem questSystem;

    void Awake()
    {
        questSystem = GetComponent<QuestSystem>();    
    }

    void Start()
    {
        foreach (var quest in questSystem.quests)
        {
            foreach (var questEvent in quest.questEvents)
            {
                Map(questEvent);
            }
        }
    }

    void Map(QuestEvent questEvent) 
    {
        //if (questEvent.questType == QuestType.Gather) 
        //{
        //    if (questEvent.questData.questLine == QuestData.QuestLine.HandGather) 
        //    {
        //        questEvent.questData.gather = new HandGather();
        //    }
        //}
    }
}
