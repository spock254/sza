using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_stateTransitionModify : BaseState<NPC_DATA_stateTransitionModify>
{
    public override void Enter()
    {
        base.Enter();

        data.ModifyStatesTransition();

    }

    public override void Execute()
    {
        if (!dialogueManager.isOpen) 
        {
            machine.ChangeState(data.GetNextStateType(data.nextState));
        }
    }
}
