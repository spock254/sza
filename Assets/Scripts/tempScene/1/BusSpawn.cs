using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BusSpawn : MonoBehaviour, ISpawn
{
    Tilemap base4;
    Tilemap upper;

    [SerializeField]
    Transform[] transforms;
    [SerializeField]
    Tile[] tiles;
    [SerializeField]
    Tile tileBlack;
    // Start is called before the first frame update
    void Init()
    {
        base4 = Global.TileMaps.GetTileMap(Global.TileMaps.BASE_4);
        upper = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
    }

    public void Spawn()
    {
        Init();

        for (int i = 0; i < transforms.Length; i++)
        {
            upper.SetTile(upper.WorldToCell(transforms[i].position), tiles[i]);
        }

        // door
        upper.SetTile(upper.WorldToCell(transforms[10].position), null);
        base4.SetTile(base4.WorldToCell(transforms[10].position), tiles[10]);

        for (int i = 0; i < 5; i++)
        {
            base4.SetTile(base4.WorldToCell(transforms[i].position), tileBlack);
        }
    }
}
