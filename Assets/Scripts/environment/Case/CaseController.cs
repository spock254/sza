using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CaseController : MonoBehaviour
{
    Tilemap caseTilemap_door;
    Tilemap caseTilemap_body;

    public Tile caseBody;
    public Tile caseOpen;
    public Tile caseClosed;

    public bool isOpen = false;
    
    EventController eventController;
    CasePanelController casePanelController;
    // Start is called before the first frame update

    //private Vector3 mousePos;

    void Start()
    {
        caseTilemap_body = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        caseTilemap_door = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);

        eventController = Global.Component.GetEventController();
        casePanelController = Global.Component.GetCasePanelController();

        caseTilemap_body.SetTile(caseTilemap_body.WorldToCell(transform.position), caseBody);
        caseTilemap_door.SetTile(caseTilemap_door.WorldToCell(transform.position), caseClosed);
    }


    public void OnCaseCloseOpen(Vector3 mousePosition) 
    {
        Debug.Log(casePanelController.caseIsOpen);
        if (!casePanelController.caseIsOpen && isOpen) 
        {
            return;
        }

        Vector3Int currentCell = caseTilemap_door.WorldToCell(mousePosition);
        caseTilemap_door.SetTile(currentCell, (!isOpen) ? caseOpen : caseClosed);
        isOpen = !isOpen;
        //mousePos = mousePosition;
    }

}
