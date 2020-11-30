using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommand : DebugCommandBase
{
    //Action command;
    public delegate void CommandAction(string args);
    CommandAction command;
    public DebugCommand(string id, string description, string format, CommandAction command) : 
        base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(string args) 
    {
        command.Invoke(args);
    }
}
public class DebugCommandBase
{
    string id;
    string description;
    string format;

    public DebugCommandBase(string id, string description, string format)
    {
        Id = id;
        Description = description;
        Format = format;
    }

    public string Id { get => id; set => id = value; }
    public string Description { get => description; set => description = value; }
    public string Format { get => format; set => format = value; }


}
