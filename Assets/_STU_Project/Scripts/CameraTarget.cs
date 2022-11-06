using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform player;
    private Player playerScript;
    [SerializeField] private PlayerControlData playerData;

    private float touchGroundY;

    private float moveInput;
    private float moveOffsetX;

    private Vector2 lookInput;
    private Vector2 lookOffset;

    private float moveInputStartTime;

    private bool canFollowPlayer = true;

    private void Start()
    {
        transform.parent = null;
        player = GameObject.FindWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        lookOffset = Vector3.Lerp(lookOffset, lookInput, Time.deltaTime * playerData.lookSensitive);

        if (canFollowPlayer)
            FollowPlayerPositionX();

        if(playerData.autoRecovery)
        {
            if(Time.time >= moveInputStartTime + playerData.recoveryTime)
            {
                moveOffsetX = Mathf.Lerp(moveOffsetX, 0, Time.deltaTime * playerData.moveLookSensitiveX);
            }
        }
    }

    private void FollowPlayerPositionX()
    {
        float x = player.position.x + lookOffset.x + moveOffsetX;
        float y = touchGroundY + playerData.touchGroundOffsetY * playerScript.CurrentSex + lookOffset.y;
        transform.position = new Vector3(x, y, 0);
    }

    public void SetLookInput(Vector3 pos)
    {
        lookInput = pos;
    }
    public void SetTouchGroundY(float value)
    {
        touchGroundY = Mathf.Round(value);
    }
    public void SetMoveInput(float value)
    {
        moveInputStartTime= Time.time;
        moveInput = value;
        moveOffsetX = Mathf.Lerp(moveOffsetX, moveInput, Time.deltaTime * playerData.moveLookSensitiveX);
    }

    public void StopFollowPlayer()
    {
        canFollowPlayer = false;
    }
    public void StartFollowPlayer()
    {
        canFollowPlayer = true;
    }
    public void SetFollowTargetPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
