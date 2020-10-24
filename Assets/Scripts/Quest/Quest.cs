using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;

    public List<QuestEvent> questEvents;
    public List<QuestDialog> questDialogs;

    public int currentEventIndex = 0;
    public int currentDialogeIndex = 0;

    QuestDialog currentQuestDialog = null;

    public QuestDialog GetCurrentQuestDialog() 
    {
        currentQuestDialog = questDialogs[currentDialogeIndex];

        return currentQuestDialog;
    }

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

    public QuestDialog NextDialog() 
    {

        if (currentDialogeIndex < questDialogs.Count)
        {
            currentQuestDialog = questDialogs[currentDialogeIndex];
            currentDialogeIndex++;
        }

        return currentQuestDialog;
    }

    // QUEST LINES

    public bool StartDialog(QuestDialog questDialog, string speaker)
    {
        DialogueManager dialogue = GameObject.FindGameObjectWithTag("dialogWindow").GetComponent<DialogueManager>();
        
        if (dialogue.speaker == speaker) 
        { 
            dialogue.SetDialog(questDialog.dialog);

        }

        return dialogue.isLastPart();
    }

    public bool Gather(Item item, GameObject gatherPoint, int count = -1)
    {
        Item itemInHand = gatherPoint.GetComponent<ItemCell>().item;

        return itemInHand.IsSameItems(item);
        
    }

    public bool Use() 
    {
        //return false;
        //Item itemInHand = gatherPoint.GetComponent<ItemCell>().item;
        //if (itemInHand.IsSameItems(item)) 
        return (GameObject.FindGameObjectWithTag("dialogText"));
        
    }
}
