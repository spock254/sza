using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TerminalAutocomplete : MonoBehaviour
{
    TerminalController terminalController;
    CommandDB commandDB;
    Dictionary<string, ICommandAction> commands = null;
    [SerializeField]
    List<Text> hintLines = null;
    void Start()
    {
        terminalController = GetComponent<TerminalController>();
        commandDB = GetComponent<CommandDB>();
    }
    public void ValueChange()
    {
        commands = commandDB.GetCommands();
        string input = terminalController.terminalInput.text;

        SetHints(new List<string>(commands.Keys), input);

        foreach (var command in commands)
        {
            if (command.Key == input.Split()[0]) 
            {
                Dictionary<string, string> param = command.Value.GetParams();

                if (param == null) 
                {
                    return;
                }

                List<string> flags = new List<string>(param.Keys);

                if (input.Split().Length != 2) 
                {
                    return;
                }

                SetHints(flags, input.Split()[1], true);

            }
        }
    }

    void SetHints(List<string> flags, string input, bool offset = false) 
    {
        List<string> maped = new List<string>();
        string offsetStr = string.Empty;

        if (offset == true) 
        {
            offsetStr = ParseToStringOffset(terminalController.terminalInput.text
                                                    .Split()[0] + " ");
        }

        foreach (var flag in flags)
        {
            if (flag.StartsWith(input) && input != string.Empty) 
            {
                maped.Add(flag);
            }
        }

        if (maped.Count > 0)
        {
            int i = 0;
            for (; i < maped.Count && i < hintLines.Count; i++)
            {
                hintLines[i].text = offsetStr + maped[i];
            }

            for (; i < hintLines.Count; i++)
            {
                hintLines[i].text = string.Empty;
            }
        }
        else 
        {
            foreach (var hint in hintLines)
            {
                hint.text = string.Empty;
            }
        }
    }

    string ParseToStringOffset(string str) 
    {
        string toReturn = string.Empty;
        
        foreach (var ch in str)
        {
            toReturn += " ";
        }

        return toReturn;
    }
}
