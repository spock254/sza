using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour
{
    [SerializeField]
    Camera uiCamera;

    [SerializeField]
    GameObject toolTip = null;

    [SerializeField]
    float textPaddinSize = 4f;

    Text textItemName;
    Text textInteraction;
    RectTransform bgRectTransform;

    void Start()
    {
        textItemName = toolTip.transform.GetChild(0).GetComponent<Text>();
        textInteraction = toolTip.transform.GetChild(1).GetComponent<Text>();
        bgRectTransform = toolTip.GetComponent<RectTransform>();

        ShowToolTip("qwe", "tretr");
        
    }

    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bgRectTransform,
            Input.mousePosition, uiCamera, out localPoint);

        toolTip.transform.localPosition = localPoint;
    }

    void ShowToolTip(string itemName, string itemInteraction) 
    {
        toolTip.SetActive(true);

        textItemName.text = itemName;
        textInteraction.text = itemInteraction;

        Vector2 bgSize = Vector2.zero;

        if (itemName.Length > itemInteraction.Length)
        {
            bgSize = new Vector2(textItemName.preferredWidth + textPaddinSize * 2,
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);
        }
        else 
        {
            bgSize = new Vector2(textInteraction.preferredWidth + textPaddinSize * 2, 
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);
        }

        bgRectTransform.sizeDelta = bgSize;
    }

    void HideToolTip() 
    { 
        toolTip.SetActive(false);
    }
}
