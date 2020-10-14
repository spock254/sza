using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneralTest : MonoBehaviour
{
    //public InventoryInit inventory;
    //public ItemInit item;

    public FightStatsInit stats;
    public SkillsInit skillsInit;
    //public Tile tileA;
    //public Tilemap tilemap;
    //public Tilemap tilemap_upper;
    //public Vector3Int previous;
    //public Transform player;
    //public Tile opened_door;
    //// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            skillsInit.cooking.AddExp(250);

        }
    }
}
