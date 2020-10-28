using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TVController : MonoBehaviour
{
    public Tile tvOff;

    public List<Tile> errorChanel;
    public List<Color> errorColors;

    public float tvFrameTime = 0.1f;

    public GameObject screenLight;
    Light2D tvLight;
    
    Tilemap tilemap;
    Vector3Int currentCell;
    bool isOpen = false;

    void Awake()
    {
        tilemap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        currentCell = tilemap.WorldToCell(transform.position);
        
        tilemap.SetTile(currentCell, tvOff);
        screenLight.SetActive(false);
        tvLight = screenLight.GetComponent<Light2D>();
        tvLight.color = errorColors[0];
    }

    public void OnTvClick() 
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(Work(errorChanel));
        }
        else 
        {
            isOpen = false;
            StopCoroutine(Work(errorChanel));
            tilemap.SetTile(currentCell, tvOff);

        }

        screenLight.SetActive(isOpen);
        
    }

    public IEnumerator Work(List<Tile> chanel) 
    {
        int index = 0;
        while (isOpen) 
        { 
            tilemap.SetTile(currentCell, chanel[index]);
            tvLight.color = errorColors[index];
            yield return new WaitForSeconds(tvFrameTime);
            
            index++;
            if (index == chanel.Count - 1) 
            {
                index = 0;
            }
        }
        
    }
}
