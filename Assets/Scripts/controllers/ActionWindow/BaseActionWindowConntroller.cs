using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActionWindowConntroller : MonoBehaviour
{
    public GameObject windowPref;

    GameObject envWindow;
    GameObject window = null;

    public List<Item> savedItems;

    protected void Init()
    {
        envWindow = Global.UIElement.GetEnvWindow();
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
