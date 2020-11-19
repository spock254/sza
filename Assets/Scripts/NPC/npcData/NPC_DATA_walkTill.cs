using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_walkTill : NPC_BaseData
{
    public List<Transform> points;
    public float walkSpeed;

    [HideInInspector]
    public NPC_AnimationController animationController;

    int pointIndex = 0;

    private void Start()
    {
        animationController = GetComponent<NPC_AnimationController>();
    }

    public Transform GetNextPoint() 
    {
        Transform pointToReturn = null;

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
