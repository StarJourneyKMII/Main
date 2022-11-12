using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Planet : MonoBehaviour
{
    public PlanetData data;
    private Button button;

    public event Action<Planet> OnClick;

    private bool canInteractable = true;

    private void Start()
    {
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

    public void StopInteraction()
    {
        canInteractable = false;
    }

    public void ResumeInteraction()
    {
        canInteractable = true;
    }

    public void Click()
    {
        if (!canInteractable) return;
        OnClick?.Invoke(this);
    }
}
