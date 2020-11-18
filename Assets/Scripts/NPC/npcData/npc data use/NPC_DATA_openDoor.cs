using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_openDoor : NPC_BaseData
{
    [SerializeField]
    GameObject doorGo;
    [SerializeField]
    public bool open;
    public void OpenDoor() 
    {
        DoorController doorController = doorGo.GetComponent<DoorController>();
        Collider2D collider2D = doorGo.GetComponent<Collider2D>();

        doorController.OnDoorClick(null, doorGo.transform.position, collider2D, false);
    }
}
