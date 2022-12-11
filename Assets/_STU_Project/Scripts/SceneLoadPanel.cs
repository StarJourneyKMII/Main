using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneLoadPanel : MonoBehaviour
{
    [SerializeField] private GameObject loadPanel;

    [SerializeField] private Image loadImage;
    [SerializeField] private Image progressImage;

    [SerializeField] private TMP_Text hintText;
    [SerializeField] private TMP_Text progressText;

    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
    }

    public void CloseLoadPanel()
    {
        loadPanel?.SetActive(false);
    }

    public void SetProgress(float value)
    {
        progressImage.fillAmount = value / 100;
        progressText.text = value.ToString("0.00") + "%";
    }
}
