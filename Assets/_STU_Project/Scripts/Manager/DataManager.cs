using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private GameData gameData;
    private List<IData> dataObjects;
    private DataHandler dataHandler;
    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more NewGameManager in the scene");
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        CheckHasOldGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SaveGame();
        else if (Input.GetKeyDown(KeyCode.U))
            Delete();
    }

    private List<IData> FindAllDataObjects()
    {
        IEnumerable<IData> newDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IData>();

        return new List<IData>(newDataObjects);
    }

    public void NewGame()
    {
        Debug.Log("NewGame");
        Debug.Log("NewGameSaveName : " + "Test01");
        this.gameData = new GameData();
        this.dataHandler = new DataHandler("Test01");
    }

    public void SaveGame()
    {
        Debug.Log("SaveGame");

        if (dataObjects == null) return;

        foreach (IData data in dataObjects)
        {
            data.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }
    public void LoadGame()
    {
        Debug.Log("LoadGameName : " + "Test01");
        this.dataHandler = new DataHandler("Test01");
        gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data found. Init data to defaults.");
            NewGame();
        }
    }

    public void LoadData()
    {
        this.dataObjects = FindAllDataObjects();

        foreach (IData data in dataObjects)
        {
            data.LoadData(gameData);
        }
    }

    private void Delete()
    {
        dataHandler.Delete();
    }

    private void CheckHasOldGame()
    {
        this.dataHandler = new DataHandler("Test01");
        gameData = dataHandler.Load();
        if (gameData != null)
            LoadButton.Instance.OpenLoadButton();
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
}
