using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public bool Use(string arg) 
    {
        GameObject dt = GameObject.FindGameObjectWithTag("dialogText");
        if (dt == null) 
        {
            return false;
        }

        return dt.GetComponent<Text>().text == arg;
    }

    public bool Spawn(GameObject spawnController, Vector2 spawnPosition) 
    {
        Instantiate(spawnController, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
        ISpawn spawn = spawnController.gameObject.GetComponent<ISpawn>();

        if (spawn != null)
        {
            spawn.Spawn();
            return true;
        }

        return false;
    }

    public bool EndQuest() 
    {

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        ProgressSceneLoader sceneLoader = Global.Component.GetProgressSceneLoader();
        sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        return true;
    }

    public bool Dialogue(string speacker) 
    {
        DialogueManager dialogue = GameObject.FindGameObjectWithTag("dialogWindow").GetComponent<DialogueManager>();
        
        if (!dialogue.isOpen) 
        {
            return false;
        }

        return dialogue.speaker == speacker && dialogue.isLastPart();
        
    }

    public bool FindGameObjectInSceneState(GameObject pref, string ending) 
    {
        GameObject prefInScene = GameObject.Find(pref.name + ending);

        if (prefInScene == null) 
        {
            return false;
        }

        return prefInScene.GetComponent<GameObjectTrigger>().GetIsTriggerd();
    }

    /* 
     * неактивный обьект должен быть дочерним 
     * пэрэент обьект должен называться тем же именем что и дочерний
     */
    public bool ActivateGameObject(GameObject pref, string ending) 
    {
        GameObject rootGo = GameObject.Find(pref.name + ending);

        if (rootGo == null) 
        {
            return false;
        }

        Transform[] trs = rootGo.GetComponentsInChildren<Transform>(true);
        GameObject childGo = null;
        
        foreach (var tr in trs)
        {
            if (tr.name == pref.name) 
            {
                childGo = tr.gameObject;
            }
        }

        //GameObject prefInScene = GameObject.Find(pref.name + ending);

        //Debug.Log(prefInScene);
        if (childGo == null)
        {
            return false;
        }

        childGo.SetActive(true);

        return true;
    }

    Item eatedItem = null;
    public bool Eat(List<Item> itemsToEat) 
    {
        //Controller controller = Global.Component.GetController();
        EventController eventController = Global.Component.GetEventController();

        int events = eventController.OnUseOnPlayerEvent.GetPersistentEventCount();
        bool isListen = false;

        for (int i = 0; i < events; i++)
        {
            Debug.Log(eventController.OnUseOnPlayerEvent.GetPersistentMethodName(i));
            if (eventController.OnUseOnPlayerEvent.GetPersistentMethodName(i) == "IsItemUsedOnPLayer")
            {
                isListen = true;
                break;
            }
        }

        if (isListen == false) 
        {
           // Debug.Log("EAT addd");
            eventController.OnUseOnPlayerEvent.AddListener(IsItemUsedOnPLayer);
        }

        if (eatedItem == null) 
        {
            return false;
        }

        bool result = eatedItem.IsSameItems(itemsToEat[0]);
        eatedItem = null;

        return result;
    }


    void IsItemUsedOnPLayer(Item item) 
    {
        eatedItem = item;
      //  Debug.Log("EAT");
    }
}
