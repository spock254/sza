using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using commands;
using UnityEngine.Experimental.Rendering.Universal;

public class CommandDB : MonoBehaviour
{
    public enum UserMode { Guest, User, Admin, Guide }

    //public UserMode userMode;

    public Dictionary<string, ICommandAction> guide = new Dictionary<string, ICommandAction>()
    {
        { "guide", new GuideCommand("guide") },
        { "light", new Guide_LightCommand("light") },
        { "help", new Guide_HelpCommand("help") },
        { "exit", new ExitCommand("exit") }
    };

    public Dictionary<string, ICommandAction> guest = new Dictionary<string, ICommandAction>() 
    {
        { "clear", new ClearCommand("clear") },
        { "guide", new GuideCommand("guide") },
        { "help", new HelpCommand("help") },
        { "exit", new ExitCommand("exit") },
        { "chuser", new ChUserCommand("chuser") },
        { "whoami", new WhoamiCommand("whoami") },
        { "file", new DocsCommand("file") }
    };

    public Dictionary<string, ICommandAction> user = new Dictionary<string, ICommandAction>()
    {
        { "printer", new PrinterCommand("printer") },
        { "disk", new DiskCommand("disk") },
        { "light", new Guide_LightCommand("light") },
        { "command_manager", new CommandManagerCommand("command_manager") },
        { "accaunt", new AccauntCommand("accaunt") },
        { "peripheral", new PeripheralCommand("peripheral") },
        { "driver", new DriverCommand("driver") }
    };

    public Dictionary<string, ICommandAction> admin = new Dictionary<string, ICommandAction>()
    {
        { "test3",  new CommonCommand( new List<string>() { "test3 terminal" } )}
    };

    public Dictionary<string, ICommandAction> installDev = new Dictionary<string, ICommandAction>()
    {
        { "bo4", new Bo4Command("bo4") }
    };

    public Dictionary<string, ICommandAction> GetCommands() 
    {
        TerminalController terminal = GetComponent<TerminalController>();

        UserMode userMode = terminal.GetCurrentPc().currentMemory.userMode;

        if (userMode == UserMode.Guide)
        {
            return guide;
        }
        else if (userMode == UserMode.Guest)
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
    Dictionary<string, string> GetFlagDescription();
    List<string> GetActionStatus(string[] param);
    string GetDescription();
    Dictionary<string, List<string>> GetParams();
    bool IsValidCommand(string command);
}

namespace commands
{
    public class ExitCommand : BaseCommand, ICommandAction
    {
        public ExitCommand(string tag) : base(tag) { }

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

        public Dictionary<string, string> GetFlagDescription()
        {
            return null;
        }

