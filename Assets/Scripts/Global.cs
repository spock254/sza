using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
