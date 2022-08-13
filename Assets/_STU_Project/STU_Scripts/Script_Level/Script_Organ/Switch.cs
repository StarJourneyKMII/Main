using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Switch : MonoBehaviour
{
    private Animator anim;
    public GameObject focusCamera;
    public GameObject switchObject;
    public bool debug;
    private bool isOpen;

    private void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = switchObject.activeInHierarchy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Switch Close Sound
            SoundManager.Instance.Play(Sound.SwitchClose);
            // Touch Switch one time
            anim.SetTrigger("Touch");
            gameObject.GetComponent<Collider2D>().enabled = false;
            OpenAnimation();
        }
    }
    private void OpenAnimation()
    {
        isOpen = !isOpen;
        if (isOpen == true)
        {
            StartCoroutine(FadeSwitchObj());
            if (focusCamera != null)
                focusCamera.SetActive(true);
            switchObject.SetActive(true);
        }
        else if (isOpen == false)
        {
            switchObject.SetActive(false);
        }
    }

    private IEnumerator FadeSwitchObj()
    {
        float fadeSec = 0.5f;
        float cameraBlendSec = 1.5f;
        SpriteRenderer[] childsSpr = switchObject.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer t in childsSpr)
            t.DOFade(1, fadeSec).SetDelay(cameraBlendSec).From(0);

        yield return new WaitForSeconds(fadeSec + cameraBlendSec);
        if (focusCamera != null)
            focusCamera.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (debug == false) return;

        Gizmos.color = Color.yellow;
        ExtensionGizmo.Label(transform.position + Vector3.up * 2, "Trigger", Color.yellow);
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.color = Color.red;
        foreach (Transform child in switchObject.transform)
        {
            //ExtensionGizmo.Label(child.position + Vector3.up * 2, "HideObj", Color.red);
            Gizmos.DrawWireCube(child.position, child.lossyScale);
        }
    }
}
