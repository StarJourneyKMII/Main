using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanel : MonoBehaviourSingleton<DetailPanel>
{
    public GameObject showPanel;
    public Image detailImage;
    public Text detailTitleText;
    public Text detailDescriptionText;

    public void Show(GameObject card, BaseData data)
    {
        if(data == null)
        {
            Debug.Log("暫無詳細資料 : " + card.name);
            return;
        }

        showPanel.SetActive(true);
        detailImage.sprite = data.detailSprite;
        detailTitleText.text = data.detailName;
        detailDescriptionText.text = data.description;
    }
}
