using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUICtrl : MonoBehaviour
{
    public GameObject saveLoad;
    public GameObject inventory;
    public GameObject pause;
    public GameObject intruction;

    private bool pauseIsOpen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchPause();
    }

    public void OpenSaveLoad()
    {
        saveLoad.SetActive(true);
    }
    public void OpenInventory()
    {
        inventory.SetActive(true);
    }
    public void OpenPause()
    {
        Time.timeScale = 0;
        pause.SetActive(true);
    }
    public void OpenIntruction()
    {
        intruction.SetActive(true);
    }
    public void ClosePause()
    {
        Time.timeScale = 1;
        pause.SetActive(false);
    }
    private void SwitchPause()
    {
        pauseIsOpen = !pauseIsOpen;
        pause.SetActive(pauseIsOpen);
        Time.timeScale = pauseIsOpen ? 0 : 1;
    }
}
