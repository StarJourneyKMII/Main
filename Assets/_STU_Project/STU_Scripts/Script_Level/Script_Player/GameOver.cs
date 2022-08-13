using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    void Start()
    {
        // 初始化 三秒後執行GoToMenu
        Invoke("GoToMenu", 3f);
    }
    void GoToMenu()
    {
        // 切換場景到
        SceneManager.LoadScene("Lobby");
    }
}


