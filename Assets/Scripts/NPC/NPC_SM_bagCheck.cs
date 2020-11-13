using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_SM_bagCheck : ByTheTale.StateMachine.MachineBehaviour
{
    public override void AddStates()
    {
        AddState<NPC_STATE_clickWaiting>();

        SetInitialState<NPC_STATE_clickWaiting>();
    }
}
