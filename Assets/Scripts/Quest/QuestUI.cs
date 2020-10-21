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
    EventController eventController;

    [SerializeField]
    QuestSystem questSystem;

    void Start()
    {
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

            if (questSystem.quests.Peek().questEvents.Count > 0) 
            {
                questEventDescription.text = questSystem.quests.Peek().questEvents.Peek().questEventDescription;
            }
        }
    }
}
