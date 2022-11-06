using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationUI : MonoBehaviour
{
    public ConstellationData data;
    private Image showImage;

    private void Start()
    {
        showImage = GetComponent<Image>();
        if(data != null)
        {
            RefreshUI();
        }
    }

    public void Click()
    {
        DetailPanel.Instance.Show(gameObject, data);
    }

    public void RefreshUI()
    {
        showImage.sprite = data.unlock ? data.unlockSprite : data.lockSprite;
    }
}
