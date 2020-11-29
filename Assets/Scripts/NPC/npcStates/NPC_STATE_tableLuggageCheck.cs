using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC_STATE_tableLuggageCheck : BaseState<NPC_DATA_tableLuggageCheck>
{
    public override void Enter()
    {
        base.Enter();

        dialogueManager.SetDialog(data.GetDialogByIndex(0));
        eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");
    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0) && dialogueManager.isOpen == false)
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
                        Item itemOnTable = itemHit.collider.GetComponent<ItemCell>().item;

                        if (itemOnTable.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Openable)
                            && itemOnTable.itemUseData.itemSize == ItemUseData.ItemSize.Middle) 
                        {
                            if (itemOnTable.innerItems.Count > 7)
                            {
                                dialogueManager.SetDialog(data.GetOptionalDialogByIndex(0));
                                eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                                return;
                            }
                            else 
                            {
                                if (itemOnTable.innerItems.Find(c => c.IsSameItems(data.card))) 
                                {
                                    dialogueManager.SetDialog(data.GetOptionalDialogByIndex(1));
                                    eventController.OnStartDialogEvent.Invoke(info.npcName, "*angry " + info.npcName + "*");
                                    return;
                                }

                                data.savedItems.Add(itemOnTable);
                                data.DestroyItem(itemHit.collider.gameObject);
                            }
                        }
                    }


                }
            }
        }
                    }
}
