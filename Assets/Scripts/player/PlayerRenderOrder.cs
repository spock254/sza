using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderOrder : MonoBehaviour
{
    const int ORDER_IN_LAYER_VALUE = 10;
    
    [SerializeField]
    SpriteRenderer baseLayer = null;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NPC") 
        {
            baseLayer.sortingOrder += ORDER_IN_LAYER_VALUE;        
        }    
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            baseLayer.sortingOrder -= ORDER_IN_LAYER_VALUE;
        }
    }
}
