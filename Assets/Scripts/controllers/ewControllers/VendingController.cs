﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode]
public class VendingController : BaseActionWindowConntroller
{
    [SerializeField]
    Tile venderBody = null;

    Tilemap upper = null;

    public string headerTitle;

    void Awake()
    {
        base.Init();
        upper = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        upper.SetTile(upper.WorldToCell(transform.position), venderBody);
    }
}
