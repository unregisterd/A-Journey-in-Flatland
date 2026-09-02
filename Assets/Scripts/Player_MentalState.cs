using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MentalState : MonoBehaviour
{
    [Header("检测参数")]
    [SerializeField] private KeyCode absorbKey;
    [SerializeField] private float absorbRadius;
    [SerializeField] private LayerMask absorbableLayers;             // 可消失物体的层
    [SerializeField] private bool absorbAllInRange = true;//true：一次按键吸收范围内所有符合条件的物体；false：只吸收距离最近的单个物体

    [Header("反馈效果")]
    public GameObject absorbEffectPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(absorbKey))
        {
            TryAbsorb();
        }
    }

    private void TryAbsorb()
    {
        // 使用 2D 圆形检测
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, absorbRadius, absorbableLayers);
        if (hits.Length == 0) return;

        if (!absorbAllInRange)
        {
            // 按距离排序（升序）
            System.Array.Sort(hits, (a, b) =>
                Vector2.Distance(a.transform.position, transform.position)
                .CompareTo(Vector2.Distance(b.transform.position, transform.position)));
        }

        foreach (Collider2D hit in hits)
        {
            // 播放特效
            if (absorbEffectPrefab != null)
                Instantiate(absorbEffectPrefab, hit.transform.position, Quaternion.identity);

            // 使物体消失
            
            hit.gameObject.SetActive(false);

            if (!absorbAllInRange) break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, absorbRadius);
    }
}