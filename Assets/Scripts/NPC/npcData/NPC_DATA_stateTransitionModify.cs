using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_stateTransitionModify : NPC_BaseData
{
    public StateNode[] stateNodes;

    public List<System.Type> types;
    public void ModifyStatesTransition() 
    {
        foreach (var state in stateNodes)
        {
            NPC_BaseData com = (NPC_BaseData)GetComponent(System.Type.GetType(state.state.ToString().Replace("STATE", "DATA")));
            com.nextState = state.transition;
        }

    }
}

[System.Serializable]
public struct StateNode 
{
    public StateTypes state;
    public StateTypes transition;
} 
