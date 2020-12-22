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
                List<string> flags = new List<string>(command.Value.GetParams().Keys);
                if (input.Split().Length != 2) 
                {
                    return;
                }

                SetHints(flags, input.Split()[1]);

            }
        }
    }

    void SetHints(List<string> flags, string input) 
    {
        List<string> maped = new List<string>();

        foreach (var flag in flags)
        {
            if (flag.StartsWith(input)) 
            {
                maped.Add(flag);
            }
        }

        if (maped.Count > 0)
        {
            int i = 0;
            for (; i < maped.Count && i < hintLines.Count; i++)
            {
                hintLines[i].text = maped[i];
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

    void SetNextHintLine(string comStr) 
    { 
        
    }
}
