using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanetArea : MonoBehaviour
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
}
