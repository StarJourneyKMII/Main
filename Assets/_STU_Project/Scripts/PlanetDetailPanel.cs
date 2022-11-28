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
    [SerializeField] private GameObject continueButton;

    [Header("Planet")]
    [SerializeField] private Text planetNameText;
    [SerializeField] private Text planetDescriptionText;

    [Header("PlanetArea")]
    [SerializeField] private Text planetAreaNameText;
    [SerializeField] private Text planetAreaDescriptionText;
    [SerializeField] private Image collectImage;
    [SerializeField] private Image unLockImage;
    [SerializeField] private GameObject startButton;

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

        selectGroup.transform.GetChild(currentSelectData.ContinueAreaIndex).GetComponent<Toggle>().isOn = true;
        RefreshArea(currentSelectData.ContinueAreaIndex);
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
            continueButton.SetActive(true);
        });
    }

    private void RefreshArea(int index)
    {
        if(index >= currentSelectData.planetArea.Count)
        {
            planetAreaNameText.text = "未知";
            planetAreaDescriptionText.text = "未知";
            collectImage.sprite = null;
            unLockImage.gameObject.SetActive(false);
            NewGameManager.Instance.SetSelectLevel(currentSelectData, null);
        }
        else
        {
            areaIndex = index;
            PlanetAreaData data = currentSelectData.planetArea[index];
            if (!data.unLock)
            {
                unLockImage.gameObject.SetActive(true);
                startButton.SetActive(false);
            }
            else
            {
                unLockImage.gameObject.SetActive(false);
                startButton.SetActive(true);
            }
            planetAreaNameText.text = data.areaName;
            planetAreaDescriptionText.text = data.description;
            collectImage.sprite = data.spaceFrags[0].showSprite;
            NewGameManager.Instance.SetSelectLevel(currentSelectData, currentSelectData.planetArea[index]);
        }
    }

    public void Go()
    {
        SceneChangeManager.Instance.LoadSceneByName("STJ_Old_Level" + currentSelectData.planetArea[areaIndex].levelIndex);
        //GameManager.instance.scenesNumber = currentSelectData.planetArea[areaIndex].levelIndex;
        //GameManager.instance.NewGame();
    }
}
