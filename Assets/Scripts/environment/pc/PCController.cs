using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum AcessLvL { USER }

public class PCController : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile open_tile;
    public Tile acess_enterTile;

    public List<Item> itemsToUnlock;
    bool isOpen;
    bool isLock;
    Vector3Int currentCell;
    public void OnPc_ClicK(Item itemInHand, Vector3 mousePosition) 
    {
        currentCell = tilemap.WorldToCell(mousePosition);
        if (isOpen == false) 
        { 
            Open();
            return;
        }

        if (itemsToUnlock.Contains(itemInHand) && !isLock && isOpen) 
        {
            tilemap.SetTile(currentCell, acess_enterTile);
            isLock = true;
        }
    }

    public void Open()
    {
        tilemap.SetTile(currentCell, open_tile);
        isOpen = true;
    }
}
