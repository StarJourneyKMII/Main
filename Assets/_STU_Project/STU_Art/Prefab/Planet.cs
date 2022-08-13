using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Planet : MonoBehaviour
{
    private RectTransform rect;
    private Image image;
    private Button button;

    public PlanetData data;

    private Vector2 oriPos;
    private Vector2 stopPos;
    private Vector2 oriSize;
    //private int oriChildIndex;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        Refresh();
    }

    public void Refresh()
    {
        if (data.unlock == false)
        {
            button.interactable = false;
            //image.color = new Color32(100, 100, 100, 255);
        }
    }

    public void Click()
    {
        PlanetCtrl.Instance.StopAllPlanet();
        FocusAnimation();
        PlanetDetailPanel.Instance.SetPlanet(this);
        oriPos = rect.anchoredPosition;
    }

    private void FocusAnimation()
    {
        float time = 0.3f;
        PlanetCtrl.Instance.HideCanvasGroup(transform, time);
        Vector2 point = new Vector2(-450, 0);
        Vector2 size = new Vector2(650, 650);
        rect.DOSizeDelta(size, time);
        rect.DOAnchorPos(point, time).OnComplete(() =>
        {
            PlanetDetailPanel.Instance.OpenDetailPanel();
        });
    }

    public void BackToOriPos()
    {
        float time = 0.3f;
        PlanetCtrl.Instance.ShowCanvasGroup(transform, time);
        rect.DOAnchorPos(oriPos, time);
        rect.DOSizeDelta(new Vector2(100, 100), time).OnComplete(() =>
        {
            PlanetCtrl.Instance.ContinuePlanet();
        });
    }
}
