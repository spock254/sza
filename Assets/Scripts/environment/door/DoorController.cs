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

    public Tile openDoorTile;
    public Tile closeDoorTile;

    public Tilemap doorTilemap;

    public EventController eventController;

    void Start()
    {
        eventController.OnDoorEvent.AddListener(OnDoorClick);

        itemsToUnlockDoor = new List<Item>();

        Item card = new Item(new ItemFightStats(0, 0), "card", 200,
        new ItemUseData(ItemUseData.ItemSize.Small, new DummyItemUse(),
                    new ItemUseData.ItemType[] { ItemUseData.ItemType.Card,
                                                         ItemUseData.ItemType.Packet_left,
                                                         ItemUseData.ItemType.Packet_right}),
        null, 2, null);
        itemsToUnlockDoor.Add(card);
    }

    public void OnDoorClick(Item itemInHand, Vector3 mousePosition, Collider2D collider, bool isLocked) 
    {
        if (isLocked)
        {
            foreach (var item in itemsToUnlockDoor)
            {
                if (itemInHand.IsSameItems(item))
                {
                    OpenCloseDoor(mousePosition, collider);
                    return;
                }
            }
        }
        else 
        {
            OpenCloseDoor(mousePosition, collider);
        }
    }

    private void OpenCloseDoor(Vector3 mousePosition, Collider2D collider) 
    {
        Vector3Int currentCell = doorTilemap.WorldToCell(mousePosition);
        doorTilemap.SetTile(currentCell, (!isOpen) ? openDoorTile : closeDoorTile);
        isOpen = !isOpen;
        collider.isTrigger = isOpen;
    }
}
