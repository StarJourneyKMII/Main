using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConstellationScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform element;
    private HorizontalLayoutGroup contentHLG;
    private float _scrollX = 0;
    private float scrollX
    {
        get { return _scrollX; }
        set
        {
            if(value < 0f)
                value = 0;
            else if(value > 1)
                value = 1;
            _scrollX = value;
        }
    }
    public int perNum = 3;
    public float scrollSec = 1f;

    private float elementSizeXNormalized
    {
        get { return (element.sizeDelta.x + contentHLG.spacing) / content.sizeDelta.x; }
    }
    private Tween scrollTween;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentHLG = content.GetComponent<HorizontalLayoutGroup>();
    }

    public void ScrollNext()
    {
        scrollX += elementSizeXNormalized * perNum;
        scrollTween?.Kill();
        scrollTween = scrollRect.DOHorizontalNormalizedPos(scrollX, scrollSec);
    }
    public void ScrollLast()
    {
        scrollX -= elementSizeXNormalized * perNum;
        scrollTween?.Kill();
        scrollTween = scrollRect.DOHorizontalNormalizedPos(scrollX, scrollSec);
    }
}
