using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlanetArea", menuName = "New PlanetArea")]
public class PlanetAreaData : ScriptableObject
{
    [Header("詳細資料")]
    public string areaName;
    [TextArea(7, 13)]
    public string description;

    [Header("可蒐集素材")]
    public List<SpacePartsFragData> spaceFrags;
}
