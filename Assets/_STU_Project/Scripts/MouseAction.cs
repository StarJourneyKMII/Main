using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent leftClick;

    void Start()
    {
        leftClick.AddListener(new UnityAction(ButtonLeftClick));
    }

    private void ButtonLeftClick() 
    {
        Debug.Log("Button Left Click");
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        this.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        this.transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("孩子你哪位? 關閉了: " + this.transform.GetChild(0).gameObject.name);
        this.transform.GetChild(1).gameObject.SetActive(true);
        Debug.Log("孩子你哪位? 閃耀吧!: " + this.transform.GetChild(1).gameObject.name);
        this.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("回復吧!!" + this.gameObject.name);
        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        this.transform.GetChild(1).gameObject.SetActive(false);
        Debug.Log("孩子你哪位? 關閉了: " + this.transform.GetChild(1).gameObject.name);
        this.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("孩子你哪位? 回復吧!: " + this.transform.GetChild(0).gameObject.name);
        this.transform.GetChild(2).gameObject.SetActive(false);
    }

    GameObject selectGO;

    private void OnMouseDown()
    {
        Debug.Log("Mouse");
        if (Input.GetMouseButtonDown(0))
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            // Debug.Log("孩子你哪位? 關閉了: " + this.transform.GetChild(0).gameObject.name);
            this.transform.GetChild(2).gameObject.SetActive(true);
            // Debug.Log("孩子你哪位? 閃耀吧!: " + this.transform.GetChild(2).gameObject.name);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) 
            {
                selectGO = hit.collider.gameObject; // get GO
                Debug.Log(selectGO.name);
            }
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            Debug.Log("孩子你哪位? 關閉了: " + this.transform.GetChild(0).gameObject.name);
            this.transform.GetChild(2).gameObject.SetActive(true);
            Debug.Log("孩子你哪位? 閃耀吧!: " + this.transform.GetChild(2).gameObject.name);
        }
        */
    }
}
