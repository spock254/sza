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

    public static class Timeflow 
    {
        public const int MAX_TICS = 60;
        public const int MAX_HOURS = 24;
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
        public const string DOORS = "doors";

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

        public static CameraFollow GetCameraFollow() 
        {
            return GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        }
    }

    public static class UIElement 
    {
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
    }
}
