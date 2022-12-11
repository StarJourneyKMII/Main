using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MiProduction.BroAudio;

public class SceneChangeManager : MonoBehaviourSingleton<SceneChangeManager>
{
    [SerializeField] private float loadTime = 1f;
    [SerializeField] private SceneConfig_MultipleSprites loadHintData;
    [SerializeField] private SceneLoadPanel loadPanel;

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

    public void LoadSceneByName(string sceneName, bool needSave = true)
    {
        //SceneManager.LoadScene(sceneName);
        if (needSave)
            DataManager.Instance.SaveGame();
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
        DataManager.Instance.LoadData();
        loadPanel.CloseLoadPanel();
    }

    private void SetProgress(ref float displayProgress)
    {
        displayProgress += Time.deltaTime * 100 / loadTime;
        if(displayProgress > 100)
            displayProgress = 100;
        loadPanel.SetProgress(displayProgress);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
