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

    Dictionary<string, ICommandAction> guide = new Dictionary<string, ICommandAction>()
    {
        { "guide", new GuideCommand() },
        { "light", new Guide_LightCommand() },
        { "help", new Guide_HelpCommand() },
        { "exit", new ExitCommand() }
    };

    Dictionary<string, ICommandAction> guest = new Dictionary<string, ICommandAction>() 
    {
        { "guide", new GuideCommand() },
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
        { "per", new PeripheralCommand() },
        { "upgrade", new DeviceUpgradeCommand() }
    };

    Dictionary<string, ICommandAction> admin = new Dictionary<string, ICommandAction>()
    {
        { "test3",  new CommonCommand( new List<string>() { "test3 terminal" } )}
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
        public virtual Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>()
            {
                { "-all", "shoes description of all command flags" },
                { "-f", "shoes flags of all commands" },
                { "-s [command]", "description of the selected command" },
                { "-sf [command]", "description and flags of the selected command" }
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

                if (param[1] == "-detail")
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

        public virtual string GetDescription()
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

                    for (int i = 0; i < pcController.peripherals.Count; i++)
                    {
                        result.Add(i + 1 + ") " + pcController.peripherals[i]
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

        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>
            {
                { "-l", "list of all mounted drives" }
            };
        }
    }
    public class DeviceUpgradeCommand : ICommandAction
    {
        public List<string> GetActionStatus(string[] param)
        {
            TerminalController terminal = Global.Component.GetTerminalController();
            PCController pcController = terminal.GetCurrentPc();

            Item item = pcController.peripherals[0].GetComponent<SubstitudeCell>().item;
            item.itemSubstitution.initState = StateTypes.NPC_STATE_stateTransitionModify;

            NPC_StateMashine mashine = pcController.peripherals[0].GetComponent<NPC_StateMashine>();
            mashine.ChangeState<NPC_STATE_stateTransitionModify>();

            return new List<string>() { "TODO" };
        }

        public string GetDescription()
        {
            return "TODO";
        }

        public Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>
            {
                { "-update [device]", "list of all mounted drives" }
            };
        }
    }


    public class LightCommand : ICommandAction
    {
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

        public virtual Dictionary<string, string> GetParams()
        {
            return new Dictionary<string, string>
            {
                { "-on [room]", "turn light on" },
                { "-off [room]", "turn light off" },
                { "-info", "get list off all rooms" }
            };
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
    public class GuideCommand : ICommandAction
    {
        TerminalController terminalController;
        static CommandDB.UserMode prevUserMode = CommandDB.UserMode.Guide;

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
                    Debug.Log(prevUserMode);
                    pcController.currentMemory = guestMemoryContent;

                    return new List<string>() { "Guide session complete" };
                }
            }

            return null;
        }

        public string GetDescription()
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, string> GetParams()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Guide_LightCommand : LightCommand 
    {
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

        public override Dictionary<string, string> GetParams()
        {
            return base.GetParams();
        }
    }

    public class Guide_HelpCommand : HelpCommand 
    {
        TerminalController terminalController;

        public override List<string> GetActionStatus(string[] param)
        {
            List<string> result = base.GetActionStatus(param);
            List<string> toReturn = new List<string>();
            terminalController = Global.Component.GetTerminalController();

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
                    "try get all information about light command"

                });

            }
            else if (result != null && result.Contains("-info")) 
            {
                int step = 4;
                toReturn = GuideStep.ProcessStep(step);
                if (toReturn.Contains(string.Empty) == false)
                {
                    return toReturn;
                }
            }

            toReturn.AddRange(new List<string>() { "", "Congratulations terminal guide session complete successfully!" });
            return toReturn;

        }
    }

    #endregion
}