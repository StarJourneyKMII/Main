using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler
{
    private string gameSaveName = "";

    public DataHandler(string saveName)
    {
        this.gameSaveName = saveName;
    }

    public GameData Load()
    {
        string jsonData = PlayerPrefs.GetString(gameSaveName, "");
        if(jsonData == "")
        {
            return null;
        }

        GameData loadData = JsonUtility.FromJson<GameData>(jsonData);

        return loadData;
    }

    public void Save(GameData data)
    {
        string toJson = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(gameSaveName, toJson);
    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Delete");
    }
}
