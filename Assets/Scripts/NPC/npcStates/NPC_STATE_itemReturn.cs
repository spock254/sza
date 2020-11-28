using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_STATE_itemReturn : BaseState<NPC_Data_itemReturn>
{
    Controller controller;
    Item itemToReturn;
    NPC_DATA_itemRequier data_ItemRequier;
    public override void Enter()
    {
        base.Enter();

        data.ResetState();

        controller = Global.Component.GetController();
        // пока возврощает 1 айтем
        data_ItemRequier = GameObject.Find(npcName).GetComponent<NPC_DATA_itemRequier>();
        itemToReturn = data_ItemRequier.savedItems[0];

        controller.currentHand.GetComponent<ItemCell>().item = itemToReturn;
        controller.currentHand.GetComponent<Image>().sprite = itemToReturn.itemSprite;

        dialogueManager.SetDialog(data.GetNextDialog());
        eventController.OnStartDialogEvent.Invoke(info.npcName, "*" + info.npcName + "*");
    }

    public override void Execute()
    {
        if (!dialogueManager.isOpen) 
        {
            data_ItemRequier.ResetState();
            machine.ChangeState(data.GetNextStateType(data.nextState));
        }
    }
}
