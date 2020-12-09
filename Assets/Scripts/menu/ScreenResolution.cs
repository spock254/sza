using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    [SerializeField]
    int width = 1920; // or something else
    [SerializeField]
    int height = 1080; // or something else
    [SerializeField]
    bool isFullScreen = false; // should be windowed to run in arbitrary resolution
    [SerializeField]
    int desiredFPS = 60; // or something else

    void Start()
    {
        Screen.SetResolution(width, height, isFullScreen, desiredFPS);
    }

}
