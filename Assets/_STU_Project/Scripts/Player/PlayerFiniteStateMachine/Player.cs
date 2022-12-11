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

    [SerializeField] private SceneConfig_LevelInformation sceneConfig;
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
    public PlayerSex CurrentSex { get; private set; } = PlayerSex.Girl;

    private Vector2 workspace;

    private Switch switchInteractive;
    private bool invincible;
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

        if(collision.CompareTag("Damage2") && !invincible)
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
        transform.position += (Vector3)((int)CurrentSex * Vector2.up * 2.8f);
        SetTouchGroundY();
    }
    public void FlipSex(bool show = true)
    {
        CurrentSex = (PlayerSex)((int)CurrentSex * -1);

        transform.Rotate(new Vector3(180, 0, 0));
        SetCurrentSex(CurrentSex, show);
    }

    public void SetCurrentSex(PlayerSex playerSex, bool show = true)
    {
        CurrentSex = playerSex;
        if (playerSex == PlayerSex.Girl)
        {
            Anim = girlAnim;
            if(show)
            {
                girlAnim.gameObject.SetActive(true);
                boyAnim.gameObject.SetActive(false);
            }
        }
        else if (playerSex == PlayerSex.Boy)
        {
            Anim = boyAnim;
            if(show)
            {
                boyAnim.gameObject.SetActive(true);
                girlAnim.gameObject.SetActive(false);
            }
        }

        Physics2D.gravity = new Vector2(0, Mathf.Abs(Physics2D.gravity.y) * -(int)playerSex);
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
    public void SetInvincible(bool value)
    {
        invincible = value;
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

        CurrentSex = data.CurrentLevelData.playerSex;
        SetCurrentSex(CurrentSex);
        SetTouchGroundY();
    }

    private void Restart()
    {
        if (sceneConfig.TryGetSceneData(out Vector3 spawnPoint))
            transform.position = spawnPoint;
        SetCurrentSex(PlayerSex.Girl);
    }

    private void OnEnable()
    {
        NewGameManager.Instance.OnRestartEvent += Restart;
    }

    private void OnDestroy()
    {
        NewGameManager.Instance.OnRestartEvent -= Restart;
    }

    #endregion
}
