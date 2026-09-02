using UnityEngine;

public class VerticalMovingPlatform : HorizontalMovingPlatform
{
    [Header("垂直移动参数")]
    [SerializeField] private float descendDistance = 1f;   // 下降距离
    //[SerializeField] private float ascendDistance = 1f;    // 上升距离（通常与下降相同）
    [SerializeField] private float moveSpeed = 2f;         // 移动速度

    private Vector3 originalPosition;      // 平台原始位置
    private Vector3 targetPosition;        // 目标位置（下降后或上升后）
    private bool isMoving = false;          // 是否正在移动中

    private void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;
    }

    protected override void Update()
    {
        // 检测玩家是否站在平台上（使用触发器或碰撞检测）
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, detectLayer);
        if (hit != null)
        {
            Player newPlayer = hit.GetComponent<Player>();
            if (newPlayer != null && player != newPlayer)
            {
                player = newPlayer;
            }
        }
        else
        {
            // 玩家离开平台范围时，恢复原位
            if (player != null)
            {
                player = null;
                StartMovingTo(originalPosition);
            }
            return;
        }

        if (player != null)
        {
            // 检查玩家是否在跳跃状态（根据你的 Player 类实现调整）
            if (player.JumpState != null && player.JumpState.IsActive) // 假设 JumpState 有 IsActive 属性
            {
                // 玩家跳跃中，平台下降
                if (!isMoving && targetPosition == originalPosition)
                {
                    StartMovingTo(originalPosition + Vector3.down * descendDistance);
                }
            }
            else if (player.IsGrounded && !player.JumpState.IsActive)
            {
                // 玩家落回地面且不在跳跃，平台上升回原位
                if (!isMoving && targetPosition != originalPosition)
                {
                    StartMovingTo(originalPosition);
                }
            }
        }
    }

    private void StartMovingTo(Vector3 newTarget)
    {
        targetPosition = newTarget;
        isMoving = true;
        // 可选：禁用玩家输入或添加其他效果
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // 平滑移动平台
            Vector3 newPos = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                rb.MovePosition(targetPosition);
            }
            
            // 让玩家跟随平台移动（如果玩家在平台上）
            if (player != null && player.transform.parent != transform)
            {
                player.transform.SetParent(transform);
            }
        }
    }
}