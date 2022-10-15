using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using MiProduction.BroAudio;

public class PlayerCtrl : MonoBehaviour
{
    // PlayerSelf
    [Header("參考")]
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private SpriteRenderer sprRenderer = null;
    [SerializeField] private Animator anim = null;
    [SerializeField] private WallDetector wallDetector = null;
    [SerializeField] private GameObject girlObj;
    [SerializeField] private GameObject boyObj;
    [SerializeField] private PlayerAbilityData ability;
    private Player_CameraTarget playerCameraTarget;

    // movement value
    [Header("移動")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;

    [Header("跳躍")]
    [SerializeField] private int exJumps = 1;
    [SerializeField] private float jumpHeight = 3f;

    [Header("地板檢測")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [SerializeField] private float envGravity = -40f;

    public Vector2 input;
    private bool isFaceRight = false;
    private bool isDie;
    public bool canCtrl = true;

    public UnityAction OnTouchGround;

    void Start()
    {
        isDie = false;

        Physics2D.gravity = new Vector2(0f, envGravity * gravityDir);
        if (GameManager.instance.life != 10)
        {
            anim.SetTrigger("Reborn");
            //transform.position = GameManager.instance.respawnPosition;
        }
        //Debug.Log("Respawn P : " + GameManager.instance.respawnPosition);
        // if (respawnPosition == Vector3.zero) respawnPosition = transform.position;
        // else transform.position = respawnPosition;

        // Record current level
        GameManager.instance.levelName = SceneManager.GetActiveScene().name;
        GameManager.instance.Save();
        playerCameraTarget = GetComponent<Player_CameraTarget>();

        OnTouchGround += playerCameraTarget.UpdateY;
        OnTouchGround += ResetStatus;
    }
    private void OnDestroy()
    {
        // GameManager.instance.respawnPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (isJumping && isExJumping  == false)
        {
            if (!Input.GetKey(KeyCode.Space) && rb.velocity.y > 0)
            {
                rb.AddForce(Vector2.down * 30);
            }
        }
    }

    void Update()
    {
        if (isDie || canCtrl == false)
            return;

        GroundCheck();
        MovementX();
        if (Input.GetKeyDown(KeyCode.Space) && ability.jump == true)
            Jump();
        

#if UNITY_EDITOR
        // Check Hp
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.instance.hp += 1f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.hp -= 1f;
        }
#endif
        if (Input.GetKeyDown(KeyCode.C) && ability.flip == true)
        {
            AntiGravity();
        }
    }

    #region 地板檢測
    /// <summary> On ground </summary>
    private bool _isGrounded = false;
    bool isGrounded
    {
        // 讀取的時候回傳_grounded
        get { return _isGrounded; }
        // 寫入的時候寫入_grounded
        // 順便修改動畫的grounded
        set
        {
            _isGrounded = value;
            anim.SetBool("OnFloor", value);
        }
    }
    private bool lateGrounded;
    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (lateGrounded == false && rb.velocity.y <= 0 && isGrounded)
        {
            OnTouchGround.Invoke();
        }
        lateGrounded = isGrounded;
    }
    #endregion

    #region 移動
    private float sp = 0f;
    private float mixSpeed = 3f;

    private void MovementX()
    {
        input.x = Input.GetAxisRaw("Horizontal"); // Get keyboard AD value -1 ~ 1
        sp = Mathf.Lerp(sp, Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed, Time.deltaTime * mixSpeed);

        if (input.x > 0 && isFaceRight == false)
            FlipSprite();
        else if (input.x < 0 && isFaceRight == true)
            FlipSprite();

        if (Mathf.Abs(input.x) > 0.1f)
        {
            anim.SetBool("IsRun", true);
        }
        else
        {
            anim.SetBool("IsRun", false);
        }

        wallDetector.SetDirection(sprRenderer.flipX);

        float wallStop = 1;
        //如果有碰到牆就停止往同方向施加Velocity
        if (wallDetector.isTouching)
            wallStop = sprRenderer.flipX ? Mathf.Clamp(input.x, -1, 0) : Mathf.Clamp(input.x, 0, 1);

        Vector2 moveNormal = new Vector2(wallStop * input.x * sp, rb.velocity.y);
        rb.velocity = moveNormal;
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector3(isFaceRight ? 1 : -1, 1, 1);
        isFaceRight = !isFaceRight;
    }
    #endregion

