using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActionWindowConntroller : MonoBehaviour
{
    //ActionWindowController actionWindow;
    public GameObject windowPref;

    GameObject envWindow;
    GameObject window = null;
    protected void Awake()
    {
        envWindow = Global.UIElement.GetEnvWindow();
        //actionWindow = Global.Component.GetActionWindowController();
    }
    public void Open() 
    {
        window = Instantiate(windowPref);
        window.transform.SetParent(envWindow.transform, false);
        window.transform.SetAsFirstSibling();
        IEWInit init = window.GetComponent<IEWInit>();
        init.Init();
    }

    //public void Close() 
    //{
    //     Destroy(windowPref);
    //     //actionWindow.CloseActionWindow(this.gameObject.tag);
        
    //}
}
