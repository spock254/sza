using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminallScralling : MonoBehaviour
{
    [SerializeField]
    RectTransform commandLineContainerRect = null;
    [SerializeField]
    float yPos = 2.6f;
    public void ValueChange(Vector2 pos) 
    {
        Debug.Log(commandLineContainerRect.position);
        if (pos.y < 0.5f) 
        {

            commandLineContainerRect.position = new Vector3(commandLineContainerRect.position.x,
                                                            yPos, 
                                                            commandLineContainerRect.position.z);

        }
    }
}
