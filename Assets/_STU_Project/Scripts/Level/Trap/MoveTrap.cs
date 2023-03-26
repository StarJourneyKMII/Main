using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTrap : MonoBehaviour
{
    [SerializeField] private Transform movePlatform;
    [SerializeField] private Transform targetAPoint;
    [SerializeField] private Transform targetBPoint;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private AnimationCurve moveCurve;

    //private Vector3 startPos;
    private Vector3 targetAPos;
    private Vector3 targetBPos;

    private bool inTargetAPos = true;

    private void Start()
    {
        //startPos = transform.position;
        targetAPos = targetAPoint.transform.position;
        targetBPos = targetBPoint.transform.position;

        float targetADistacne = Vector2.Distance(movePlatform.position, targetAPoint.position);
        float targetBDistacne = Vector2.Distance(movePlatform.position, targetBPoint.position);

        if (targetADistacne < targetBDistacne)
        {
            StartCoroutine(MoveTo(targetAPoint.transform.position));
            inTargetAPos = true;
        }
        else
        {
            StartCoroutine(MoveTo(targetBPoint.transform.position));
            inTargetAPos = false;
        }
     }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 dir = (target - movePlatform.position).normalized;
        while(Vector2.Distance(movePlatform.position, target) > 0.05f)
        {
            movePlatform.position += dir * Time.deltaTime * moveSpeed;
            yield return null;
        }
        movePlatform.position = target;

        StartCoroutine(MoveTo(inTargetAPos ? targetBPos : targetAPos));
        inTargetAPos = !inTargetAPos;
    }
}
