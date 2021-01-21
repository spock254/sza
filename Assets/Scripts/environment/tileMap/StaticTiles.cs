using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class StaticTiles
{
    public Sprite mainSprite = null;
    public Sprite secondarySprite = null;

    Tile main = null;
    Tile secondary = null;
    
    public void Init() 
    {
        if (mainSprite != null)
        {
            main = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            main.sprite = mainSprite;
        }

        if (secondarySprite != null)
        {
            secondary = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            secondary.sprite = secondarySprite;
        }
    }

    public Tile GetMainTile()
    {
        return main;
    }

    public Tile GetSecondaryTile()
    {
        return secondary;
    }
    
}
