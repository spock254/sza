using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    EventController eventController;

    

    public Queue<Quest> quests = new Queue<Quest>();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            QuestEvent qe = quests.Peek().NextQuestEvent();

            if (qe == null) { Debug.Log("End"); }
            else { Debug.Log(qe.questEventDescription); eventController.OnNextQuestEvent.Invoke(); }
        }
    }
}
