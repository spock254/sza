using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_clickWaiting : BaseState<NPC_DATA_clickWaiting>
{
    public override void Enter()
    {
        base.Enter();

        data.animationController.ChangeAllSprites();
        data.animationController.Turn(data.turnDiraction);

    }
    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
    }

    public override void PostExecute()
    {
        base.PostExecute();

        data.animationController.UpdateSprites();
    }
}
