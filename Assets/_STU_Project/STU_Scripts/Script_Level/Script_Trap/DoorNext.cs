using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorNext : MonoBehaviour
{
    //private string nextLevelName = "STJ_Old_Level";
    //private string playName = "STJ_PLAY1";
    [SerializeField] private EvaluationForm winPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果碰到我的東西它的名稱是Player
        if (collision.CompareTag("Player"))
        {
            //打開評分表
            winPanel.Show();
        }
    }

    public void GotoNextLevel()
    {
        GameManager.instance.scenesNumber++;
        SceneManager.LoadScene(GameManager.instance.levelName);
    }
}