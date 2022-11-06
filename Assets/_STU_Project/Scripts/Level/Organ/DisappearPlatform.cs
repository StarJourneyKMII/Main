using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DisappearPlatform : MonoBehaviour
{
    public SpriteRenderer platform;
    public float appearStaySec = 2f;
    public float disappearStaySec = 1f;

    public float blendSec = 0.5f;
    public AnimationCurve disappearCurve;

    private bool isDisappear = false;

    private void Start()
    {
        Switch();
    }

    private void Switch()
    {
        isDisappear = !isDisappear;
        if(isDisappear == true)
        {
            Disappear();
        }
        else
        {
            Appear();
        }
    }

    private void Disappear()
    {
        platform.DOFade(0, blendSec).SetEase(disappearCurve).OnComplete(() =>
        {
            platform.gameObject.SetActive(false);
            Invoke("Switch", disappearStaySec);
        });
    }
    private void Appear()
    {
        platform.gameObject.SetActive(true);
        platform.DOFade(1, blendSec).SetEase(disappearCurve).OnComplete(() =>
        {
            Invoke("Switch", appearStaySec);
        });
    }
}
