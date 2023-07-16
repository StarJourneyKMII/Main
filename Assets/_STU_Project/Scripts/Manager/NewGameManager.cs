using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager
{
    private static NewGameManager instance;
    public static NewGameManager Instance
    {
        get
        {
            if(instance == null)
                instance = new NewGameManager();
            return instance;
        }
    }

    private PlanetData currentPlanet;

    public int currentLevelIndex;

    public string CurrentLevelName
    {
        get { return SceneManager.GetActiveScene().name; }
    }

    public event Action OnRestartEvent;

    public void GameOver()
    {
        SceneChangeManager.Instance.LoadSceneByName("GameOver", false);
    }

    public void SetSelectLevel(PlanetData planetData, PlanetAreaData planetAreaData)
    {
        this.currentPlanet = planetData;
        if (planetAreaData != null)
            currentLevelIndex = planetAreaData.levelIndex;
    }

    public void GotoNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex > 14)
            SceneChangeManager.Instance.LoadSceneByName("Lobby");
        else
        {
            PlanetAreaData areaData = GetAreaDataByIndex(currentLevelIndex, out PlanetData planet);
            if (currentPlanet != planet)
            {
                planet.unLock = true;
            }
            areaData.unLock = true;

            DataManager.Instance.SaveScriptableData();
            SceneChangeManager.Instance.LoadSceneByName("STJ_Old_Level" + currentLevelIndex);
        }
    }

    public PlanetAreaData GetAreaDataByIndex(int index, out PlanetData planetData)
    {
        PlanetData[] planets = Resources.LoadAll<PlanetData>("Data_¬P²y");
        foreach(PlanetData planet in planets)
        {
            foreach(PlanetAreaData areaData in planet.planetArea)
            {
                if (areaData.levelIndex == index)
                {
                    planetData = planet;
                    return areaData;
                }
            }
        }
        planetData = null;
        return null;
    }

    public void ClearRestartEvent()
    {
        OnRestartEvent = null;
    }

    public void Restart()
    {
        OnRestartEvent?.Invoke();
    }
}
