using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CameraTarget : MonoBehaviour
{
    private PlayerCtrl playerCtrl;
    private Player_Look playerLook;

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float maxDistanceX = 2;
    [SerializeField] private float sensitiveX = 1;
    [SerializeField] private bool autoRecover;
    [SerializeField] private float autoRecoverSec = 1;
    private float autoRecoverTimer;

    [SerializeField] private float offsetY= 1;

    private Vector3 followOffsetVector3
    {
        get { return new Vector3(followOffsetX, followOffsetY, 0); }
    }
    private float followOffsetX = 0;
    private float followOffsetY = 0;

    private void Start()
    {
        cameraTarget.parent = null;
        playerCtrl =GetComponent<PlayerCtrl>();
        playerLook = GetComponent<Player_Look>();
        followOffsetY = transform.position.y + offsetY;
    }

    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal") * maxDistanceX;
        if (inputX != 0)
        {
            followOffsetX = Mathf.Lerp(followOffsetX, inputX, Time.deltaTime * sensitiveX);
            autoRecoverTimer = autoRecoverSec;
        }
        else if (autoRecover)
        {
            autoRecoverTimer -= Time.deltaTime;
            if (autoRecoverTimer <= 0)
            {
                followOffsetX = Mathf.Lerp(followOffsetX, 0, Time.deltaTime * sensitiveX);
            }
        }

        LimitOffsetY();
        cameraTarget.transform.position = new Vector3(transform.position.x, 0, 0) + playerLook.lookOffsetVector3 + followOffsetVector3;
    }

    private void LimitOffsetY()
    {
        float maxY = 7f;
        if (playerCtrl.isGirl)
        {
            if (transform.position.y > followOffsetY + maxY)
                followOffsetY = transform.position.y - maxY;
            else if (transform.position.y < followOffsetY - maxY)
                followOffsetY = transform.position.y + maxY;
        }
        else
        {
            if (transform.position.y < followOffsetY - maxY)
                followOffsetY = transform.position.y + maxY;
            else if (transform.position.y > followOffsetY)
                followOffsetY = transform.position.y;
        }
    }

    public void UpdateY()
    {
        followOffsetY = transform.position.y + offsetY * -playerCtrl.gravityDir;
    }
}
