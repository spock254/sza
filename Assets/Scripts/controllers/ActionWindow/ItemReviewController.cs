using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemReviewController : MonoBehaviour
{
    ActionWindowController actionWindow;

    GameObject instawItem;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();
    }

    public void Init(Item item)
    {
        instawItem = Instantiate(item.itemOptionData.awPrefab);
        instawItem.transform.SetParent(this.transform, false);
        instawItem.transform.SetAsFirstSibling();
        //foreach (Transform child in transform)
        //{
        //    if (child.name == "close")
        //    {
        //        Button closeBtn = child.gameObject.GetComponent<Button>();
        //        closeBtn.onClick.AddListener(OnClose);
        //        return;
        //    }
        //}
    }

    public void OnClose()
    {
        Destroy(instawItem);
        actionWindow.CloseActionWindow(this.gameObject.tag);
    }
}
