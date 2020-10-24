using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperReviewController : MonoBehaviour
{
    ActionWindowController actionWindow;

    public TextMeshProUGUI text;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();
    }

    public void Init(Item item) 
    {
        text.text = item.itemOptionData.text;
    }

    public void OnClose() 
    {
        actionWindow.CloseActionWindow(this.gameObject.tag);
    }
}
