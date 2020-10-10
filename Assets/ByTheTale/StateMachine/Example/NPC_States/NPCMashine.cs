using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMashine : ByTheTale.StateMachine.MachineBehaviour
{
    public NPCData npcData;

    public override void AddStates()
    {
        AddState<NPC_Idle_State>();
        AddState<NPC_Walk_State>();

        SetInitialState<NPC_Walk_State>();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        //if (IsCurrentState<NPC_Idle_State>()) 
        //{
        //    Debug.Log("Idle");
        //}

        //if (IsCurrentState<NPC_Walk_State>()) 
        //{
        //    Debug.Log("Walk");
        //}
    }
}
