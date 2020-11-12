using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public Text questName;
    public Text questDescription;
    public Text questEventDescription;

    [SerializeField]
    EventController eventController = null;

    [SerializeField]
    QuestSystem questSystem = null;

    void Start()
    {
        eventController = Global.Component.GetEventController();
        questSystem = Global.Component.GetQuestSystem();
        eventController.OnNextQuestEvent.AddListener(RefreshQuest);

        RefreshQuest();
    }

    void RefreshQuest() 
    {
        //if (questSystem.quests.Count > 0 && questSystem.quests.Peek().questEvents.Count > 0) 
        //{ 
        //    questName.text = questSystem.quests.Peek().questEvents.Peek().questData.questName;
        //    questDescription.text = questSystem.quests.Peek().questEvents.Peek().questData.questDescription;

        //}
        if (questSystem.quests.Count > 0) 
        {
            questName.text = questSystem.quests.Peek().questName;
            questDescription.text = questSystem.quests.Peek().questDescription;

            if (questSystem.GetCurrentQuestEvent() != null) 
            {
                questEventDescription.text = questSystem.GetCurrentQuestEvent().questEventDescription;
                //questEventDescription.text = questSystem.quests.Peek().questEvents.Peek().questEventDescription;
            }
        }
    }
}
