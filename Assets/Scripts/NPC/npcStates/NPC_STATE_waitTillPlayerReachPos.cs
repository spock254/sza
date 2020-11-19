using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_waitTillPlayerReachPos : BaseState<NPC_DATA_waitTillPlayerReachPos>
{
    Transform playerTransform;
    public override void Enter()
    {
        base.Enter();

        playerTransform = Global.Obj.GetPlayerGameObject().GetComponent<Transform>();
    }

    public override void Execute()
    {
        if (data.IsOnPosition(playerTransform.position)) 
        {
            machine.ChangeState(data.GetNextStateType(data.nextState));
        }
    }
}
