using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderOrder : MonoBehaviour
{
    [SerializeField]
    int newPlayerOrder = 0;
    int originPlayerOrder = 0;
    
    [SerializeField]
    string[] contactTriggers = null;

    [SerializeField]
    SpriteRenderer baseLayer = null;

    void Start()
    {
        originPlayerOrder = baseLayer.sortingOrder;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsContain(contactTriggers, collision.tag)) 
        {
            //baseLayer.sortingOrder += ADITIONAL_ORDER_IN_LAYER;        
            baseLayer.sortingOrder = newPlayerOrder;        
        }    
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (IsContain(contactTriggers, collision.tag))
        {
            baseLayer.sortingOrder = originPlayerOrder;
        }
    }

    bool IsContain(string[] arr, string tag) 
    {
        foreach (var item in arr)
        {
            if (item == tag) 
            {
                return true;
            }
        }

        return false;
    }
}