    #region 跳躍
    private int jumpCounter;
    private bool isJumping;
    private bool isExJumping;

    private float CalculateJumpForce(float gravityStrength, float jumpHeight)
    {
        //h = v^2/2g
        //2gh = v^2
        //sqrt(2gh) = v
        return Mathf.Sqrt(2 * gravityStrength * jumpHeight);
    }
    private void Jump()
    {
        if (isGrounded)  //FirstJump
        {
            isJumping = true;
            jumpCounter = exJumps;
            JumpForce();
        }
        else if(isJumping && jumpCounter > 0 && ability.doubleJump == true)  //ExJump
        {
            isExJumping = true;
            jumpCounter--;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            JumpForce();
        }
    }

    private void JumpForce()
    {
        float jumpForce = CalculateJumpForce(Physics2D.gravity.magnitude, jumpHeight);
        rb.AddForce(Vector2.down * jumpForce * gravityDir, ForceMode2D.Impulse);
        // Jump Sound
        //SoundManager.Instance.PlaySFX(Sound.Jump);
    }
    #endregion

    #region 翻轉
    public int gravityDir = -1;
    public bool isGirl
    {
        get { return gravityDir == -1; }
    }
    public void AntiGravity()
    {
        if (!isGrounded) return;

        gravityDir = -gravityDir;
        if (isGirl == true)
        {
            girlObj.SetActive(true);
            boyObj.SetActive(false);
            anim = girlObj.GetComponent<Animator>();
        }
        else
        {
            girlObj.SetActive(false);
            boyObj.SetActive(true);
            anim = boyObj.GetComponent<Animator>();
        }
        // Antigravity
        Physics2D.gravity = new Vector2(0, (envGravity * gravityDir));
        transform.rotation *= Quaternion.Euler(180f, 0f, 0f);
        transform.position += Vector3.down * 3 * gravityDir;
        playerCameraTarget.UpdateY();
    }
    public void AntiGravityByProps(bool isInvert)
    {
        if (isInvert)
        {
            // Negative
            Physics2D.gravity = new Vector2(0, Mathf.Abs(Physics2D.gravity.y));
            gravityDir = -1;
            transform.rotation = Quaternion.Euler(-180f, 0f, 0f);
            //isAntiPlayer = true;
        }
        else
        {
            // Positive
            Physics2D.gravity = new Vector2(0, Mathf.Abs(Physics2D.gravity.y) * -1f);
            gravityDir = 1;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            //isAntiPlayer = false;
        }
    }
    #endregion

    /// <summary> 與任何東西碰撞 </summary>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Damage2")
        {
            GameManager.instance.hp -= 2f;
            EvaluationForm.touchTrapCount++;
            anim.SetTrigger("Hit");
        }
        if (other.transform.tag == "Damage999")
        {
            GameManager.instance.hp -= 999f;
            anim.SetTrigger("Hit");
        }

        if (GameManager.instance.hp <= 0f)
        {
            isDie = true;
            // Die Sound
            //SoundManager.Instance.PlaySFX(Sound.Die, 2f);
            anim.SetTrigger("Die");
            // Pause
            // Time.timeScale = 0f;
            
            Invoke("Regeneration",1f);
        }
    }

   private void Regeneration()
    {
        GameManager.instance.life -= 1;
        GameManager.instance.hp = 1f;        
        GameManager.instance.LoadGame();
    }

    public void StopCtrl()
    {
        canCtrl = false;
        rb.velocity = Vector3.zero;
        anim.SetBool("IsRun", false);
        anim.SetFloat("Y", 0);
    }

    /* Lock Taskmgr
    [DllImport("user32")]
    public static extern bool LockWorkStation();
    private void Form1_Load(object sender, EventArgs e)
    {
        FileStream fs = new FileStream(Environment.ExpandEnvironmentVariables("%windir%\\system32\\taskmgr.exe"), FileMode.Open);
        BlockInput(true); // Lock 
        System.Threading.Thread.Sleep(1000); // Lock 1 sec
        BlockInput(false); // Unlock
    }
    */
    /* Lock All Keyboard & Mouse
    [DllImport("user32.dll")]
    static extern void BlockInput(bool Block);
    */

    private void ResetStatus()
    {
        isJumping = false;
        isExJumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}




