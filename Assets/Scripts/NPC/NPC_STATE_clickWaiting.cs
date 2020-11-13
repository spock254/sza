using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_clickWaiting : ByTheTale.StateMachine.State
{
    string npcName;
    float playerActionRadius = 0;
    Transform playerTransform = null;
    public override void Enter()
    {
        npcName = machine.name;
        playerActionRadius = Global.Component.GetController().GetActioPlayerRadius();
        playerTransform = Global.Obj.GetPlayerGameObject().GetComponent<Transform>();

        // увиличить радиус на 20% (не может одстать больше чем 1 тайл радиус)
        playerActionRadius += playerActionRadius / 100 * 20;
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
                    if (Vector2.Distance(playerTransform.position, hit.transform.position) <= playerActionRadius) 
                    { 
                        Debug.Log(npcName + " clicked");
                        return;
                    }
                }
            }
        }
    }
}
