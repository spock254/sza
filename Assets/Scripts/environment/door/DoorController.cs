using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class DoorController : MonoBehaviour
{
    public List<Item> itemsToUnlockDoor;
    
    public bool isLocked;
    public bool isOpen = false;
    public bool isFrontDoor = true;

    public List<Tile> doorTiles;

    Tilemap doorTilemap;
    public float doorSpeed = 0.1f;

    bool doorInAction = false;
    void Start()
    {
        doorTilemap = (isFrontDoor == true) ? Global.TileMaps.GetTileMap(Global.TileMaps.DOORS) :
                                              Global.TileMaps.GetTileMap(Global.TileMaps.DOORS_SIDE);
        doorTilemap.SetTile(doorTilemap.WorldToCell(transform.position), doorTiles[0]);
    }

    public void OnDoorClick(Item itemInHand, Vector3 mousePosition, Collider2D collider, bool isLocked) 
    {
        if (isLocked)
        {
            foreach (var item in itemsToUnlockDoor)
            {
                if (itemInHand.IsSameItems(item) && doorInAction == false)
                {
                    StartCoroutine(CloseOpenDoor(mousePosition, collider));
                    return;
                }
            }
        }
        else 
        {
            if (doorInAction == false) 
            { 
                StartCoroutine(CloseOpenDoor(mousePosition, collider));
            }
        }
    }


    IEnumerator CloseOpenDoor(Vector3 mousePosition, Collider2D collider) 
    {
        doorInAction = true;

        Vector3Int currentCell = doorTilemap.WorldToCell(transform.position);

        if (isOpen)
        {
            for (int i = doorTiles.Count - 1; i >= 0; i--)
            {
                doorTilemap.SetTile(currentCell, doorTiles[i]);

                yield return new WaitForSeconds(doorSpeed);
            }
        }
        else 
        { 
            for (int i = 0; i < doorTiles.Count; i++)
            {
                doorTilemap.SetTile(currentCell, doorTiles[i]);

                yield return new WaitForSeconds(doorSpeed);
            }
        }


        isOpen = !isOpen;
        collider.isTrigger = isOpen;

        doorInAction = false;
    }
}
