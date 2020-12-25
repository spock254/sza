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
    [SerializeField]
    Text directory;

    int hintIndex = -1;

    [SerializeField]
    Color hintColor = Color.red;
    [SerializeField]
    Color defaultHintColor = Color.red;

    void Start()
    {
        terminalController = GetComponent<TerminalController>();
        commandDB = GetComponent<CommandDB>();

        ResetHintColor();
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
                terminalController.terminalInput.enabled = false;
                List<Text> hintWithContent = new List<Text>();

                foreach (var hint in hintLines)
                {
                    if (hint.text != string.Empty)
                    {
                        hintWithContent.Add(hint);
                    }
                }

                
                if (hintIndex > 0)
                {
                    hintIndex--;

                    foreach (var hint in hintLines)
                    {
                        hint.color = defaultHintColor;
                    }

                    hintLines[hintIndex].color = hintColor;
                }
                else
                {
                    terminalController.terminalInput.enabled = true;
                    StartCoroutine(SetCarret());

                    hintIndex = hintWithContent.Count;
                    foreach (var hint in hintLines)
                    {
                        hint.color = defaultHintColor;
                    }
                }
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

                    ResetHintColor();

                    hintLines[hintIndex].color = hintColor;
                }
                else
                {
                    terminalController.terminalInput.enabled = true;
                    StartCoroutine(SetCarret());

                    hintIndex = -1;
                    ResetHintColor();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (hintIndex != -1)
                {
                    string currentInput = terminalController.terminalInput.text;

                    if (currentInput.Split().Length == 2
                        || (currentInput.Split().Length == 1 && currentInput.EndsWith(" ")))
                    {
                        string flagToappend = hintLines[hintIndex].text.Trim().Split()[0].Substring(1);
                        terminalController.terminalInput.text = terminalController.terminalInput
                                                                .text.Split()[0] + " -" + flagToappend;
                    }
                    else if (currentInput.Split().Length == 3 
                        || (currentInput.Split().Length == 2 && currentInput.EndsWith(" "))) 
                    {
                        string[] splitedInput = terminalController.terminalInput.text.Split();
                        string argToappend = hintLines[hintIndex].text.Trim();
                        
                        terminalController.terminalInput.text = splitedInput[0] + " " 
                                                                + splitedInput[1] + " " 
                                                                + argToappend;
                    }
                    else
                    {
                        terminalController.terminalInput.text = hintLines[hintIndex].text;
                    }

                    hintIndex = -1;
                    ResetHintColor();

                    terminalController.terminalInput.enabled = true;
                    StartCoroutine(SetCarret());
                    setCarret = true;

                    //ValueChange();

                }
            }
        }
    }

    void OnGUI()
    {
        if (terminalController.isOpen == true) 
        { 
            Event e = Event.current;

            if (e.type == EventType.KeyDown &&
                ((e.keyCode.ToString().Length == 1 &&
                char.IsLetter(e.keyCode.ToString()[0]))) || e.keyCode == KeyCode.Backspace)
            {

                if (hintIndex != -1) 
                {
                    string userInput = terminalController.terminalInput.text;

                    hintIndex = -1;
                    ResetHintColor();

                    terminalController.terminalInput.enabled = true;
                    StartCoroutine(SetCarret());
                    setCarret = true;

                    if (e.keyCode == KeyCode.Backspace) 
                    {
                        terminalController.terminalInput.text = userInput.Remove(userInput.Length - 1);
                        return;
                    }

                    terminalController.terminalInput.text += e.keyCode.ToString().ToLower();
                }
            }
        }
    }

    IEnumerator SetCarret() 
    {
        yield return new WaitForEndOfFrame();
        terminalController.terminalInput.caretPosition = terminalController.terminalInput.text.Length;
        terminalController.terminalInput.ForceLabelUpdate();
    }

    void ResetHintColor() 
    {
        foreach (var hint in hintLines)
        {
            hint.color = defaultHintColor;
        }
    }

    public void ValueChange()
    {
        commands = commandDB.GetCommands();

        string input = terminalController.terminalInput.text;
        string[] spletedInput = input.Split();

        KeyValuePair<string, ICommandAction> ?commandKeyValue = null;

        CommandValidation(spletedInput, commands);

        if (input == string.Empty) 
        {
            foreach (var hint in hintLines)
            {
                hint.text = string.Empty;
            }

            return; 
        }

        SetHints(new List<string>(commands.Keys), input);


        foreach (var command in commands)
        {
            if (command.Key == spletedInput[0]) 
            {
                commandKeyValue = command;
                Dictionary<string, string> param = command.Value.GetFlagDescription();

                if (param == null) 
                {
                    return;
                }

                List<string> flags = new List<string>(param.Keys);

                if (spletedInput.Length == 2) 
                {
                    SetHints(flags, spletedInput[1], spletedInput[0].Length + 1);
                    //return;
                }
            }
        }

        if (spletedInput.Length == 3 && commandKeyValue != null)
        {
            var param = commandKeyValue.Value.Value.GetParams();
            string flag = spletedInput[1];
            string arg = spletedInput[2];

            if (param != null && param.Keys.Contains(flag))
            {
                SetHints(param[flag], arg, spletedInput[0].Length + 1 + spletedInput[1].Length + 1);
            }
        }
    }

    void SetHints(List<string> flags, string input, int offset = 0) 
    {
        List<string> maped = new List<string>();
        string offsetStr = ParseToStringOffset(offset);

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

    string ParseToStringOffset(int spaces) 
    {
        string toReturn = string.Empty;

        for (int i = 0; i < spaces; i++)
        {
            toReturn += " ";
        }

        return toReturn;
    }

    void CommandValidation(string[] spletedInput, Dictionary<string, ICommandAction> commands) 
    {
        foreach (var command in commands)
        {
            if (command.Key == spletedInput[0].Trim())
            {
                if (command.Value.IsValidCommand(terminalController.terminalInput.text))
                {
                    SetDirectoryColor(Color.green);
                }
                else
                {
                    SetDirectoryColor(Color.red);
                }

                return;
            }
        }

        SetDirectoryColor(Color.red);
    }
    void SetDirectoryColor(Color color) 
    {
        directory.color = color;
    }
}
