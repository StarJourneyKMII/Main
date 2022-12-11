using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Planet : MonoBehaviour, IData
{
    private Image image;
    private Button button;

    public PlanetData data;

    public event Action<Planet> OnClick;

    private bool canInteractable = true;

    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        Refresh();
    }

    public void Refresh()
    {
        button.interactable = data.unLock;
        image.sprite = data.Image;
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

    public void SaveData(ref GameData data)
    {
        if (data.planetUnlockData.ContainsKey(this.data))
        {
            data.planetUnlockData.Remove(this.data);
        }
        data.planetUnlockData.Add(this.data, this.data.unLock);

        foreach (PlanetAreaData areaData in this.data.planetArea)
        {
            if (data.planetAreaUnlockData.ContainsKey(areaData))
            {
                data.planetAreaUnlockData.Remove(areaData);
            }
            data.planetAreaUnlockData.Add(areaData, areaData.unLock);
        }
    }

    public void LoadData(GameData data)
    {
        if (data.planetUnlockData.TryGetValue(this.data, out bool unLock))
        {
            this.data.unLock = unLock;
        }

        foreach (PlanetAreaData areaData in this.data.planetArea)
        {
            if(data.planetAreaUnlockData.TryGetValue(areaData, out bool areaUnLock))
            {
                areaData.unLock = areaUnLock;
            }
        }
    }
}
