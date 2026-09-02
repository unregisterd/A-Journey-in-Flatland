using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_3_1 : MonoBehaviour
{
    [SerializeField] private GameObject text;

    [Header("检测参数")]
    [SerializeField] private float detectRadius = 1f;
    [SerializeField] private LayerMask detectLayer; 

    [Header("是否永久显示")]
    [SerializeField] private bool activeForever = false;

    [Header("文字出现时间")]    
    [SerializeField] private float textTime = 2f;

    private void Start()
    {
        //显示的文本
        text.SetActive(false);
    }

    private void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, detectLayer);

        if(!hit) return;

        text.SetActive(true);//显示文字

        if (!activeForever)
        {
            StartCoroutine(Wait());
        }

    }

    //使用圆形检测
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(textTime);
        text.SetActive(false);
    }
}
