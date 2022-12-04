using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlanetArea", menuName = "Data/Planet Data/New PlanetArea")]
public class PlanetAreaData : ScriptableObject
{
    public bool unLock = false;
    public int levelIndex;

    [Header("�ԲӸ��")]
    public string areaName = "Unknow Name";
    [TextArea(7, 13)]
    public string description = "No Description";

    [Header("�i�`������")]
    public List<SpacePartsFragData> spaceFrags;

    public void ResetDefault()
    {
        unLock = false;
    }

    public void UnLock()
    {
        unLock = true;
    }
}
