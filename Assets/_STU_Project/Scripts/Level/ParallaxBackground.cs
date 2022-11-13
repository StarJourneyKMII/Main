using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    
    [SerializeField]
    private float _parallaxFactor = 0;

    private Transform _mainCamera = null;
    private Vector3 _camStartPos = Vector3.zero;
    void Start()
    {
        _mainCamera = Camera.main.transform;
        _camStartPos = _mainCamera.position;
    }

    void Update()
    {
        Vector3 distance = _mainCamera.position - _camStartPos;

        transform.position = new Vector3(_camStartPos.x + distance.x * _parallaxFactor,_camStartPos.y + distance.y * _parallaxFactor,transform.position.z);

    }
}
