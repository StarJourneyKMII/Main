using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MiProduction.BroAudio;

public class Switch : MonoBehaviour
{
    private Animator anim;
    public GameObject focusCamera;
    public GameObject switchObject;
    public bool debug;
    private bool isOpen;
    private PlayerCtrl playerCtrl;

    private void Start()
    {
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        anim = GetComponent<Animator>();
        isOpen = switchObject.activeInHierarchy;
        focusCamera.transform.position = new Vector3(switchObject.transform.position.x,switchObject.transform.position.y,Camera.main.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Switch Close Sound
            SoundSystem.PlaySFX(Sound.Switch);
            // Touch Switch one time
            anim.SetTrigger("Touch");
            gameObject.GetComponent<Collider2D>().enabled = false;
            OpenAnimation();
        }
    }
    private void OpenAnimation()
    {
        playerCtrl.StopCtrl();
        isOpen = !isOpen;
        if (isOpen == true)
        {
            StartCoroutine(FadeInSwitchObj());
            focusCamera?.SetActive(true);
            switchObject?.SetActive(true);
        }
        else if (isOpen == false)
        {
            StartCoroutine(FadeOutSwitchObj());
            focusCamera?.SetActive(true);
        }
    }

    private IEnumerator FadeInSwitchObj()
    {
        float fadeSec = 0.5f;
        float cameraBlendSec = 1.5f;
        SpriteRenderer[] childsSpr = switchObject.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer t in childsSpr)
            t.DOFade(1, fadeSec).SetDelay(cameraBlendSec).From(0);

        yield return new WaitForSeconds(fadeSec + cameraBlendSec);
        focusCamera?.SetActive(false);
        playerCtrl.canCtrl = true;
    }
    private IEnumerator FadeOutSwitchObj()
    {
        float fadeSec = 0.5f;
        float cameraBlendSec = 1.5f;
        SpriteRenderer[] childsSpr = switchObject.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer t in childsSpr)
            t.DOFade(0, fadeSec).SetDelay(cameraBlendSec).From(1);

        yield return new WaitForSeconds(fadeSec + cameraBlendSec);
        focusCamera?.SetActive(false);
        switchObject?.SetActive(false);
        playerCtrl.canCtrl = true;
    }

    private void OnDrawGizmos()
    {
        if (debug == false) return;

        Gizmos.color = Color.yellow;
        ExtensionGizmo.Label(transform.position + Vector3.up * 2, "Trigger", Color.yellow);
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.color = Color.red;
        foreach (SpriteRenderer child in switchObject.GetComponentsInChildren<SpriteRenderer>())
        {
            //ExtensionGizmo.Label(child.position + Vector3.up * 2, "HideObj", Color.red);
            if (child.CompareTag("Star"))
            {
                ExtensionGizmo.DrawWireDisc(child.transform.position, Vector3.forward, child.transform.lossyScale.x / 2);
            }
            else
            {
                Gizmos.DrawWireCube(child.transform.position, child.transform.lossyScale);
            }
        }
    }
}
