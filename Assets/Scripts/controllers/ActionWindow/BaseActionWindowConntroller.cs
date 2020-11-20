using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActionWindowConntroller : MonoBehaviour
{
    public GameObject windowPref;

    GameObject envWindow;
    GameObject window = null;

    [HideInInspector]
    public List<Item> savedItems = new List<Item>();

    protected void Init()
    {
        envWindow = Global.UIElement.GetEnvWindow();
        //actionWindow = Global.Component.GetActionWindowController();
    }
    public void Open(GameObject envObj) 
    {
        // только 1 окно
        if (window != null) 
        {
            return;
        }

        window = Instantiate(windowPref);
        window.transform.SetParent(envWindow.transform, false);
        window.transform.SetAsFirstSibling();

        IEWInit init = window.GetComponent<IEWInit>();
        init.Init(window, envObj);
    }
}
