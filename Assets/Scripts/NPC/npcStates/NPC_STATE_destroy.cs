using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_destroy : BaseState<NPC_DATA_destroy>
{
    public override void Enter()
    {
        base.Enter();

        GameObject.Destroy(data.gameObject);
    }
}
