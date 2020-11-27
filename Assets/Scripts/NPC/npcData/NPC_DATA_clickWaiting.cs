using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_clickWaiting : NPC_BaseData
{
    [HideInInspector]
    public NPC_AnimationController animationController;

    public Vector2 turnDiraction;

    private void Start()
    {
        animationController = GetComponent<NPC_AnimationController>();
    }
}
