using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Idle_State : ByTheTale.StateMachine.State
{
    public NPCMashine npcMashine { get { return (NPCMashine)machine; } }

    float idleTime = 0;

    public override void Enter()
    {
        base.Enter();

        idleTime = 0;
    }

    public override void Execute()
    {
        base.Execute();

        idleTime += Time.deltaTime;

        if (idleTime >= npcMashine.npcData.timeInIdle) 
        {
            npcMashine.ChangeState<NPC_Walk_State>();
        }
    }
}
