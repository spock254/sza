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

    public string baseSpriteSheet = "ch_base";
    public string eyesSpriteSheet = "eyes";

    [Header("Body Parts")]
    public GameObject eyesGo;

    Sprite[] baseSprites;
    Sprite[] eyeSprites;
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();

        rendererBase = GetComponent<SpriteRenderer>();
        rendererEyes = eyesGo.GetComponent<SpriteRenderer>();

        baseSprites = Resources.LoadAll<Sprite>("Images/player/base/" + baseSpriteSheet);
        eyeSprites = Resources.LoadAll<Sprite>("Images/player/eyes/" + eyesSpriteSheet);

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

        if (newBaseSprite) 
        {
            rendererBase.sprite = newBaseSprite;
        }

        if (newEyesSprite) 
        {
            rendererEyes.sprite = newEyesSprite;
        }
        
    }
}
