using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomConstellation : MonoBehaviour
{
    public Image icon;

    public List<ConstellationData> datas;
    public ConstellationTouch constellation;

    public void RandomSprite()
    {
        int r = Random.Range(0, datas.Count);
        while(r == datas.IndexOf(constellation.data))
        {
            r = Random.Range(0, datas.Count);
        }
        icon.sprite = datas[r].detailSprite;
        constellation.data = datas[r];
    }
}
