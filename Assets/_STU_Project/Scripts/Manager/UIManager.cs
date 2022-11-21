using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Image hpBar;
    [SerializeField] private Image starBar;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

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
