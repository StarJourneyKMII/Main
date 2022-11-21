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

    public PlanetData currentPlanet;
    public PlanetAreaData currentArea;

    private int continuePlanetIndex = 0;
    private int continueAreaIndex = 0;
    public int currentLevelIndex;

    public string CurrentLevelName
    {
        get { return SceneManager.GetActiveScene().name; }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void SetSelectLevel(PlanetData planetData, PlanetAreaData planetAreaData)
    {
        this.currentPlanet = planetData;
        this.currentArea = planetAreaData;
        currentLevelIndex  = planetAreaData.levelIndex;
    }

    public void GotoNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex > 14)
            SceneChangeManager.Instance.GoToLobby();
        else
            SceneChangeManager.Instance.LoadSceneByName("STJ_Old_Level" + currentLevelIndex);
    }
}
