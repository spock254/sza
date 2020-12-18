using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_dialogue : BaseState<NPC_DATA_dialogue>
{
    public override void Enter()
    {
        base.Enter();

        if (data.animationController != null) 
        { 
            data.animationController.ChangeAllSprites();
            data.animationController.Turn(data.GetNpcDiraction(data.playerPosition.position, data.transform.position));
        }

        dialogueManager.SetDialog(data.GetDialogByIndex(data.currentDialogIndex));
        eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");
    }

    public override void Execute()
    {
        if (dialogueManager.isOpen == false) 
        {
            machine.ChangeState(data.GetNextStateType(data.nextState));
        }
    }

    public override void PostExecute()
    {
        base.PostExecute();

        if (data.animationController != null) 
        { 
            data.animationController.UpdateSprites();
        }
    }
}
