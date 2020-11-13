using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_STATE_itemRequier : BaseState
{
    Controller controller;
    DialogueManager dialogueManager;
    EventController eventController;

    Item requieredItem;
    string rejectDialog = null;
    public override void Enter()
    {
        base.Enter();

        controller = Global.Component.GetController();
        dialogueManager = Global.Component.GetDialogueManager();
        eventController = Global.Component.GetEventController();

        if (data.isLastRequierdItem()) 
        {
            machine.ChangeState<NPC_STATE_thinking>();
            return;
        }

        requieredItem = data.GetNextItem();
        rejectDialog = data.GetNextOptionDialog();


        dialogueManager.SetDialog(data.GetNextDialog());
        eventController.OnStartDialogEvent.Invoke(data.npcName, "*" + data.npcName + "*");

    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(1))
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
                        Item itemInHand = controller.GetItemInHand(controller.currentHand);
                        
                        if (requieredItem.IsSameItems(itemInHand)) 
                        {
                            data.savedItems.Add(itemInHand);
                            controller.SetDefaultItem(controller.currentHand);
                            machine.ChangeState<NPC_STATE_itemRequier>();
                        
                        }
                        else 
                        { 
                            dialogueManager.SetDialog(rejectDialog);
                            eventController.OnStartDialogEvent.Invoke(data.npcName, "*disgruntled " + data.npcName + "*");
                        }
                        return;
                    }
                }
            }
        }
    }

    //public override void PostExecute()
    //{
    //    if (data.isLastOptionDialog()) 
    //    {
    //        machine.ChangeState<NPC_STATE_thinking>();
    //    }
    //}
}
