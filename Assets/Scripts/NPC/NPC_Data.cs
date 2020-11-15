using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByTheTale.StateMachine;
using System;

public class NPC_Data : MonoBehaviour
{
    public string npcName = null;

    #region NPC_STATE_clickWaiting
    [Space(10)]
    public StateTypes NextState_clickWaiting;
    #endregion

    #region NPC_STATE_itemRequier
    [Header("NPC_STATE_itemRequier")]
    [Space(10)]
    [SerializeField]
    List<Item> items = null;

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

    public StateTypes NextState_itemRequier;
    [Space(10)]
    #endregion

    #region NPC_STATE_thinking
    [Header("NPC_STATE_thinking")]
    [SerializeField]
    [TextArea(3, 10)]
    string dialog_thinking;

    public StateTypes NextState_thinking;
    #endregion

    #region NPC_STATE_tableItemCheck
    [Space(10)]
    [Header("NPC_STATE_tableItemCheck")]
    [SerializeField]
    [TextArea(3, 10)]
    List<string> dialogs_tableItemCheck;

    [SerializeField]
    [TextArea(3, 10)]
    List<string> optionDialog_tableItemCheck;

    int dialogIndex_tableItemCheck = 0;
    int optionDialogIndex_tableItemCheck = 0;

    public StateTypes NextState_tableItemCheck;

    public Transform table_tableItemCheck;
    public List<Item> restrictItems_tableItemCheck;
    #endregion

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
        else if (stateTypes == StateTypes.NPC_STATE_tableItemCheck) 
        {
            return (isOption == false) ? NextDialog(dialogs_tableItemCheck, ref dialogIndex_tableItemCheck)
                           : NextDialog(optionDialog_tableItemCheck, ref optionDialogIndex_tableItemCheck);
        }

        return null;
    }

    public string GetDialogByIndex(StateTypes stateTypes, int index, bool isOptional = false) 
    {
        if (stateTypes == StateTypes.NPC_STATE_tableItemCheck) 
        {
            return optionDialog_tableItemCheck[index];
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

    public Type GetNextStateType(StateTypes stateTypes) 
    {
        return Type.GetType(stateTypes.ToString());
    }
}
