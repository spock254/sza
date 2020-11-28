using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BaseData : MonoBehaviour
{
    [SerializeField]
    [TextArea(3, 10)]
    List<string> dialog = null;

    [SerializeField]
    [TextArea(3, 10)]
    List<string> optionDialog = null;

    int dialogIndex = 0;
    int optionDialogIndex = 0;

    public StateTypes nextState;

    public virtual void ResetState() 
    {
        dialogIndex = 0;
        optionDialogIndex = 0;
    }

    public string GetNextDialog()
    {
        string lineToReturn = null;

        if (dialog.Count > dialogIndex)
        {
            lineToReturn = dialog[dialogIndex];
            dialogIndex++;
        }

        return lineToReturn;
    }

    public string GetNextOptionalDialog()
    {
        string lineToReturn = null;

        if (optionDialog.Count > optionDialogIndex)
        {
            lineToReturn = optionDialog[optionDialogIndex];
            optionDialogIndex++;
        }

        return lineToReturn;
    }

    public string GetDialogByIndex(int index) 
    {
        return dialog[index];
    }

    public string GetOptionalDialogByIndex(int index) 
    {
        return optionDialog[index];
    }
    public Type GetNextStateType(StateTypes stateTypes)
    {
        return Type.GetType(stateTypes.ToString());
    }

    public Vector2 GetNpcDiraction(Vector2 to, Vector2 from)
    {
        Vector2 temp = to - from;

        return Mathf.Abs(temp.x) > Mathf.Abs(temp.y)
                                      ? new Vector2(temp.x, 0)
                                      : new Vector2(0, temp.y);
    }
}
