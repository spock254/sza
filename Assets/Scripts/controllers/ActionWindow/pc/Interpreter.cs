using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    Dictionary<string, ICommandAction> commands = null;
    CommandDB commandDB;

    void Awake()
    {
        commandDB = GetComponent<CommandDB>();
    }

    public List<string> Interpret(string userInput) 
    {
        string[] args = userInput.Split();
        commands = commandDB.GetCommands();

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

}
