using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanetCtrl : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public EllipticalOrbit[] ellipticals;

    public void StopAllPlanet()
    {
        foreach(EllipticalOrbit elliptical in ellipticals)
            elliptical.orbitActive = false;
    }

    public void ContinuePlanet()
    {
        foreach (EllipticalOrbit elliptical in ellipticals)
        {
            elliptical.orbitActive = true;
            StartCoroutine(elliptical.AnimaterOrbit());
        }
    }

    public void HideCanvasGroup(Transform currentPlanet, float time)
    {
        currentPlanet.SetParent(transform);
        canvasGroup.DOFade(0, time).From(1);
    }

    public void ShowCanvasGroup(Transform currentPlanet, float time)
    {
        canvasGroup.DOFade(1, time).From(0);
        currentPlanet.SetParent(canvasGroup.transform);
    }
}
