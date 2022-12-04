using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestGM : MonoBehaviourSingleton<TestGM>
{
    [SerializeField] private GameObject panel;
    private PlanetsDataBase planetsDataBase;

    public event Action OnPlanetDataChange;

    protected override void DidAwake()
    {
        base.DidAwake();

    }
    private void Start()
    {
        planetsDataBase = Resources.Load<PlanetsDataBase>("Data_¬P²y/PlanetsDataBase");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SwitchPanel();
        }
    }

    private void SwitchPanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }

    public void ResetLevel()
    {
        foreach (PlanetData planetData in planetsDataBase.planetsData)
        {
            planetData.ResetDefault();
        }
        planetsDataBase.UnLockLevelByLevelIndex(0);
        OnPlanetDataChange?.Invoke();
    }

    public void UnLockAllLevel()
    {
        foreach(PlanetData planetData in planetsDataBase.planetsData)
        {
            if (planetData.planetIndex == -1) continue;
            planetData.UnLock();
            planetData.UnLockAllArea();
        }
        OnPlanetDataChange?.Invoke();
    }

    public void UnLockNextLevel()
    {
        planetsDataBase.UnLockNextLevel();
        OnPlanetDataChange?.Invoke();
    }

    private void OnApplicationQuit()
    {
        ResetLevel();
    }
}
