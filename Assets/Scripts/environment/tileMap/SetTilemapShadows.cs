﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class SetTilemapShadows : MonoBehaviour
{
    public EventController eventController;

    public GameObject shadow_caster_go;
    public static SetTilemapShadows Instance;

    private CompositeCollider2D tilemapCollider;
    private GameObject shadowCasterContainer;
    private List<GameObject> shadowCasters = new List<GameObject>(), toDelete = new List<GameObject>();
    private List<PolygonCollider2D> shadowPolygons = new List<PolygonCollider2D>();
    private List<ShadowCaster2D> shadowCasterComponents = new List<ShadowCaster2D>();

    public bool doReset = true, doCleanup = true;

    public void Start()
    {
        eventController.OnMouseClickEvent.AddListener(ResetAndCleanUp);

        Instance = this;
        tilemapCollider = GetComponent<CompositeCollider2D>();
        shadowCasterContainer = shadow_caster_go;
        //shadowCasterContainer = GetComponent<ShadowCaster2D>().gameObject;
        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathVertices);

            GameObject shadowCaster = new GameObject("shadow_caster_^" + i);
            shadowCasters.Add(shadowCaster);
            PolygonCollider2D shadowPolygon = (PolygonCollider2D)shadowCaster.AddComponent(typeof(PolygonCollider2D));
            shadowPolygons.Add(shadowPolygon);
            shadowCaster.transform.parent = shadowCasterContainer.transform;
            shadowPolygon.points = pathVertices;
            shadowPolygon.enabled = true;
            shadowPolygon.isTrigger = true;
            //if (shadowCaster.GetComponent<ShadowCaster2D>() != null) // remove existing caster?
            //    Destroy(shadowCaster.GetComponent<ShadowCaster2D>());
            ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
            shadowCasterComponents.Add(shadowCasterComponent);
            shadowCasterComponent.selfShadows = true;
            
        }
    }

    private void Reset()
    {
        toDelete = new List<GameObject>(shadowCasters);
        shadowCasters.Clear();
        shadowPolygons.Clear();
        shadowCasterComponents.Clear();

        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathVertices);
            GameObject shadowCaster = new GameObject("shadow_caster_^" + i);
            shadowCasters.Add(shadowCaster);
            PolygonCollider2D shadowPolygon = (PolygonCollider2D)shadowCaster.AddComponent(typeof(PolygonCollider2D));
            shadowPolygons.Add(shadowPolygon);
            shadowCaster.transform.parent = shadowCasterContainer.transform;
            shadowPolygon.points = pathVertices;
            shadowPolygon.enabled = true;
            shadowPolygon.isTrigger = true;
            //if (shadowCaster.GetComponent<ShadowCaster2D>() != null) // remove existing caster?
            //    Destroy(shadowCaster.GetComponent<ShadowCaster2D>());
            ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
            shadowCasterComponents.Add(shadowCasterComponent);
            shadowCasterComponent.selfShadows = true;
        }
        doCleanup = true;
    }

    private void LateUpdate()
    {
        if (doReset)
        {
            Reset();
            doReset = false;
        }
        if (doCleanup)
        {
            StartCoroutine(Cleanup());
            doCleanup = false;
        }
    }

    IEnumerator Cleanup()
    {
        yield return null;
        for (int i = 0; i < toDelete.Count; i++)
        {
            Destroy(toDelete[i]);
        }
        toDelete.Clear();
    }

    public void UpdateShadows()
    {
        doReset = true;
    }

    void ResetAndCleanUp() 
    {
        doReset = doCleanup = true;
    }
}