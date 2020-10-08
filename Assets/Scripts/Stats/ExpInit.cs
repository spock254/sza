using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpInit : MonoBehaviour
{
    [HideInInspector]
    public Exp exp;
    void Awake()
    {
        exp = new Exp(0, 0);
    }

}
