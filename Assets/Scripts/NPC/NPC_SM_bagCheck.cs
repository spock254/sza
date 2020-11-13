using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_SM_bagCheck : ByTheTale.StateMachine.MachineBehaviour
{
    public override void AddStates()
    {
        AddState<NPC_STATE_clickWaiting>();
        AddState<NPC_STATE_itemRequier>();
        AddState<NPC_STATE_thinking>();

        SetInitialState<NPC_STATE_clickWaiting>();
    }
}
