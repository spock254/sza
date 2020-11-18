using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class NPC_STATE_tableItemCheck : BaseState<NPC_DATA_tableItemCheck>
{
    Controller controller;

    ItemCounDetection itemCounDetection;
    int itemCount = 0;
    int itemCountOnTable = 0;
    public override void Enter()
    {
        base.Enter();

        itemCounDetection = GameObject.Find("icd").GetComponent<ItemCounDetection>();
        itemCount = itemCounDetection.itemCount;

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

                    itemCountOnTable = onTableHits.Length;

                    foreach (var itemHit in onTableHits)
                    {
                        Item itemOnTable = itemHit.collider.GetComponent<ItemCell>().item;

                        if (itemOnTable.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Body))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(8));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*stunning " + info.npcName + "*");
                            return;
                        }

                        if (IsEqupmentOnTable(itemHit))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(9));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*stunning " + info.npcName + "*");
                            return;
                        }

                        foreach (var restrictItem in data.restrictItems)
                        {

                            if (restrictItem.IsSameItems(itemOnTable))
                            {
                                dialogueManager.SetDialog(data.GetOptionalDialogByIndex(5));
                                eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");

                                GameObject itemGo = itemHit.collider.gameObject;
                                itemGo.transform.position = data.rejectTable.position;
                                itemGo.name = Global.UNPICKABLE_ITEM + itemOnTable.itemName;
                                itemCount--;

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

                                            //скопирывать айтем на второй стол и сделать не пикаемым
                                            data.InstItem(innerItem);
                                            itemOnTable.innerItems.Remove(innerItem);
                                            itemCount--;
                                            //itemCountOnTable--;
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        if (itemOnTable.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Openable)) 
                        {
                            itemCountOnTable += itemOnTable.innerItems.Count;
                        }
                    }
                    
                    if (!dialogueManager.isOpen)
                    {

                        Item bag_item = controller.bag_btn.GetComponent<ItemCell>().item;

                        if (!controller.IsEmpty(controller.left_hand_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(0));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                            return;
                        }
                        else if (!controller.IsEmpty(controller.right_hand_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(1));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                            return;
                        }
                        else if (!controller.IsEmpty(controller.left_pack_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(2));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                            return;
                        }
                        else if (!controller.IsEmpty(controller.right_pack_btn))
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(3));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                            return;
                        }
                        else if (bag_item.innerItems.Count > 0)
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(4));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                            return;
                        }

                        if (itemCountOnTable < itemCount) 
                        {
                            dialogueManager.SetDialog(data.GetOptionalDialogByIndex(7));
                            eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                        }
                        else 
                        { 
                            machine.ChangeState(data.GetNextStateType(data.nextState));
                        }
                    }

                }

            
            }
        }
    }

    bool IsEqupmentOnTable(RaycastHit2D hit) 
    {
        Item item = hit.collider.GetComponent<ItemCell>().item;
        ItemUseData.ItemType[] itemTypes = item.itemUseData.itemTypes;

        return (itemTypes.Contains(ItemUseData.ItemType.Lags)
                || itemTypes.Contains(ItemUseData.ItemType.Arm)
                || itemTypes.Contains(ItemUseData.ItemType.Face)
                || itemTypes.Contains(ItemUseData.ItemType.Head));
    }
}
