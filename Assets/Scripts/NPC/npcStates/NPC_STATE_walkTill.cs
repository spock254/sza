using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_walkTill : BaseState<NPC_DATA_walkTill>
{
    Transform point;
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
        }
        else 
        {
            point = data.GetNextPoint();
        }
        //if (data.points[0].position != data.transform.position) 
        //{
        //    data.transform.position = Vector3.MoveTowards(data.transform.position,
        //                                                  data.points[0].position, 
        //                                                  data.walkSpeed * Time.deltaTime);
        //}
    }

    public override void OnAnimatorIK(int layerIndex)
    {
        base.OnAnimatorIK(layerIndex);

        Debug.Log("qwe");
    }
}
