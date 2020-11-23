using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWBase : MonoBehaviour
{
    public enum TextColor { Green, Red, Default };

    const string RED_COLOR_PREFIX = "<color=#8B3837>";
    const string GREEN_COLOR_PREFIX = "<color=#98B819>";
    const string END_COLOR_PREFIX = "</color>";
    
    protected GameObject window = null;
    protected Controller controller = null;

    protected float actioPlayerRadius = 0;
    protected Vector2 vendorPosition;
    protected Transform player = null;

    CameraFollow cameraFollow;
    protected void BaseInit(GameObject window, GameObject envObj) 
    {
        controller = Global.Component.GetController();
        player = controller.player;

        this.window = window;
        vendorPosition = envObj.transform.position;
        actioPlayerRadius = controller.GetActioPlayerRadius();

        cameraFollow = Global.Component.GetCameraFollow();
        cameraFollow.SetCameraTarger(envObj.gameObject);
        cameraFollow.ZoomCamera(false, 0.01f, 4.5f);
    }

    protected bool IsPlayerInEWindowRadius() 
    {
        return Vector2.Distance(player.position, vendorPosition) < actioPlayerRadius;
    }

    public void Close()
    {
        cameraFollow.SetCameraTarger(player.gameObject);
        cameraFollow.ZoomCamera(true, 0.01f);
        Destroy(this.window);
    }

    protected string SetTextColor(string text, TextColor textColor) 
    {
        if (textColor == TextColor.Green)
        {
            return GREEN_COLOR_PREFIX + text + END_COLOR_PREFIX;
        }
        else if (textColor == TextColor.Red) 
        {
            return RED_COLOR_PREFIX + text + END_COLOR_PREFIX;
        }

        return text;
    }
}
