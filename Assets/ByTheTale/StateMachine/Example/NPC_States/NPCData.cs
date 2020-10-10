using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public GameObject NPC;

    [Header("Walk data")]
    public List<Transform> walkingPints;
    public float walkSpeed = 1;

    [Header("Idle data")]
    public float timeInIdle = 5;
}
