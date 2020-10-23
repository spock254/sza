using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperController : MonoBehaviour
{
    ActionWindowController actionWindow;

    public TMP_InputField input;
    public Button closeBtn;
    public Item item;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();    
    }

    public void OnClose() 
    {

        item.itemOptionData.text = input.text;

        actionWindow.CloseActionWindow(this.gameObject.tag);
    }

    public void Init(Item item) 
    {
        //this.item = Instantiate(item);

        this.item = item;
        input.text = item.itemOptionData.text;

    }
}
