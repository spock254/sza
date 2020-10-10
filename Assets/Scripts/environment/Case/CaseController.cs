using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CaseController : MonoBehaviour
{
    public Tilemap caseTilemap;
    public Tile caseOpen;
    public Tile caseClosed;

    public bool isOpen = false;
    public EventController eventController;
    public CasePanelController casePanelController;
    // Start is called before the first frame update

    //private Vector3 mousePos;

    void Start()
    {
        //eventController.OnCaseEvent.AddListener(OnCaseCloseOpen);
    }


    public void OnCaseCloseOpen(Vector3 mousePosition) 
    {
        Debug.Log(casePanelController.caseIsOpen);
        if (!casePanelController.caseIsOpen && isOpen) 
        {
            return;
        }

        Vector3Int currentCell = caseTilemap.WorldToCell(mousePosition);
        caseTilemap.SetTile(currentCell, (!isOpen) ? caseOpen : caseClosed);
        isOpen = !isOpen;
        //mousePos = mousePosition;
    }

}
