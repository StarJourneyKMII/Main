using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager : MonoBehaviour
{
    public static NewGameManager Instance;
    private CameraTarget cameraTarget;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        cameraTarget = FindObjectOfType<CameraTarget>();
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
