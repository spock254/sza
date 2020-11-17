using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_thinking : BaseState
{
    DialogueManager dialogueManager;
    EventController eventController;

    NPC_DATA_thinking data;
    NPC_Info info;

    float currentThinkingTime = 0f;
    public override void Enter()
    {
        base.Enter();

        data = GetData<NPC_DATA_thinking>();
        info = GetInfo();

        eventController = Global.Component.GetEventController();
        dialogueManager = Global.Component.GetDialogueManager();
        
        dialogueManager.SetDialog(data.GetNextDialog());
        eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");


    }

    public override void Execute()
    {
        if (dialogueManager.isOpen == false) 
        {
            currentThinkingTime += Time.deltaTime;

            if (currentThinkingTime >= data.thinkingTime) 
            {
                machine.ChangeState(data.GetNextStateType(data.nextState));
            }
        }
    }

}
