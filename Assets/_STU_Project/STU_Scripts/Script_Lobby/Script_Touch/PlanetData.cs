using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Planet", menuName = "New Planet")]
public class PlanetData : ScriptableObject
{
    [Header("解鎖")]
    public bool unlock;
    public Sprite lockSprite;
    public Sprite unlockSprite;

    [Header("詳細資料")]
    public string planetName;
    [TextArea(7, 13)]
    public string description;

    [Header("區域")]
    public int levelIndex;
    public int planetIndex;
    public List<PlanetAreaData> planetArea;
}
