using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : ByTheTale.StateMachine.State
{
    protected string npcName;
    protected float playerActionRadius = 0;
    protected NPC_Data data;
    
    Transform playerTransform = null;

    public override void Enter()
    {
        npcName = machine.name;
        data = GameObject.Find(npcName).GetComponent<NPC_Data>();
        playerActionRadius = Global.Component.GetController().GetActioPlayerRadius();
        playerTransform = Global.Obj.GetPlayerGameObject().GetComponent<Transform>();

        // увиличить радиус на 20% (не может одстать больше чем 1 тайл радиус)
        playerActionRadius += playerActionRadius / 100 * 20;
    }

    protected bool IsInNpcRadius(Vector2 npcPosition)
    {
        return Vector2.Distance(playerTransform.position, npcPosition) <= playerActionRadius;
    }
}
