using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum PlayerSex 
{
    Girl = 1, 
    Boy = -1
}

public class Player : MonoBehaviour, IData
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
    public PlayerInteractiveState InteractiveState { get; private set; }

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

    private Animator boyAnim;
    private Animator girlAnim;

    private CameraTarget cameraTarget;
    #endregion

    #region Other Variables         

    private Vector2 workspace;

    public PlayerSex CurrentSex = PlayerSex.Girl;

    private Switch switchInteractive;
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
        InteractiveState = new PlayerInteractiveState(this, StateMachine, playerData, "idle");
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

        CurrentSex = PlayerSex.Girl;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Switch"))
        {
            switchInteractive = collision.GetComponent<Switch>();
        }

        if(collision.CompareTag("Damage2"))
        {
            Core.GetCoreComponent<Stats>().DecreaseHealth(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            switchInteractive = null;
        }
    }
    #endregion

    #region Other Functions
    public void Hide()
    {
        girlAnim.gameObject.SetActive(false);
        boyAnim.gameObject.SetActive(false);
    }
    public void Show()
    {
        if (CurrentSex == PlayerSex.Girl)
            girlAnim.gameObject.SetActive(true);
        else
            boyAnim.gameObject.SetActive(true);
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
        transform.position += (Vector3)((int)CurrentSex * Vector2.up * 2.8f);
        SetTouchGroundY();
    }
    public void FlipSex()
    {
        CurrentSex = (PlayerSex)((int)CurrentSex * -1);
        if(CurrentSex == PlayerSex.Girl)
        {
            Anim = girlAnim;
            girlAnim.gameObject.SetActive(true);
            boyAnim.gameObject.SetActive(false);
        }
        else if(CurrentSex == PlayerSex.Boy)
        {
            Anim = boyAnim;
            boyAnim.gameObject.SetActive(true);
            girlAnim.gameObject.SetActive(false);
        }
        Physics2D.gravity *= -1;
    }
    public void FlipSexNoShow()
    {
        CurrentSex = (PlayerSex)((int)CurrentSex * -1);
        if (CurrentSex == PlayerSex.Girl)
        {
            Anim = girlAnim;
        }
        else if (CurrentSex == PlayerSex.Boy)
        {
            Anim = boyAnim;
        }
        Physics2D.gravity *= -1;
    }
    public void CheckNeedFlip(PlayerSex value)
    {
        if (CurrentSex != value)
            FlipSex();
    }

    public bool CanInteractive(out Switch interactiveObject)
    {
        interactiveObject = switchInteractive;
        return switchInteractive != null && switchInteractive.CanInteractive();
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    public void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public void SaveData(ref GameData data)
    {
        data.CurrentLevelData.spawnPoint = transform.position;
        data.CurrentLevelData.playerSex = CurrentSex;
    }

    public void LoadData(GameData data)
    {
        if (data.CurrentLevelData.spawnPoint != Vector3.zero)
            transform.position = data.CurrentLevelData.spawnPoint;
        if (data.CurrentLevelData.playerSex != CurrentSex)
            FlipSex();
    }


    #endregion
}
