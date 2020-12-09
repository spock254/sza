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

    //public Tile openDoorTile;
    //public Tile closeDoorTile;
    public List<Tile> doorTiles;

    Tilemap doorTilemap;
    public float doorSpeed = 0.1f;

    //public EventController eventController;

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
                if (itemInHand.IsSameItems(item))
                {
                    StartCoroutine(CloseOpenDoor(mousePosition, collider));
                    return;
                }
            }
        }
        else 
        {
            StartCoroutine(CloseOpenDoor(mousePosition, collider));
            //OpenCloseDoor(mousePosition, collider);
        }
    }

    //private void OpenCloseDoor(Vector3 mousePosition, Collider2D collider) 
    //{
    //    Vector3Int currentCell = doorTilemap.WorldToCell(mousePosition);
    //    doorTilemap.SetTile(currentCell, (!isOpen) ? openDoorTile : closeDoorTile);
    //    isOpen = !isOpen;
    //    collider.isTrigger = isOpen;
    //}

    IEnumerator CloseOpenDoor(Vector3 mousePosition, Collider2D collider) 
    {
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
    }
}
