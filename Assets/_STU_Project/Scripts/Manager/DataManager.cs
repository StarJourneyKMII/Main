using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviourSingleton<DataManager>
{
    private GameData gameData;
    private List<IData> dataObjects;
    private DataHandler dataHandler;

    [SerializeField] private bool autoSave;
    [SerializeField] private float autoSaveSec = 120f;
    private float autoSaveStartTime;

    private SavingAnimation saveAnimation;

    protected override void DidAwake()
    {
        base.DidAwake();
        CheckHasOldGame();
        saveAnimation= GetComponentInChildren<SavingAnimation>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SaveGame();
        else if (Input.GetKeyDown(KeyCode.U))
            Delete();

        if(autoSave)
        {
            if(Time.time >= autoSaveStartTime + autoSaveSec)
            {
                autoSaveStartTime = Time.time;
                SaveGame();
            }
        }
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
        //SaveScriptableData();
        dataHandler.Save(gameData);

        saveAnimation.Play();
    }
    public void LoadGame()
    {
        Debug.Log("LoadGameName : " + "Test01");
        this.dataHandler = new DataHandler("Test01");
        gameData = dataHandler.Load();
        LoadScriptableData();
        if (this.gameData == null)
        {
            Debug.Log("No data found. Init data to defaults.");
            NewGame();
        }
    }

    private void LoadScriptableData()
    {
        PlanetsDataBase planetsDataBase = Resources.Load<PlanetsDataBase>("Data_星球/PlanetsDataBase");
        foreach (PlanetData planetData in planetsDataBase.planetsData)
        {
            gameData.planetUnlockData.TryGetValue(planetData, out bool planetValue);
            planetData.unLock = planetValue;
            foreach (PlanetAreaData planetArea in planetData.planetArea)
            {
                gameData.planetAreaUnlockData.TryGetValue(planetArea, out bool areaValue);
                planetArea.unLock = areaValue;
            }
        }
    }

    public void SaveScriptableData()
    {
        PlanetsDataBase planetsDataBase = Resources.Load<PlanetsDataBase>("Data_星球/PlanetsDataBase");
        foreach (PlanetData planetData in planetsDataBase.planetsData)
        {
            if (gameData.planetUnlockData.ContainsKey(planetData))
            {
                gameData.planetUnlockData.Remove(planetData);
            }
            gameData.planetUnlockData.Add(planetData, planetData.unLock);

            foreach (PlanetAreaData planetArea in planetData.planetArea)
            {
                if (gameData.planetAreaUnlockData.ContainsKey(planetArea))
                {
                    gameData.planetAreaUnlockData.Remove(planetArea);
                }
                gameData.planetAreaUnlockData.Add(planetArea, planetArea.unLock);
            }
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
        DataHandler checkDataHandler = new DataHandler("Test01");
        gameData = checkDataHandler.Load();
        if (gameData != null && SceneManager.GetActiveScene().name == "StartUp")
        {
            LoadButton.Instance.OpenLoadButton();
        }
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
}
