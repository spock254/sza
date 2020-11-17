using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByTheTale.StateMachine;
using System;
public enum StateTypes 
{
    NPC_STATE_clickWaiting,
    NPC_STATE_itemRequier,
    NPC_STATE_thinking,
    NPC_STATE_tableItemCheck,
    NPC_STATE_itemReturn
}

public class NPC_StateMashine : MachineBehaviour
{
    public List<StateTypes> stateList;

    public override void AddStates()
    {
        foreach (var st in stateList)
        {
            AddState(Type.GetType(st.ToString()));
        }

        SetInitialState(Type.GetType(stateList[0].ToString()));
    }
}
