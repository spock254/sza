using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWBase : MonoBehaviour
{
    protected GameObject window = null;

    protected Controller controller = null;

    protected float actioPlayerRadius = 0;
    protected Vector2 ewindowPosition;
    protected Transform player = null;

    protected void BaseInit(GameObject window, GameObject envObj) 
    {
        controller = Global.Component.GetController();
        player = controller.player;

        this.window = window;
        ewindowPosition = envObj.transform.position;
        actioPlayerRadius = controller.GetActioPlayerRadius();
    }

    protected bool IsPlayerInEWindowRadius() 
    {
        return Vector2.Distance(player.position, ewindowPosition) < actioPlayerRadius;
    }

    public void Close()
    {
        Destroy(this.window);
    }
}
