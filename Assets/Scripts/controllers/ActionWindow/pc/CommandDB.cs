using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using commands;

public class CommandDB : MonoBehaviour
{
    public enum UserMode { Guest, User, Admin }

    //public UserMode userMode;

    Dictionary<string, ICommandAction> guest = new Dictionary<string, ICommandAction>() 
    {
        { "test",  new CommonCommand(new List<string>() { "test terminal" })},
        { "help", new HelpCommand() },
        { "exit", new ExitCommand() },
        { "chuser", new ChUserCommand() }
    };

    Dictionary<string, ICommandAction> user = new Dictionary<string, ICommandAction>()
    {
        { "test2",  new CommonCommand(new List<string>() { "test2 terminal" }) },
        { "printer", new PrinterCommand() }
    };

    Dictionary<string, ICommandAction> admin = new Dictionary<string, ICommandAction>()
    {
        { "test3",  new CommonCommand( new List<string>() { "test3 terminal" } )}
    };

    public Dictionary<string, ICommandAction> GetCommands() 
    {
        TerminalController terminal = GetComponent<TerminalController>();


        if (terminal.GetCurrentPc().userMode == UserMode.Guest)
        {
            return guest;
        }
        else if (terminal.GetCurrentPc().userMode == UserMode.User) 
        {
            return guest.Union(user).ToDictionary(k => k.Key, v => v.Value);
        }

        return guest.Union(user).Union(admin).ToDictionary(k => k.Key, v => v.Value);
    }

}

public interface ICommandAction 
{
    Dictionary<string, string> GetParams();
    List<string> GetActionStatus(string[] param);
    string GetDescription();
}

namespace commands 
{ 
    public class ExitCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            ActionWindowController actionWindow = Global.Component.GetActionWindowController();
            TerminalController terminalController = Global.Component.GetTerminalController();

            terminalController.GetCurrentPc().Close();
            actionWindow.CloseActionWindow("awpc");

