using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;
using DG.Tweening;

public class NewPlanetSelect : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private PathCreator creator;
    [SerializeField] private RectTransform[] planetArray;
    [SerializeField] private Button[] planetButtonArray;

    [SerializeField] private Vector2 startSize;
    [SerializeField] private Vector2 decreseSize;

    [SerializeField] private RectTransform startPos;

    [SerializeField] private float moveTime = 2f;
    [SerializeField] private NewPlanetDetailPanel planetDetail;

    [SerializeField] private GameObject[] enterObjArray;
    [SerializeField] private GameObject[] exitObjArray;

    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> sizes = new List<Vector2>();

    private int currentIndex = 0;
    private int selectIndex = 0;

    private bool isFocusing;
    private bool isFocus;
    private bool isMoving;
    private Planet planet;

    [ContextMenu("test")]
    public void Test()
    {
        
        for (int i = 0; i < planetArray.Length; i++)
        {
            planetArray[i].anchoredPosition = creator.path.GetPoint(i) - transform.position;
            planetArray[i].sizeDelta = startSize - decreseSize * i;
        }
    }

    private void Start()
    {
        points.Add(startPos.anchoredPosition);
        foreach(var planet in planetArray)
        {
            points.Add(planet.anchoredPosition);
            sizes.Add(planet.sizeDelta);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            GoToNext(1);
            Debug.Log("Test");
        }
    }

    public void Select(int index, Planet planet)
    {
        this.planet = planet;
        if(index == currentIndex)
        {
            Focus();
        }
        if (isMoving || index == currentIndex) return;

        selectIndex = index;
        float perTime = moveTime / (selectIndex - currentIndex);
        GoToNext(perTime);
    }

    private void GoToNext(float time)
    {
        StartCoroutine(DoGoToNext(time));
    }

    public void CancelFocus()
    {
        if(isFocus && !isFocusing)
        {
            isFocusing = true;
            planetDetail.gameObject.SetActive(false);
            root.DOScale(Vector3.one, 0.3f).OnComplete(()=>{
                isFocusing = false;
                isFocus = false;
            });

        }
    }

    private IEnumerator DoGoToNext(float time)
    {
        isMoving = true;
        for (int i = planetArray.Length - 1; i > planetArray.Length - 11; i--)
        {
            RectTransform rect = root.GetChild(i).GetComponent<RectTransform>();
            int index = planetArray.Length - i - 1;

            if(i == planetArray.Length - 1)
            {
                rect.GetComponent<Image>().DOFade(0, time / 2).From(1);
            }
            else if(i == planetArray.Length - 9)
            {
                rect.GetComponent<Image>().DOFade(1, time).From(0);
                rect.gameObject.SetActive(true);
            }
            rect.DOAnchorPos(points[index], time).SetEase(Ease.Linear);
            rect.DOSizeDelta(sizes[index], time).SetEase(Ease.Linear);
        }
        yield return new WaitForSeconds(time);

        RectTransform last = root.GetChild(root.childCount - 2).GetComponent<RectTransform>();
        last.SetAsFirstSibling();
        last.gameObject.SetActive(false);
        last.anchoredPosition = points[points.Count - 1];
        last.sizeDelta = sizes[sizes.Count - 1];

        if (currentIndex + 1 != selectIndex)
        {
            currentIndex++;
            StartCoroutine(DoGoToNext(time));
        }
        else
        {
            currentIndex = 0;
            selectIndex = 0;
            isMoving = false;
        }
        if(!isMoving)
        {
            Focus();
        }
    }
    private void Focus()
    {
        if(!isFocus)
        {
            isFocusing = true;
            root.DOScale(Vector3.one * 0.8f, 0.3f).OnComplete(()=>{
                planetDetail.gameObject.SetActive(true);
                planetDetail.SetInfo(planet);
                isFocusing = false;
            });
            isFocus = true;
        }
        else
        {
            planetDetail.SetInfo(planet);
        }
    }

    public void EnterSelectPlanet()
    {
        foreach(GameObject exitObj in exitObjArray)
        {   
            exitObj.SetActive(false);
        }
        root.DOScale(Vector3.one, 0.3f).OnComplete(()=>{
            foreach(var child in planetButtonArray)
            {
                child.interactable = true;
            }
            foreach(GameObject enterObj in enterObjArray)
            {
                enterObj.SetActive(true);
            }
        });
    }
    public void ExitSelectPlanet()
    {
        if(isMoving || isFocusing) return;

        foreach(GameObject enterObj in enterObjArray)
        {
            enterObj.SetActive(false);
        }
        foreach(var child in planetButtonArray)
        {
            child.interactable = false;
        }
        root.DOScale(Vector3.one * 0.7f, 0.3f).OnComplete(()=>{
            foreach(GameObject exitObj in exitObjArray)
            {   
                exitObj.SetActive(true);
            }
        });

        if(isFocus)
        {
            planetDetail.gameObject.SetActive(false);
            isFocus = false;
        }
    }
}
