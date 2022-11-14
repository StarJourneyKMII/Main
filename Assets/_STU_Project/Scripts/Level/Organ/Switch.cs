using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MiProduction.BroAudio;

public class Switch : MonoBehaviour, IData
{
    private Animator anim;

    [SerializeField] private GameObject focusCamera;
    [SerializeField] private GameObject switchObject;

    [SerializeField] private bool debug;
    [SerializeField] private float switchBlendSec = 0.5f;

    [SerializeField] private bool isOpen;
    private bool isUsed;
    private string id;

    private SpriteRenderer[] switchsSpriteRenderer;

    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        anim = GetComponentInChildren<Animator>();
        focusCamera.transform.position = new Vector3(switchObject.transform.position.x,switchObject.transform.position.y,Camera.main.transform.position.z);
        switchObject.SetActive(isOpen);
        switchsSpriteRenderer = switchObject.GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            SoundSystem.PlaySFX(Sound.Switch);
            anim.SetTrigger("Touch");
            if (isOpen)
                StartCoroutine(Close());
            else
                StartCoroutine(Open());
            isUsed = true;
        }
    }

    private IEnumerator Open()
    {
        focusCamera.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        switchObject.SetActive(true);
        foreach(SpriteRenderer spriteRenderer in switchsSpriteRenderer)
        {
            spriteRenderer.DOFade(1, switchBlendSec).From(0);
        }
        yield return new WaitForSeconds(switchBlendSec);
        focusCamera.SetActive(false);
    }

    private IEnumerator Close()
    {
        focusCamera.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        foreach(SpriteRenderer spriteRenderer in switchsSpriteRenderer)
        {
            spriteRenderer.DOFade(0, switchBlendSec).From(1);
        }
        yield return new WaitForSeconds(switchBlendSec);
        switchObject.SetActive(false);
        focusCamera.SetActive(false);
    }

    public void LookTarget()
    {
        StartCoroutine(DoLookTarget());
    }

    private IEnumerator DoLookTarget()
    {
        focusCamera.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        focusCamera.SetActive(false);
    }

    public void SaveData(ref GameData data)
    {
        if (data.switchUsed.ContainsKey(id))
        {
            data.switchUsed.Remove(id);
        }
        data.switchUsed.Add(id, isUsed);
    }

    public void LoadData(GameData data)
    {
        data.switchUsed.TryGetValue(id, out isUsed);
        if(isUsed)
        {
            anim.SetTrigger("Touch");
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
