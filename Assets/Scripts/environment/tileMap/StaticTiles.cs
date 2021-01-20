using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class StaticTiles
{
    public Sprite back = null;
    public Sprite front = null;

    Tile backTile = null;
    Tile frontTile = null;

    public void Init() 
    {
        backTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
        backTile.sprite = back;

        backTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
        backTile.sprite = back;
    }

    public Tile GetBackTile()
    {
        return backTile;
    }

    public Tile GetFrontTile()
    {
        return frontTile;
    }
}
