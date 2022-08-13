using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetArea : MonoBehaviour
{
    public int areaIndex;

    public void Click()
    {
        PlanetDetailPanel.Instance.RefreshArea(areaIndex);
    }
}
