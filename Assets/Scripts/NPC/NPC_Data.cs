using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByTheTale.StateMachine;

public class NPC_Data : MonoBehaviour
{
    public string npcName = null;

    StateMachineBehaviour stateMachine;

    [SerializeField]
    List<Item> items = null;

    [Header("NPC_STATE_itemRequier")]
    #region NPC_STATE_itemRequier

    [SerializeField]
    [TextArea(3,10)]
    List<string> dialogs_itemRequier;

    [SerializeField]
    [TextArea(3, 10)]
    List<string> optionDialog_itemRequier;

    [HideInInspector]
    public List<Item> savedItems = new List<Item>();

    int dialogIndex_itemRequier = 0;
    int optionDialogIndex_itemRequier = 0;
    int itemIndex = 0;
    #endregion

    #region NPC_STATE_itemRequier
    [Header("NPC_STATE_thinking")]
    [SerializeField]
    [TextArea(3, 10)]
    string dialog_thinking;
    #endregion

    void Start()
    {
        
        //stateMachine = GetComponent<StateMachineBehaviour>();    
    }

    //public string GetNextDialog() 
    //{
    //    string toReturn = null;

    //    if (dialogs.Count > dialogIndex) 
    //    {
    //        toReturn = dialogs[dialogIndex];
    //        dialogIndex++;
    //    }

    //    return toReturn;
    //}

    string NextDialog(List<string> dialog, ref int index) 
    {
        string lineToReturn = null;

        if (dialog.Count > index) 
        {
            lineToReturn = dialog[index];
            index++;
        }

        return lineToReturn;
    }


    public string GetNextDialog(StateTypes stateTypes, bool isOption = false)
    {
        if (stateTypes == StateTypes.NPC_STATE_itemRequier) 
        { 
            return (isOption == false) ? NextDialog(dialogs_itemRequier, ref dialogIndex_itemRequier) 
                                       : NextDialog(optionDialog_itemRequier, ref optionDialogIndex_itemRequier);
        }
        else if (stateTypes == StateTypes.NPC_STATE_thinking) 
        {
            return dialog_thinking;
        }

        return null;
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

    //public bool isLastOptionDialog() 
    //{
    //    return optionDialogIndex == optionDialog.Count;
    //}

    public bool isLastRequierdItem()
    {
        return itemIndex == items.Count;
    }
}
