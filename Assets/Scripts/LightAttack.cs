using System.Collections;
using UnityEngine;

public class LightAttack : MonoBehaviour
{
    [Header("显现周期")]
    public float appearDuration = 1f;
    public float disappearDuration = 2f;
    public bool startActive = false;

    [Header("行为")]
    public bool destroyOnHit = false;   // 是否在击中后销毁（可选）
    public bool loopForever = true;     // 是否无限循环

    private Collider2D attackCollider;
    private SpriteRenderer visualRenderer;

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
        visualRenderer = GetComponent<SpriteRenderer>();

        if (visualRenderer == null)
            visualRenderer = GetComponentInChildren<SpriteRenderer>();

        SetActive(startActive);
    }

    private void Start()
    {
        if (loopForever)
            StartCoroutine(AttackCycle());
        else
            StartCoroutine(OneShotCycle());
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            // 消失阶段
            SetActive(false);
            yield return new WaitForSeconds(disappearDuration);

            // 显现阶段
            SetActive(true);
            yield return new WaitForSeconds(appearDuration);
        }
    }

    private IEnumerator OneShotCycle()
    {
        // 初始等待消失阶段（如果 startActive 为 false，可调整）
        SetActive(false);
        yield return new WaitForSeconds(disappearDuration);

        SetActive(true);
        yield return new WaitForSeconds(appearDuration);

        SetActive(false);

        // 可选：自我销毁
        // Destroy(gameObject);
    }

    private void SetActive(bool active)
    {
        if (attackCollider != null)
            attackCollider.enabled = active;

        if (visualRenderer != null)
            visualRenderer.enabled = active;
    }

    // 注意：不再直接调用玩家死亡，只负责碰撞判定。死亡逻辑由 GameController 处理
    // 但为了确保光线只在激活时触发，仍然保留这个检查，不过不执行死亡。
    // 如果希望光线触发后自我销毁，可以启用 destroyOnHit。
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attackCollider.enabled) return; // 未激活时不处理

        if (other.CompareTag("Player"))
        {
            // 死亡由 GameController 的 OnTriggerEnter2D 处理，这里只做额外反馈（可选）
            // 如果需要销毁光线（一次性陷阱），可取消注释：
            // if (destroyOnHit) Destroy(gameObject);
        }
    }
}