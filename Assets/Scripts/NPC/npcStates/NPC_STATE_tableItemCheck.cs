using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC_STATE_tableItemCheck : BaseState
{
    DialogueManager dialogueManager;
    EventController eventController;
    Controller controller;

    NPC_DATA_tableItemCheck data;
    NPC_Info info;

    public override void Enter()
    {
        base.Enter();

        data = GetData<NPC_DATA_tableItemCheck>();
        info = GetInfo();

        eventController = Global.Component.GetEventController();
        dialogueManager = Global.Component.GetDialogueManager();
        controller = Global.Component.GetController();

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
                    RaycastHit2D[] onTableHits = Physics2D.RaycastAll(data.table.position, Vector2.zero)
                        .Where(i => i.collider.name.Contains(Global.DROPED_ITEM_PREFIX)).ToArray();
                    
                    foreach (var itemHit in onTableHits)
                    {
                        Debug.Log(itemHit.collider.name);
                        
                        foreach (var restrictItem in data.restrictItems)
                        {
                            Item itemOnTable = itemHit.collider.GetComponent<ItemCell>().item;

                            if (restrictItem.IsSameItems(itemOnTable))
                            {
                                dialogueManager.SetDialog(data.GetOptionalDialogByIndex(5));
                                eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                                return;
                            }
                            else 
                            {
                                if (itemOnTable.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Openable)) 
                                {
                                    foreach (var innerItem in itemOnTable.innerItems)
                                    {
                                        if (restrictItem.IsSameItems(innerItem)) 
                                        {
                                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(6));
                                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!dialogueManager.isOpen)
                    {

                        Item bag_item = controller.bag_btn.GetComponent<ItemCell>().item;

                        if (!controller.IsEmpty(controller.left_hand_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(0));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                        }
                        else if (!controller.IsEmpty(controller.right_hand_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(1));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                        }
                        else if (!controller.IsEmpty(controller.left_pack_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(2));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                        }
                        else if (!controller.IsEmpty(controller.right_pack_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(3));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                        }
                        else if (bag_item.innerItems.Count > 0)
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(4));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                        }

                    }
                }

            
            }
        }
    }
}
