using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public ActionPanelController actionPanelController;

    public void OnTableClick(Vector2 mousePosition, Item item) 
    {
        if (item != null)
        {
            actionPanelController.SpawnItem(actionPanelController.prefab, mousePosition, item);
        }
        else 
        {
            Debug.Log("nothing");
        }
    }
}
