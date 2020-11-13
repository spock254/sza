using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_thinking : BaseState
{
    DialogueManager dialogueManager;
    EventController eventController;
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Thinking");
        eventController = Global.Component.GetEventController();
        dialogueManager = Global.Component.GetDialogueManager();
        
        dialogueManager.SetDialog(data.GetNextDialog());
        eventController.OnStartDialogEvent.Invoke(data.npcName, "*" + data.npcName + "*");

    }

}
