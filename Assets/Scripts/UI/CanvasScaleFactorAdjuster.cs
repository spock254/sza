using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
 
public class CanvasScaleFactorAdjuster : MonoBehaviour
{
    public GameObject cam;
 
    void Start()
    {
        AdjustScalingFactor();
    }
 
    void LateUpdate()
    {
        AdjustScalingFactor();
    }
 
    void AdjustScalingFactor()
    {
        GetComponent<CanvasScaler>().scaleFactor = cam.GetComponent<PixelPerfectCamera>().pixelRatio;
    }
 
}