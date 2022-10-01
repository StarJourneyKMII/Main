using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Look : MonoBehaviour
{
    [SerializeField] private float maxDistanceX = 3f;
    [SerializeField] private float maxDistanceY = 3f;
    [SerializeField] private float sensitiveX = 10f;
    [SerializeField] private float sensitiveY = 10f;

    private float lookOffsetX;
    private float lookOffsetY;

    public Vector3 lookOffsetVector3
    {
        get { return new Vector3(lookOffsetX, lookOffsetY, 0); }
    }

    private void Update()
    {


        float inputX = Input.GetAxisRaw("LookHorizontal") * maxDistanceX;
        lookOffsetX = Mathf.Lerp(lookOffsetX, inputX, Time.deltaTime * sensitiveX);

        float inputY = Input.GetAxisRaw("LookVertical") * maxDistanceY;
        lookOffsetY = Mathf.Lerp(lookOffsetY, inputY, Time.deltaTime * sensitiveY);
    }
}
