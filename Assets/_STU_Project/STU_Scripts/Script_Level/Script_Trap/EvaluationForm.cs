using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EvaluationForm : MonoBehaviour
{
    private static EvaluationForm _instance = null;

    public static EvaluationForm Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EvaluationForm>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(EvaluationForm).Name);
                    _instance = obj.AddComponent<EvaluationForm>();
                }
            }
            return _instance;
        }
        set { }
    }

    public GameObject panel;

    public GameObject gameTimeTitle;
    public Text gameTimeText;
    public GameObject collectStarTitle;
    public Text collectStarText;
    public GameObject touchTrapTitle;
    public Text touchTrapText;

    private float oriTime;
    public int collectTotal;
    public int collectCount;
    public static int touchTrapCount;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        oriTime = Time.time;
    }

    public void Show()
    {
        panel.SetActive(true);
        float gameTime = Time.time - oriTime;
        float gameTimeCounter = 0;
        int collectCounter = 0;
        int touchTrapCounter = 0;
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
