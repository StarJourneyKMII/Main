using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorNext : MonoBehaviour
{
    //private string nextLevelName = "STJ_Old_Level";
    //private string playName = "STJ_PLAY1";
    [SerializeField] private EvaluationForm winPanel;
    [SerializeField] private Animator animator;
    private bool isTrigger;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTrigger)
        {
            //winPanel.Show();
            isTrigger = true;
            animator.SetTrigger("OpenDoor");
            StartCoroutine(PlayAnimation(collision.transform.gameObject));
        }
    }

    private IEnumerator PlayAnimation(GameObject player)
    {
        yield return new WaitForSeconds(0.8f);
        player.SetActive(false);

        yield return new WaitForSeconds(2f);
        winPanel.Show();
    }


    public void GotoNextLevel()
    {
        GameManager.instance.scenesNumber++;
        SceneManager.LoadScene(GameManager.instance.levelName);
    }
}