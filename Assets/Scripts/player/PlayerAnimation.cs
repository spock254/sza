using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using UnityScript.Lang;
using System.Linq;
using System;

public class PlayerAnimation : MonoBehaviour
{
    public enum SpritePart { Base, body, Hair, face, head, arm, lags, Eyes, bag, None }

    Animator anim;
    PlayerMovement movement;
    EventController eventController;

    SpriteRenderer rendererBase;
    SpriteRenderer rendererEyes;
    SpriteRenderer rendererHair;
    SpriteRenderer rendererSute;
    SpriteRenderer rendererHat;
    SpriteRenderer rendererShoes;


    public string baseSpriteSheet = "ch_base";
    public string eyesSpriteSheet = "eyes2";
    public string hairSpriteSheet = "hair";
    public string suteSpriteSheet = "sute";
    public string hatSpriteSheet = "";
    public string shoesSpriteSheet = "";

    [Header("Body Parts")]
    public GameObject eyesGo;
    public GameObject hairsGo;
    public GameObject suteGo;
    public GameObject hatGo;
    public GameObject shoesGo;

    Dictionary<SpritePart, Sprite[]> spritesDict = new Dictionary<SpritePart, Sprite[]>();
    Dictionary<SpritePart, GameObject> partsGo = new Dictionary<SpritePart, GameObject>();
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();

        eventController = Global.Component.GetEventController();
        eventController.OnChangeSpriteEvent.AddListener(OnSpriteChange);

        rendererBase = GetComponent<SpriteRenderer>();
        rendererEyes = eyesGo.GetComponent<SpriteRenderer>();
        rendererHair = hairsGo.GetComponent<SpriteRenderer>();
        rendererSute = suteGo.GetComponent<SpriteRenderer>();
        rendererHat = hatGo.GetComponent<SpriteRenderer>();
        rendererShoes = shoesGo.GetComponent<SpriteRenderer>();

        partsGo.Add(SpritePart.Base, this.gameObject);
        partsGo.Add(SpritePart.Eyes, eyesGo);
        partsGo.Add(SpritePart.head, hatGo);
        partsGo.Add(SpritePart.Hair, hairsGo);
        partsGo.Add(SpritePart.body, suteGo);
        partsGo.Add(SpritePart.lags, shoesGo);

        Sprite[] baseSprites = Resources.LoadAll<Sprite>("Images/player/base/" + baseSpriteSheet);
        Sprite[] eyeSprites = Resources.LoadAll<Sprite>("Images/player/eyes/" + eyesSpriteSheet);
        Sprite[] hairSprites = Resources.LoadAll<Sprite>("Images/player/hair/" + hairSpriteSheet);
        //Sprite[] suteSprites = Resources.LoadAll<Sprite>("Images/player/sute/" + suteSpriteSheet);
        //Sprite[] headSprites = Resources.LoadAll<Sprite>("Images/player/hat/" + hatSpriteSheet);

        spritesDict.Add(SpritePart.Base, baseSprites);
        spritesDict.Add(SpritePart.Eyes, eyeSprites);
        spritesDict.Add(SpritePart.Hair, hairSprites);
        spritesDict.Add(SpritePart.body, null);
        spritesDict.Add(SpritePart.head, null);
        spritesDict.Add(SpritePart.lags, null);


        //UpdateSprites();
    }

    public void OnSpriteChange(string spriteSheet, SpritePart spritePart) 
    {
        if (spriteSheet == string.Empty) 
        {
            partsGo[spritePart].GetComponent<SpriteRenderer>().sprite = null;
            spritesDict[spritePart] = null;
            return;
        }


        spritesDict[spritePart] = Resources.LoadAll<Sprite>("Images/player/"+ 
                                  spritePart.ToString().ToLower() +"/" + spriteSheet);

    }

    void Update()
    {
        if (movement.input.x > 0)
        {
            anim.Play("walk_right");
        }
        else if (movement.input.x < 0)
        {
            anim.Play("walk_left");
        }
        else if (movement.input.y > 0)
        {
            anim.Play("walk_up");
        }
        else if (movement.input.y < 0)
        {
            anim.Play("walk_down");
        }
        else 
        { 
            anim.Play("idle");
        }
    }

    void LateUpdate()
    {
        UpdateSprites();
    }

    void UpdateSprites() 
    {
        string spriteName = rendererBase.sprite.name;

        Sprite newBaseSprite = null;
        Sprite newEyesSprite = null;
        Sprite newHairSprite = null;
        Sprite newSuteSprite = null;
        Sprite newHatSprite = null;
        Sprite newShoesSprite = null;

        newBaseSprite = Array.Find(spritesDict[SpritePart.Base], i => i.name == spriteName);
        newEyesSprite = Array.Find(spritesDict[SpritePart.Eyes], i => i.name == spriteName);
        newHairSprite = Array.Find(spritesDict[SpritePart.Hair], i => i.name == spriteName);

        if (spritesDict[SpritePart.body] != null)
        {
            newSuteSprite = Array.Find(spritesDict[SpritePart.body], i => i.name == spriteName);

        }
        if (spritesDict[SpritePart.head] != null)
        {
            newHatSprite = Array.Find(spritesDict[SpritePart.head], i => i.name == spriteName);

        }
        if (spritesDict[SpritePart.lags] != null)
        {
            newShoesSprite = Array.Find(spritesDict[SpritePart.lags], i => i.name == spriteName);

        }

        if (newBaseSprite)
        {
            rendererBase.sprite = newBaseSprite;
        }

        if (newEyesSprite)
        {
            rendererEyes.sprite = newEyesSprite;
        }

        if (newHairSprite)
        {
            rendererHair.sprite = newHairSprite;
        }

        if (newSuteSprite)
        {
            rendererSute.sprite = newSuteSprite;
        }

        if (newHatSprite)
        {
            rendererHat.sprite = newHatSprite;
        }

        if (newShoesSprite)
        {
            rendererShoes.sprite = newShoesSprite;
        }

    }
}
