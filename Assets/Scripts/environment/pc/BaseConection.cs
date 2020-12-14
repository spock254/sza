using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConection : MonoBehaviour
{
    PCController connectedPc = null;

    [SerializeField]
    float connectionRadius = 1f;

    Vector3[] diractions;

    void Awake() 
    {
        diractions = new Vector3[] { new Vector3(0, connectionRadius, 0), 
                                     new Vector3(0, -connectionRadius, 0),
                                     new Vector3(connectionRadius, 0, 0),
                                     new Vector3(-connectionRadius, 0, 0)
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            PCController pcController = FindPcInRadius();
            Debug.Log(pcController == null ? "Null" : "NOT NULL");
        }    
    }

    public PCController FindPcInRadius() 
    {
        foreach (var dir in diractions)
        {
            Vector3 rcPos = transform.position + dir;
            Vector2 rcPos2D = new Vector2(rcPos.x, rcPos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(rcPos2D, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.tag == "pc") 
                {
                    return hit.collider.GetComponent<PCController>();
                }
            }
        }

        return null;
    }

    void OnDrawGizmosSelected()
    {
        if (diractions != null) 
        { 
            foreach (var dir in diractions)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + dir);
            }
        }
    }

    public float GetConnectionRadius() 
    {
        return connectionRadius;
    }
}
