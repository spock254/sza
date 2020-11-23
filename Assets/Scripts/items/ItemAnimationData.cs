using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemAnimationData
{
    public List<Sprite> itemSpriteFrames;

    int frameIndex = 0;
    public Sprite GetNextFrameSprite() 
    {
        frameIndex++; // increment index
        frameIndex %= itemSpriteFrames.Count; // clip index (turns to 0 if index == items.Count)
        //if (frameIndex == itemSpriteFrames.Count) 
        //{
        //    frameIndex = -1;
        //}

        //frameIndex++;

        return itemSpriteFrames[frameIndex];
    }
}
