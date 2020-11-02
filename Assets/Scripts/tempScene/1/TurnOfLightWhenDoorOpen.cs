using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class TurnOfLightWhenDoorOpen : MonoBehaviour
{
    Light2D light2D;
    public float fading = 0.05f;
    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //light2D.enabled = true;
        StartCoroutine(FadeUp());
    }
    void OnTriggerExit2D(Collider2D col)
    {
        //light2D.enabled = false;
        StartCoroutine(FadeDown());
    }

    IEnumerator FadeDown()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            light2D.intensity = ft;
            yield return new WaitForSeconds(fading);
        }
    }
    IEnumerator FadeUp()
    {
        for (float ft = 0f; ft <= 1; ft += 0.1f)
        {
            light2D.intensity = ft;
            yield return new WaitForSeconds(fading);
        }
    }
}
