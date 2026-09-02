using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock_TeleportState : MonoBehaviour
{
    public Player_TeleportState teleportState{ get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查是否为玩家
        if (!collision.CompareTag("Player")) return;

        // 尝试获取 RecallAbility 组件（可能在玩家自身或其父级）
        Player_TeleportState recall = collision.GetComponent<Player_TeleportState>();
        if (recall == null)
            recall = collision.GetComponentInParent<Player_TeleportState>();

        if (recall != null)
        {
            recall.Unlock();
            Destroy(gameObject);
        }
    }
}
