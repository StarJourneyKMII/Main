using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>GM, Only one for all</summary>
public class GameManager
{
    // 單例模式自己創造自己
    // static 全域值
    // 全域宣告一個GameManager的實體
    // 在有人需要我的時候 檢查我的記憶體是否存在
    // 如果不存在就憑空分配一個記憶體給自己
    // 然後回傳給對方
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            // 有人讀取我了 有人需要我
            // 檢查我是否存在
            if (_instance == null)
            {
                // 我不存在於記憶體中的任何位置 我要憑空製造我自己
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    //----------------------------------------------

    string playName = "STU_PLAY";
    int playNumber = 1;
    string scenesName = "STJ_Old_Level";
    public int scenesNumber = 0;

    /// <summary>關卡(存讀檔到硬碟)</summary>
    public string levelName
    {
        get { return PlayerPrefs.GetString("LEVEL", playName + playNumber + "/" + scenesName + scenesNumber); }
        set { PlayerPrefs.SetString("LEVEL", value); }
    }

    /// <summary>命數(存讀檔到硬碟)</summary>
    public int life
    {
        get { return PlayerPrefs.GetInt("LIFE", 9); }
        set { PlayerPrefs.SetInt("LIFE", value); }
    }


    float maxHp = 10f;
    /// <summary>訂閱生命值發生變化(附加生命值的百分比)</summary>
    public Action<float> Act_HpChange = null;
    /// <summary> current Hp </summary>
    public float hp
    {
        //get { return _hp; }
        // 有人需要血量時直接從硬碟讀取
        get { return PlayerPrefs.GetFloat("HP", 1f); }
        set
        {
            // 直接寫入硬碟
            PlayerPrefs.SetFloat("HP", Mathf.Clamp(value, 0f, maxHp));
            // _hp = Mathf.Clamp(value, 0f, maxHp);
            // 在任何情況下修改血量時 同步更新畫面上的血條
            // 先試著在畫面上找出血條
            // GameObject hpObject = GameObject.Find("MainHpBar");
            // Trying to find the program of the Image category
            // Image hpObjectImagePro = hpObject.GetComponent<Image>();
            // Modify the filling percentage
            // hpObjectImagePro.fillAmount = hp / maxHp;

            // 當血量發生變化的時候 檢查有沒有人訂閱我 如果有的話就發送通知
            // != 是否不相等
            // null = 空
            if (Act_HpChange != null)
            {
                Act_HpChange.Invoke(hp / maxHp);
            }

        }
    }
    //float _hp = 10f;

    /// <summary>是否可以讀檔</summary>
    public bool canLoad
    {
        get { return PlayerPrefs.GetInt("CANLOAD", 0) == 1; }
        set { PlayerPrefs.SetInt("CANLOAD", value ? 1 : 0); }
    }

    /// <summary> Save </summary>
    public void Save()
    {
        PlayerPrefs.SetString("LEVEL", levelName);
        PlayerPrefs.SetInt("LIFE", life);
        PlayerPrefs.SetFloat("HP", Mathf.Clamp(hp, 0f, maxHp));

    }
    /// <summary> Load </summary>
    public void Load()
    {
        levelName = PlayerPrefs.GetString("LEVEL", "Level0");
        life = PlayerPrefs.GetInt("LIFE", 10);
        hp = PlayerPrefs.GetFloat("HP", 10f);
    }

    // 儲存復活點的位置資訊
    public Vector3 respawnPosition = Vector3.zero;

    public void GoToStartUp()
    {
        SceneManager.LoadScene("StartUp");
    }

    public void GoToCharacters() 
    {
        SceneManager.LoadScene("CharactersSelect");
    }

    public void GoToLobby() 
    {
        SceneManager.LoadScene("Lobby");
    }
    public void GoToSelectLevel()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void GoToGallery() 
    {
        // SceneManager.LoadScene("Gallery");
    }

    public void GoToProperty()
    {
        // SceneManager.LoadScene("Property");
    }
    public void GoToAdventure()
    {
        SceneManager.LoadScene(levelName);
    }

    public void GoToMenu()
    {
        // SceneManager.LoadScene("Menu");
    }

    public void GoToConstellation()
    {
        // SceneManager.LoadScene("Constellation");
    }

    public void NewGame()
    {
        // 手動修改紀錄為全新的狀態
        hp = 1f;
        life = 10;
        //levelName = playName + playNumber + "/" + scenesName + scenesNumber;
        levelName = scenesName + scenesNumber;
        canLoad = true; // New Game can load tag
        // 載入遊戲
        LoadGame();
    }
    public void LoadGame()
    {
        // 載入遊戲的時候 檢查命是否夠用 如果命用完了就回主畫面
        if (life > 0)
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            canLoad = false; // No life can't load
            SceneManager.LoadScene("GameOver");
        }
    }

}
