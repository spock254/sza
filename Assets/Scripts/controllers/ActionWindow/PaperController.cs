using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperController : MonoBehaviour
{
    ActionWindowController actionWindow;

    public TMP_InputField input;

    GameObject goOnTable;
    Item item;
    Item resultItem;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();    
    }

    public void OnClose() 
    {
        //if (resultItem) 
        //{
        //    resultItem.itemOptionData.text = input.text;
        //}

        //if (input.text.Length == 0) 
        //{
        //    resultItem = item;
        //}
        //item.itemOptionData.text = input.text;

        if (input.text.Length == 0)
        {
            goOnTable.GetComponent<ItemCell>().item = item;
            goOnTable.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        }
        else 
        {
            goOnTable.GetComponent<ItemCell>().item = resultItem;
            goOnTable.GetComponent<SpriteRenderer>().sprite = resultItem.itemSprite;
        }

        goOnTable.GetComponent<ItemCell>().item.itemOptionData.text = input.text;
        
        actionWindow.CloseActionWindow(this.gameObject.tag);
    }

    public void Init(GameObject goOnTable, Item item, Item resultItem) 
    {
        //this.item = Instantiate(item);

        this.goOnTable = goOnTable;
        this.item = item;
        this.resultItem = resultItem;

        input.text = item.itemOptionData.text;

    }
}
