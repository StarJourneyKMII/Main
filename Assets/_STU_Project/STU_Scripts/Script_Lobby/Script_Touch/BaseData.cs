using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData : ScriptableObject
{
    [Header("解鎖")]
    public bool unlock;
    public Sprite lockSprite;
    public Sprite unlockSprite;

    [Header("詳細資料")]
    public string detailName;
    public Sprite detailSprite;
    [TextArea(7, 13)]
    public string description;
}
