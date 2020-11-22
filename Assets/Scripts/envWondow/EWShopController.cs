using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EWShopController : EWBase, IEWInit
{
    public GameObject itemContentPref;
    public Transform itemList;

    List<Item> savedItems;

    public void Init(GameObject window, GameObject envObj)
    {
        BaseInit(window, envObj);

        savedItems = envObj.GetComponent<VendingController>().savedItems;

        FillItemList();
    }


    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInEWindowRadius() == false)
        {
            Close();
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //    RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        //    foreach (var hit in hits)
        //    {
        //        if (hit.collider.tag == "envObj")
        //        {
        //        }
        //    }
        //}
    }

    void FillItemList() 
    {
        for (int i = 0; i < savedItems.Count; i++)
        {
            // копирую и засовываю в контейнер
            GameObject itemContentCpy = Instantiate(itemContentPref, itemList);
            
            FillItemContent(itemContentCpy, savedItems[i], i);
            
            //itemContentCpy.gameObject.transform.SetParent(itemList.transform);
            //itemContentCpy.transform.SetAsLastSibling();
        }
    }

    void FillItemContent(GameObject itemContentCpy, Item item, int index) 
    {
        //GameObject itemContentCpy = Instantiate(itemContentPref);
        // image
        itemContentCpy.transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
        // item name
        itemContentCpy.transform.GetChild(1).GetComponent<Text>().text = item.itemName;
        // item price
        itemContentCpy.transform.GetChild(2).GetComponent<Text>().text = item.itemPrice.ToString();
        // item count
        itemContentCpy.transform.GetChild(3).GetComponent<Text>().text = "TODO";
        // button
        itemContentCpy.transform.GetChild(4).GetComponent<Button>()
            .onClick.AddListener(() => ClickOnChoose(index));
    }

    void ClickOnChoose(int index) 
    {
        Debug.Log(index);
    }
}
