using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_walk : BaseState<NPC_DATA_walk>
{
    Transform point;
    Vector2 diraction;
    public override void Enter()
    {
        base.Enter();

        if (point == null) 
        { 
            point = data.GetNextPoint();
        }

    }

    public override void Execute()
    {
        //if (data.IsLastPoint() && point.position == data.transform.position)
        //{
        //    machine.ChangeState(data.GetNextStateType(data.nextState));
        //}

        if (Input.GetMouseButtonDown(0))
        {
            //point = data.GetNextPoint();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.name == npcName)
                {
                    if (IsInNpcRadius(hit.transform.position))
                    {
                        machine.ChangeState(data.GetNextStateType(data.nextState));
                        return;
                    }
                }
            }
        }

        if (point.position != data.transform.position)
        {
            data.transform.position = Vector3.MoveTowards(data.transform.position,
                                                          point.position,
                                                          data.walkSpeed * Time.deltaTime);

            diraction = GetNpcDiraction();

            data.animationController.ChangeAllSprites();
            data.animationController.Play(diraction);

            //Debug.Log(diraction);
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

    Vector2 GetNpcDiraction()
    {
        Vector2 temp = point.position - data.transform.position;

        return Mathf.Abs(temp.x) > Mathf.Abs(temp.y)
                                      ? new Vector2(temp.x, 0)
                                      : new Vector2(0, temp.y);
    }
}
