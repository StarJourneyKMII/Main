using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovePlatform : MonoBehaviour
{
    public Transform platform;
    public Transform targetA;
    public Transform targetB;
    public float moveSec = 1.5f;
    public float waitSec = 2f;
    public AnimationCurve moveCurve;

    private bool currentOnA = true;

    private void Start()
    {
        Move();
    }

    private void Move()
    {
        Vector3 targetPoint = currentOnA ? targetB.position : targetA.position;
        platform.DOMove(targetPoint, moveSec).SetEase(moveCurve).OnComplete(() =>
        {
            currentOnA = !currentOnA;
            Invoke("Move", waitSec);
        });
    }

    [ContextMenu("ResetPlatform")]
    public void ResetPlatformToTargetA()
    {
        platform.position = targetA.position;
    }

}
