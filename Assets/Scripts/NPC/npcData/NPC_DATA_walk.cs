using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_walk : NPC_BaseData
{
    [Header("this.gameObject.name + \" walkpoints\"")]
    public bool findPoints;
    public List<Transform> points;
    public float walkSpeed;

    [HideInInspector]
    public NPC_AnimationController animationController;

    int pointIndex = 0;

    private void Start()
    {
        animationController = GetComponent<NPC_AnimationController>();

        if (findPoints == true && points.Count == 0) 
        {
            GameObject pointsInWorld = GameObject.Find(this.gameObject.name + " walkpoints");
            foreach (Transform point in pointsInWorld.transform)
            {
                points.Add(point);
            }
        }
    }

    public Transform GetNextPoint()
    {
        Transform pointToReturn = null;

        if (IsLastPoint()) 
        {
            pointIndex = 0;
        }

        if (points.Count > pointIndex)
        {
            pointToReturn = points[pointIndex];
            pointIndex++;
        }

        return pointToReturn;
    }

    public bool IsLastPoint()
    {
        return pointIndex == points.Count;
    }
}
