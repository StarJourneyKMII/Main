using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataManager : MonoBehaviour
{
    private GameData gameData;
    private List<IData> dataObjects;
    private DataHandler dataHandler;
    public DataManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more NewGameManager in the scene");
        }
        Instance = this;
    }

    private void Start()
    {
        this.dataHandler = new DataHandler("TesA");
        this.dataObjects = FindAllDataObjects();
        LoadGame();
    }

    private List<IData> FindAllDataObjects()
    {
        IEnumerable<IData> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IData>();

        return new List<IData>(dataObjects);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach(IData data in dataObjects)
        {
            data.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }
    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data found. Init data to defaults.");
            NewGame();
        }

        foreach (IData data in dataObjects)
        {
            data.LoadData(gameData);
        }    
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
