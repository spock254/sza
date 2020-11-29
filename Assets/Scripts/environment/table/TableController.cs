using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    ActionPanelController actionPanelController;
    Controller controller;
    public bool isCraftTable = true;
    void Start()
    {
        actionPanelController = Global.Component.GetActionPanelController();
        controller = Global.Component.GetController();
    }

    public void OnTableClick(Vector2 mousePosition, Item item) 
    {
        if (item != null)
        {
            controller.CloseContainer();
            actionPanelController.SpawnItem(actionPanelController.prefab, transform.position, item);
        }
        else 
        {
            Debug.Log("nothing");
        }
    }
}
