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
        frameIndex++; 
        frameIndex %= itemSpriteFrames.Count;

        return itemSpriteFrames[frameIndex];
    }
}
