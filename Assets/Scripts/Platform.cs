using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    [SerializeField] private MovingPlatform movement; // 注入移动组件

    public Rigidbody2D rb;

    public UnityEvent<GameObject> onPlayerStep;

    public virtual void Awake()
    {
        if(movement == null) movement = GetComponent<MovingPlatform>();
        rb = GetComponent<Rigidbody2D>();
    }
    // 平台自己的逻辑，例如当角色站在上面时的处理
    // 移动完全由 movement 组件控制

    
    //平台的碰撞检测
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onPlayerStep?.Invoke(gameObject);
        }
    }
}