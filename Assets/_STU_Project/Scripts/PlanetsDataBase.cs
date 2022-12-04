using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlanetsDataBase", menuName = "Data/Planet Data/New PlanetsDataBase", order = 1)]
public class PlanetsDataBase : ScriptableObject
{
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

    public void UnLockNextLevel()
    {
        UnLockLevelByLevelIndex(GetNextLevelIndex());
    }
}
