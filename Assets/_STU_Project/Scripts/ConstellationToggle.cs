using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationToggle : MonoBehaviour
{
    private Toggle toggle;
    private Image image;
    private Text text;
    private int pageNum;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        pageNum = (transform.GetSiblingIndex() + 1) * 2;
    }

    public void OnClick()
    {
        if(toggle.isOn == true)
        {
            text.color = Color.yellow;
            text.fontSize = 45;
            text.fontStyle = FontStyle.Bold;
            FindObjectOfType<AutoFlip>().FlipToPage(pageNum);
        }
        else
        {
            text.color = Color.white;
            text.fontSize = 35;
            text.fontStyle = FontStyle.Normal;
        }
    }
}
