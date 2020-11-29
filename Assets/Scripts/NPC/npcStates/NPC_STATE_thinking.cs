using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_thinking : BaseState<NPC_DATA_thinking>
{
    float currentThinkingTime = 0f;

    public override void Enter()
    {
        Debug.Log("THINKING");
        base.Enter();

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
