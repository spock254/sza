using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using UnityScript.Lang;
using System.Linq;
using System;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement movement;

    SpriteRenderer rendererBase;
    SpriteRenderer rendererEyes;
    SpriteRenderer rendererHair;
    SpriteRenderer rendererSute;
    SpriteRenderer rendererHat;


    public string baseSpriteSheet = "ch_base";
    public string eyesSpriteSheet = "eyes2";
    public string hairSpriteSheet = "hair";
    public string suteSpriteSheet = "sute";
    public string hatSpriteSheet = "hat";

    [Header("Body Parts")]
    public GameObject eyesGo;
    public GameObject hairsGo;
    public GameObject suteGo;
    public GameObject hatGo;


    Sprite[] baseSprites;
    Sprite[] eyeSprites;
    Sprite[] hairSprites;
    Sprite[] suteSprites;
    Sprite[] hatSprites;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();

        rendererBase = GetComponent<SpriteRenderer>();
        rendererEyes = eyesGo.GetComponent<SpriteRenderer>();
        rendererHair = hairsGo.GetComponent<SpriteRenderer>();
        rendererSute = suteGo.GetComponent<SpriteRenderer>();
        rendererHat = hatGo.GetComponent<SpriteRenderer>();
        

        baseSprites = Resources.LoadAll<Sprite>("Images/player/base/" + baseSpriteSheet);
        eyeSprites = Resources.LoadAll<Sprite>("Images/player/eyes/" + eyesSpriteSheet);
        hairSprites = Resources.LoadAll<Sprite>("Images/player/hair/" + hairSpriteSheet);
        suteSprites = Resources.LoadAll<Sprite>("Images/player/sute/" + suteSpriteSheet);
        hatSprites = Resources.LoadAll<Sprite>("Images/player/hat/" + hatSpriteSheet);

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
        string spriteName = rendererBase.sprite.name;

        var newBaseSprite = Array.Find(baseSprites, i => i.name == spriteName);
        var newEyesSprite = Array.Find(eyeSprites, i => i.name == spriteName);
        var newHairSprite = Array.Find(hairSprites, i => i.name == spriteName);
        var newSuteSprite = Array.Find(suteSprites, i => i.name == spriteName);
        var newHatSprite = Array.Find(hatSprites, i => i.name == spriteName);

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
        
    }
}
