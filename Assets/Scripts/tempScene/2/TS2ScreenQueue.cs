using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TS2ScreenQueue : MonoBehaviour, IAction
{
    [SerializeField]
    float updateTime = 0;

    public List<Transform> screenPositions;

    public List<Tile> screenFrames;
    int frameIndex = 0;

    Tilemap screenTilemap = null;

    void Start()
    {
        screenTilemap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);

        SetNextScreenFrame();

        
    }
    public void Action()
    {
        StartCoroutine(UpdateScreen());
    }

    IEnumerator UpdateScreen() 
    {
        while (!IsLastFrame()) 
        {
            SetNextScreenFrame();

            yield return new WaitForSeconds(updateTime);

        }
    }

    bool SetNextScreenFrame() 
    {
        if (IsLastFrame()) 
        {
            return false;
        }

        for (int i = 0; i < screenPositions.Count; i++)
        {
            Vector3 tilePosition = screenPositions[i].position;
            screenTilemap.SetTile(screenTilemap.WorldToCell(tilePosition), screenFrames[frameIndex]);
        }

        frameIndex++;

        return true;
    }

    public bool IsLastFrame() 
    {
        return frameIndex > screenPositions.Count + 1;
    }

}
