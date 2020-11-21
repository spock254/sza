using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_waitTillPlayerReachPos : NPC_BaseData
{
    public enum Diraction { X, Y }
    public enum Sign { Negative, Positive }

   // [SerializeField] 
    public Transform point;
    
    [SerializeField]
    Diraction diraction = Diraction.X;

    [SerializeField]
    public Sign sign;

    public bool onPosition = false;

    public bool IsOnPosition(Vector2 position) 
    {
        if (diraction == Diraction.X)
        {
            return (sign == Sign.Positive) ? point.position.x < position.x : point.position.x > position.x;
        }
        else if (diraction == Diraction.Y) 
        { 
            return (sign == Sign.Positive) ? point.position.y < position.y : point.position.y < position.y;
        }

        return false;
    }
}