            return new List<string>() { "exit status 0" };
        
        }

        public string GetDescription()
        {
            return "cause the shell to exit";
        }

        public Dictionary<string, string> GetParams()
        {
            return null;
        }
    }
    public class CommonCommand : ICommandAction
    {
        List<string> responce;

        public Dictionary<string, string> GetParams() 
        {
            return null;
        }

        public CommonCommand(List<string> responce) 
        {
            this.responce = responce;
        }


        public List<string> GetActionStatus(string[] param)
        {
            return param.Length == 1 ? responce : null;
        }

        public string GetDescription()
        {
            return "test description";
        }
    }
    public class HelpCommand : ICommandAction
    {
        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>() 
            {
                { "-all", "shoes description of all command flags" },
                { "-f", "shoes flags of all commands" },
                { "-s [command]", "description of the selected command" },
                { "-sf [command]", "description and flags of the selected command" }
            };
        }
    


        public List<string> GetActionStatus(string[] param)
        {
            CommandDB commandDB = Global.UIElement.GetTerminalWindow().GetComponent<CommandDB>();


            if (param.Length == 2)
            {
                if (param[1] == "-all")
                {
                    List<string> responce = new List<string>();

                    foreach (KeyValuePair<string, ICommandAction> entry in commandDB.GetCommands())
                    {
                        responce.Add(entry.Key + " - " + entry.Value.GetDescription());

                        if (entry.Value.GetParams() != null)
                        {
                            List<string> flags = new List<string>(entry.Value.GetParams().Keys);
                            responce.Add(entry.Key + " ( " + string.Join(", ", flags) + " )");

                            foreach (KeyValuePair<string, string> flagData in entry.Value.GetParams())
                            {
                                responce.Add(flagData.Key + " " + flagData.Value);
                            }

                        }

                        responce.Add("");
                    }

                    return responce;
                }
                else if (param[1] == "-f")
                {
                    List<string> responce = new List<string>();

                    foreach (KeyValuePair<string, ICommandAction> entry in commandDB.GetCommands())
                    {

                        if (entry.Value.GetParams() != null)
                        {
                            List<string> flags = new List<string>(entry.Value.GetParams().Keys);
                            responce.Add(entry.Key + " ( " + string.Join(", ", flags) + " )");

                        }
                        else
                        {
                            responce.Add(entry.Key + "( No flags )");
                        }

                        responce.Add("");
                    }

                    return responce;
                }
            }
            else if (param.Length == 1)
            {
                return new List<string>(commandDB.GetCommands().Keys);
            }
            else if (param.Length == 3) 
            {
                if (param[1] == "-s")
                {
                    CommandDB commandDb = Global.Component.GetCommandDB();

                    Dictionary<string, ICommandAction> commands = commandDb.GetCommands();

                    if (commands.ContainsKey(param[2]))
                    {
                        return new List<string>() { param[2] + ": " + commands[param[2]].GetDescription() };
                    }
                    else
                    {
                        return new List<string> { "Command not found" };
                    }
                }

                if (param[1] == "-sf") 
                {
                    CommandDB commandDb = Global.Component.GetCommandDB();

                    Dictionary<string, ICommandAction> commands = commandDb.GetCommands();

                    if (commands.ContainsKey(param[2]))
                    {
                        List<string> resp = new List<string>() { param[2] + ": " + commands[param[2]].GetDescription() };
                        
                        List<string> temp = new List<string>();

                        foreach (var item in commands[param[2]].GetParams())
                        {
                            temp.Add(item.Key + " " + item.Value);
                        }

                        resp.AddRange(temp);

                        return resp;
                    }
                    else
                    {
                        return new List<string> { "Command not found" };
                    }
                }
            }

            return null;
        }

        public string GetDescription()
        {
            return "description of commands";
        }
    }
    public class PrinterCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminalController = Global.Component.GetTerminalController();
            PCController pcController = terminalController.GetCurrentPc();
            List<GameObject> peripherals = pcController.peripherals;

            if (param.Length > 1) 
            { 
                if (param[1] == "-status") 
                {
                
                    string enabledStatus = "printer status ( enabled )";
                    string paperStatus = "";
                
                    foreach (var item in peripherals)
                    {
                        if (item.tag == "printer")
                        {

                            if (item.GetComponent<PrinterController>().isPaperInside())
                            {
                                paperStatus = "paper status ( present )";
                            }
                            else
                            {
                                paperStatus = "paper status ( no paper )";
                            }
                        }
                        else 
                        {
                            return new List<string>() { "printer status ( enabled )" };
                        }
                    }

                    return new List<string>() { enabledStatus, paperStatus };
                }
            
                if (param[1] == "-s") 
                {
                    
                    if (param.Length == 2)
                    {
                        return new List<string>() { "Document not selected.", "use: printer -s [ docname ]" };
                    }
                    else if (param.Length == 3)
                    {
                        if (isPrinterPresent(peripherals))
                        {
                            if (pcController.docs.ContainsKey(param[2]))
                            {
                                Item item = pcController.docs[param[2]];
                                PrinterController printerController = GetPrinterFromPeref(peripherals);
                                printerController.itemToPrint = item;

                                return new List<string>() { "Document uploaded successfully." };
                            }
                            else
                            {
                                return new List<string>() { "Incorect document name." };
                            }
                        }
                        else 
                        {
                            return new List<string>() { "printer status ( disabled )" };
                        }
                    }
                    else if (param.Length > 3) 
                    {
                        return new List<string>() { "incorect command syntax", "use: printer -s [ docname ]" };
                    }
                }

                if (param[1] == "-r") 
                {
                    if (isPrinterPresent(peripherals))
                    {
                        PrinterController printerController = GetPrinterFromPeref(peripherals);

                        if (printerController.itemToPrint)
                        {
                            if (!printerController.isPaperInside()) 
                            {
                                return new List<string>() { "printer interrupted", "paper status ( no paper )" };
                            }

                            printerController.OnPrinterClick();

                            return new List<string>() { "the printer finished successfully" };
                        }
                        else 
                        {
                            return new List<string>() { "document not uploaded", "use: printer -s [docname]" };
                        }
                    }
                    else 
                    {
                        return new List<string>() { "printer status ( disabled )" };
                    }
                }
            }

            return new List<string>() { "use with flags -status -s", "for more information use help" };
        }

        bool isPrinterPresent(List<GameObject> peref) 
        {
            foreach (var item in peref)
            {
                if (item.tag == "printer") 
                {
                    return true;
                }
            }

            return false;
        }
        PrinterController GetPrinterFromPeref(List<GameObject> peref) 
        {
            foreach (var item in peref)
            {
                if (item.tag == "printer") 
                {
                    return item.GetComponent<PrinterController>();
                }
            }

            return null;
        }
        public string GetDescription()
        {
            return "working with the printer";
        }

        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>
            {
                { "-status", "shows printer status" },
                { "-s [ docname ]", "set up document for\n\tprinting" },
                { "-r", "run the printer" }
            };
        }
    }

    public class ChUserCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            if (param.Length == 4)
            {
                if (param[1] == "-l") 
                { 
                    PCController pcController = Global.Component.GetTerminalController().GetCurrentPc();

                    Dictionary<string, string> accaunts = pcController.accounts;

                    if (accaunts.ContainsKey(param[2]) && accaunts.ContainsValue(param[3]))
                    {
                        pcController.userMode = CommandDB.UserMode.User;

                        return new List<string>() { "Welcome " + param[2] };
                    }
                    else 
                    {
                        return new List<string>() { "User name or password incorrect" };
                    }
                
                }
            }
            else 
            {
                return null;
            }

            return null;
        }

        public string GetDescription()
        {
            return "login or logout";
        }

        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>()
            {
                { "-l [username] [password]" , "login" }

            };
        }
    }
}