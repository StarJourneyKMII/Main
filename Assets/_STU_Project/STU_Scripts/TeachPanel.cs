using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TeachPanel : MonoBehaviour
{
    private SpriteRenderer spr;

    public Transform panel;
    public Vector3 offset = Vector3.zero;
    public float durationTime = 0.5f;

    private Vector3 oriScale;
    private bool isOpening;

    public UnityAction  OnTouchEvent;

    protected virtual void Start()
    {
        oriScale = panel.localScale;
        panel.position = transform.position;
        panel.localScale = Vector3.zero;
        spr = panel.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpening == true) return;

        OpenPanel(oriScale, offset);
        StartCoroutine(ClosePanel(3));

        OnTouchEvent?.Invoke();
    }

    private IEnumerator ClosePanel(float time)
    {
        yield return new WaitForSeconds(time);
        spr.DOFade(0, 0.5f).OnComplete(() =>
        {
            isOpening = false;
        });
    }

    private void OpenPanel(Vector3 scale, Vector3 offset)
    {
        isOpening = true;
        spr.color = spr.color.GetAlphaColor(1);

        //面板縮放
        panel.DOScale(scale, durationTime).From(Vector3.zero);
        //面板移動
        panel.DOLocalMove(offset, durationTime).From(Vector3.zero);
    }
}
