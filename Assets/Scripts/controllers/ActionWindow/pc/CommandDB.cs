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
        { "help", new HelpCommand() },
        { "exit", new ExitCommand() },
        { "chuser", new ChUserCommand() },
        { "whoami", new WhoamiCommand() },
        { "docs", new DocsCommand() }
    };

    Dictionary<string, ICommandAction> user = new Dictionary<string, ICommandAction>()
    {
        { "printer", new PrinterCommand() },
        { "disk", new DiskCommand() },
        { "accaunt", new AccauntCommand() },
        { "per", new PeripheralCommand() }
    };

    Dictionary<string, ICommandAction> admin = new Dictionary<string, ICommandAction>()
    {
        { "test3",  new CommonCommand( new List<string>() { "test3 terminal" } )}
    };

    public Dictionary<string, ICommandAction> GetCommands() 
    {
        TerminalController terminal = GetComponent<TerminalController>();

        UserMode userMode = terminal.GetCurrentPc().currentMemory.userMode;

        if (userMode == UserMode.Guest)
        {
            return guest;
        }
        else if (userMode == UserMode.User) 
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
                
                    string enabledStatus = "printer status ( disabled )";
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

                            enabledStatus = "printer status ( enabled )";
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
                            if (pcController.currentMemory.docs.ContainsKey(param[2]))
                            {
                                Item item = pcController.currentMemory.docs[param[2]];
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
                { "-s [docname]", "set up document for printing" },
                { "-r", "run the printer" }
            };
        }
    }
    public class ChUserCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            PCController pcController = Global.Component.GetTerminalController().GetCurrentPc();
            List<PCMempryContent> mempryContents = pcController.memoryContents;
            
            if (param.Length == 4)
            {
                if (param[1] == "-login")
                {

                    foreach (var item in mempryContents)
                    {
                        if (item.userName == param[2] && item.password == param[3])
                        {
                            pcController.currentMemory = item;

                            return new List<string>() { "Welcome " + param[2] };
                        }

                    }

                    return new List<string>() { "User name or password incorrect" };

                }

            }
            else if (param.Length == 2) 
            {
                if (param[1] == "-logout")
                {
                    if (pcController.currentMemory.userMode != CommandDB.UserMode.Guest)
                    {
                        PCMempryContent guestMemoryContent = mempryContents.Where(x => x.userMode == CommandDB.UserMode.Guest)
                                                                      .FirstOrDefault();
                        pcController.currentMemory = guestMemoryContent;

                        return new List<string>() { "Your user mode status is guest" };
                    }
                    else
                    {
                        return new List<string>() { "You are already a guest" };
                    }
                }
                else if (param[1] == "-l") 
                {
                    List<string> allUsers = new List<string>();

                    foreach (var item in pcController.memoryContents)
                    {
                        if (pcController.currentMemory.userName == item.userName)
                        {
                            allUsers.Add(item.userName + ": " + item.userMode + "*");
                        }
                        else 
                        { 
                            allUsers.Add(item.userName + ": " + item.userMode);
                        }
                    }

                    return allUsers;
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
                { "-login [username] [password]" , "login as user" },
                { "-logout", "turn to guest mode" },
                { "-l", "list all registered users" }

            };
        }
    }
    public class WhoamiCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            if (param.Length == 1) 
            {
                TerminalController terminal = Global.Component.GetTerminalController();
                return new List<string>() { "You are logged in as " + terminal.GetCurrentPc().currentMemory.userName, 
                                            "User mode " + terminal.GetCurrentPc().currentMemory.userMode.ToString() };
            }

            return null;
        }

        public string GetDescription()
        {
            return "shows you user name and user mode";
        }

        public Dictionary<string, string> GetParams()
        {
            return null;
        }
    }
    public class DocsCommand : ICommandAction
    {

        public List<string> GetActionStatus(string[] param)
        {
            if (param.Length == 1) 
            { 
                TerminalController terminal = Global.Component.GetTerminalController();
                Dictionary<string, Item> docs = terminal.GetCurrentPc().currentMemory.docs;

                List<string> res = new List<string>();

                foreach (var item in docs)
                {
                    res.Add(item.Key);
                }

                if (res.Count == 0) 
                {
                    return new List<string>() { "Documents not found" };
                }

                return res;
            }

            return null;
        }

        public string GetDescription()
        {
            return "prints all documents";
        }

        public Dictionary<string, string> GetParams()
        {
            return null;
        }
    }
    public class DiskCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();

            if (param.Length == 2)
            {
                if (param[1] == "-status")
                {

                    if (pcController.disk != null)
                    {
                        return new List<string>() { "disk status ( enabled )", pcController.disk.itemDescription };
                    }
                    else
                    {
                        return new List<string>() { "disk status ( disabled )" };
                    }
                }
                else if (param[1] == "-out")
                {
                    if (pcController.disk != null)
                    {
                        PrefbDB prefbDB = Global.Component.GetPrefbDB();
                        GameObject itemPref = prefbDB.GetItemPrefab();
                        string discDescription = pcController.disk.itemDescription;

                        itemPref.GetComponent<ItemCell>().item = pcController.disk;
                        itemPref.GetComponent<SpriteRenderer>().sprite = pcController.disk.itemSprite;
                        prefbDB.InstantiateItemPref(pcController.transform.position);


                        pcController.disk = null;

                        return new List<string>() { "disk " + discDescription + " logged out successfully" };
                    }
                    else
                    {
                        return new List<string>() { "disk status ( disabled )" };
                    }
                }
                else if (param[1] == "-c")
                {
                    if (pcController.disk != null)
                    {
                        List<string> docsName = new List<string>();
                        foreach (var item in pcController.disk.innerItems)
                        {
                            docsName.Add(item.itemDescription);
                        }

                        if (docsName.Count == 0)
                        {
                            return new List<string>() { "Disk is empty" };
                        }

                        return docsName;
                    }
                    else
                    {
                        return new List<string>() { "disk status ( disabled )" };
                    }
                }
            }
            else if (param.Length == 3) 
            {
                if (param[1] == "-cpy")
                {
                    if (pcController.disk != null)
                    {
                        foreach (var item in pcController.disk.innerItems)
                        {
                            if (item.itemDescription == param[2])
                            {
                                //если на компе есть тот же документ
                                foreach (var pcItem in pcController.currentMemory.docs)
                                {
                                    if (item.itemDescription == pcItem.Key)
                                    {
                                        return new List<string>() { item.itemDescription + " is already exist" };
                                    }
                                }

                                pcController.currentMemory.docs.Add(item.itemDescription, item);

                                return new List<string>() { item.itemDescription + " copied successfully" };
                            }
                        }
                    }
                    else
                    {
                        return new List<string>() { "disk status ( disabled )" };
                    }
                }
            }

            return null;
        }

        public string GetDescription()
        {
            return "working with the disk";
        }

        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>()
            {
                { "-status", "shows disk status" },
                { "-out", "plug out disk" },
                { "-c", "shows disk content" },
                { "-cpy [docName]", "copies selected document" }
            };
        }
    }
    public class AccauntCommand : ICommandAction
    {

        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            PlayerInfo playerInfo = Global.Component.GetPlayerInfo();
            AccauntController accaunt = Global.Component.GetAccauntController();


            if (param.Length == 4) 
            {
                if (param[1] == "-login") 
                {
                    if (pcController.currentMemory.isInAccauntEntered) 
                    {
                        return new List<string>() { "already logged in" };
                    }

                    Debug.Log(playerInfo.accauntID == param[2]);
                    Debug.Log(playerInfo.accauntPass == param[3]);
                    if (playerInfo.accauntID == param[2] && playerInfo.accauntPass == param[3]) 
                    {
                        pcController.currentMemory.isInAccauntEntered = true;
                        return new List<string>() { "logged in successfully" };
                    }
                    else 
                    {
                        return new List<string>() { "incorrect accaunt id or password" };
                    }
                }
            }
            if (param.Length == 2) 
            {
                if (pcController.currentMemory.isInAccauntEntered)
                {
                    if (param[1] == "-b")
                    {
                        return new List<string>() { "accaunt balance [ " + accaunt.GetAccautBalance() + " ]" };
                    }
                    else if (param[1] == "-logout") 
                    {
                        pcController.currentMemory.isInAccauntEntered = false;

                        return new List<string>() { "logged out successfully" };
                    }
                }
                else 
                {
                    return new List<string>() { "not logged in", "use -login [accauntID] [pass]" };
                }
            }

            return null;
        }

        public string GetDescription()
        {
            return "bank account info";
        }

        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>()
            {
                { "-login [accauntID] [pass]", "login to bank accaunt" },
                { "-logout", "logout bank accaunt" },
                { "-b", "get accaunt balance" }
            };
        }
    }

    public class PeripheralCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            
            if (param.Length == 2) 
            {
                if (param[1] == "-l") 
                {
                    List<string> result = new List<string>();

                    foreach (var dev in pcController.peripherals)
                    {
                        result.Add(dev.GetComponent<IPeripheral>().DeviseDescription());
                    }

                    if (result.Count == 0) 
                    {
                        return new List<string> { "Devices not detected" };
                    }

                    return result;
                }
            }

            return null;
        }

        public string GetDescription()
        {
            return "show hardware, including peripherals";
        }

        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>
            {
                { "-l", "list of all mounted drives" }
            };
        }
    }
}