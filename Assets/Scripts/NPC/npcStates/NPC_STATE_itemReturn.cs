using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_STATE_itemReturn : BaseState<NPC_Data_itemReturn>
{
    Controller controller;
    Item itemToReturn;
    public override void Enter()
    {
        base.Enter();

        controller = Global.Component.GetController();
        // пока возврощает 1 айтем
        itemToReturn = GameObject.Find(npcName).GetComponent<NPC_DATA_itemRequier>().savedItems[0];

        controller.currentHand.GetComponent<ItemCell>().item = itemToReturn;
        controller.currentHand.GetComponent<Image>().sprite = itemToReturn.itemSprite;

        dialogueManager.SetDialog(data.GetNextDialog());
        eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");
    }

    public override void Execute()
    {
        if (!dialogueManager.isOpen) 
        {
            machine.ChangeState(data.GetNextStateType(data.nextState));
        }
    }
}
