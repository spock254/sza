﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class TurnOfLightWhenDoorOpen : MonoBehaviour
{
    Light2D light2D;
    public float fading = 0.05f;
    public bool isOriginalyInside = true;

    public List<Light2D> lightsInHouse;

    List<float> houseLightInvencity = new List<float>();
    private void Awake()
    {
        light2D = GetComponent<Light2D>();

        foreach (var ligh in lightsInHouse)
        {
            houseLightInvencity.Add(ligh.intensity);
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (isOriginalyInside == true)
        {
            StartCoroutine(FadeUp());
        }
        else 
        {
            StartCoroutine(FadeDown());
        }

    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (isOriginalyInside == true)
        {
            StartCoroutine(FadeDown());
        }
        else
        {
            StartCoroutine(FadeUp());
        }

    }

    IEnumerator FadeDown()
    {
        if (light2D != null) 
        { 
            for (float ft = 1f; ft >= 0; ft -= 0.1f)
            {
                light2D.intensity = ft;

                yield return new WaitForSeconds(fading);
            }

            light2D.intensity = 0;
        }

        for (int i = 0; i < lightsInHouse.Count; i++)
        {
            lightsInHouse[i].intensity = houseLightInvencity[i];
        }
    }
    IEnumerator FadeUp()
    {
        if (light2D != null) 
        { 
            for (float ft = 0f; ft <= 1; ft += 0.1f)
            {
                light2D.intensity = ft;
                yield return new WaitForSeconds(fading);
            }

            light2D.intensity = 1;
        }

        foreach (var ligh in lightsInHouse)
        {
            ligh.intensity = 0;
        }
    }
}
