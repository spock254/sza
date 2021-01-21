using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileAnim
{
    public delegate void FinalAction(Object[] args);
    public float frameRate = 0;
    public float actionTime = 0;
    public List<Sprite> frames;
    CircularList<Tile> sl;

    Tilemap tilemap;
    MonoBehaviour mb;

    public void Init(MonoBehaviour mb, Tilemap tilemap)
    {
        this.mb = mb;
        this.tilemap = tilemap;

        sl = new CircularList<Tile>();

        foreach (Sprite sprite in frames)
        {
            Tile tile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            tile.sprite = sprite;
            sl.Add(tile);
        }
    }

    public void StartAnim(FinalAction finalAction, Object[] args)
    {
        mb.StartCoroutine(Anim(finalAction, args));
    }
    
    IEnumerator Anim(FinalAction finalAction, Object[] args)
    {
        if (sl.IsEmpty() == false)
        {
            for (float i = 0; i <= actionTime; i += frameRate)
            {
                tilemap.SetTile(tilemap.WorldToCell(mb.transform.position), sl.GetNext());
                yield return new WaitForSeconds(frameRate); 
            }

            sl.Reset();
        }
        else
        {
            yield return new WaitForSeconds(actionTime);
        }

        finalAction(args);
    }
}
