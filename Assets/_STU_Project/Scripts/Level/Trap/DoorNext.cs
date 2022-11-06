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
        if (collision.CompareTag("Player"))
        {
            winPanel.Show();
        }
    }

    public void GotoNextLevel()
    {
        GameManager.instance.scenesNumber++;
        SceneManager.LoadScene(GameManager.instance.levelName);
    }
}