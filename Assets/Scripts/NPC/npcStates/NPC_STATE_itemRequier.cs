using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC_STATE_itemRequier : BaseState<NPC_DATA_itemRequier>
{
    Controller controller;

    Item requieredItem;
    string rejectDialog = null;
    public override void Enter()
    {
        base.Enter();

        controller = Global.Component.GetController();

        if (data.isLastRequierdItem()) 
        {
            machine.ChangeState(data.GetNextStateType(data.nextState));
            return;
        }

        requieredItem = data.GetNextItem();
        // option dialog
        rejectDialog = data.GetNextOptionalDialog();


        dialogueManager.SetDialog(data.GetNextDialog());
        eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");

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
                        if (IsInNpcRadius(hit.transform.position) && data.table != null)
                        {
                            RaycastHit2D[] onTableHits = Physics2D.RaycastAll(data.table.position, Vector2.zero)
                                                    .Where(i => i.collider.name
                                                    .Contains(Global.DROPED_ITEM_PREFIX))
                                                    .ToArray();

                            foreach (var itemHit in onTableHits)
                            {
                                Item itemOnTable = itemHit.collider.GetComponent<ItemCell>().item;

                                if (requieredItem.IsSameItems(itemOnTable))
                                {
                                    data.savedItems.Add(itemOnTable);
                                    data.DestroyItem(itemHit.collider.gameObject);
                                    machine.ChangeState<NPC_STATE_itemRequier>();
                                }
                            }

                            if (!dialogueManager.isOpen)
                            {
                                dialogueManager.SetDialog(rejectDialog);
                                eventController.OnStartDialogEvent.Invoke(info.npcName, "*disgruntled " + info.npcName + "*");
                            }
                        }
                        else
                        {
                            //data.ResetState();

                            dialogueManager.SetDialog(data.GetDialogByIndex(0));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");
                            return;
                        }
                }
            }
            
            //else 
            //{
            //    //data.ResetState();

            //    dialogueManager.SetDialog(data.GetDialogByIndex(0));
            //    eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");
            //    return;
            //}
        }

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
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*disgruntled " + info.npcName + "*");
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
