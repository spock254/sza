using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TS2ScreenQueue : MonoBehaviour
{
    public List<Transform> screenPositions;

    public List<Tile> screenFrames;

    Tilemap screenTilemap = null;

    void Start()
    {
        screenTilemap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);
    }
}
