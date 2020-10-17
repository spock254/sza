using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Global
{
    public static int MAX_LVL = 100;
    public const string DROPED_ITEM_PREFIX = "item_";

    public static class Item 
    {
        public static int BIG_SIZE = 5;
        public static int MIDDLE_SIZE = 3;
        public static int SMALL_SIZE = 1;
    }

    public static class Path
    {
        public const string FOOD_SPRITES_ROOT = "Images/Items/Food/";
        public const string EQUIPMENT_SPRITES_ROOT = "Images/Items/Equipment/";
        public const string RECEPT = "scriptableObjects/recept";
        public const string VESSELS = "scriptableObjects/item/Vessels";
    }

    public static class TileMaps
    {
        public const string BASE = "base";
        public const string BASE_2 = "base2";
        public const string BASE_3 = "base3";

        public const string UPPER = "upper";
        public const string UPPER_2 = "upper2";

        public const string WALLS = "walls";
        public const string DOORS = "doors";

        public static Tilemap GetTileMap(string tag) 
        {
            return GameObject.FindGameObjectWithTag(tag).GetComponent<Tilemap>();
        }
    }
}
