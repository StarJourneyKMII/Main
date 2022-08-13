using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_UI : MonoBehaviour
{
    public void BackToLobby()
    {
        GameManager.instance.GoToLobby();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
