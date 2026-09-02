using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPlatform : Platform
{
    [Header("单向平台设置")]
    [SerializeField] private string playerTag = "Player";          // 玩家标签
    [SerializeField] private float verticalOffset = 0.1f;          // 垂直容差，防止抖动
    [SerializeField] private float checkInterval = 0.1f;           // 检测间隔（秒）

    private Collider2D platformCollider;        // 平台自身的碰撞体
    private GameObject player;                  // 缓存的玩家对象
    private float checkTimer;                   // 检测计时器

    public override void Awake()
    {
        base.Awake();                           // 调用基类 Awake，初始化 movement 和 rb
        platformCollider = GetComponent<Collider2D>();
        // 初始状态设为触发器，允许从下方穿过
        platformCollider.isTrigger = true;

        // 尝试获取玩家引用
        TryGetPlayer();
    }

    private void Update()
    {
        // 定时检测，避免每帧都执行开销较大的操作
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            UpdateColliderState();
        }
    }

    /// <summary>
    /// 更新碰撞体的触发器状态
    /// </summary>
    private void UpdateColliderState()
    {
        // 确保玩家引用有效
        if (!IsPlayerValid())
            return;

        bool playerAbove = IsPlayerAbovePlatform();
        platformCollider.isTrigger = !playerAbove;   // 玩家在上方 → 实体碰撞
    }

    /// <summary>
    /// 检查玩家是否有效（存在且活跃）
    /// </summary>
    private bool IsPlayerValid()
    {
        if (player == null || !player.activeInHierarchy)
        {
            TryGetPlayer();
            return player != null && player.activeInHierarchy;
        }
        return true;
    }

    /// <summary>
    /// 尝试获取玩家对象（通过标签查找）
    /// </summary>
    private void TryGetPlayer()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    /// <summary>
    /// 通过向量检测玩家是否位于平台上方
    /// </summary>
    private bool IsPlayerAbovePlatform()
    {
        // 获取平台顶部 Y 坐标
        float platformTopY = platformCollider.bounds.max.y;

        // 获取玩家的碰撞体（用于精确底部位置）
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        float playerBottomY;

        if (playerCollider != null)
        {
            // 使用玩家碰撞体的最低点（最底部 Y 坐标）
            playerBottomY = playerCollider.bounds.min.y;
        }
        else
        {
            // 如果没有碰撞体，回退到玩家物体的中心点（精度较低，但能工作）
            playerBottomY = player.transform.position.y;
        }

        // 判断玩家底部是否高于平台顶部（减去容差）
        return playerBottomY > platformTopY - verticalOffset;
    }
}
    
