using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    ActionPanelController actionPanelController;
    public bool isCraftTable = true;
    void Start()
    {
        actionPanelController = Global.Component.GetActionPanelController();
    }

    public void OnTableClick(Vector2 mousePosition, Item item) 
    {
        if (item != null)
        {
            actionPanelController.SpawnItem(actionPanelController.prefab, transform.position, item);
        }
        else 
        {
            Debug.Log("nothing");
        }
    }
}
