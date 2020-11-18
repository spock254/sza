using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState<T> : ByTheTale.StateMachine.State
{
    //enum StateInitType 
    //{ 
    //    Base,
    //    Dialog
    //}

    protected T data;
    protected string npcName;
    protected float playerActionRadius = 0;
    protected EventController eventController;
    protected DialogueManager dialogueManager;
    protected NPC_Info info;

    Transform playerTransform = null;

    public override void Enter()
    {
        //Debug.Log("current state " + this.ToString());
        npcName = machine.name;
        playerActionRadius = Global.Component.GetController().GetActioPlayerRadius();
        playerTransform = Global.Obj.GetPlayerGameObject().GetComponent<Transform>();

        eventController = Global.Component.GetEventController();
        dialogueManager = Global.Component.GetDialogueManager();

        data = GameObject.Find(npcName).GetComponent<T>();
        info = GameObject.Find(npcName).GetComponent<NPC_Info>();
        // увиличить радиус на 20% (не может одстать больше чем 1 тайл радиус)
        playerActionRadius += playerActionRadius / 100 * 20;
    }

    protected bool IsInNpcRadius(Vector2 npcPosition)
    {
        return Vector2.Distance(playerTransform.position, npcPosition) <= playerActionRadius;
    }

    //public T GetData<T>() 
    //{ 
    //    return GameObject.Find(npcName).GetComponent<T>();
    //}

    //NPC_Info GetInfo()
    //{
    //    return GameObject.Find(npcName).GetComponent<NPC_Info>();
    //}
}
