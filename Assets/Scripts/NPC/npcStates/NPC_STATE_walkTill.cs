﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_walkTill : BaseState<NPC_DATA_walkTill>
{
    Transform point;
    Vector2 diraction;
    public override void Enter()
    {
        base.Enter();

        point = data.GetNextPoint();
        Debug.Log(point.position.ToString());
    }

    public override void Execute()
    {
        if (data.IsLastPoint() && point.position == data.transform.position)
        {
            machine.ChangeState(data.GetNextStateType(data.nextState));    
        }

        if (point.position != data.transform.position) 
        {
            data.transform.position = Vector3.MoveTowards(data.transform.position,
                                                          point.position,
                                                          data.walkSpeed * Time.deltaTime);
            
            diraction = GetNpcDiraction();

            data.animationController.ChangeAllSprites();
            
            data.animationController.Play(diraction);
            
            Debug.Log(diraction);
        }
        else 
        {
            point = data.GetNextPoint();
        }
    }

    public override void PostExecute()
    {
        base.PostExecute();

        data.animationController.UpdateSprites();
    }
    //public override void OnAnimatorIK(int layerIndex)
    //{
    //    base.OnAnimatorIK(layerIndex);

    //    diraction = GetNpcDiraction();

    //    data.animationController.Play(diraction);
    //}

    Vector2 GetNpcDiraction() 
    {
        Vector2 temp = point.position - data.transform.position;

        return Mathf.Abs(temp.x) > Mathf.Abs(temp.y)
                                      ? new Vector2(temp.x, 0)
                                      : new Vector2(0, temp.y);
    }
}