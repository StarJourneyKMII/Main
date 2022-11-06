using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpacePartsUI : MonoBehaviour
{
    public SpacePartsData data;
    public Image showImage;

    private void Start()
    {
        if(data != null)
        {
            RefreshUI();
        }    
    }

    public void Click()
    {
        if (data == null) return;

        FragPanel.Instance.ShowDetail(data);
    }

    [ContextMenu("RefreshUI")]
    public void RefreshUI()
    {
        showImage.sprite = data.showSprite;
    }
}
