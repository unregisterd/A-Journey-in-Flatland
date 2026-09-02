using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePlatform : Platform
{
    [Header("碰撞检测")]
    [SerializeField] private LayerMask otherHalf;
    [SerializeField] private float otherHalfCheckDistance;

    [Header("显示的平台")]
    [SerializeField] private GameObject otherHalfPlatform;  // 允许为 null

    [Header("消失的平台")]
    [SerializeField] private GameObject activePlatform;     // 允许为 null

    private RaycastHit2D otherHalfHit;

    public override void Awake()
    {
        base.Awake();

        // 初始化：显示的平台隐藏，消失的平台显示（如果引用非空）
        if (otherHalfPlatform != null)
            otherHalfPlatform.SetActive(false);

        if (activePlatform != null)
            activePlatform.SetActive(true);
    }

    private void Update()
    {
        otherHalfHit = Physics2D.Raycast(transform.position, Vector2.right, otherHalfCheckDistance, otherHalf);

        if (otherHalfHit)
        {
            // 命中时：显示 otherHalfPlatform，隐藏 activePlatform
            if (otherHalfPlatform != null)
                otherHalfPlatform.SetActive(true);
            if (activePlatform != null)
                activePlatform.SetActive(false);
        }
        else
        {
            // 未命中时：隐藏 otherHalfPlatform，显示 activePlatform
            if (otherHalfPlatform != null)
                otherHalfPlatform.SetActive(false);
            if (activePlatform != null)
                activePlatform.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(otherHalfCheckDistance, 0, 0));
    }
}