using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Data : MonoBehaviour
{
    public string npcName = null;

    [SerializeField]
    List<Item> items = null;

    [Header("dialog lists")]
    [SerializeField]
    [TextArea(3,10)]
    List<string> dialogs;

    [SerializeField]
    [TextArea(3, 10)]
    List<string> optionDialog;

    [HideInInspector]
    public List<Item> savedItems = new List<Item>();

    int dialogIndex = 0;
    int optionDialogIndex = 0;
    int itemIndex = 0;
    public string GetNextDialog() 
    {
        string toReturn = null;

        if (dialogs.Count > dialogIndex) 
        {
            toReturn = dialogs[dialogIndex];
            dialogIndex++;
        }

        return toReturn;
    }

    public string GetNextOptionDialog()
    {
        string toReturn = null;

        if (optionDialog.Count > optionDialogIndex)
        {
            toReturn = optionDialog[optionDialogIndex];
            optionDialogIndex++;
        }

        return toReturn;
    }

    public Item GetNextItem() 
    {
        Item toReturn = null;

        if (items.Count > itemIndex)
        {
            toReturn = items[itemIndex];
            itemIndex++;
        }

        return toReturn;
    }

    public bool isLastOptionDialog() 
    {
        return optionDialogIndex == optionDialog.Count;
    }

    public bool isLastRequierdItem()
    {
        return itemIndex == items.Count;
    }
}
