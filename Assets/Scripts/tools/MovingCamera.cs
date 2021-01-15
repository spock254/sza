using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
public class MovingCamera : MonoBehaviour
{

    // Should be the same value as defined on 
    // Pixel Perfect Camera script
    public float pixelPerUnit = 32.0f;
    public float cameraSpeed;
    public GameObject cameraObject;
    public GameObject objectToFollow;

    private float multiple;

    void Start()
    {
        // Calculates the minimum size of a screen pixel
        multiple = 1.0f / pixelPerUnit;
    }

    // This function rounds to a multiple of pixel 
    // screen value based on pixel per unit
    private float RoundToMultiple(float value, float multipleOf)
    {
        // Using Mathf.Round at each frame is a performance killer
        return (int)((value / multipleOf) + 0.5f) * multipleOf;
    }

    private void FixedUpdate()
    {
        float t = RoundToMultiple(cameraSpeed * Time.deltaTime, multiple);
        cameraObject.transform.position = Vector3.Lerp(new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y, -10), new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y,-10), t);
    }

}