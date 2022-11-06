using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerFlipHorizontalState FlipHorizontalState { get; private set; }
    public PlayerLookState LookState { get; private set; }

    [SerializeField]
    private PlayerControlData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    private SpriteRenderer spr;

    private Animator boyAnim;
    private Animator girlAnim;

    private CameraTarget cameraTarget;
    #endregion

    #region Other Variables         

    private Vector2 workspace;

    public int CurrentSex { get; private set; }
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        FlipHorizontalState = new PlayerFlipHorizontalState(this, StateMachine, playerData, "flip");
        LookState = new PlayerLookState(this, StateMachine, playerData, "idle");
    }

    private void Start()
    {
        cameraTarget = FindObjectOfType<CameraTarget>();
        boyAnim = transform.Find("Boy").GetComponent<Animator>();
        girlAnim = transform.Find("Girl").GetComponent<Animator>();
        Anim = girlAnim;
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        InputHandler = GetComponent<PlayerInputHandler>();
        MovementCollider = GetComponent<BoxCollider2D>();
        RB = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        CurrentSex = 1;

        SetTouchGroundY();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
        if (Input.GetKeyDown(KeyCode.B))
            Core.GetCoreComponent<Stats>().DecreaseHealth(1);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Other Functions
    public void Hide()
    {
        spr.enabled = false;
    }
    public void Show()
    {
        spr.enabled = true;
    }
    public void SetTouchGroundY()
    {
        cameraTarget.SetTouchGroundY(transform.position.y);
    }
    public void SetLookInput(Vector2 input)
    {
        cameraTarget.SetLookInput(input);
    }
    public void SetMoveInputX(float inputX)
    {
        cameraTarget.SetMoveInput(inputX);
    }
    public void FlipHorizontal()
    {
        FlipSex();
        transform.Rotate(180, 0, 0);
        transform.position += (Vector3)(CurrentSex * Vector2.up * 2.8f);
        SetTouchGroundY();
    }
    public void FlipSex()
    {
        CurrentSex *= -1;
        if(CurrentSex == 1)
        {
            Anim = girlAnim;
            girlAnim.gameObject.SetActive(true);
            boyAnim.gameObject.SetActive(false);
        }
        else if(CurrentSex == -1)
        {
            Anim = boyAnim;
            boyAnim.gameObject.SetActive(true);
            girlAnim.gameObject.SetActive(false);
        }
        Physics2D.gravity *= -1;
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    public void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

   
    #endregion
}
