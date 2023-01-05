using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTrap : MonoBehaviour
{
    [SerializeField] private Transform movePlatform;
    [SerializeField] private Transform targetPoint;

    [SerializeField] private float moveTime = 2f;
    [SerializeField] private AnimationCurve moveCurve;

    private Vector3 startPos;
    private Vector3 targetPos;

    private bool inStartPos = true;

    private void Start()
    {
        startPos = transform.position;
        targetPos = targetPoint.transform.position;

        StartCoroutine(MoveTo(targetPos));
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 startPos = movePlatform.position;
        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime / moveTime;
            movePlatform.position = Vector3.Lerp(startPos, target, moveCurve.Evaluate(timer));
            yield return null;
        }
        movePlatform.position = target;
        inStartPos = !inStartPos;

        StartCoroutine(MoveTo(inStartPos ? targetPos : this.startPos));
    }
}
