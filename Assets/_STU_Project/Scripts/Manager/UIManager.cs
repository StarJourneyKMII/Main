using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private Image hpBar;
    [SerializeField] private Image starBar;

    private void Start()
    {
        RefreshStarBar();
        RefreshHpBar();
    }

    public void RefreshHpBar()
    {

    }

    public void RefreshStarBar()
    {
        if (PlayerCollection.Instance.StarTotal == 0) return;
        starBar.fillAmount = (float)PlayerCollection.Instance.CollectStarCount / PlayerCollection.Instance.StarTotal;
    }
}
