using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MiProduction.BroAudio;

public class SceneChangeManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StarUp");
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
