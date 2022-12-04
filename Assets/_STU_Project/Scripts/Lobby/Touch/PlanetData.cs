using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Planet", menuName = "Data/Planet Data/New Planet")]
public class PlanetData : ScriptableObject
{
    [Header("解鎖")]
    public bool unLock;
    public Sprite lockSprite;
    public Sprite unlockSprite;

    [Header("詳細資料")]
    public string planetName;
    [TextArea(7, 13)]
    public string description;

    [Header("區域")]
    public int planetIndex = -1;
    public PlanetAreaData[] planetArea;
    
    public int MaxLevelIndex
    {
        get
        {
            if (planetArea.Length == 0) 
                return -1;

            List<PlanetAreaData>  deSort = planetArea.OrderByDescending(i => i.levelIndex).ToList();
            return deSort[0].levelIndex;
        }
    }
    public int ContinueAreaLevelIndex
    {
        get
        {
            if(IsAllClear)
            {
                return MaxLevelIndex;
            }
            for (int i = planetArea.Length - 1; i >= 0; i--)
            {
                if (planetArea[i].unLock == true)
                    return planetArea[i].levelIndex;
            }
            return -1;
        }
    }
    public int ContinueAreaIndex
    {
        get
        {
            for (int i = planetArea.Length - 1; i >= 0; i--)
            {
                if (planetArea[i].unLock == true)
                    return i;
            }
            return -1;
        }
    }
    public bool IsAllClear
    {
        get
        {
            foreach(PlanetAreaData area in planetArea)
            {
                if (area.unLock)
                    continue;
                else
                    return false;
            }
            return true;

        }
    }

    public PlanetAreaData GetPlanetAreaByIndex(int areaIndex)
    {
        if (areaIndex < planetArea.Length)
            return planetArea[areaIndex];

        return null;
    }

    public PlanetAreaData GetPlanetAreaByLevelIndex(int levelIndex)
    {
        foreach(PlanetAreaData area in planetArea)
        {
            if(area.levelIndex == levelIndex)
                return area;
        }

        return null;
    }

    public PlanetAreaData GetContinuePlanetArea()
    {
        if(ContinueAreaIndex == -1)
            return null;
        return planetArea[ContinueAreaIndex];
    }

    public void ResetDefault()
    {
        unLock = false;
        foreach (PlanetAreaData area in planetArea)
        {
            area.ResetDefault();
        }
    }

    public void UnLock()
    {
        unLock = true;
    }

    public void UnLockAllArea()
    {
        foreach (PlanetAreaData area in planetArea)
        {
            area.UnLock();
        }
     }
}
