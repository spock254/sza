using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWFormCheckerController : EWBase, IEWInit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (IsPlayerInEWindowRadius() == false) 
        {
            Close();
        }
    }
    public void Init(GameObject window, GameObject envObj)
    {
        BaseInit(window, envObj);
    }
}
