using UnityEngine;
using System.Collections.Generic;
using System;

public class MemoryManager : MonoBehaviour
{
    [Header("对话列表")]
    public List<DialogLine> dialogLines = new List<DialogLine>();

    public bool isTriggered{get; private set; } = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerMovement = collision.GetComponent<Player>();

        isTriggered = true;
         // ... (玩家tag检查和isTriggered检查)
        if (DialogManager.Instance == null)
        {
            Debug.LogError("场景中不存在 DialogManager 实例！");
            return;
        }
        // 通过单例调用
        DialogManager.Instance.StartDialog(dialogLines, playerMovement);
        gameObject.SetActive(false);
    }
}
