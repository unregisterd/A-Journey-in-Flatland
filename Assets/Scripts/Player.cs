using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 组件引用
    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR {get; private set; }

    // 状态机与各个具体状态
    private StateMachine stateMachine;
    public Player_idleState IdleState { get; private set; }
    public Player_moveState MoveState { get; private set; }
    public Player_jumpState JumpState { get; private set; }
    public Player_fallState fallState { get; private set; }

    private GameController gameController;

    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float jumpForce = 3f;
    //[SerializeField] float liftSpeed = 5f;
    private bool facingRight = true;//判断玩家的朝向
    private bool canMove = true;//判断玩家是否可以移动
    

    [Range(0,1)]
    public float inAirMoveMultiplier = .7f;

    [Header("碰撞检测")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    private RaycastHit2D groundHit;

    public bool IsGrounded {get; private set; } = false;

    // 输入相关
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput; // 供状态读取
    
    private Transform currentPlatform;   // 当前站立的平台
    private Vector3 platformOffset;      // 玩家相对于平台的位置偏移

    private void Awake()
    {

        
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        SR=GetComponentInChildren<SpriteRenderer>();

        gameController = GameObject.Find("GameManager02").GetComponent<GameController>();

        //初始化状态机
        stateMachine = new StateMachine();

        //创建各个具体状态
        IdleState = new Player_idleState(this,stateMachine,"idle");
        MoveState = new Player_moveState(this,stateMachine,"move");
        JumpState = new Player_jumpState(this,stateMachine,"jump");
        fallState = new Player_fallState(this,stateMachine,"fall");
    }

    private void Start()
    {
        stateMachine.Initialize(IdleState);//设定初始状态为静止
    }
    private void Update()
    { 
        //获取输入
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        Anim.SetFloat("Speed", Mathf.Abs(moveInput.x)); // 只控制动画状态机混合
        // 根运动会自动移动角色，不需要手动 RB.velocity
        if (moveInput.x != 0)
        {
            bool isMovingRight = moveInput.x > 0;
            if (isMovingRight != facingRight)
                Flip();
        }

        //进行地面检测
        // 获取角色碰撞体的大小（假设正方形）
        Collider2D col = GetComponent<Collider2D>();
        float boxWidth = col.bounds.size.x * 0.9f;  // 宽度略缩，避免边缘卡墙
        float boxHeight = 0.1f;                    // 薄片高度

        // 盒状检测位置：角色脚底中心
        Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (col.bounds.extents.y - boxHeight / 2);

        // 执行 BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, new Vector2(boxWidth, boxHeight), 0f, Vector2.down, groundCheckDistance, whatIsGround);
        IsGrounded = hit.collider != null;


        // 每帧让状态机更新当前状态
        stateMachine.UpdateActiveState();
    }

    //设置速度的方法
    public void SetVelocity(float xVelocity,float yVelocity)
    {
        RB.velocity = new Vector2(xVelocity,yVelocity);
    }

    private void OnDrawGizmos()
    {
         // 仅用于编辑器显示
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;
        float boxWidth = col.bounds.size.x * 0.9f;
        float boxHeight = 0.1f;
        Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (col.bounds.extents.y - boxHeight / 2);
        Gizmos.DrawWireCube(boxCenter, new Vector3(boxWidth, boxHeight, 0));
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    private void Flip()
    {
        if(facingRight){
            SR.flipX = true;
            facingRight = false;
        }
        else if(!facingRight){
            SR.flipX = false;
            facingRight = true;
        }   
    }

    public void SetCanMove(bool moveEnabled)
    {
        canMove = moveEnabled;
        if (!canMove)
        {
            RB.velocity = Vector2.zero; // 立即停止移动
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //一旦触碰尖刺就会死
        if (collision.gameObject.CompareTag("Spikes"))
        {
            gameController.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Light"))
        {
            gameController.Die();
        }
    }
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("UpLight"))
        {
            RB.velocity = new Vector2(RB.velocity.x, liftSpeed);
        }
    }*/
}
