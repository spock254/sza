using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SwitchLight : MonoBehaviour
{
    public Light2D globalLight;

    Color outsideLightColor = Color.white;
    Color insideLightColor = Color.white;

    public float fading = 0.3f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player") 
        { 
            StartCoroutine(FadeUp());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            StartCoroutine(FadeDown());
        }
    }

    void Awake()
    {
        outsideLightColor = globalLight.color;    
    }

    IEnumerator FadeDown()
    {
        for (float ft = 1f; ft >= 0.6f; ft -= 0.1f)
        {
            globalLight.intensity = ft;
            yield return new WaitForSeconds(fading);
        }

        globalLight.intensity = 0.6f;
        globalLight.color = outsideLightColor;
    }
    IEnumerator FadeUp()
    {
        for (float ft = 0.6f; ft <= 1f; ft += 0.1f)
        {
            globalLight.intensity = ft;
            yield return new WaitForSeconds(fading);
        }

        globalLight.intensity = 1f;
        globalLight.color = insideLightColor;
    }
}
