using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalMovingPlatform : Platform
{
    [Header("检测参数")]
    [SerializeField] protected float detectRadius = 3f;
    [SerializeField] protected LayerMask detectLayer;

    public Player player { get; protected set; }

    public override void Awake()
    {
        base.Awake();

    }
    protected virtual void Update()
    {
        // 检测圆形范围内的碰撞体
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, detectLayer);
        
        if (hit != null)
        {
            // 尝试获取 Player 组件
            Player newPlayer = hit.GetComponent<Player>();

            //如果还没有记录或者记录的已经不是当前玩家
            if (newPlayer != null && player != newPlayer)
            {
                player = newPlayer;   // 记录新进入的玩家
            }
        }
        else
        {
            // 没有检测到任何碰撞体，玩家离开
            player = null;
            return;
        }

        // 如果 player 有效，则应用水平移动
        if (player != null)
        {
            setVelocity(-player.MoveInput.x * player.moveSpeed, rb.velocity.y);
        }
    }

    //圆形检测
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    protected void setVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }
}