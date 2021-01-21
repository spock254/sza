using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode]
public class CaseController : MonoBehaviour
{
    public string caseName;

    Tilemap caseTileMap;

    //public Tile caseBody;
    //public Tile caseOpen;
    //public Tile caseClosed;
    [SerializeField]
    StaticTiles baseTiles;

    public bool isOpen = false;
    
    EventController eventController;
    CasePanelController casePanelController;
    // Start is called before the first frame update

    //private Vector3 mousePos;

    void Start()
    {
        caseTileMap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        //caseTilemap_door = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);

        eventController = Global.Component.GetEventController();
        casePanelController = Global.Component.GetCasePanelController();

        baseTiles.Init();

        caseTileMap.SetTile(caseTileMap.WorldToCell(transform.position), baseTiles.GetMainTile());
        //caseTilemap_door.SetTile(caseTilemap_door.WorldToCell(transform.position), caseClosed);
    }


    public void OnCaseCloseOpen(Vector3 mousePosition) 
    {
        if (!casePanelController.caseIsOpen && isOpen) 
        {
            return;
        }

        Vector3Int currentCell = caseTileMap.WorldToCell(mousePosition);
        caseTileMap.SetTile(currentCell, (isOpen == false) ? baseTiles.GetSecondaryTile() : baseTiles.GetMainTile());
        isOpen = !isOpen;
        //mousePos = mousePosition;
    }

}
