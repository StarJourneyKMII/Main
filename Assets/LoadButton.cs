using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public static LoadButton Instance;
    [SerializeField] private GameObject loadButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public void OpenLoadButton()
    {
        loadButton.SetActive(true);
    }
}
