using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusController : MonoBehaviour
{
    public Item ticketPatern;
    Item ticket = null;
    DialogueManager dialogue;

    [TextArea(3, 10)]
    public string noTicketDialogue;
    [TextArea(3, 10)]
    public string ticketDialogue;

    Controller controller;
    void Start()
    {
        dialogue = Global.Component.GetDialogueManager();
        controller = Global.Component.GetController();
    }

    public void Activate() 
    {
        if (ticket == null) 
        {
            dialogue.SetDialog(noTicketDialogue);
            Global.Component.GetEventController().OnStartDialogEvent.Invoke("*bus driver*", string.Empty);
            
        }
        else
        {
            dialogue.SetDialog(ticketDialogue);
            Global.Component.GetEventController().OnStartDialogEvent.Invoke("*happy bus driver*", string.Empty);
        }

    }
    public void GiveTicket(Button ticketBtn) 
    {
        Item ticketInHand = ticketBtn.GetComponent<ItemCell>().item;

        if (ticketInHand.IsSameItems(ticketPatern)) 
        {
            ticket = ticketInHand;
            controller.SetDefaultItem(ticketBtn);
        }

    }
}
