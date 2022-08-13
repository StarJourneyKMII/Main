using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpacePartsData", menuName = "New SpaceParts")]
public class SpacePartsData : ScriptableObject
{
    [Header("¸ÑÂê")]
    public bool unlock;
    public Sprite lockSprite;
    public Sprite unlockSprite;

    public SpacePartsFragData[] frags;

    public Sprite showSprite
    {
        get
        {
            if(unlock == true)
                return unlockSprite;
            else
                return lockSprite;
        }
    }
}


