using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_spawnEnvObj : BaseState<NPC_DATA_spawnEnvObj>
{
    public override void Enter()
    {
        base.Enter();

        GameObject spawnedGo = GameObject.Instantiate(data.prefToSpawn, data.pointToSpawn);
        spawnedGo.transform.position = data.pointToSpawn.position;
        machine.ChangeState(data.GetNextStateType(data.nextState));
    }

    public override void Execute()
    {
        base.Execute();
    }
}
