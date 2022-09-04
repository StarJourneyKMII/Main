using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    // movement value
    [Header("移動")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;

    [Header("攝影機跟隨")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float maxDistance = 2;
    [SerializeField] private float followSensitive = 1;
    private float followPosX = 0;
    private float followPosY = 0;

    [Header("查看")]
    public float lookDistanceX = 3f;
    public float lookDistanceY = 3f;
    private float lookOffsetX;
    private float lookOffsetY;

    [Header("跳躍")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpPower = 20f;

    [Header("地板檢測")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [SerializeField] private float envGravity = -40f;

    private float input;
    private bool isFaceRight = false;
    private GameObject playerGOF;
    private bool isDie;
    public bool canCtrl = true;

    private UnityAction OnTouchGround;

    void Start()
    {
        isDie = false;
        cameraTarget.parent = null;
        OnTouchGround += UpdateY;

        playerGOF =gameObject;
        if (Physics2D.gravity.y != envGravity)
        {
            Physics2D.gravity = new Vector2(0f, envGravity);
        }
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
    }
    private void OnDestroy()
    {
        // GameManager.instance.respawnPosition = transform.position;
    }

    void Update()
    {
        if (isDie || canCtrl == false)
            return;

        MovementX();
        JumpmentY();

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

        if (input == 0 && onFloor == true)
            Look();
    }

    private void Look()
    {
        lookOffsetY = Input.GetAxis("LookVertical") * lookDistanceY;
        lookOffsetX = Input.GetAxis("LookHorizontal") * lookDistanceX;
    }
    #region 移動
    private float sp = 0f;
    private float mixSpeed = 3f;

    private void MovementX()
    {
        input = Input.GetAxisRaw("Horizontal"); // Get keyboard AD value -1 ~ 1
        sp = Mathf.Lerp(sp, Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed, Time.deltaTime * mixSpeed);

        if (input != 0)
            followPosX = Mathf.Lerp(followPosX,  input * maxDistance, Time.deltaTime * followSensitive);
        cameraTarget.transform.position = new Vector3(transform.position.x + followPosX + lookOffsetX, followPosY + lookOffsetY, 0);

        if (input > 0 && isFaceRight == false)
            FlipSprite();
        else if (input < 0 && isFaceRight == true)
            FlipSprite();

        if (Mathf.Abs(input) > 0.1f)
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
            wallStop = sprRenderer.flipX ? Mathf.Clamp(input, -1, 0) : Mathf.Clamp(input, 0, 1);

        Vector2 moveNormal = new Vector2(wallStop * input * sp, rb.velocity.y);
        rb.velocity = moveNormal;
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector3(isFaceRight ? 1 : -1, 1, 1);
        isFaceRight = !isFaceRight;
    }
#endregion

    #region 跳躍
    private int jumps;

    /// <summary> On floor </summary>
    private bool _onFloor = false;
    bool onFloor
    {
        // 讀取的時候回傳_onFloor
        get { return _onFloor; }
        // 寫入的時候寫入_onFloor
        // 順便修改動畫的onFloor
        set { 
            _onFloor = value; 
            anim.SetBool("OnFloor", value); }
    }

    private bool _onStick = false;
    bool onStick
    {
        get { return _onStick; }
        set { 
            _onStick = value; }
    }
    private bool _onMid = false;
    bool onMid
    {
        get { return _onMid; }
        set { 
            _onMid = value; }
    }

    public void JumpmentY()
    {
        bool isJump = Input.GetKeyDown(KeyCode.Space);
        if (isJump)
        {
            if (onFloor && ability.jump == true)
            {
                jumps = maxJumps;
                JumpG();
                jumps -= 1;
            }
            else if(ability.doubleJump == true)
            {
                if (jumps > 0)
                {
                    JumpG();
                    jumps -= 1;
                }
                else
                    return;
            }
        }

        anim.SetFloat("Y", rb.velocity.y); // Send Y into the animation
    }
    public void JumpG()
    {
        // 取代當前的慣性
        // velocity 空間中的相對牛頓力
        rb.velocity = new Vector2(0, jumpPower * antiGravity);
        // Jump Sound
        SoundManager.Instance.Play(Sound.Jump);
    }
    #endregion

    #region 翻轉
    private int antiGravity = 1;
    private bool isGirl = true;
    //private bool isAntiPlayer = default;
    public void AntiGravity()
    {
        if (!onFloor || onStick) return;

        isGirl = !isGirl;
        if(isGirl == true)
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
        Physics2D.gravity = new Vector2(0, (Physics2D.gravity.y * -1f));
        if (Physics2D.gravity.y > 0)
        {
            antiGravity = -1;
            playerGOF.transform.rotation *= Quaternion.Euler(180f, 0f, 0f);
            if (onFloor)
                playerGOF.transform.position = new Vector3(playerGOF.transform.position.x, playerGOF.transform.position.y - 3f, playerGOF.transform.position.z);
            //isAntiPlayer = true;
        }
        else
        {
            antiGravity = 1;
            playerGOF.transform.rotation *= Quaternion.Euler(180f, 0f, 0f);
            if (onFloor)
                playerGOF.transform.position = new Vector3(playerGOF.transform.position.x, playerGOF.transform.position.y + 3f, playerGOF.transform.position.z);
            //isAntiPlayer = false;
        }
    }
    public void AntiGravityByProps(bool isInvert)
    {
        if (isInvert)
        {
            // Negative
            Physics2D.gravity = new Vector2(0, Mathf.Abs(Physics2D.gravity.y));
            antiGravity = -1;
            playerGOF.transform.rotation = Quaternion.Euler(-180f, 0f, 0f);
            //isAntiPlayer = true;
        }
        else
        {
            // Positive
            Physics2D.gravity = new Vector2(0, Mathf.Abs(Physics2D.gravity.y) * -1f);
            antiGravity = 1;
            playerGOF.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            //isAntiPlayer = false;
        }
    }
    #endregion

    // 所有碰撞檢定都是基於物理
    /// <summary> 物理刷新的瞬間 (一秒約50次)</summary>
    private void FixedUpdate()
    {
        // 預設立場為不著地
        onFloor = false;
        onStick = false;
        // 
        onMid = false;
        // 在指定位置與指定半徑下畫一個碰撞器 並且回傳碰到的東西
        Collider2D[] allStuff = Physics2D.OverlapCircleAll(groundCheck.position, 0.3f, groundLayer);
        // 跑回圈檢查碰到的「每個」東西
        foreach (Collider2D stuff in allStuff)
        {
            // Debug.Log("碰到了 : " + stuff.name);
            if (stuff.gameObject.tag == "MgStick")
            {
                onStick = true;
            }
            if (stuff.gameObject.name == "Midground1")
            {
                onMid = true;
            }
            onFloor = true;
            OnTouchGround.Invoke();
        }

        float maxY = 3.5f;
        if (onFloor == false)
        {
            if (isGirl == true)
            {
                if (transform.position.y > followPosY + maxY)
                    followPosY = transform.position.y - maxY;
                else if (transform.position.y < followPosY)
                    followPosY = transform.position.y;
            }
            else
            {
                if (transform.position.y < followPosY - maxY)
                    followPosY = transform.position.y + maxY;
                else if (transform.position.y > followPosY)
                    followPosY = transform.position.y;
            }
        }
    }

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
            SoundManager.Instance.Play(Sound.Die, 2f);
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
        rb.Sleep();
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

    private void UpdateY()
    {
        float offset = 0.7f;
        if (isGirl == true)
            followPosY = transform.position.y + offset;
        else
            followPosY = transform.position.y - offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}




