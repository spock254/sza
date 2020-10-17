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
    bool isOpen = false;

    //public Tile openDoorTile;
    //public Tile closeDoorTile;
    public List<Tile> doorTiles;

    Tilemap doorTilemap;
    public float doorSpeed = 0.1f;

    //public EventController eventController;

    void Start()
    {
        doorTilemap = Global.TileMaps.GetTileMap(Global.TileMaps.DOORS);
        //eventController.OnDoorEvent.AddListener(OnDoorClick);

        //itemsToUnlockDoor = new List<Item>();

        //Item card = new Item(new ItemFightStats(0, 0), "card", 200,
        //new ItemUseData(ItemUseData.ItemSize.Small, new DummyItemUse(),
        //            new ItemUseData.ItemType[] { ItemUseData.ItemType.Card,
        //                                                 ItemUseData.ItemType.Packet_left,
        //                                                 ItemUseData.ItemType.Packet_right}),
        //null, 2, null);
        //itemsToUnlockDoor.Add(card);
    }

    public void OnDoorClick(Item itemInHand, Vector3 mousePosition, Collider2D collider, bool isLocked) 
    {
        if (isLocked)
        {
            foreach (var item in itemsToUnlockDoor)
            {
                if (itemInHand.IsSameItems(item))
                {
                    Debug.Log(isLocked);
                    StartCoroutine(CloseOpenDoor(mousePosition, collider));
                    //OpenCloseDoor(mousePosition, collider);
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
        Vector3Int currentCell = doorTilemap.WorldToCell(mousePosition);

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
