using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelController : MonoBehaviour
{
    public Transform player;
    public Transform prefab;
    //Button currrentHand;
    Controller uiContrall;

    public float maxThrowDistance = 1;

    bool isThrown = false;

    GameObject goToThrow;
    void Start()
    {
        uiContrall = GameObject.FindGameObjectWithTag("ui")
                        .GetComponent<Controller>();
    }


    public void OnDropClick() 
    {
        if (!uiContrall.IsEmpty(uiContrall.currentHand))
        {
            //если открытака сумка, перед дропом закрыть
            if (uiContrall.isBagOpen) 
            {
                uiContrall.CloseOpenContainer(uiContrall.bag_panel, ref uiContrall.isBagOpen);
            }

            Item item = uiContrall.currentHand.GetComponent<ItemCell>().item;
            item.itemUseData.use.Use_To_Drop(prefab, player, item);

            DropItem(prefab, player, item);

        }
        else 
        {
            Debug.Log("nothing");
        }
    }

    public void OnThrowClick()
    {
        if (!uiContrall.IsEmpty(uiContrall.currentHand))
        {
            if (uiContrall.isBagOpen)
            {
                uiContrall.CloseOpenContainer(uiContrall.bag_panel, ref uiContrall.isBagOpen);
            }

            Item item = uiContrall.currentHand.GetComponent<ItemCell>().item;
            item.itemUseData.use.Use_To_Drop(prefab, player, item);

            Vector2 offset = uiContrall.mousePosRight - player.position;
            Vector2 throwPosition = new Vector2(player.position.x, player.position.y) + 
                                        Vector2.ClampMagnitude(offset, maxThrowDistance);

            ThrowItem(prefab, new Vector3(throwPosition.x, throwPosition.y, player.position.z), item);
            
        }
        else
        {
            Debug.Log("nothing");
        }
    }

    void ThrowItem(Transform prefab, Vector3 position, Item item) 
    {
        prefab.GetComponent<ItemCell>().item = item;
        prefab.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        prefab.name = Global.DROPED_ITEM_PREFIX + item.itemName;

        Instantiate(prefab, position, Quaternion.identity);

        uiContrall.SetDefaultItem(uiContrall.currentHand);
    }
    void DropItem(Transform prefab, Transform player, Item item) 
    {
        prefab.GetComponent<ItemCell>().item = item;
        prefab.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        prefab.name = Global.DROPED_ITEM_PREFIX + item.itemName;

        Instantiate(prefab, player.position, Quaternion.identity);

        uiContrall.SetDefaultItem(uiContrall.currentHand);
    }
}
