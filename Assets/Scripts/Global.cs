using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Global
{
    public const int MAX_LVL = 100;
    public const string DROPED_ITEM_PREFIX = "item_";
    public const string REPLACE_CHAR = "|";
    public const string UNPICKABLE_ITEM = "unpick_";
    public const string MONEY_SIGN = "$";
    public const string ITEM_SWITCH_PREFIX = "iswitch_";

    

    public static class Buff 
    {
        public const float CONSTANT_BUFF_TIME = -1;

        public static IBuff GetIBuffByType(BuffType buffType) 
        {
            Type type = Type.GetType(buffType.ToString());
            return (IBuff)Activator.CreateInstance(type);
        }

        public static class Player 
        {
            public const float SPEED = 1.2f;
        }
    }

    public static class Timeflow
    {
        public const int MAX_TICS = 60;
        public const int MAX_HOURS = 24;
    }

    public static class Command 
    { 
        public const string COMMAND_FORMAT = ".cmd";
    }

    public static class Tooltip
    {
        public const string LM_USE = "LM to use";
        public const string RM_USE = "RM to use";
        
        public const string LM_OPEN = "LM to open";
        public const string LM_CLOSE = "LM to close";
        
        public const string LM_PICK_UP = "LM to pick up";
        public const string LM_SWIPE = "LM to swipe";
        public const string RM_GIVE = "RM to give";
        public const string LM_PUT = "LM to put";
        public const string RM_CRAFT = "RM to craft";
        public const string NO_ACTIONS = "no actions";
        
        public const string LM_TURN_ON = "LM to turn on";
        public const string LM_TURN_OFF = "LM to turn off";
        public const string RM_TURN_OFF = "RM to turn off";
        public const string RM_TURN_ON = "RM to turn on";
        
        public const string RM_NEXT_CHANNEL = "RM next channel";
        public const string LM_CONNECT = "LM to connect";
        public const string LM_DISCONNECT = "LM to disconnect";
        public const string LM_INTERACT = "LM to interact";

        public const string RM_INSERT = "RM to insert";
        public const string RM_PULL_THE_DISK = "RM pull out";

        public static Vector3 EnvObjOffset() 
        {
            return new Vector3(0, 0.32f, 0);
        }
    }

    public static class Color
    {
        public const string RED_COLOR_PREFIX = "<color=#8B3837>";
        public const string GREEN_COLOR_PREFIX = "<color=#98B819>";
        public const string END_COLOR_PREFIX = "</color>";
    }

    public static class Item 
    {
        public const int BIG_SIZE = 5;
        public const int MIDDLE_SIZE = 3;
        public const int SMALL_SIZE = 1;
    }

    public static class Path
    {
        public const string FOOD_SPRITES_ROOT = "Images/Items/Food/";
        public const string EQUIPMENT_SPRITES_ROOT = "Images/Items/Equipment/";
        public const string RECEPT = "scriptableObjects/recept";
        public const string VESSELS = "scriptableObjects/item/Vessels";
        public const string QUESTS = "scriptableObjects/Quest";
        public const string QUESTS_DIALOGS = "scriptableObjects/Quest";
    }

    public static class TerminalResponce 
    {
        public static string COMMAND_NOT_FOUND = "Command not found.";
        public static string INCORRECT_ARG = "Incorrect argument.";
    }

    public static class TileMaps
    {
        public const string BASE = "base";
        public const string BASE_2 = "base2";
        public const string BASE_3 = "base3";
        public const string BASE_4 = "base4";

        public const string UPPER = "upper";
        public const string UPPER_2 = "upper2";

        public const string WALLS = "walls";
        public const string DOORS = "door";
        public const string DOORS_SIDE = "doorSide";

        public static Tilemap GetTileMap(string tag) 
        {
            return GameObject.FindGameObjectWithTag(tag).GetComponent<Tilemap>();
        }
    }

    public static class Component 
    {
        public static Controller GetController() 
        {
            return GameObject.FindGameObjectWithTag("ui").GetComponent<Controller>();
        }
        public static PlayerInfo GetPlayerInfo() 
        {
            return GameObject.FindGameObjectWithTag("player").GetComponent<PlayerInfo>();
        }
        public static EventController GetEventController() 
        {
            return GameObject.FindGameObjectWithTag("eventSystem").GetComponent<EventController>();
        }
        public static CasePanelController GetCasePanelController()
        {
            return GameObject.FindGameObjectWithTag("ui").GetComponent<CasePanelController>();
        }
        public static ActionPanelController GetActionPanelController() 
        { 
            return GameObject.FindGameObjectWithTag("ui").GetComponent<ActionPanelController>();
        }
        public static ActionWindowController GetActionWindowController() 
        {
            return GameObject.FindGameObjectWithTag("actionWindow").GetComponent<ActionWindowController>();
        }
        public static PaperController GetPaperController() 
        {
            return GameObject.FindGameObjectWithTag("awPaper").GetComponent<PaperController>();
        }
        public static PaperReviewController GetPaperReviewController() 
        { 
            return GameObject.FindGameObjectWithTag("awPaperHand").GetComponent<PaperReviewController>();
        }
        public static FormController GetFormController() 
        { 
            return GameObject.FindGameObjectWithTag("awssForm").GetComponent<FormController>();
        }
        public static FormReviewControlle GetFormReviewController() 
        { 
            return GameObject.FindGameObjectWithTag("awssFormHand").GetComponent<FormReviewControlle>();
        }
        public static ItemReviewController GetItemReviewController() 
        { 
            return GameObject.FindGameObjectWithTag("awitemHand").GetComponent<ItemReviewController>();
        }
        public static TerminalController GetTerminalController() 
        {
            return GameObject.FindGameObjectWithTag("awpc").GetComponent<TerminalController>();
        }
        public static CommandDB GetCommandDB()
        {
            return GameObject.FindGameObjectWithTag("awpc").GetComponent<CommandDB>();
        }
        public static PrefbDB GetPrefbDB() 
        { 
            return GameObject.FindGameObjectWithTag("itemManager").GetComponent<PrefbDB>();
        }
        public static DialogueManager GetDialogueManager() 
        {
            return GameObject.FindGameObjectWithTag("dialogWindow").GetComponent<DialogueManager>();
        }
        public static AccauntController GetAccauntController() 
        {
            return GameObject.FindGameObjectWithTag("accauntManager").GetComponent<AccauntController>();
        }
        public static QuestSystem GetQuestSystem() 
        {
            return GameObject.FindGameObjectWithTag("questSystem").GetComponent<QuestSystem>();
        }
        public static ProgressSceneLoader GetProgressSceneLoader() 
        {
            return GameObject.FindGameObjectWithTag("screenLoader").GetComponent<ProgressSceneLoader>();
        }
        public static BuffController GetBuffController() 
        {
            return GameObject.FindGameObjectWithTag("ui").GetComponent<BuffController>();
        }

        public static CollisionCounter GetCollisionCounter()
        {
            return GameObject.FindGameObjectWithTag("collisionCounter").GetComponent<CollisionCounter>();
        }
    }

    public static class UIElement 
    {
        public static GameObject GetBuffPanel() 
        {
            return GameObject.FindGameObjectWithTag("buffPanel");
        }
        public static GameObject GetDeBuffPanel()
        {
            return GameObject.FindGameObjectWithTag("debuffPanel");
        }

        public static GameObject GetUI()
        {
            return GameObject.FindGameObjectWithTag("ui");
        }

        public static GameObject GetDialogWindow() 
        {
            return GameObject.FindGameObjectWithTag("dialogWindow");
        }

        public static GameObject GetTerminalWindow() 
        {
            return GameObject.FindGameObjectWithTag("awpc");
        }

        public static GameObject GetStaticItemPanel() 
        {
            return GameObject.FindGameObjectWithTag("staticItemPanel");
        }

        public static GameObject GetEnvWindow() 
        { 
            return GameObject.FindGameObjectWithTag("envWindow");
        }

        public static GameObject GetScreenLoader() 
        {
            return GameObject.FindGameObjectWithTag("screenLoader");
        }
    }

    public static class Obj 
    {
        public static GameObject GetPlayerGameObject() 
        {
            return GameObject.FindGameObjectWithTag("player");
        }
        public static GameObject GetEffectListObject() 
        {
            return GetPlayerGameObject().transform.Find("effectList").gameObject;
        }
        public static GameObject GetVirtualCamera1() 
        { 
            return GameObject.FindGameObjectWithTag("vcam1");
        }
    }
}
