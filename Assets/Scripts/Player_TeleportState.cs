using UnityEngine;

public class Player_TeleportState : MonoBehaviour
{
    [Header("技能设置")]
    [SerializeField] private KeyCode skillKey = KeyCode.Mouse1;
    [SerializeField] private float recallWindow = 3f;
    public GameObject recallEffectPrefab;
    public AudioClip recallSound;
    public GameObject recordEffectPrefab;
    public GameObject anchorMarkerPrefab;          // 新增：锚点标识预制体

    [Header("状态")]
    [SerializeField] private bool isUnlocked = false;
    private Vector2 anchorPoint;
    private bool hasAnchor = false;
    private float anchorTime;
    private GameObject currentMarker;              // 当前锚点标识实例

    private Rigidbody2D rb;
    private Transform playerTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = transform;
    }

    private void Update()
    {
        if (!isUnlocked) return;
        HandleInput();
    }

    private void HandleInput()
    {
        if (!Input.GetKeyDown(skillKey)) return;

        if (!hasAnchor)
        {
            // 记录锚点
            anchorPoint = playerTransform.position;
            hasAnchor = true;
            anchorTime = Time.time;

            // 生成锚点标识
            if (anchorMarkerPrefab != null)
            {
                currentMarker = Instantiate(anchorMarkerPrefab, anchorPoint, Quaternion.identity);
            }

            // 可选：播放记录特效
            if (recordEffectPrefab != null)
                Instantiate(recordEffectPrefab, anchorPoint, Quaternion.identity);

            Debug.Log("[瞬移回溯] 锚点已记录：" + anchorPoint);
        }
        else
        {
            if (Time.time - anchorTime <= recallWindow)
            {
                Recall();
            }
            else
            {
                // 锚点失效，销毁标识
                DestroyMarker();
                hasAnchor = false;
                Debug.Log("[瞬移回溯] 锚点已失效");
            }
        }
    }

    private void Recall()
    {
        // 瞬移
        playerTransform.position = anchorPoint;

        // 重置速度
        if (rb != null)
            rb.velocity = Vector2.zero;

        // 播放特效和音效
        if (recallEffectPrefab != null)
            Instantiate(recallEffectPrefab, anchorPoint, Quaternion.identity);
        if (recallSound != null)
            AudioSource.PlayClipAtPoint(recallSound, anchorPoint);

        // 清除锚点和标识
        DestroyMarker();
        hasAnchor = false;

        Debug.Log("[瞬移回溯] 回溯至锚点");
    }

    /// <summary>
    /// 销毁锚点标识物（如果存在）
    /// </summary>
    private void DestroyMarker()
    {
        if (currentMarker != null)
        {
            Destroy(currentMarker);
            currentMarker = null;
        }
    }

    /// <summary>
    /// 解锁技能
    /// </summary>
    public void Unlock()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            Debug.Log("[瞬移回溯] 技能已解锁！");
        }
    }

    /// <summary>
    /// 重置技能状态（玩家死亡等）
    /// </summary>
    public void ResetState()
    {
        DestroyMarker();      // 销毁锚点标识
        hasAnchor = false;    // 清除锚点
    }
}