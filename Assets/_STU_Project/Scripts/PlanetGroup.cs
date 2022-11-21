using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlanetGroup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public Planet currentSelectPlanet;

    private Planet[] planets;
    private EllipticalOrbit[] ellipticals;

    public event Action<PlanetData> OnFocusEnd;

    private Vector2 oriPos;

    private void Start()
    {
        planets = GetComponentsInChildren<Planet>();
        ellipticals = GetComponentsInChildren<EllipticalOrbit>();

        foreach (Planet planet in planets)
            planet.OnClick += HandleClickPlanet;
    }

    private void HandleClickPlanet(Planet planet)
    {
        currentSelectPlanet = planet;
        StopAllPlanetMove();

        float time = 0.3f;
        planet.transform.SetParent(transform);
        canvasGroup.DOFade(0, time).From(1);

        RectTransform rect = planet.GetComponent<RectTransform>();
        Vector2 point = new Vector2(-450, 0);
        Vector2 size = new Vector2(650, 650);
        oriPos = rect.anchoredPosition;
        rect.DOSizeDelta(size, time);
        rect.DOAnchorPos(point, time).OnComplete(() =>
        {
            OnFocusEnd?.Invoke(currentSelectPlanet.data);
        });
    }


    public void PlanetBack()
    {
        float time = 0.3f;
        currentSelectPlanet.transform.SetParent(canvasGroup.transform);
        canvasGroup.DOFade(1, time).From(0);

        RectTransform rect = currentSelectPlanet.GetComponent<RectTransform>();
        rect.DOAnchorPos(oriPos, time);
        rect.DOSizeDelta(new Vector2(100, 100), time).OnComplete(() =>
        {
            StartPlanetMove();
            currentSelectPlanet = null;
        });
    }

    private void StopAllPlanetMove()
    {
        foreach (EllipticalOrbit elliptical in ellipticals)
            elliptical.StopMove();
        foreach(Planet planet in planets)
            planet.StopInteraction();
    }

    private void StartPlanetMove()
    {
        foreach (EllipticalOrbit elliptical in ellipticals)
            elliptical.StartMove();
        foreach (Planet planet in planets)
            planet.ResumeInteraction();
    }

    public void SelectContinueLevel()
    {
        foreach(Planet planet in planets)
        {
            if (planet.data.IsAllClear)
                continue;

        }
    }
}
