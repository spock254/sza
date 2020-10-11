using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public ActionPanelController actionPanelController;
    //public Controller controller;
    //public Transform prefab;
    //public Transform player;
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
        //if (!controller.IsEmpty(controller.currentHand))
        //{
        //    if (controller.isBagOpen)
        //    {
        //        controller.CloseOpenContainer(controller.bag_panel, ref controller.isBagOpen);
        //    }

        //    Item item = controller.currentHand.GetComponent<ItemCell>().item;
        //    item.itemUseData.use.Use_To_Drop(prefab, player, item);

        //    float maxThrowDistance = ThrowDistance(item);

        //    Vector2 offset = uiContrall.mousePosRight - player.position;
        //    Vector2 throwPosition = new Vector2(player.position.x, player.position.y) +
        //                                Vector2.ClampMagnitude(offset, maxThrowDistance);

        //    SpawnItem(prefab, new Vector3(throwPosition.x, throwPosition.y, player.position.z), item);

        //}
        //else
        //{
        //    Debug.Log("nothing");
        //}
    }
}
