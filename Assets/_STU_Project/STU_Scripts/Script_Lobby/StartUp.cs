using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    // [SerializeField] GameObject LoadButton = null;
    private void Start()
    {   
        /*
        if (GameManager.instance.canLoad == false)
        {
            LoadButton.SetActive(false);
        }
        */
    }
    public void GoToLobby()
    {
        GameManager.instance.GoToLobby();
    }
    public void GoToCharacters()
    {
        GameManager.instance.GoToCharacters();
    }

    public void GoExit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        GameManager.instance.NewGame();
    }
    public void LoadGame()
    {
        GameManager.instance.LoadGame();
    }

    public void PlayUISound(int sound)
    {
        SoundManager.Instance.PlayWithUI((Sound)sound);
    }
}
