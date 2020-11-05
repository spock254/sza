using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EWVendingController : MonoBehaviour, IEWInit
{
    Button closeBtn;

    public void Init() 
    {
        //Transform tr = this.gameObject.transform.Find("close");
        //closeBtn = tr.GetComponent<Button>();
        //closeBtn = GameObject.Find("close").GetComponent<Button>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "close")
            {
                //child.gameObject.GetComponent<Button>().clicked += Close;
            }
        }
        //closeBtn.clicked += Close;
    }
    public void Close() 
    {
        // разблочить движение
        Destroy(this);
    }
}