        public Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public bool IsValidCommand(string command)
        {
            //return command.Trim() == "exit";
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class CommonCommand  : ICommandAction
    {

        List<string> responce;

        public Dictionary<string, string> GetFlagDescription()
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

        public Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public bool IsValidCommand(string command)
        {
            return false;
        }
    }
    public class HelpCommand : BaseCommand, ICommandAction
    {
        public HelpCommand(string tag) : base(tag) { }
        public virtual Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>()
            {
                { "-all", "shoes description of all command flags" },
                { "-f", "shoes flags of all commands" },
                { "-s [command]", "description of the selected command" },
                { "-detail [command]", "description and flags of the selected command" }
            };
        }

        public virtual List<string> GetActionStatus(string[] param)
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

                        if (entry.Value.GetFlagDescription() != null)
                        {
                            List<string> flags = new List<string>(entry.Value.GetFlagDescription().Keys);
                            responce.Add(entry.Key + " ( " + string.Join(", ", flags) + " )");

                            foreach (KeyValuePair<string, string> flagData in entry.Value.GetFlagDescription())
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

                        if (entry.Value.GetFlagDescription() != null)
                        {
                            List<string> flags = new List<string>(entry.Value.GetFlagDescription().Keys);
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

                if (param[1] == "-detail")
                {
                    CommandDB commandDb = Global.Component.GetCommandDB();

                    Dictionary<string, ICommandAction> commands = commandDb.GetCommands();

                    if (commands.ContainsKey(param[2]))
                    {
                        List<string> resp = new List<string>() { param[2] + ": " + commands[param[2]].GetDescription() };

                        List<string> temp = new List<string>();

                        Dictionary<string, string> description = commands[param[2]].GetFlagDescription();

                        if (description != null) 
                        { 
                            foreach (var item in commands[param[2]].GetFlagDescription())
                            {
                                temp.Add(item.Key + " " + item.Value);
                            }
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

        public virtual string GetDescription()
        {
            return "description of commands";
        }

        public virtual Dictionary<string, List<string>> GetParams()
        {
            CommandDB commandDB = Global.UIElement.GetTerminalWindow().GetComponent<CommandDB>();
            List<string> detail = commandDB.GetCommands().Keys.ToList();

            return new Dictionary<string, List<string>>()
            {
                { "-detail", detail }
            };
        }

        public virtual bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class PrinterCommand : BaseCommand, ICommandAction
    {
        public PrinterCommand(string tag) : base(tag) { }

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

                if (param[1] == "-upload")
                {

                    if (param.Length == 2)
                    {
                        return new List<string>() { "Document not selected.", "use: printer -upload [ docname ]" };
                    }
                    else if (param.Length == 3)
                    {
                        if (isPrinterPresent(peripherals))
                        {
                            if (pcController.currentMemory.docs.ContainsKey(param[2]))
                            {
                                if (param[2].EndsWith(".txt") == false)
                                {
                                    return new List<string>() { "incorrect file format" ,"acceptable file formats (txt, pdf)" };
                                }

                                if (param[2].EndsWith(".pdf") == false)
                                {
                                    return new List<string>() { "incorrect file format", "acceptable file formats (txt, pdf)" };
                                }

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

                if (param[1] == "-run")
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
                            return new List<string>() { "document not uploaded", "use: printer -upload [docname]" };
                        }
                    }
                    else
                    {
                        return new List<string>() { "printer status ( disabled )" };
                    }
                }
            }

            return new List<string>() { "use with flags -status -upload", "for more information use help" };
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

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>
            {
                { "-status", "shows printer status" },
                { "-upload [docname]", "set up document for printing" },
                { "-run", "run the printer" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            TerminalController terminalController = Global.Component.GetTerminalController();
            PCController pcController = terminalController.GetCurrentPc();

            List<string> upload = new List<string>();

            foreach (var file in pcController.currentMemory.docs)
            {
                if (file.Key.EndsWith(".txt") || file.Key.EndsWith(".pdf")) 
                {
                    upload.Add(file.Key);
                }
            }

            return new Dictionary<string, List<string>>()
            {
                { "-upload", upload }
            };
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class ChUserCommand : BaseCommand, ICommandAction
    {
        public ChUserCommand(string tag) : base(tag) { }
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

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>()
            {
                { "-login [username] [password]" , "login as user" },
                { "-logout", "turn to guest mode" },
                { "-l", "list all registered users" }

            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            PCController pcController = Global.Component.GetTerminalController().GetCurrentPc();
            List<PCMempryContent> mempryContents = pcController.memoryContents;
            List<string> login = new List<string>();

            foreach (var memory in mempryContents)
            {
                login.Add(memory.userName);
            }

            return new Dictionary<string, List<string>>() 
            {
                { "-login", login }
            };
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class WhoamiCommand : BaseCommand, ICommandAction
    {
        public WhoamiCommand(string tag) : base(tag) { }
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

        public Dictionary<string, string> GetFlagDescription()
        {
            return null;
        }

        public Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class DocsCommand : BaseCommand, ICommandAction
    {
        public DocsCommand(string tag) : base(tag) { }
        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            Dictionary<string, Item> docs = pcController.currentMemory.docs;

            if (param.Length == 3) 
            {
                if (param[1] == "-copy") 
                {

                    if (pcController.disk == null) 
                    {
                        return new List<string>() { "disk not found" };
                    }

                    if (param[2] == "*") 
                    {
                        int copyFileCount = 0;
                        foreach (var file in pcController.disk.innerItems)
                        {
                            if (docs.Keys.Contains(file.itemName) == false) 
                            {
                                copyFileCount++;
                                docs.Add(file.itemName, file);
                            }
                        }

                        return new List<string>() { "copied " + copyFileCount + " files" };
                    }

                    foreach (var item in pcController.disk.innerItems)
                    {
                        if (param[2] == item.itemName)
                        {
                            if (pcController.currentMemory.docs.Keys.Contains(param[2]) == false)
                            {
                                pcController.currentMemory.docs.Add(param[2], item);
                                return new List<string>() { "file copied successfully" };
                            }
                            else
                            {
                                return new List<string>() { "file " + param[2] + " already exist" };
                            }
                        }
                    }

                    return new List<string>() { "file not found" };
                }
            }

            if (param.Length == 2)
            {
                if (param[1] == "-all")
                {
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
                else if (param[1] == "-all_txt")
                {
                    return GetAllFilesInFormat(".txt", docs);
                }
                else if (param[1] == "-all_pdf")
                {
                    return GetAllFilesInFormat(".pdf", docs);
                }
                else if (param[1] == "-all_cmd")
                {
                    return GetAllFilesInFormat(".cmd", docs);
                }
                else if (param[1] == "-all_exe") 
                { 
                    return GetAllFilesInFormat(".exe", docs);
                }
            }
            else if (param.Length == 3) 
            {
                if (param[1] == "-read") 
                {
                    if (param[2].EndsWith(".txt") == false)
                    {
                        return new List<string>() { "can't read not txt file format" };
                    }
                    
                    if (docs.Keys.Contains(param[2]))
                    {

                        List<string> fileContent = docs[param[2]].itemOptionData.text
                                                        .Split('\n').ToList();

                        terminal.AddContent(fileContent);

                        return new List<string>() { string.Empty };
                    }
                    else if (pcController.disk != null) 
                    {
                        foreach (var file in pcController.disk.innerItems)
                        {
                            if (param[2] == file.itemName) 
                            {
                                List<string> fileContent = file.itemOptionData.text
                                                                .Split('\n').ToList();

                                terminal.AddContent(fileContent);

                                return new List<string>() { string.Empty };
                            }
                        }
                    }
                    else
                    {
                        return new List<string>() { "File not found" };
                    }
                }
            }

            return null;
        }

        public string GetDescription()
        {
            return "file manager";
        }

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>() 
            {
                { "-read [file name]", "prints file content" },
                { "-all", "list all files in the system" },
                { "-copy [file name]", "copy file from disk" },
                { "-all_txt", "list all txt files in the system" },
                { "-all_pdf", "list pdf pdf files in the system" },
                { "-all_cmd", "list cmd txt files in the system" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            Dictionary<string, Item> docs = terminal.GetCurrentPc().currentMemory.docs;
            Item disk = terminal.GetCurrentPc().disk;
            List<string> read = new List<string>();
            List<string> copy = new List<string>();
            
            /* -copy */

            if (disk != null) 
            { 
                foreach (var file in disk.innerItems)
                {
                    copy.Add(file.itemName);
                }
            }

            /* -read */
            foreach (var doc in docs)
            {
                if (doc.Key.EndsWith(".txt")) 
                {
                    read.Add(doc.Key);
                }
            }

            if (disk != null) 
            { 
                foreach (var doc in disk.innerItems)
                {
                    if (read.Contains(doc.itemName) == false) 
                    {
                        read.Add(doc.itemName);
                    }
                }
            }

            return new Dictionary<string, List<string>>() 
            {
                { "-read", read },
                { "-copy", copy }
            };
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }

        List<string> GetAllFilesInFormat(string format, Dictionary<string, Item> docs) 
        {
            List<string> res = new List<string>();

            foreach (var item in docs)
            {
                if (item.Key.EndsWith(format))
                {
                    res.Add(item.Key);
                }
            }

            if (res.Count == 0)
            {
                return new List<string>() { "Documents not found" };
            }

            return res;
        }
    }
    public class DiskCommand : BaseCommand, ICommandAction
    {
        public DiskCommand(string tag) : base(tag) { }
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
                else if (param[1] == "-content")
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
                if (param[1] == "-copy")
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

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>()
            {
                { "-status", "shows disk status" },
                { "-out", "plug out disk" },
                { "-content", "shows disk content" },
                { "-copy [document name]", "copies selected document" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();

            List<string> cpy = new  List<string>();

            if (pcController.disk != null) 
            { 
                foreach (var item in pcController.disk.innerItems)
                {
                    cpy.Add(item.itemDescription);
                }
            }

            return new Dictionary<string, List<string>>()
            {
                { "-copy", cpy}
            };
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class AccauntCommand : BaseCommand, ICommandAction
    {
        public AccauntCommand(string tag) : base(tag) { }
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

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>()
            {
                { "-login [accauntID] [pass]", "login to bank accaunt" },
                { "-logout", "logout bank accaunt" },
                { "-b", "get accaunt balance" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class PeripheralCommand : BaseCommand, ICommandAction
    {
        public PeripheralCommand(string tag) : base(tag) { }
        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();

            if (param.Length == 2)
            {
                if (param[1] == "-all")
                {
                    List<string> result = new List<string>();

                    for (int i = 0; i < pcController.peripherals.Count; i++)
                    {
                        result.Add("port #" + "000" +(i + 1) + " : " + pcController.peripherals[i]
                            .GetComponent<IPeripheral>().DeviseDescription());
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

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>
            {
                { "-all", "list of all mounted drives" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class CommandManagerCommand : BaseCommand, ICommandAction
    {
        public CommandManagerCommand(string tag) : base(tag) { }
        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            CommandDB commandDB = Global.Component.GetCommandDB();

            if (param.Length == 3)
            {
                if (param[1] == "-install")
                {

                    Item disk = pcController.disk;

                    string key = param[2].Split('.')[0];

                    if (disk == null)
                    {
                        if (pcController.currentMemory.docs.Keys.Contains(param[2]) == true)
                        {
                            commandDB.user.Add(key, commandDB.installDev[key]);
                            return new List<string>() { "command " + key + " installed successfully" };
                        }
                    }
                    else
                    {
                        if (param[2].EndsWith(Global.Command.COMMAND_FORMAT))
                        {
                            foreach (var cmd in disk.innerItems)
                            {
                                if (cmd.itemDescription == param[2])
                                {
                                    if (commandDB.user.Keys.Contains(key) == false)
                                    {
                                        commandDB.user.Add(key, commandDB.installDev[key]);
                                        return new List<string>() { "command " + key + " installed successfully" };
                                    }
                                    else
                                    {
                                        return new List<string>() { "command " + key + " already installed" };
                                    }
                                }
                            }
                        }
                    }

                    return new List<string>() { "command to install not found" };
                }
            }
            else if (param.Length == 2) 
            {
                if (param[1] == "-all") 
                {
                    List<string> commandNames = new List<string>();

                    foreach (var command in commandDB.GetCommands())
                    {
                        commandNames.Add(command.Key);
                    }

                    return commandNames;
                }
            }
            return null;
        }

        public string GetDescription()
        {
            return "use for installing command into system";
        }

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>()
            {
                { "-install [command name]", "install command into system" },
                { "-all", "list all commands in the system" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            CommandDB commandDB = Global.UIElement.GetTerminalWindow().GetComponent<CommandDB>();

            List<string> install = new List<string>();

            if (pcController.disk != null) 
            { 
                foreach (var com in pcController.disk.innerItems)
                {
                    if (com.itemName.EndsWith(".cmd")
                        && commandDB.GetCommands().Keys.Contains(com.itemName) == false) 
                    {
                        install.Add(com.itemName);
                    }
                }
            }

            foreach (var fileName in pcController.currentMemory.docs.Keys)
            {
                if (fileName.EndsWith(".cmd")
                    && install.Contains(fileName) == false
                    && commandDB.GetCommands().Keys.Contains(fileName) == false) 
                {
                    install.Add(fileName);
                }
            }

            return new Dictionary<string, List<string>>()
            {
                { "-install", install }
            };
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }

    public class DriverCommand : BaseCommand, ICommandAction
    {
        public DriverCommand(string tag) : base(tag) { }

        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            Item disk = pcController.disk;

            if (param.Length == 3) 
            {
                if (param[1] == "-install") 
                {
                    bool deviceFound = false;
                    GameObject bo4Go = null;

                    if (disk == null) 
                    {
                        return new List<string>() { "driver disk missing" };
                    }

                    if (param[2].EndsWith(".exe") == false)
                    {
                        return new List<string>() { "driver not found" };
                    }

                    foreach (var driver in disk.innerItems)
                    {
                        if (driver.itemName.EndsWith(".exe") && param[2] == driver.itemName)
                        {
                            for (int i = 0; i < pcController.peripherals.Count; i++)
                            {
                                string description = pcController.peripherals[i].GetComponent<IPeripheral>().DeviseDescription();
                                string tagDevice = driver.itemOptionData.text;

                                if (description.Contains(tagDevice) == true)
                                {
                                    bo4Go = pcController.peripherals[i];
                                    deviceFound = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (deviceFound == true)
                    {
                        Item item = bo4Go.GetComponent<SubstitudeCell>().item;
                        item.itemSubstitution.initState = StateTypes.NPC_STATE_stateTransitionModify;

                        NPC_StateMashine mashine = bo4Go.GetComponent<NPC_StateMashine>();
                        mashine.ChangeState<NPC_STATE_stateTransitionModify>();

                        return new List<string>() { "driver installed successfully" };
                    }
                    else 
                    {
                        return new List<string>() { "device not found" };
                    }
                }
            }

            return null;
        }

        public string GetDescription()
        {
            return "install device driver";
        }

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>()
            {
                { "-install [driver name]", "install driver for device" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            Item disk = pcController.disk;
            List<string> install = new List<string>();

            if (disk != null) 
            { 
                foreach (var driver in disk.innerItems)
                {
                    if (driver.itemName.EndsWith(".exe")) 
                    {
                        install.Add(driver.itemName);
                    }
                }
            }
            
            return new Dictionary<string, List<string>>()
            {
                { "-install", install }
            };
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }

    #region installed command

    public class Bo4Command : DeviceUpgradeCommand 
    {
        public Bo4Command(string tag) : base(tag) { }
        public override List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();
            bool deviceFound = false;
            GameObject bo4Go = null;

            for (int i = 0; i < pcController.peripherals.Count; i++)
            {
                string description = pcController.peripherals[i].GetComponent<IPeripheral>().DeviseDescription();

                if (description.Contains("bo4") == true)
                {
                    bo4Go = pcController.peripherals[i];
                    deviceFound = true;
                    break;
                }
            }

            if (deviceFound == true)
            {
                Item item = bo4Go.GetComponent<SubstitudeCell>().item;
                item.itemSubstitution.initState = StateTypes.NPC_STATE_stateTransitionModify;

                NPC_StateMashine mashine = bo4Go.GetComponent<NPC_StateMashine>();
                mashine.ChangeState<NPC_STATE_stateTransitionModify>();

                return new List<string>() { "device bo4 upgraded" };

            }

            return new List<string>() { "device not found" };
        }

        public override string GetDescription()
        {
            return base.GetDescription();
        }

        public override Dictionary<string, string> GetFlagDescription()
        {
            return base.GetFlagDescription();
        }

        public override Dictionary<string, List<string>> GetParams()
        {
            return base.GetParams();
        }

        public override bool IsValidCommand(string command)
        {
            return base.IsValidCommand(command);
        }
    }
    
    #endregion

    public class DeviceUpgradeCommand : BaseCommand, ICommandAction
    {
        public DeviceUpgradeCommand(string tag) : base(tag) { }
        public virtual List<string> GetActionStatus(string[] param)
        {
            if (param.Length == 2) 
            {
                if (param[1] == "-install_driver") 
                { 
                    TerminalController terminal = Global.Component.GetTerminalController();
                    PCController pcController = terminal.GetCurrentPc();

                    Item item = pcController.peripherals[0].GetComponent<SubstitudeCell>().item;
                    item.itemSubstitution.initState = StateTypes.NPC_STATE_stateTransitionModify;

                    NPC_StateMashine mashine = pcController.peripherals[0].GetComponent<NPC_StateMashine>();
                    mashine.ChangeState<NPC_STATE_stateTransitionModify>();

                    return new List<string>() { "device  upgraded" };
                
                }
            }

            return null;
        }

        public virtual string GetDescription()
        {
            return "TODO";
        }

        public virtual Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>
            {
                { "-upgrade", "upgrade device" }
            };
        }

        public virtual Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public virtual bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class LightCommand : BaseCommand, ICommandAction
    {
        public LightCommand(string tag) : base(tag) { }

        Light2D light;

        public virtual List<string> GetActionStatus(string[] param)
        {
            light = GameObject.Find("Point Light 2D").GetComponent<Light2D>();

            if (param.Length == 3)
            {
                if (param[1] == "-on")
                {
                    light.enabled = true;
                    return new List<string>() { "light is on successfully" };
                }
                else if (param[1] == "-off")
                {
                    light.enabled = false;
                    return new List<string>() { "light is off successfully" };
                }
            }

            return null;
        }

        public virtual string GetDescription()
        {
            return "manipulating with light";
        }

        public virtual Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>
            {
                { "-on [room]", "turn light on" },
                { "-off [room]", "turn light off" },
                { "-info", "get list off all rooms" }
            };
        }

        public virtual Dictionary<string, List<string>> GetParams()
        {
            return new Dictionary<string, List<string>>() 
            {
                { "-on", new List<string>(){ "room", "kitchen", "bathroom" } },
                { "-off", new List<string>(){ "room", "kitchen", "bathroom" } }
            };
        }

        public virtual bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class ClearCommand : BaseCommand, ICommandAction
    {
        public ClearCommand(string tag) : base(tag) { }

        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();

            if (param.Length == 1) 
            {
                terminal.AddContent(new List<string>() 
                { 
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
                });

                return new List<string>() { string.Empty };
            }

            return null;
        }

        public string GetDescription()
        {
            return "clear content of terminal window";
        }

        public Dictionary<string, string> GetFlagDescription()
        {
            return null;
        }

        public Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }

    #region guide commands
    public class GuideStep
    {
        static int guide_step = 0;
        static bool IsSameStep(int step) 
        {
            return step == guide_step;
        }

        public static List<string> ProcessStep(int step) 
        {
            if (IsSameStep(step)) 
            {
                guide_step++;
                return new List<string>() { string.Empty };

            }

            return new List<string>() { "Plase complete previouse guide step",
                                        "or you can exit guide session: guide -off" };
        }
    }
    public class GuideCommand : BaseCommand, ICommandAction
    {
        public GuideCommand(string tag) : base(tag) { }

        TerminalController terminalController;
        public static CommandDB.UserMode prevUserMode = CommandDB.UserMode.Guide;

        public List<string> GetActionStatus(string[] param)
        {
            const int step = 0;
            terminalController = Global.Component.GetTerminalController();
            PCController pcController = terminalController.GetCurrentPc();
            List<PCMempryContent> mempryContents = pcController.memoryContents;


            if (param.Length == 2) 
            {
                if (param[1] == "-on")
                {
                    GuideStep.ProcessStep(step);

                    CommandDB.UserMode currentUserMode = terminalController.GetCurrentPc().currentMemory.userMode;
                    
                    if (currentUserMode != CommandDB.UserMode.Guest) 
                    {
                        prevUserMode = currentUserMode;
                    }
                    
                    PCMempryContent guestMemoryContent = mempryContents.Where(x => x.userMode == CommandDB.UserMode.Guide)
                                              .FirstOrDefault();

                    pcController.currentMemory = guestMemoryContent;

                    terminalController.AddContent(new List<string>
                    {
                        "Welcome to begining <color=#FFFFFF>SYSTEM_32s</color> terminal guide session",
                        "Terminal command contains from one till three parts",
                        "For example a command to manipulate the light in the room",
                        "",
                        "light -off room",
                        "^      ^   ^       ",
                        "|      |   argument",
                        "|      command flag",
                        "command",
                        "",
                        "Sometimes you can meet more then one argument",
                        "Use this command if you are too lazy ",
                        "to come up to light switcher"
                    });

                    return new List<string>() { string.Empty };
                }
                else if (param[1] == "-off") 
                {
                    if (terminalController.GetCurrentPc().currentMemory.userMode != CommandDB.UserMode.Guide) 
                    {
                        return new List<string>() { "You are not in guide session" };
                    }

                    PCMempryContent guestMemoryContent = mempryContents.Where(x => x.userMode == prevUserMode)
                          .FirstOrDefault();
                    pcController.currentMemory = guestMemoryContent;

                    return new List<string>() { "Guide session complete" };
                }
            }

            return null;
        }

        public string GetDescription()
        {
            return "terminal begining guide session";
        }

        public Dictionary<string, string> GetFlagDescription()
        {
            return new Dictionary<string, string>() 
            {
                { "-on", "start guide session" },
                { "-off", "finish guide session" }
            };
        }

        public Dictionary<string, List<string>> GetParams()
        {
            return null;
        }

        public bool IsValidCommand(string command)
        {
            CommandValidator validator = new CommandValidator(this, this.tag);
            return validator.Validate(command);
        }
    }
    public class Guide_LightCommand : LightCommand 
    {
        public Guide_LightCommand(string tag) : base(tag) { }

        TerminalController terminalController;

        public override List<string> GetActionStatus(string[] param)
        {

            List<string> toReturn = new List<string>();
            List<string> result = base.GetActionStatus(param);
            terminalController = Global.Component.GetTerminalController();

            if (result != null && result.Contains("light is off successfully"))
            {
                int step = 1;
                toReturn = GuideStep.ProcessStep(step);
                if (toReturn.Contains(string.Empty) == false) 
                {
                    return toReturn;
                }

                terminalController.AddContent(new List<string>
                {
                    "Also some command can be used with set of different flags",
                    "Turn on the light with -on flag"
                });
            }
            else if (result != null && result.Contains("light is on successfully")) 
            {
                int step = 2;
                toReturn = GuideStep.ProcessStep(step);
                if (toReturn.Contains(string.Empty) == false)
                {
                    return toReturn;
                }

                terminalController.AddContent(new List<string>
                {
                    "Well done!",
                    "One of the usefull command witch you will use often",
                    "help command",
                    "help command displays information about built-in commands",
                    "Try help command with out flags and arguments to see list ",
                    "of all accessible commands (accessible in guide session)"
                });
            }

            return toReturn;
        }

        public override string GetDescription()
        {
            return base.GetDescription();
        }

        public override Dictionary<string, string> GetFlagDescription()
        {
            return base.GetFlagDescription();
        }

        public override Dictionary<string, List<string>> GetParams()
        {
            return base.GetParams();
        }

        public override bool IsValidCommand(string command)
        {
            return base.IsValidCommand(command);
        }
    }
    public class Guide_HelpCommand : HelpCommand 
    {
        public Guide_HelpCommand(string tag) : base(tag) { }

        TerminalController terminalController;

        public override List<string> GetActionStatus(string[] param)
        {
            List<string> result = base.GetActionStatus(param);
            List<string> toReturn = new List<string>();
            terminalController = Global.Component.GetTerminalController();
            PCController pcController = terminalController.GetCurrentPc();
            List<PCMempryContent> mempryContents = pcController.memoryContents;

            if (result != null && result.Contains("light") && result.Contains("help"))
            {
                int step = 3;
                toReturn = GuideStep.ProcessStep(step);
                if (toReturn.Contains(string.Empty) == false)
                {
                    return toReturn;
                }

                terminalController.AddContent(new List<string>
                {
                    "help",
                    "guide",
                    "light",
                    "exit",
                    "",
                    "",
                    "To get more detail information about spacific command ",
                    "you can use -detail flag and command name as argumend",
                    "try to get all information about light command"

                });

                return new List<string>() { string.Empty };

            }
            else if (param[1] == "-detail" && param[2] == "light") 
            {
                int step = 4;
                toReturn = GuideStep.ProcessStep(step);
                if (toReturn.Contains(string.Empty) == false)
                {
                    return toReturn;
                }

                result.AddRange(new List<string>() { 
                    "", 
                    "",
                    "Congratulations!",
                    "Terminal guide session complete successfully!",
                    "Return to " + GuideCommand.prevUserMode + " mode"
                });


                PCMempryContent guestMemoryContent = mempryContents.Where(x => x.userMode == GuideCommand.prevUserMode)
                                                      .FirstOrDefault();
                pcController.currentMemory = guestMemoryContent;
            }


            return result;

        }

        public override Dictionary<string, List<string>> GetParams()
        {
            return base.GetParams();
        }

        public override bool IsValidCommand(string command)
        {
            return base.IsValidCommand(command);
        }
    }
    #endregion


    public class CommandValidator
    {
        ICommandAction command = null;
        string tag = string.Empty;
        public CommandValidator(ICommandAction command, string tag) 
        {
            this.command = command;
            this.tag = tag;
        }

        public bool Validate(string userInput)
        {
            string[] spletedCommand = userInput.Trim().Split();

            if (spletedCommand[0] == tag) 
            {
                Dictionary<string, string> flagsDescription = command.GetFlagDescription();
                Dictionary<string, List<string>> parameters = command.GetParams();
                List<string> flags = null;

                if (flagsDescription != null) 
                { 
                    flags = flagsDescription.Keys.ToList();
                }
                // соло команда типа exit
                if (parameters == null && spletedCommand.Length == 1)
                {
                    return true;
                }

                if (spletedCommand.Length == 2)
                {
                    foreach (var flag in flags)
                    {
                        string[] spletedFlag = flag.Split();

                        // если команда без параметров
                        if (spletedFlag[0] == spletedCommand[1] 
                            && spletedFlag.Length == 1)
                        {
                            return true;
                        }
                    }
                }
                else if (spletedCommand.Length == 3) 
                {
                    foreach (var flag in flags)
                    {
                        string[] spletedFlag = flag.Split();

                        if (spletedFlag[0] == spletedCommand[1]
                            && spletedFlag.Length > 1) 
                        {
                            if (parameters != null && parameters.Keys.Contains(spletedCommand[1])) 
                            { 
                                List<string> flagParam = parameters[spletedCommand[1]];

                                if (flagParam.Contains(spletedCommand[2]) == true) 
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }


                //if (spletedCommand.Length == 2)
                //{
                //    if (spletedCommand[1] == "-all")
                //    {
                //        return true;
                //    }
                //}
                //else if (spletedCommand.Length == 3)
                //{
                //    if (spletedCommand[1] == "-detail")
                //    {
                //        Dictionary<string, List<string>> parameters = GetParams();

                //        //return param.Keys.ContainsspletedCommand[2];
                //        List<string> param = parameters[spletedCommand[1]];
                //        return param.Contains(spletedCommand[2]);
                //    }
                //}
            }

            return false;
        }
    }
    public class BaseCommand 
    {
        protected string tag = string.Empty;

        public BaseCommand(string tag) 
        {
            this.tag = tag;
        }
    }
}