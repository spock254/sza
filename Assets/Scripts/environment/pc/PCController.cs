using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum AcessLvL { USER }

public class PCController : MonoBehaviour
{
    Tilemap tilemap;
    public Tile open_tile;
    public Tile acess_enterTile;
    public Light2D light2D;

    // pc inner data----------------------------------------------------------------
    public List<GameObject> peripherals;
    public List<PCMempryContent> memoryContents;
    public PCMempryContent currentMemory;
    //-----------------------------------------------------------------------------
    public List<Item> itemsToUnlock;
    public Item disk;

    bool isOpen;
    bool isLock;
    Vector3Int currentCell;

    ActionWindowController actionWindow;
    TerminalController terminalController;
    EventController eventController;

    private void Awake()
    {
        tilemap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);

        actionWindow = Global.Component.GetActionWindowController();
        terminalController = Global.Component.GetTerminalController();
        eventController = Global.Component.GetEventController();
                                                                                 // USER MODE FOR TESTING
        currentMemory = memoryContents.Where(i => i.userMode == CommandDB.UserMode.User).FirstOrDefault();

        InitAllDocs();
    }

    public void OnPc_ClicK(Item itemInHand, Vector3 mousePosition) 
    {
        currentCell = tilemap.WorldToCell(mousePosition);
        
        if (isOpen == false)
        {
            Open();

            eventController.OnTerminalOpen.Invoke();
        }
        else 
        {
            Close();

            eventController.OnTerminalClose.Invoke();
        }
    }

    public void OnPc_Disck(Button btnHand) 
    {
        Item itemInHand = null;
        Controller controller = Global.Component.GetController();

        if (!controller.IsEmpty(btnHand)) 
        { 
            itemInHand = btnHand.GetComponent<ItemCell>().item;
        }

        if (controller.IsEmpty(btnHand) && disk == null) 
        {
            return;
        }

        if (controller.IsEmpty(btnHand) && disk != null) 
        {
            controller.DressCell(btnHand, disk);
            disk = null;
        }
        else if (itemInHand.itemName.Contains("disk") && !disk)
        {
            Global.Component.GetController().SetDefaultItem(btnHand);
            disk = itemInHand;
        }
    }
     
    public void Open()
    {
        tilemap.SetTile(currentCell, open_tile);
        isOpen = true;

        actionWindow.OpenActionWindow("awpc");

        terminalController.isOpen = true;
        terminalController.SetCurrentPc(this);
        light2D.enabled = true;
    }

    public void Close() 
    {
        tilemap.SetTile(currentCell, null);
        isOpen = false;


        actionWindow.CloseActionWindow("awpc");

        terminalController.isOpen = false;
        terminalController.SetCurrentPc(null);
        light2D.enabled = false;
    }

    void InitAllDocs() 
    {
        foreach (var item in memoryContents)
        {
            item.DocsInit();
        }
    }
}

[System.Serializable]
public class PCMempryContent 
{
    public string userName;
    public string password;
    public bool isInAccauntEntered = false;

    public CommandDB.UserMode userMode = CommandDB.UserMode.Guest;
    [SerializeField]
    DocData[] docDatas = null;
    public Dictionary<string, Item> docs = new Dictionary<string, Item>();

    public void DocsInit()
    {
        foreach (var item in docDatas)
        {
            docs.Add(item.docName, item.item);
        }
    }
}
[System.Serializable]
public struct DocData
{
    public string docName;
    public Item item;
}
