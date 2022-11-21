using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MiProduction.BroAudio;
using System;

public class Switch : MonoBehaviour, IData
{
    private Animator anim;

    [SerializeField] private GameObject focusCamera;
    [SerializeField] private GameObject switchObject;
    [SerializeField] private GameObject hintSprite;

    [SerializeField] private bool debug;
    [SerializeField] private float switchBlendSec = 0.5f;

    [SerializeField] private bool isOpen;
    private bool isUsed;
    private bool isPlaying;
    private string id;

    private SpriteRenderer[] switchsSpriteRenderer;

    private void Awake() 
    {
        id = GetComponent<UniqueID>().ID;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        focusCamera.transform.position = new Vector3(switchObject.transform.position.x,switchObject.transform.position.y,Camera.main.transform.position.z);
        switchsSpriteRenderer = switchObject.GetComponentsInChildren<SpriteRenderer>();
        if (isUsed == false)
            switchObject.SetActive(isOpen);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isUsed)
            {
                SoundSystem.PlaySFX(Sound.Switch);
                anim.SetTrigger("Touch");
                if (isOpen)
                    StartCoroutine(Close());
                else
                    StartCoroutine(Open());
                isUsed = true;
            }
            else
            {
                hintSprite.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hintSprite.SetActive(false);
        }
    }

    private IEnumerator Open()
    {
        isPlaying = true;
        focusCamera.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        switchObject.SetActive(true);
        foreach(SpriteRenderer spriteRenderer in switchsSpriteRenderer)
        {
            spriteRenderer.DOFade(1, switchBlendSec).From(0);
        }
        yield return new WaitForSeconds(switchBlendSec);
        focusCamera.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        isPlaying = false;
    }

    private IEnumerator Close()
    {
        isPlaying = true;
        focusCamera.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        foreach(SpriteRenderer spriteRenderer in switchsSpriteRenderer)
        {
            spriteRenderer.DOFade(0, switchBlendSec).From(1);
        }
        yield return new WaitForSeconds(switchBlendSec);
        switchObject.SetActive(false);
        focusCamera.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        isPlaying = false;
    }

    public void LookTarget(Action finishAction = null)
    {
        StartCoroutine(DoLookTarget(finishAction));
    }

    private IEnumerator DoLookTarget(Action finishAction = null)
    {
        isPlaying = true;
        focusCamera.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        focusCamera.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        finishAction?.Invoke();
        isPlaying = false;
    }

    public bool CanInteractive()
    {
        return isUsed && !isPlaying;
    }

    public void SaveData(ref GameData data)
    {
        if (data.CurrentLevelData.switchs.ContainsKey(id))
        {
            data.CurrentLevelData.switchs.Remove(id);
        }
        data.CurrentLevelData.switchs.Add(id, isUsed);
    }

    public void LoadData(GameData data)
    {
        if(data.CurrentLevelData.switchs.TryGetValue(id, out isUsed))
        {
            if (isUsed)
            {
                anim.SetTrigger("Touch");
                switchObject.SetActive(!isOpen);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (debug == false) return;

        Gizmos.color = Color.red;
        ExtensionsGizmos.Label(transform.position + Vector3.up * 2, "Trigger", Color.red);
        Gizmos.DrawWireCube(transform.position, Vector3.one);

        Gizmos.color = Color.yellow;
        ExtensionsGizmos.DrawArrow_Point(transform.position, switchObject.transform.position, 1);
    }
}
