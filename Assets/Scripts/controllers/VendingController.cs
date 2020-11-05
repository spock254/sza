using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VendingController : BaseActionWindowConntroller
{
    [SerializeField]
    Tile venderBody;
    Tilemap upper;

    void Awake()
    {
        base.Awake();
        upper = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        upper.SetTile(upper.WorldToCell(transform.position), venderBody);
    }



}
