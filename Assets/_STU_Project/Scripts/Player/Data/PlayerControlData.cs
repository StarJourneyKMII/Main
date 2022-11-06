using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerControlData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Look State")]
    public float lookSensitive = 6f;
    public Vector2 lookDistanceX = new Vector2(-3, 3);
    public Vector2 lookDistanceY = new Vector2(-7, 3);

    [Header("TouchGround")]
    public float touchGroundOffsetY = 3f;

    [Header("Move Look")]
    public float moveLookSensitiveX = 3f;
    public float moveLookMaxDistanceX = 2f;
    public bool autoRecovery;
    public float recoveryTime = 3f;
}
