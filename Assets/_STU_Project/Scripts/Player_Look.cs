using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Look : MonoBehaviour
{
    [SerializeField] private Vector2 xRange = new Vector2(-3f, 3f);
    [SerializeField] private Vector2 yRange = new Vector2(-5f, 3f);
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
        float inputX = Input.GetAxisRaw("LookHorizontal");
        float xOffset = 0;
        if (inputX > 0)
            xOffset = xRange.y;
        else if (inputX < 0)
            xOffset = xRange.x;
        lookOffsetX = Mathf.Lerp(lookOffsetX, xOffset, Time.deltaTime * sensitiveX);


        float inputY = Input.GetAxisRaw("LookVertical");
        float yOffset = 0;
        //if (inputY > 0)
        //{
        //    yOffset = playerCtrl.isGirl ? yRange.y : -yRange.x;
        //}
        //else if (inputY < 0)
        //{
        //    yOffset = playerCtrl.isGirl ? yRange.x : -yRange.y;
        //}
        lookOffsetY = Mathf.Lerp(lookOffsetY, yOffset, Time.deltaTime * sensitiveY);
    }
}
