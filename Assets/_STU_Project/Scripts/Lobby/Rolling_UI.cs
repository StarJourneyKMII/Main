using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Rolling_UI : MonoBehaviour
{
    [Header("圓圈設定")]
    public Vector3 circleCenter = Vector3.zero;
    public float circleRadius = 100;
    public float Offset_Y;

    [Header("隨著越遠變化")]
    public bool alphaChangeToggle = true;
    public Vector2 alphaChange = new Vector2(1, 0.2f);
    public bool scaleChangeToggle = false;
    public Vector2 scaleChange = new Vector2(1.3f, 0.7f);
    public bool onlySelectScaleChange = true;
    public Vector2 selectScale = new Vector2(1.5f, 0.8f);

    [Header("移動")]
    public float moveTime = 0.3f;

    public RectTransform currentSelect;
    private int _currentSelectIndex;
    private int currentSelectIndex
    {
        get { return _currentSelectIndex; }
        set { _currentSelectIndex = (int)Mathf.Repeat(value, childRects.Count); }
    }

    public List<RectTransform> childRects;
    private Dictionary<RectTransform, Vector3> rectDic = new Dictionary<RectTransform, Vector3>();

    private bool moveing;


    public Image mainIcon;
    public Image selectIcon;

    private void Start()
    {
        currentSelectIndex = childRects.IndexOf(currentSelect);
        InitPos();
    }

    [ContextMenu("Init")]
    private void InitPos()
    {
        int childCount = childRects.Count;
        float perAngle = 360 / childCount;
        int startAngle = -90;

        for (int i = 0; i < childCount; i++)
        {
            float angle = startAngle + (i * perAngle);
            float x = circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float y = 0;

            if (i > childCount / 2)
                y = Offset_Y * (childCount - i);
            else
                y = Offset_Y * i;

            Vector3 pos = circleCenter + new Vector3(x, y, z);
            childRects[i].anchoredPosition3D = pos;
            rectDic.Add(childRects[i], pos);
        }
        SetSibling();
        SetAlphaAndSacle();
    }

    private void SetSibling()
    {
        Dictionary<RectTransform, int> order = new Dictionary<RectTransform, int>();

        for (int i = 0; i < childRects.Count; i++)
        {
            float maxValue = float.MinValue;
            RectTransform thisRect = new RectTransform();
            foreach(var dic in rectDic)
            {
                if (order.ContainsKey(dic.Key)) continue;

                if (dic.Value.z > maxValue)
                {
                    maxValue = dic.Value.z;
                    thisRect = dic.Key;
                }
            }
            order.Add(thisRect, i);
        }

        foreach (var dic in order)
        {
            dic.Key.SetSiblingIndex(dic.Value);
        }
        currentSelect = childRects[currentSelectIndex];
    }

    private void SetAlphaAndSacle(bool useScale = true)
    {
        float startZValue = circleCenter.z - circleRadius;
        foreach(var dic in rectDic)
        {
            float proportion = Mathf.Abs(dic.Value.z - startZValue) / (circleRadius * 2);
            float alpha = alphaChange.x - proportion * (alphaChange.x - alphaChange.y);
            var image = dic.Key.GetComponent<Image>();
            image.DOFade(alpha, moveTime).SetEase(Ease.Linear);

            if(scaleChangeToggle == true)
            {
                float sacle = scaleChange.x - proportion * (scaleChange.x - scaleChange.y);
                dic.Key.transform.DOScale(Vector3.one * sacle, moveTime).SetEase(Ease.Linear);
            }
            if (onlySelectScaleChange == true && useScale == true)
            {
                if (proportion <= 0)
                    dic.Key.transform.DOScale(Vector3.one * selectScale.x, moveTime).SetEase(Ease.Linear);
                else
                    dic.Key.transform.DOScale(Vector3.one * selectScale.y, moveTime).SetEase(Ease.Linear);
            }
        }
    }

    public void Next()
    {
        if (moveing == true) return;

        for(int i = 0; i < rectDic.Count; i++)
        {
            int nextCount = (i + 1) % rectDic.Count;
            Vector3 nextPos = childRects[nextCount].anchoredPosition3D;
            childRects[i].DOAnchorPos3D(nextPos, moveTime).SetEase(Ease.Linear);
            rectDic[childRects[i]] = nextPos;
        }
        StartCoroutine(EnterCD(-1));
    }

    public void Last()
    {
        if (moveing == true) return;

        for (int i = 0; i < rectDic.Count; i++)
        {
            int nextCount = (i + rectDic.Count - 1) % rectDic.Count;
            Vector3 nextPos = childRects[nextCount].anchoredPosition3D;
            childRects[i].DOAnchorPos3D(nextPos, moveTime).SetEase(Ease.Linear);
            rectDic[childRects[i]] = nextPos;
        }
        StartCoroutine(EnterCD(1));
    }

    private IEnumerator EnterCD(int index)
    {
        moveing = true;
        SetAlphaAndSacle();
        yield return new WaitForSeconds(moveTime);
        currentSelectIndex += index;
        SetSibling();
        moveing = false;
    }

    public void Select()
    {
        GameManager.instance.scenesNumber = currentSelectIndex;
        Sprite currentSprite = currentSelect.GetComponent<Image>().sprite;
        mainIcon.sprite = currentSprite;
        selectIcon.sprite = currentSprite;
    }
}
