using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum AcessLvL { USER }

public class PCController : MonoBehaviour
{
    Tilemap tilemap;
    public Tile open_tile;
    public Tile acess_enterTile;

    // pc inner data----------------------------------------------------------------
    public List<GameObject> peripherals;                                         
                                                                                 
    [SerializeField]
    DocData[] docDatas = null;
    public Dictionary<string, Item> docs = new Dictionary<string, Item>();
    
    public CommandDB.UserMode userMode = CommandDB.UserMode.Guest;
    
    public Dictionary<string, string> accounts = new Dictionary<string, string>() 
    { 
        { "spock43", "254u" } 
    };
    //-----------------------------------------------------------------------------
    public List<Item> itemsToUnlock;

    bool isOpen;
    bool isLock;
    Vector3Int currentCell;

    ActionWindowController actionWindow;
    TerminalController terminalController;

    private void Awake()
    {
        tilemap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);

        actionWindow = Global.Component.GetActionWindowController();
        terminalController = Global.Component.GetTerminalController();

        DocsInit();
    }

    public void OnPc_ClicK(Item itemInHand, Vector3 mousePosition) 
    {
        currentCell = tilemap.WorldToCell(mousePosition);
        
        if (isOpen == false)
        {
            Open();
        }
        else 
        {
            Close();
        }

        return;
        //if (itemsToUnlock.Contains(itemInHand) && !isLock && isOpen) 
        //{
        //    tilemap.SetTile(currentCell, acess_enterTile);
        //    isLock = true;
        //}
    }

    public void Open()
    {
        tilemap.SetTile(currentCell, open_tile);
        isOpen = true;

        actionWindow.OpenActionWindow("awpc");

        terminalController.isOpen = true;
        terminalController.SetCurrentPc(this);
    }

    public void Close() 
    {
        tilemap.SetTile(currentCell, null);
        isOpen = false;


        actionWindow.CloseActionWindow("awpc");

        terminalController.isOpen = false;
        terminalController.SetCurrentPc(null);
    }

    void DocsInit() 
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
