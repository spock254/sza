using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_setTrigger : BaseState<NPC_DATA_setTrigger>
{
    public override void Enter()
    {
        base.Enter();

        data.gameObject.GetComponent<GameObjectTrigger>().SetIsTriggerd(true);

        machine.ChangeState(data.GetNextStateType(data.nextState));
    }
}
