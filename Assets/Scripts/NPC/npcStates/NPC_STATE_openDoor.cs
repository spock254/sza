using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_openDoor : BaseState<NPC_DATA_openDoor>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("DOOOOOOOOOOOOOOOOOOOOOOR");
        data.OpenDoor();
    }

    
}
