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

    int hintIndex = -1;

    

    void Start()
    {
        terminalController = GetComponent<TerminalController>();
        commandDB = GetComponent<CommandDB>();
    }
    bool setCarret = false;
    string prevInput = string.Empty;
    void Update()
    {
        if (terminalController.isOpen == true) 
        { 
            if (hintIndex == -1) 
            {
                prevInput = terminalController.terminalInput.text;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                terminalController.terminalInput.enabled = false;

                List<Text> hintWithContent = new List<Text>();
                foreach (var hint in hintLines)
                {
                    if (hint.text != string.Empty)
                    {
                        hintWithContent.Add(hint);
                    }
                }

                if (hintIndex < hintWithContent.Count - 1)
                {
                    hintIndex++;

                    foreach (var hint in hintLines)
                    {
                        hint.color = Color.white;
                    }
                    hintLines[hintIndex].color = Color.red;
                }
                else
                {
                    terminalController.terminalInput.enabled = true;
                    StartCoroutine(SetCarret());
                    setCarret = true;
                    //terminalController.terminalInput.Select();
                    //terminalController.terminalInput.ActivateInputField();
                    hintIndex = -1;
                    foreach (var hint in hintLines)
                    {
                        hint.color = Color.white;
                    }
                }

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (hintIndex != -1)
                {
                    if (hintLines[hintIndex].text[0] == ' ')
                    {
                        string flagToappend = hintLines[hintIndex].text.Trim().Split()[0].Substring(1);
                        terminalController.terminalInput.text += flagToappend;
                    }
                    else 
                    { 
                        terminalController.terminalInput.text = hintLines[hintIndex].text;
                    }

                    hintIndex = -1;
                    foreach (var hint in hintLines)
                    {
                        hint.color = Color.white;
                    }

                    terminalController.terminalInput.enabled = true;
                    StartCoroutine(SetCarret());
                    setCarret = true;

                }
            }
            //else 
            //{
                
            //    if (terminalController.terminalInput.enabled == false) 
            //    {
            //        terminalController.terminalInput.enabled = true;
            //    }
            //}
        
        }
    }

    IEnumerator SetCarret() 
    {
        yield return new WaitForEndOfFrame();
        terminalController.terminalInput.caretPosition = terminalController.terminalInput.text.Length;
        terminalController.terminalInput.ForceLabelUpdate();
    }

    //private void LateUpdate()
    //{
    //    if (setCarret == true) 
    //    {
    //        terminalController.terminalInput.caretPosition = terminalController.terminalInput.text.Length;
    //        terminalController.terminalInput.ForceLabelUpdate();
    //        setCarret = false;
    //    }
    //}
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
