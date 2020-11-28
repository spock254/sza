using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TS2CloseAndMoveBackNPC : MonoBehaviour
{
    //[SerializeField] List<Transform> points = null;
    //[SerializeField] Transform npcTransform = null;
    
    [SerializeField] DoorController doorController = null;
    [SerializeField] Transform playerTransform = null;
    public Diraction diraction;
    public bool isSelfDistroyable;

    bool closed = false;
    //int pointIndex = 0;

    void Update()
    {
        if (diraction == Diraction.Y ? playerTransform.position.y > transform.position.y 
                        : playerTransform.position.x > transform.position.x && !closed) 
        {
            doorController.OnDoorClick(null, doorController.transform.position,
                doorController.gameObject.GetComponent<Collider2D>(), false);
            
            if (isSelfDistroyable == true) 
            { 
                Destroy(this.gameObject);
            }

            closed = true;
            doorController.isLocked = false;
        }

        //if (closed) 
        //{
        //    if (pointIndex < points.Count) 
        //    { 
        //        npcTransform.position = Vector3.MoveTowards(npcTransform.po)
        //    }
        //}
    }
}
