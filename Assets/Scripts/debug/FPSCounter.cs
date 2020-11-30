using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;
    public float deltaTime;

    bool isRunning = false;

    void Start() {
        //Application.targetFrameRate = 60;

    }

    void Update() 
    {
        if (isRunning == true) 
        { 
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString();
        }
    }

    public void TurnFps(bool isRun) 
    {
        if (isRun == true) 
        { 
            isRunning = true; 
        }
        else 
        {
            isRunning = false;
            fpsText.text = string.Empty; 
        }
    }
}
