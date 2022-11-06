using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler
{
    private string saveName = "GameData_Level1";

    public DataHandler(string saveName)
    {
        this.saveName = saveName;
    }

    public GameData Load()
    {
        string jsonData = PlayerPrefs.GetString(saveName);
        GameData loadData = JsonUtility.FromJson<GameData>(jsonData);

        return loadData;
    }

    public void Save(GameData data)
    {
        string toJson = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveName, toJson);
    }
}
