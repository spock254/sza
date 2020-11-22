using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_dialogue : BaseState<NPC_DATA_dialogue>
{
    public override void Enter()
    {
        base.Enter();

        dialogueManager.SetDialog(data.GetDialogByIndex(0));
        eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");
    }

    public override void Execute()
    {
        if (dialogueManager.isOpen == false) 
        {
            machine.ChangeState(data.GetNextStateType(data.nextState));
        }
    }
}
