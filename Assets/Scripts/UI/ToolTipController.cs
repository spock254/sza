using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour
{
    static ToolTipController instance;

    [SerializeField]
    Camera uiCamera;

    [SerializeField]
    GameObject toolTip = null;

    [SerializeField]
    float textPaddinSize = 4f;

    Image bgImage;
    Text textItemName;
    Text textInteraction;
    RectTransform bgRectTransform;
    RectTransform UIRectTransform;

    bool isTooltipOpen = false;
    float preferredHeight = 0;

    Vector2 tooltipPosition = Vector2.zero;
    void Start()
    {
        bgImage = toolTip.transform.GetChild(0).GetComponent<Image>();
        textItemName = bgImage.transform.GetChild(0).GetComponent<Text>();
        textInteraction = bgImage.transform.GetChild(1).GetComponent<Text>();
        bgRectTransform = toolTip.GetComponent<RectTransform>();
        UIRectTransform = Global.UIElement.GetUI().GetComponent<RectTransform>();
        instance = this;

        preferredHeight = textItemName.preferredHeight;

        //ShowToolTip("qwe", "tretr");
        HideToolTip();
    }

    bool isdetected = false;
    void Update()
    {
        if (isTooltipOpen == true) 
        { 
            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(UIRectTransform,
                Input.mousePosition, uiCamera, out localPoint);
            

            localPoint.y = localPoint.y + preferredHeight;
            toolTip.transform.localPosition = localPoint;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length == 0) 
        {
            isdetected = false;
        }

        foreach (var hit in hits)
        {
            //Debug.Log(hit.collider.name);

            if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX))
            {
                Item item = hit.collider.GetComponent<ItemCell>().item;

                isdetected = true;
                tooltipPosition = hit.collider.transform.position;
                //tooltipPosition = uiCamera.WorldToViewportPoint(hit.collider.transform.position);
                ShowToolTip(item.itemName, "qwe");
                return;
            }
            else 
            {
                isdetected = false;
            }
        }

        if (isTooltipOpen == true && isdetected == false) 
        { 
            HideToolTip();
        }

    }

    void ShowToolTip(string itemName, string itemInteraction) 
    {
        toolTip.SetActive(true);
        isTooltipOpen = true;

        textItemName.text = itemName;
        textInteraction.text = itemInteraction;

        Vector2 bgSize = Vector2.zero;

        if (itemName.Length > itemInteraction.Length)
        {
            bgSize = new Vector2(textItemName.preferredWidth + textPaddinSize * 2,
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);

          //  textInteraction.alignment = TextAnchor.MiddleCenter;
        }
        else 
        {
            bgSize = new Vector2(textInteraction.preferredWidth + textPaddinSize * 2, 
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);

           // textItemName.alignment = TextAnchor.MiddleCenter;
        }

        bgRectTransform.sizeDelta = bgSize;
        bgSize.y = bgSize.y + preferredHeight / 2;
        bgImage.rectTransform.sizeDelta = bgSize;
    }

    void HideToolTip() 
    {
      //  textItemName.alignment = TextAnchor.MiddleLeft;
      //  textInteraction.alignment = TextAnchor.MiddleLeft;

        toolTip.SetActive(false);
        isTooltipOpen = false;
    }

    public static void Show(string itemName, string itemInteraction) 
    {
        instance.ShowToolTip(itemName, itemInteraction);
    }

    public static void Hide()
    {
        instance.HideToolTip();
    }
}
