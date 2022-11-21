using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public Vector3 spawnPoint;

    public SerializableDictionary<string, bool> switchs;
    public SerializableDictionary<string, StarData> stars;

    public LevelData()
    {
        spawnPoint = Vector3.zero;

        stars = new SerializableDictionary<string, StarData>();
        switchs = new SerializableDictionary<string, bool>();
    }
}
