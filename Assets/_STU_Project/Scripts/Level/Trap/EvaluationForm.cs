using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EvaluationForm : MonoBehaviour
{
    public GameObject panel;

    [SerializeField] private GameObject gameTimeTitle;
    [SerializeField] private Text gameTimeText;
    [SerializeField] private GameObject collectStarTitle;
    [SerializeField] private Text collectStarText;
    [SerializeField] private GameObject touchTrapTitle;
    [SerializeField] private Text touchTrapText;
    [SerializeField] private Image[] starArray;

    private float gameStartTime;
    public static int touchTrapCount;

    private void Start()
    {
        gameStartTime = Time.time;
    }

    public void Show()
    {
        panel.SetActive(true);
        float gameTime = Time.time - gameStartTime;
        float gameTimeCounter = 0;
        int collectCounter = 0;
        int touchTrapCounter = 0;
        int collectCount = PlayerCollection.Instance.CollectStarCount;
        int collectTotal = PlayerCollection.Instance.StarTotal;

        if(collectCount / (float)collectTotal >= 1)
        {
            starArray[0].color = Color.white;
            starArray[1].color = Color.white;
            starArray[2].color = Color.white;
        }
        else if(collectCount / (float)collectTotal >= 2 / 3f)
        {
            starArray[0].color = Color.white;
            starArray[1].color = Color.white;
        }
        else if(collectCount / (float)collectTotal >= 1 / 3f)
        {
            starArray[0].color = Color.white;
        }

        DOTween.To(() => gameTimeCounter, x => gameTimeCounter = x, gameTime, 1).SetEase(Ease.Linear).OnUpdate(() =>{
            gameTimeText.text = gameTimeCounter.ToString("0.00");
        });
        DOTween.To(() => collectCounter, x => collectCounter = x, collectCount, 1).SetEase(Ease.Linear).OnUpdate(() => {
            collectStarText.text = collectCounter + "/" + collectTotal.ToString();
        });
        DOTween.To(() => touchTrapCounter, x => touchTrapCounter = x, touchTrapCount, 1).SetEase(Ease.Linear).OnUpdate(() => {
            touchTrapText.text = touchTrapCounter.ToString();
        });
    }
}
