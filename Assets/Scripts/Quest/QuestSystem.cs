using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    EventController eventController;

    QuestEvent currentQuestEvent;

    public Queue<Quest> quests;

    public QuestEvent GetCurrentQuestEvent() 
    {
        return currentQuestEvent;
    }
    void LoadQuests() 
    {
        quests = new Queue<Quest>(Resources.LoadAll<Quest>(Global.Path.QUESTS)
                                           .OrderBy(x => x.name)
                                           .ToList());
    }

    void SortQuestEvents() 
    {
        foreach (var quest in quests)
        {
            quest.questEvents.Sort();
        }
    }

    void Awake()
    {
        LoadQuests();
        SortQuestEvents();

        currentQuestEvent = quests.Peek().questEvents[0];
        eventController.OnNextQuestEvent.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            currentQuestEvent = quests.Peek().NextQuestEvent();
            eventController.OnNextQuestEvent.Invoke();

        }
    }

    private void OnApplicationQuit()
    {
        // для тест что бы не менять скр обдж поле
        quests.Peek().currentEventIndex = 0;
    }
}
