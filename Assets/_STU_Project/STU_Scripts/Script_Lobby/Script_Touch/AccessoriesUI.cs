using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoriesUI : MonoBehaviour
{
    public AccessoriesData data;
    public Image showImage;

    private void Start()
    {
        if (data != null)
        {
            RefreshUI();
        }
    }
    public void Click()
    {
        if (data.unlock == false) return;

        DetailPanel.Instance.Show(gameObject, data);
    }

    public void RefreshUI()
    {
        showImage.sprite = data.unlock ? data.unlockSprite : data.lockSprite;
    }
}
