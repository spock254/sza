using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MicrowaveController : MonoBehaviour
{
    public enum MicrowaveStatus { CloseOpen, PutItem, TakeItem, None }

    public Tilemap tilemap;
    public Tile open_tile;
    public Tile close_tile;
    public Tile wark_tile;
    public float cookingTime;

    public bool isOpen = false;
    public bool isBlocked = false;
    //[HideInInspector]
    public Item itemInside;

    Vector3Int currentCell;
    public MicrowaveStatus OnMicrowaveClick(Item itemInHand, Vector3 mousePosition)
    {
        currentCell = tilemap.WorldToCell(mousePosition);

        if (isBlocked == false) 
        { 
            if (isOpen == false && itemInHand == null)
            {
                Open();

                Debug.Log("open");
                return MicrowaveStatus.CloseOpen;
            }
            else if (isOpen == true && itemInHand == null && itemInside == null)
            {
                Close();

                Debug.Log("close");
                return MicrowaveStatus.CloseOpen;
            }
            else if (isOpen == true && itemInHand != null && itemInside == null)
            {
                itemInside = itemInHand;
                return MicrowaveStatus.PutItem;
            }
            else if (isOpen == true && itemInHand == null && itemInside != null) 
            {
                return MicrowaveStatus.TakeItem;
            }
        }


        return MicrowaveStatus.None;
    }

    public void Close() 
    {
        tilemap.SetTile(currentCell, close_tile);
        isOpen = false;
    }    
    
    public void Open() 
    {
        tilemap.SetTile(currentCell, open_tile);
        isOpen = true;
    }
    
    public IEnumerator Work() 
    {
        isBlocked = true;

        tilemap.SetTile(currentCell, wark_tile);

        yield return new WaitForSeconds(cookingTime);

        tilemap.SetTile(currentCell, close_tile);

        isBlocked = false;
    }
}
