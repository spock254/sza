using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByTheTale.StateMachine;
using System;
public enum StateTypes 
{
    NPC_STATE_clickWaiting,
    NPC_STATE_itemRequier,
    NPC_STATE_thinking
}

public class NPC_SM_bagCheck : MachineBehaviour
{
    public List<StateTypes> stateTypes;

    public override void AddStates()
    {
        foreach (var st in stateTypes)
        {
            AddState(Type.GetType(st.ToString()));
            Debug.Log(st.GetType().ToString());
        }

        SetInitialState(Type.GetType(stateTypes[0].ToString()));
    }
}
