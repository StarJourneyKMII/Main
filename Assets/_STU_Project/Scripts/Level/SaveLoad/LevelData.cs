using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public Vector3 spawnPoint;
    public PlayerSex playerSex;

    public SerializableDictionary<string, bool> switchs;
    public SerializableDictionary<string, StarData> stars;

    public LevelData()
    {
        spawnPoint = Vector3.zero;
        playerSex = PlayerSex.Girl;

        stars = new SerializableDictionary<string, StarData>();
        switchs = new SerializableDictionary<string, bool>();
    }
}
