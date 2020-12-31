using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_walkTillReverce : BaseState<NPC_DATA_walkTillReverce>
{
    Transform point;
    Vector2 diraction;
    public override void Enter()
    {
        base.Enter();

        point = data.GetNextPoint();

    }

    public override void Execute()
    {
        if (IsDialogOpen() == false)
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

                diraction = data.GetNpcDiraction(point.position, data.transform.position);

                if (data.animationController != null)
                {
                    data.animationController.ChangeAllSprites();
                    data.animationController.Play(diraction);
                }

            }
            else
            {
                point = data.GetNextPoint();
            }

        }
    }

    public override void PostExecute()
    {
        base.PostExecute();

        if (data.animationController != null)
        {
            data.animationController.UpdateSprites();
        }
    }
}
