using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TestGM : MonoBehaviourSingleton<TestGM>
{
    [SerializeField] private SceneConfig_Music sceneConfig;
    [SerializeField] private GameObject planetPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Text levelUI;
    private PlanetsDataBase planetsDataBase;

    public event Action OnPlanetDataChange;

    protected override void DidAwake()
    {
        base.DidAwake();

    }
    private void Start()
    {
        planetsDataBase = Resources.Load<PlanetsDataBase>("Data_星球/PlanetsDataBase");
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
        if(sceneConfig.TryGetSceneData(out Music music))
        {
            switch(music)
            {
                case Music.Menu:
                    planetPanel.SetActive(!planetPanel.activeInHierarchy);
                    break;
                case Music.MainGame:
                    levelPanel.SetActive(!levelPanel.activeInHierarchy);
                    break;
            }
        }
    }

    public void ResetLevel()
    {
        foreach (PlanetData planetData in planetsDataBase.planetsData)
        {
            planetData.ResetDefault();
        }
        planetsDataBase.UnLockLevelByLevelIndex(0);
        OnPlanetDataChange?.Invoke();
        levelUI.text = "當前最新關卡 : Level" + planetsDataBase.GetCurrentLevel();
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
        levelUI.text = "當前最新關卡 : Level" + planetsDataBase.GetCurrentLevel();
    }

    public void UnLockNextLevel()
    {
        planetsDataBase.UnLockNextLevel();
        OnPlanetDataChange?.Invoke();
        levelUI.text = "當前最新關卡 : Level" + planetsDataBase.GetCurrentLevel();
    }

    public void OnInvincibleChange()
    {
        FindObjectOfType<Player>().SetInvincible(toggle.isOn);
    }

    public void RestartLevel()
    {
        NewGameManager.Instance.Restart();
    }

    private void OnApplicationQuit()
    {
        ResetLevel();
    }
}
