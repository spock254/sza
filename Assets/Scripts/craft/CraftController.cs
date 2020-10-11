using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour
{
    public void Craft(RaycastHit2D[] hits) 
    {
        foreach (var hit in hits)
        {
            if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX)) 
            {
                
                Debug.Log("+");
            }
        }

        Debug.Log("CRAFT!");
    }
}
