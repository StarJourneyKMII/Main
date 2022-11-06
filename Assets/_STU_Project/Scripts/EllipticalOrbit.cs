using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EllipticalOrbitMode{ Clockwise, AnitClockwise }
public class EllipticalOrbit : MonoBehaviour
{
    public RectTransform rect;
    public RectTransform center;

    public Ellipse ellipse;

    public EllipticalOrbitMode mode = EllipticalOrbitMode.Clockwise;
    [Range(0f, 1f)]
    public float orbitProgress = 0;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;

    [Header("Debug")]
    public bool debug = true;
    [Range(3, 36)]
    public int segments = 15;

    private void OnEnable()
    {
        SetPosition();
        StartCoroutine(AnimaterOrbit());
    }
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        orbitProgress = Random.Range(0, 1f);
    }

    private void SetPosition()
    {
        float tempProgress = orbitProgress;
        if (mode == EllipticalOrbitMode.Clockwise)
            tempProgress = Mathf.Abs(orbitProgress) * -1;
        Vector2 orbitPos = ellipse.Evaluate(tempProgress);
        rect.anchoredPosition = orbitPos;
    }

    public IEnumerator AnimaterOrbit()
    {
        float orbitSpeed = 1f / orbitPeriod;
        while(orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f;
            SetPosition();
            yield return null;
        }
    }

    public void ResetProgress(float time)
    {
        DOTween.To(() => orbitProgress, x => orbitProgress = x, 0, time).OnUpdate(() =>
        {
            SetPosition();
        });
    }

    private void OnValidate()
    {
        SetPosition();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDrawGizmos()
    {
        if(debug == false || center == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < segments; i++)
        {
            Vector2 positon2D =(Vector2)center.transform.position + ellipse.Evaluate(i / (float)segments);
            Vector2 nextPositon2D = (Vector2)center.transform.position + ellipse.Evaluate((i + 1) / (float)segments);
            Gizmos.DrawLine(positon2D, nextPositon2D);
        }
        
    }
}
