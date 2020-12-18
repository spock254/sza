﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_dialogue : NPC_BaseData
{
    [HideInInspector]
    public NPC_AnimationController animationController;
    [HideInInspector]
    public Transform playerPosition;
    [HideInInspector]
    public int currentDialogIndex = 0;

    private void Start()
    {
        animationController = GetComponent<NPC_AnimationController>();
        playerPosition = Global.Obj.GetPlayerGameObject().transform;
    }
}
