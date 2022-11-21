using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MiProduction.BroAudio;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance;

    [SerializeField] private SceneLoadPanel loadPanel;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        DontDestroyOnLoad(this);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StarUp");
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby");
        //StartCoroutine(LoadScene("Lobby"));
    }
    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadSceneByName(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        float displayProgress = 0;
        float toProgress = 0;
        loadPanel.OpenLoadPanel();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while(operation.progress < 0.9f)
        {
            toProgress = operation.progress * 100;
            while(displayProgress < toProgress)
            {
                SetProgress(ref displayProgress);
                yield return null;
            }
        }

        toProgress = 100;
        while(displayProgress < toProgress)
        {
            SetProgress(ref displayProgress);
            yield return null;
        }

        operation.allowSceneActivation = true;
        yield return null;
        loadPanel.CloseLoadPanel();
        DataManager.Instance.LoadData();
    }

    private void SetProgress(ref float displayProgress)
    {
        displayProgress += Time.deltaTime * 65;
        if(displayProgress > 100)
            displayProgress = 100;
        loadPanel.SetProgress(displayProgress);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
