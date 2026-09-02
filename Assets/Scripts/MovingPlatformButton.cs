using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformButton : MonoBehaviour
{
    public MovingPlatform targetPlatform;
    [SerializeField] private float new_movingSpeed = 3;

    private void Start()
    {
        // 初始时确保平台静止
        targetPlatform.moveSpeed = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && targetPlatform != null)
        {
            // 激活平台移动：重置路径索引并从起点开始移动
            targetPlatform.StartMoving(new_movingSpeed);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (targetPlatform != null)
        {
            // 停止移动并瞬间回到起点
            targetPlatform.ResetToStart();
        }
    }
}