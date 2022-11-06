using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public Vector3 spawnPoint;
    public List<string> collectItems;
    public SerializableDictionary<string, ItemPickUpSaveData> activeItems;

    public LevelSaveData()
    {
        spawnPoint = Vector3.zero;
        collectItems = new List<string>();
        activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
    }
}
