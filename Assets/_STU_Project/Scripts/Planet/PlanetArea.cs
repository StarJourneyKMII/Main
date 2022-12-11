using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanetArea : MonoBehaviour, IData
{
    public int areaIndex;

    public event Action<int> OnClick;

    private void Start()
    {
        areaIndex = transform.GetSiblingIndex();
    }

    public void Click()
    {
        OnClick?.Invoke(areaIndex);
    }

    public void SaveData(ref GameData data)
    {
        
    }

    public void LoadData(GameData data)
    {
        
    }
}
