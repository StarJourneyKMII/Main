using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpBar : MonoBehaviour
{
    void Start()
    {
        // 我剛出現! 訂閱血量更新
        GameManager.instance.Act_HpChange += UpdateHpBar;
    }

    private void OnDestroy()
    {
        // 當我掛了的時候! 順手退訂血量更新
        GameManager.instance.Act_HpChange -= UpdateHpBar;
    }

    [SerializeField] Image bar = null;
    void UpdateHpBar(float input)
    {
        bar.fillAmount = input;
    }
}



