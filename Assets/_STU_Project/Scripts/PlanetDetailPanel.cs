using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlanetDetailPanel : MonoBehaviourSingleton<PlanetDetailPanel>
{
    private CanvasGroup canvasGroup;
    public PlanetData currentSelectData;
    public GameObject panelGroup;
    public GameObject selectGroup;

    public Text planetNameText;
    public Text planetDescriptionText;

    public Text planetAreaNameText;
    public Text planetAreaDescriptionText;
    public Image collectImage;

    private Planet planet;
    public List<Toggle> toggles;
    public int areaIndex;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void SetPlanet(Planet planet)
    {
        this.planet = planet;
        currentSelectData = planet.data;
    }

    public void OpenDetailPanel()
    {
        panelGroup.SetActive(true);
        selectGroup.SetActive(true);
        canvasGroup.DOFade(1, 0.3f).From(0);
        RefreshPlanet();
    }
    public void CloseDetailPanel()
    {
        planet.BackToOriPos();
        canvasGroup.DOFade(0, 0.2f).From(1).OnComplete(() =>
        {
            panelGroup.SetActive(false);
            selectGroup.SetActive(false);
        });
    }

    public void RefreshPlanet()
    {
        planetNameText.text = currentSelectData.planetName;
        planetDescriptionText.text = currentSelectData.description;

        RefreshArea(currentSelectData.planetIndex);
    }

    public void RefreshArea(int index)
    {
        if(index >= currentSelectData.planetArea.Count)
        {
            planetAreaNameText.text = "未知";
            planetAreaDescriptionText.text = "未知";
            collectImage.sprite = null;
        }
        else
        {
            //toggles[index].isOn = true;
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
