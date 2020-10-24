using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    Controller controller = null;

    [SerializeField]
    EventController eventController = null;

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

    void Awake()
    {
        LoadQuests();
        //SortQuestEvents();

        currentQuestEvent = quests.Peek().questEvents[0];

        //eventController.OnNextQuestEvent.AddListener(OnNextQuestEvent);
        eventController.OnNextQuestEvent.Invoke();

        StartCoroutine("QuestEventControll");
    }


    IEnumerator QuestEventControll() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(0.5f);

            if (currentQuestEvent.questType == QuestType.Gather)
            {
                if (quests.Peek().Gather(currentQuestEvent.questData.necessaryItems[0],
                                        controller.currentHand.gameObject))
                {
                    currentQuestEvent = quests.Peek().NextQuestEvent();
                    eventController.OnNextQuestEvent.Invoke();
                }
            }
            else if (currentQuestEvent.questType == QuestType.Syntax)
            {
                if (quests.Peek().StartDialog(quests.Peek().GetCurrentQuestDialog(), currentQuestEvent.questData.arg))
                {
                    quests.Peek().NextDialog();
                    currentQuestEvent = quests.Peek().NextQuestEvent();
                    eventController.OnNextQuestEvent.Invoke();
                }
            }
            else if (currentQuestEvent.questType == QuestType.Use)
            {
                if (quests.Peek().Use())
                {
                    currentQuestEvent = quests.Peek().NextQuestEvent();
                    eventController.OnNextQuestEvent.Invoke();
                }
            }
        }
    }

    //private void LateUpdate()
    //{
    //    if (currentQuestEvent.questType == QuestType.Gather)
    //    {
    //        if (quests.Peek().Gather(currentQuestEvent.questData.necessaryItems[0],
    //                                controller.currentHand.gameObject))
    //        {
    //            currentQuestEvent = quests.Peek().NextQuestEvent();
    //            eventController.OnNextQuestEvent.Invoke();
    //        }
    //    }
    //    else if (currentQuestEvent.questType == QuestType.Syntax)
    //    {
    //        if (quests.Peek().StartDialog(quests.Peek().NextDialog())) 
    //        {

    //            currentQuestEvent = quests.Peek().NextQuestEvent();
    //            eventController.OnNextQuestEvent.Invoke();
    //        }
    //    }
    //    else if (currentQuestEvent.questType == QuestType.Use) 
    //    {
    //        if (quests.Peek().Use()) 
    //        {
    //            currentQuestEvent = quests.Peek().NextQuestEvent();
    //            eventController.OnNextQuestEvent.Invoke();
    //        }
    //    }

    //}

    private void OnApplicationQuit()
    {
        // для тест что бы не менять скр обдж поле
        quests.Peek().currentEventIndex = 0;
        quests.Peek().currentDialogeIndex = 0;
    }
}
