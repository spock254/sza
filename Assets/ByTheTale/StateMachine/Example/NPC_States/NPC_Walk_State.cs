using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Walk_State : ByTheTale.StateMachine.State
{
    public NPCMashine npcMashine { get { return (NPCMashine)machine; } }

    int current_point = 0;
    GameObject npc;
    List<Transform> points;
    float speed;

    int circles = 3;

    public override void Enter()
    {
        base.Enter();

        npc = npcMashine.npcData.NPC;
        points = npcMashine.npcData.walkingPints;
        speed = npcMashine.npcData.walkSpeed;

        circles = 3;
    }

    public override void Execute()
    {
        base.Execute();

        float step = speed * Time.deltaTime;

        if (points[current_point].position == npc.transform.position) 
        {
            current_point++;
        }

        if (current_point == points.Count) 
        {
            current_point = 0;
            circles--;
        }


        npc.transform.position = Vector2.MoveTowards(npc.transform.position,
            points[current_point].position, step);

        
        if (circles == 0) 
        {
            npcMashine.ChangeState<NPC_Idle_State>();
        }
    }
}
