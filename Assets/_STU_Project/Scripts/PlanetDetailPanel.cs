using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlanetDetailPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private PlanetGroup planetGroup;
    [SerializeField] private GameObject panelGroup;
    [SerializeField] private GameObject selectGroup;

    [Header("Planet")]
    [SerializeField] private Text planetNameText;
    [SerializeField] private Text planetDescriptionText;

    [Header("PlanetArea")]
    [SerializeField] private Text planetAreaNameText;
    [SerializeField] private Text planetAreaDescriptionText;
    [SerializeField] private Image collectImage;

    private PlanetArea[] areas;

    private PlanetData currentSelectData;
    private int areaIndex;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        areas = GetComponentsInChildren<PlanetArea>();

        planetGroup.OnFocusEnd += HandleFocusEnd;

        foreach (PlanetArea area in areas)
            area.OnClick += HandleOnAreaClick;

        panelGroup.SetActive(false);
        selectGroup.SetActive(false);
    }
    
    private void HandleOnAreaClick(int index)
    {
        RefreshArea(index);
    }

    private void HandleFocusEnd(PlanetData planet)
    {
        currentSelectData = planet;

        panelGroup.SetActive(true);
        selectGroup.SetActive(true);
        canvasGroup.DOFade(1, 0.3f).From(0);

        planetNameText.text = currentSelectData.planetName;
        planetDescriptionText.text = currentSelectData.description;

        RefreshArea(currentSelectData.planetIndex);
    }

    public void CloseDetailPanel()
    {
        planetGroup.PlanetBack();
        currentSelectData = null;
        areaIndex = -1;
        canvasGroup.DOFade(0, 0.2f).From(1).OnComplete(() =>
        {
            panelGroup.SetActive(false);
            selectGroup.SetActive(false);
        });
    }

    private void RefreshArea(int index)
    {
        if(index >= currentSelectData.planetArea.Count)
        {
            planetAreaNameText.text = "未知";
            planetAreaDescriptionText.text = "未知";
            collectImage.sprite = null;
        }
        else
        {
            areaIndex = index;
            PlanetAreaData data = currentSelectData.planetArea[index];
            planetAreaNameText.text = data.areaName;
            planetAreaDescriptionText.text = data.description;
            collectImage.sprite = data.spaceFrags[0].showSprite;
        }
    }

    public void Go()
    {
        GameManager.instance.scenesNumber = currentSelectData.levelIndex * 4 + areaIndex;
        GameManager.instance.NewGame();
    }
}
