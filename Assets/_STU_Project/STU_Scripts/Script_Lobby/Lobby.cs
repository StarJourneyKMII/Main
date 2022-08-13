using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public void GoToGallery()
    {
        GameManager.instance.GoToGallery();
    }

    public void GoToProperty()
    {
        GameManager.instance.GoToProperty();
    }
    public void GoToSelectLevel()
    {
        GameManager.instance.GoToSelectLevel();
    }
    public void GoToAdventure()
    {
        GameManager.instance.NewGame();
    }

    public void GoToMenu()
    {
        GameManager.instance.GoToMenu();
    }

    public void GoToConstellation()
    {
        GameManager.instance.GoToConstellation();
    }

    public void GoTo()
    {
        
    }

    public void PlayUISound(int sound)
    {
        SoundManager.Instance.PlayWithUI((Sound)sound);
    }
}
