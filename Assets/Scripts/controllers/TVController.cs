using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TVController : MonoBehaviour
{
    public Tile tvOff;
    public List<TvChanelData> chanels;
    DialogueManager dialogueManager;

    public float tvFrameTime = 0.1f;


    int currentChanelIndex = 0;

    public GameObject screenLight;
    Light2D tvLight;
    
    Tilemap tilemap;
    Vector3Int currentCell;
    bool isOpen = false;

    void Awake()
    {
        dialogueManager = Global.Component.GetDialogueManager();

        tilemap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        currentCell = tilemap.WorldToCell(transform.position);
        
        tilemap.SetTile(currentCell, tvOff);
        screenLight.SetActive(false);
        tvLight = screenLight.GetComponent<Light2D>();
        tvLight.color = chanels[0].ChanelFramesColor[0];
    }

    public void OnTvClick() 
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(Work(chanels));
        }
        else 
        {
            isOpen = false;
            StopCoroutine(Work(chanels));
            tilemap.SetTile(currentCell, tvOff);

        }

        screenLight.SetActive(isOpen);
        
    }

    public void NextChanel() 
    {
        if (isOpen) 
        {
            
            StopCoroutine(Work(chanels));

            currentChanelIndex++;
            if (currentChanelIndex >= chanels.Count) 
            {
                currentChanelIndex = 0;
            }

            if (chanels[currentChanelIndex].Dialog.Length > 0) 
            {
                dialogueManager.SetDialog(chanels[currentChanelIndex].Dialog);
                Global.Component.GetEventController().OnStartDialogEvent.Invoke("tv", string.Empty);
            }
            
            StartCoroutine(Work(chanels));
        }
    }

    public IEnumerator Work(List<TvChanelData> chanel) 
    {
        int index = 0;
        while (isOpen) 
        { 
            tilemap.SetTile(currentCell, chanel[currentChanelIndex].ChanelFrames[index]);
            tvLight.color = chanel[currentChanelIndex].ChanelFramesColor[index];
            yield return new WaitForSeconds(tvFrameTime);
            
            index++;
            if (index == chanel[currentChanelIndex].ChanelFrames.Count - 1) 
            {
                index = 0;
            }
        }
        
    }
}

[System.Serializable]
public class TvChanelData 
{
    [SerializeReference]
    List<Tile> chanelFrames;
    [SerializeReference]
    List<Color> chanelFramesColor;
    [SerializeField]
    bool isSkipable;
    [TextArea(1,25)]
    [SerializeField]
    string dialog;
    
    public TvChanelData(List<Tile> chanelFrames, List<Color> chanelFramesColor, bool isSkipable, string dialog)
    {
        this.chanelFrames = chanelFrames;
        this.chanelFramesColor = chanelFramesColor;
        this.isSkipable = isSkipable;
        this.dialog = dialog;
    }

    public List<Tile> ChanelFrames { get => chanelFrames; set => chanelFrames = value; }
    public List<Color> ChanelFramesColor { get => chanelFramesColor; set => chanelFramesColor = value; }
    public bool IsSkipable { get => isSkipable; set => isSkipable = value; }
    public string Dialog { get => dialog; set => dialog = value; }
}