using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TS2LaggadgeTable : MonoBehaviour
{
    public GameObject actionGo;
    public float itemMoveSpeed = 0;

    public Transform[] points;
    int pointIndex = 0;
    
    GameObject dropedItem;
    IAction action = null;
    Vector2 targetPosition;

    void Start()
    {
        action = actionGo.GetComponent<IAction>();
        targetPosition = NextPoint().position;
    }

    
    void Update()
    {
        if (action.IsInAction()) 
        {

            if (dropedItem == null) 
            { 
                Vector2 mousePos2D = new Vector2(points[0].position.x, points[0].position.y);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

                foreach (var hit in hits)
                {
                    if (hit.collider.name.Contains(Global.UNPICKABLE_ITEM)) 
                    {
                        dropedItem = hit.collider.gameObject;
                        return;
                    }
                }
            }
            else 
            {

                float step = itemMoveSpeed * Time.deltaTime;
                dropedItem.transform.position = Vector2.MoveTowards(dropedItem.transform.position,
                                targetPosition, step);

                if (Vector2.Distance(targetPosition, dropedItem.transform.position) < 0.001f) 
                {
                    if (IsLastPoint() == false)
                    {
                        targetPosition = NextPoint().position;

                    }
                    else 
                    {
                        Destroy(dropedItem.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }

    Transform NextPoint() 
    {
        Transform pointToReturn = null;

        if (!IsLastPoint())
        {
            pointToReturn = points[pointIndex];
            pointIndex++;
        }

        return pointToReturn;
    }

    bool IsLastPoint() 
    {
        return pointIndex == points.Length;
    }
}
