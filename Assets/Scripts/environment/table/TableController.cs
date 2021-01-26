using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    ActionPanelController actionPanelController;
    Controller controller;
    public bool isCraftTable = true;
    public string tableName = string.Empty;

    Vector3 dropPosition = Vector3.zero;

    void Start()
    {
        actionPanelController = Global.Component.GetActionPanelController();
        controller = Global.Component.GetController();
        
        dropPosition = new Vector3(transform.position.x + Global.TileMaps.TILE_OFFSET, 
                                    transform.position.y + Global.TileMaps.TILE_OFFSET, 
                                    transform.position.z);
    }

    public void OnTableClick(Vector2 mousePosition, Item item) 
    {
        if (item != null)
        {
            controller.CloseContainer();
            actionPanelController.SpawnItem(actionPanelController.prefab, dropPosition, item);
        }
        else 
        {
            Debug.Log("nothing");
        }
    }
}
