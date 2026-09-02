using System.Collections;
using UnityEngine;

public class MovingPlatform : Platform
{
    [Header("移动参数")]
    [SerializeField] public  float moveSpeed;                  // 正向移动速度
    [SerializeField] private  float returnSpeed = 2f;         // 返回起点的速度
    [SerializeField] private  Transform[] waysPoints;


    private int pointIndex;

    private bool isReturning = false;       // 是否正在返回起点
    private bool isMovingForward = false;   // 是否正在正向移动
    [SerializeField] private bool startStatic = true;

    public virtual void Start()
    {
        pointIndex = 1;
        // 初始状态：静止，不移动
        if (startStatic)
        {
            isMovingForward = false;
            isReturning = false;
            moveSpeed = 0;
        }
        else
        {
            isMovingForward =true;
        }
        
    }

    public virtual void Update()
    {
        // 优先处理返回逻辑
        if (isReturning)
        {
            // 向起点移动（waysPoints[0]）
            transform.position = Vector2.MoveTowards(
                transform.position,
                waysPoints[0].position,
                returnSpeed* Time.deltaTime
            );

            // 到达起点
            if (startStatic && Vector2.Distance(transform.position, waysPoints[0].position) < 0.01f)
            {
                transform.position = waysPoints[0].position; // 精确归位
                isReturning = false;
                moveSpeed = 0;          // 完全停止
            }
            return; // 返回过程中不执行正向移动
        }

        // 正向移动
        if (isMovingForward && moveSpeed != 0)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                waysPoints[pointIndex].position,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, waysPoints[pointIndex].position) < 0.1f)
            {
                pointIndex++;
                if (pointIndex >= waysPoints.Length)
                {
                    pointIndex = 0;
                }
            }
        }
    }

    /// <summary>
    /// 启动平台正向移动（从第一个路径点开始）
    /// </summary>
    public void StartMoving(float speed)
    {
        // 中断任何正在进行的返回
        if (isReturning)
        {
            isReturning = false;
            Debug.Log("中断返回，开始正向移动");
        }

        // 重置状态
        isMovingForward = true;
        pointIndex = 1;                     // 从起点走向第二个点
        transform.position = waysPoints[0].position; // 确保从起点开始
        moveSpeed = speed;                  // 设置移动速度
    }

    /// <summary>
    /// 停止正向移动并开始平滑返回起点
    /// </summary>
    public void ResetToStart()
    {
        // 停止正向移动
        isMovingForward = false;
        // 开始返回过程
        isReturning = true;
        // 使用返回速度（如果 returnSpeed 为 0 则借用 moveSpeed，但此时 moveSpeed 可能为 0，所以需要给个默认值）
        // 注意：moveSpeed 在返回过程中不再控制移动，由 returnSpeed 决定，但为了兼容旧逻辑，我们保留 moveSpeed 不变
        // 但返回移动会使用 returnSpeed（或默认值）
    }

    // 玩家跟随逻辑保持不变
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}