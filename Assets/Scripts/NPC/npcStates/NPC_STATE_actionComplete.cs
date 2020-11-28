using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_actionComplete : BaseState<NPC_DATA_actionComplete>
{
    IAction action;

    public override void Enter()
    {
        base.Enter();

        action = data.actionGo.GetComponent<IAction>();

        if (!dialogueManager.isOpen) 
        { 
            if (!action.IsInAction())
            {
                machine.ChangeState(data.GetNextStateType(data.actionCompleteState));
            }
            else 
            {
                machine.ChangeState(data.GetNextStateType(data.nextState));
            }
        }


    }
}
