using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class TurnOfLightWhenDoorOpen : MonoBehaviour
{
    Light2D light2D;
    public float fading = 0.05f;

    bool isOutside = false;
    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }
    int c = 0;
    void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(FadeUp());
        ////light2D.enabled = true;
        //if (light2D.intensity == 0)
        //{
        //    StartCoroutine(FadeUp());
        //    //isOutside = true;

        //}
        //else
        //{
        //    StartCoroutine(FadeDown());
        //}
    }
    void OnTriggerExit2D(Collider2D col)
    {
        StartCoroutine(FadeDown());
        ////light2D.enabled = false;
        ////StartCoroutine(FadeDown());
        //if (light2D.intensity == 0)
        //{
        //    StartCoroutine(FadeUp());

        //}
        //else
        //{
        //    StartCoroutine(FadeDown());
        //}
    }

    IEnumerator FadeDown()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            light2D.intensity = ft;
            yield return new WaitForSeconds(fading);
        }

        light2D.intensity = 0;
    }
    IEnumerator FadeUp()
    {
        for (float ft = 0f; ft <= 1; ft += 0.1f)
        {
            light2D.intensity = ft;
            yield return new WaitForSeconds(fading);
        }
        light2D.intensity = 1;
    }
}
