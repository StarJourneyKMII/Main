using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiProduction.Scene;
using UnityEditor;

[CreateAssetMenu(fileName = "NewPlanetsDataBase", menuName = "Data/Planet Data/New PlanetsDataBase", order = 1)]
public class PlanetsDataBase : ScriptableObject
{
    public PlanetSetting[] planetSetting;
    public PlanetData[] planetsData;

    private int GetLevelTotal()
    {
        int maxLevel = -1;
        foreach(PlanetData planetData in planetsData)
        {
            if(maxLevel < planetData.MaxLevelIndex)
            {
                maxLevel = planetData.MaxLevelIndex;
            }
        }
        return maxLevel;
    }

    private int GetNextLevelIndex()
    {
        int continueIndex = -1;
        foreach (PlanetData planetData in planetsData)
        {
            if (planetData.unLock == false) continue;

            if (planetData.ContinueAreaLevelIndex >= continueIndex)
            {
                continueIndex = planetData.ContinueAreaLevelIndex;
            }
        }

        return continueIndex + 1;
    }

    public PlanetData GetPlanetDataByName(string planetName)
    {
        foreach(PlanetData planetData in planetsData)
        {
            if(planetData.planetName == planetName)
            {
                return planetData;
            }
        }

        return null;
    }

    public PlanetData GetPlanetDataByLevelIndex(int levelIndex)
    {
        foreach (PlanetData planetData in planetsData)
        {
            PlanetAreaData planetAreaData = planetData.GetPlanetAreaByLevelIndex(levelIndex);

            if(planetAreaData != null)
            return planetData;
        }

        return null;
    }

    public PlanetAreaData GetPlanetAreaDataByLevelIndex(int levelIndex)
    {
        foreach (PlanetData planetData in planetsData)
        {
            PlanetAreaData planetAreaData = planetData.GetPlanetAreaByLevelIndex(levelIndex);

            if (planetAreaData != null)
                return planetAreaData;
        }

        return null;
    }

    public PlanetData GetContinuePlanetData()
    {
        return GetPlanetDataByLevelIndex(GetNextLevelIndex() - 1);
    }

    public PlanetAreaData GetContinuePlanetAreaData()
    {
        return GetPlanetAreaDataByLevelIndex(GetNextLevelIndex() - 1);
    }

    public void UnLockLevelByLevelIndex(int levelIndex)
    {
        if (levelIndex > GetLevelTotal()) return;

        PlanetData levelPlanet = GetPlanetDataByLevelIndex(levelIndex);
        PlanetAreaData levelPlanetArea = GetPlanetAreaDataByLevelIndex(levelIndex);

        if (levelPlanet.unLock == false)
            levelPlanet.UnLock();
        levelPlanetArea.UnLock();
    }

    public int GetCurrentLevel()
    {
        return GetNextLevelIndex() - 1;
    }

    public void UnLockNextLevel()
    {
        UnLockLevelByLevelIndex(GetNextLevelIndex());
    }


    private void Awake()
    {
        if(planetSetting == null)
        {
            Initilize();
        }
    }

    private void Initilize()
    {
        PlanetData[] planetData = Resources.LoadAll<PlanetData>("Data_¬P²y");
        string[] planetsPath = new string[planetData.Length];
        for (int i = 0; i < planetData.Length; i++)
            planetsPath[i] = AssetDatabase.GetAssetPath(planetData[i]);

        planetSetting = new PlanetSetting[planetsPath.Length];
        for (int i = 0; i < planetsPath.Length; i++)
            planetSetting[i].scriptable = planetsPath[i];

        AutoSetPlanetIndex();
    }

    private void AutoSetPlanetIndex()
    {
        for(int i = 0; i < planetSetting.Length; i++)
        {
            planetSetting[i].planetIndex = i + 1;
        }
    }
}

[System.Serializable]
public struct PlanetSetting
{
    public int planetIndex;
    [PlanetDataSelector]
    public string scriptable;
    public bool unLock;
    public AreaSetting[] area;
}

[System.Serializable]
public struct AreaSetting
{
    public int areaIndex;
    public bool unLock;
}
