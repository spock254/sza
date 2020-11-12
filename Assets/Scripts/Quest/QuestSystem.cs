using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestSystem : MonoBehaviour
{
    Controller controller = null;

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
        controller = Global.Component.GetController();
        eventController = Global.Component.GetEventController();

        DontDestroyOnLoad(transform.gameObject);

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
                Debug.Log("answer");
                if (quests.Peek().Use(currentQuestEvent.questData.arg))
                {
                    currentQuestEvent = quests.Peek().NextQuestEvent();
                    eventController.OnNextQuestEvent.Invoke();
                }
            }
            else if (currentQuestEvent.questType == QuestType.Spawn)
            {
                if (quests.Peek().Spawn(currentQuestEvent.questData.pref, currentQuestEvent.questData.pref.transform.position))
                {
                    currentQuestEvent = quests.Peek().NextQuestEvent();
                    eventController.OnNextQuestEvent.Invoke();
                }
            }
            else if (currentQuestEvent.questType == QuestType.EndQuest)
            {
                if (quests.Peek().EndQuest())
                {
                    quests.Dequeue();
                    Quest nextQuest = quests.Peek();
                    Debug.Log(nextQuest.questName + " : " + nextQuest.currentEventIndex);
                    currentQuestEvent = nextQuest.NextQuestEvent();
                    eventController.OnNextQuestEvent.Invoke();
                }
            }
            else if (currentQuestEvent.questType == QuestType.Dialogue) 
            { 
                if (quests.Peek().Dialogue(currentQuestEvent.questData.arg)) 
                {
                    currentQuestEvent = quests.Peek().NextQuestEvent();
                    eventController.OnNextQuestEvent.Invoke();
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        // для тест что бы не менять скр обдж поле
        quests.Peek().currentEventIndex = 0;
        quests.Peek().currentDialogeIndex = 0;
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        controller = Global.Component.GetController();
        eventController = Global.Component.GetEventController();
    }
}
