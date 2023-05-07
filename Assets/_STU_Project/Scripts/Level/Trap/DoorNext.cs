using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorNext : MonoBehaviour
{
    //private string nextLevelName = "STJ_Old_Level";
    //private string playName = "STJ_PLAY1";
    [SerializeField] private EvaluationForm winPanel;
    private Animator animator;

    private bool isUsed;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isUsed)
        {
            isUsed = true;
            StartCoroutine(PlayAnimation(collision.transform.gameObject));
        }
    }

    private IEnumerator PlayAnimation(GameObject player)
    {
        animator.SetTrigger("OpenDoor");

        yield return new WaitForSeconds(0.8f);
        player.SetActive(false);

        yield return new WaitForSeconds(1f);
        winPanel.Show();
    }


    public void GotoNextLevel()
    {
        DataManager.Instance.SaveGame();
        NewGameManager.Instance.GotoNextLevel();
    }

    public void GoToLobby()
    {
        DataManager.Instance.SaveGame();
        NewGameManager.Instance.GoToLobby();
    }
}