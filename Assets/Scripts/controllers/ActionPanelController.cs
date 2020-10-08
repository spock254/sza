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

    public float maxThrowDistance_SmallItem = 3;
    public float maxThrowDistance_MidleItem = 2;
    public float maxThrowDistance_BigItem = 1;

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

            SpawnItem(prefab, player.position, item);

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

            float maxThrowDistance = ThrowDistance(item);

            Vector2 offset = uiContrall.mousePosRight - player.position;
            Vector2 throwPosition = new Vector2(player.position.x, player.position.y) + 
                                        Vector2.ClampMagnitude(offset, maxThrowDistance);

            SpawnItem(prefab, new Vector3(throwPosition.x, throwPosition.y, player.position.z), item);
            
        }
        else
        {
            Debug.Log("nothing");
        }
    }

    void SpawnItem(Transform prefab, Vector3 position, Item item) 
    {
        //Item prefabItem = prefab.GetComponent<ItemCell>().item;

        //prefabItem = item;
        //prefabItem.itemUseData = item.itemUseData;
        //prefabItem.itemUseData = new ItemUseData(item.itemUseData.itemSize, item.itemUseData.use, item.itemUseData.itemTypes);
        //prefabItem.ItemFightStats.Attack = item.ItemFightStats.Attack;
        //prefabItem.ItemFightStats.Defence = item.ItemFightStats.Defence;
        //prefabItem.itemUseData.use = item.itemUseData.use;
        //prefabItem.stats = item.stats;

        prefab.GetComponent<ItemCell>().item = item;
        prefab.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        prefab.name = Global.DROPED_ITEM_PREFIX + item.itemName;
        

        Instantiate(prefab, position, Quaternion.identity);

        uiContrall.SetDefaultItem(uiContrall.currentHand);
    }
    //void DropItem(Transform prefab, Transform player, Item item) 
    //{
    //    prefab.GetComponent<ItemCell>().item = item;
    //    prefab.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
    //    prefab.name = Global.DROPED_ITEM_PREFIX + item.itemName;

    //    Instantiate(prefab, player.position, Quaternion.identity);

    //    uiContrall.SetDefaultItem(uiContrall.currentHand);
    //}



    float ThrowDistance(Item item) 
    {
        if (item.itemUseData.itemSize == ItemUseData.ItemSize.Small)
        {
            return maxThrowDistance_SmallItem;
        }
        else if (item.itemUseData.itemSize == ItemUseData.ItemSize.Middle) 
        {
            return maxThrowDistance_MidleItem;
        }

        return maxThrowDistance_BigItem;
    }
}
