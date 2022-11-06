using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConstellationTouch : MonoBehaviour
{
    public ConstellationData data;

    public GameObject dialogueObj;
    public Text dialogue;

    private Coroutine hideCoroutine;

    public void Touch()
    {
        int dialogueCount = data.dialogues.Length;
        int randNum = Random.Range(0, dialogueCount);
        if(data.dialogues[randNum].animation != null)
        {

        }
        else
        {
            transform.localScale = Vector3.one;
            transform.DOPunchScale(-Vector3.one * 0.1f, 0.5f, 2);
        }
        dialogueObj.SetActive(true);
        dialogue.text = data.dialogues[randNum].content;

        if(hideCoroutine != null)
            StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HideDialogue());
    }

    private IEnumerator HideDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogueObj.SetActive(false);
    }
}
