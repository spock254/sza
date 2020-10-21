using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCell : MonoBehaviour
{
    
    public Item item;
    
    void Start()
    {
        SpriteRenderer spriteRenderer;
        
        if (TryGetComponent<SpriteRenderer>(out spriteRenderer)) 
        { 
           spriteRenderer.sprite = item.itemSprite;
        }
    }
}
