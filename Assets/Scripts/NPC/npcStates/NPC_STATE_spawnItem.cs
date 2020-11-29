using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_spawnItem : BaseState<NPC_DATA_spawnItem>
{
    public override void Enter()
    {
        base.Enter();

    }

    public override void Execute()
    {
        if (IsDialogOpen() == false) 
        {
            data.SpawnSavedItems();

            machine.ChangeState(data.GetNextStateType(data.nextState));
        }
    }

}
