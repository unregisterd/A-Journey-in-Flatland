using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DisApealPlatform : Platform
{
    [Header("瞬移参数")]
    public Transform[] wayPoints;               // 路径点（第一个点为初始位置）
    [SerializeField] private float[] waitingTime;     // 每个点的等待时间（索引与 wayPoints 对应）

    private SpriteRenderer spriteRenderer;
    private bool hasStarted = false;            // 确保只启动一次

    private bool forward = true;  //平台向前走的标识

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = wayPoints[0].position;
        spriteRenderer.enabled = true;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (hasStarted) return;                 // 已经启动过，不再重复启动

        hasStarted = true;
        StartCoroutine(TeleportLoop());
    }

    private IEnumerator TeleportLoop()
    {
        int index = 1; // 从第二个点开始移动（第一个点为起点）

        while (true)
        {
            // 等待指定时间（如果数组不够，使用默认 1 秒）
            float wait = (index < waitingTime.Length) ? waitingTime[index] : 1f;
            yield return new WaitForSeconds(wait);

            // 瞬移前隐藏
            spriteRenderer.enabled = false;

            // 瞬移到目标点
            transform.position = wayPoints[index].position;

            // 瞬移后显示
            spriteRenderer.enabled = true;

            // 更新索引
            if(forward) index++;
            else index--;
            if (index >= wayPoints.Length)
            {
                forward = false;
                index --;
            }
            else if(index <= 0)
            {
                forward = true;
            }
        }
    }
}