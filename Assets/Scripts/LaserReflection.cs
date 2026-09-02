using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class LaserReflection : MonoBehaviour
{
    [Header("光源设置")]
    public int maxReflections = 5;          // 最大反射次数（防止无限循环）
    public float maxDistance = 100f;        // 最大射程
    public LayerMask collisionMask = -1;    // 碰撞层（默认所有层）
    public bool updateEveryFrame = true;    // 每帧更新光线（动态反射）
    public string reflectiveTag = "Reflective"; // 反射面标签

    [Header("视觉效果")]
    public Color laserColor = Color.red;    // 光线颜色
    public float laserWidth = 0.1f;         // 光线宽度

    [Header("接收器事件（可选）")]
    public UnityEvent onLaserHitReceiver;   // 当任意接收器被首次击中时触发
    public UnityEvent onLaserLeaveReceiver; // 当光线离开接收器时触发

    private LineRenderer lineRenderer;
    private List<Vector2> points;           // 存储路径点（复用，避免每帧创建）

    // 记录当前被激光击中的接收器
    private LaserReceiver currentReceiver;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
        lineRenderer.useWorldSpace = true;

        points = new List<Vector2>(maxReflections * 2 + 2);
    }

    void Start()
    {
        // 避免射线从自身内部发射时碰撞到自己（如果发射器有 Collider）
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        if (updateEveryFrame)
            CalculateAndDrawLaser();
    }

    // 手动调用时更新（例如只在物体移动后调用，节省性能）
    public void UpdateLaser()
    {
        CalculateAndDrawLaser();
    }

    void CalculateAndDrawLaser()
    {
        points.Clear();

        Vector2 currentOrigin = (Vector2)transform.position;
        Vector2 currentDirection = transform.right; // 假设光线沿物体右方向射出

        bool hitSomething = false;
        LaserReceiver hitReceiver = null;   // 本次射线击中的接收器

        for (int i = 0; i <= maxReflections; i++)
        {
            points.Add(currentOrigin);

            RaycastHit2D hit = Physics2D.Raycast(currentOrigin, currentDirection, maxDistance, collisionMask);
            if (hit.collider != null)
            {
                points.Add(hit.point);
                hitSomething = true;

                // 检查是否击中接收器
                if (hit.collider.TryGetComponent(out LaserReceiver receiver))
                {
                    hitReceiver = receiver;
                    
                    // 注意：这里不立即 break，可以继续反射，也可以停止
                    // 通常解谜中光线击中接收器后会停止，所以我们直接 break
                    // 如果需要光线穿透接收器继续反射，请删除 break
                    break;
                }

                // 检查是否是反射面
                if (hit.collider.CompareTag(reflectiveTag))
                {
                    // 计算反射方向
                    currentDirection = Vector2.Reflect(currentDirection, hit.normal);
                    // 从碰撞点稍微偏移，避免陷入无限反射
                    currentOrigin = hit.point + currentDirection * 0.05f;
                    continue; // 继续下一次反射
                }
                else
                {
                    // 碰到普通不可反射物体 → 射线停止
                    break;
                }
            }
            else
            {
                // 没有命中任何物体，添加射线末端点
                points.Add(currentOrigin + currentDirection * maxDistance);
                hitSomething = false;
                break;
            }
        }

        // 如果从未命中任何物体（完全空荡荡），至少显示一条直线
        if (!hitSomething && points.Count < 2 && maxReflections >= 0)
        {
            points.Add(currentOrigin);
            points.Add(currentOrigin + currentDirection * maxDistance);
        }

        // 更新 LineRenderer
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0f));
        }

        // 处理接收器的激活/失活逻辑
        HandleReceiver(hitReceiver);
    }

    private void HandleReceiver(LaserReceiver newReceiver)
    {
        // 如果当前击中的接收器与上次不同
        if (currentReceiver != newReceiver)
        {
            // 让旧的接收器失活
            if (currentReceiver != null)
                currentReceiver.Deactivate();

            // 激活新的接收器
            currentReceiver = newReceiver;

            if (currentReceiver != null)
            {
                currentReceiver.Activate();
                onLaserHitReceiver?.Invoke();
            }
            else
            {
                onLaserLeaveReceiver?.Invoke();
            }
        }
        // 如果相同且已经激活，则无需重复操作
    }

    // 可选：让外部重置接收器状态（例如重新开始关卡时）
    public void ResetReceiverState()
    {
        if (currentReceiver != null)
            currentReceiver.Deactivate();
        currentReceiver = null;
    }

    // 编辑器下调试：绘制射线（不运行时也可见）
    void OnDrawGizmos()
    {
        if (!Application.isPlaying && gameObject.activeInHierarchy)
        {
            Gizmos.color = Color.yellow;
            Vector3 end = transform.position + transform.right * maxDistance;
            Gizmos.DrawLine(transform.position, end);
        }
    }
}