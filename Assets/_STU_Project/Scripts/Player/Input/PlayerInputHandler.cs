using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool FlipInput { get; private set; }
    public bool InteractiveInput { get; private set; }
    public Vector2 RawLookInput { get; private set; }
    public int NormLookInputX { get; private set; }
    public int NormLookInputY { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float flipInputStartTime;
    private float interactiveInputStartTime;

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckFlipInputHoldTime();
        CheckInteractiveInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnFlipInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            FlipInput = true;
            flipInputStartTime = Time.time;
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        RawLookInput = context.ReadValue<Vector2>();

        NormLookInputX = Mathf.RoundToInt(RawLookInput.x);
        NormLookInputY = Mathf.RoundToInt(RawLookInput.y);

    }

    public void OnInteractiveInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            InteractiveInput = true;

        }
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseFlipInput() => FlipInput = false;
    public void UseInteractiveInput() => InteractiveInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
    private void CheckFlipInputHoldTime()
    {
        if (Time.time >= flipInputStartTime + inputHoldTime)
        {
            FlipInput = false;
        }
    }

    private void CheckInteractiveInputHoldTime()
    {
        if (Time.time >= interactiveInputStartTime + inputHoldTime)
        {
            InteractiveInput = false;
        }
    }
}

public enum CombatInputs
{
    primary,
    secondary
}
