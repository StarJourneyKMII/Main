using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlanetGroup : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private CanvasGroup canvasGroup;

    public Planet currentSelectPlanet;

    private Planet[] planets;
    private EllipticalOrbit[] ellipticals;

    public event Action<PlanetData> OnFocusEnd;

    private Vector2 oriPos;
    private PlanetsDataBase planetsData;

    private void Start()
    {
        planetsData = Resources.Load<PlanetsDataBase>("Data_¬P²y/PlanetsDataBase");
        planets = GetComponentsInChildren<Planet>();
        ellipticals = GetComponentsInChildren<EllipticalOrbit>();

        foreach (Planet planet in planets)
            planet.OnClick += HandleClickPlanet;
    }

    public void RefreshAllPlanet()
    {
        foreach(Planet planet in planets)
        {
            planet.Refresh();
        }
    }

    private void HandleClickPlanet(Planet planet)
    {
        continueButton.SetActive(false);
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
            if (planet.data == planetsData.GetContinuePlanetData())
            {
                HandleClickPlanet(planet);
                break;
            }
        }
    }

    private void OnEnable()
    {
        TestGM.Instance.OnPlanetDataChange += RefreshAllPlanet;
    }

    private void OnDisable()
    {
        if (TestGM.Instance != null)
            TestGM.Instance.OnPlanetDataChange -= RefreshAllPlanet;
    }
}
