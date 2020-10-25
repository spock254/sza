using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    Dictionary<string, ICommandAction> commands;
    CommandDB commandDB;

    CommandDB.UserMode currentUserMode;

    void Awake()
    {
        commandDB = GetComponent<CommandDB>();
        commandDB.userMode = CommandDB.UserMode.Guest;
        currentUserMode = CommandDB.UserMode.Guest;
        commands = commandDB.GetCommands();
    }

    public List<string> Interpret(string userInput) 
    {
        //responce.Clear();

        string[] args = userInput.Split();

        CheckUserMode();

        if (commands.ContainsKey(args[0]))
        {
            List<string> res = commands[args[0]].GetActionStatus(args);

            if (res == null) 
            {
                return new List<string>() { "Incorrect argument." };
            }

            return res;
        }

        return new List<string>() { "Command not found." };
        
    }

    void CheckUserMode() 
    {
        if (currentUserMode != commandDB.userMode)
        {
            currentUserMode = commandDB.userMode;
            commands = commandDB.GetCommands();
        }
    }
}
