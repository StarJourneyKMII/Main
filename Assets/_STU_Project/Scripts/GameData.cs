using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, bool> switchUsed;
    public SerializableDictionary<string, bool> starCollected;

    public GameData()
    {
        starCollected = new SerializableDictionary<string, bool>();
        switchUsed = new SerializableDictionary<string, bool>();
    }
}
