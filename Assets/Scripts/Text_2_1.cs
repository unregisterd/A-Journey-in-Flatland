using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Text_2_1 : MonoBehaviour
{

    [SerializeField] private GameObject text;

    [Header("文字出现时间")]    
    [SerializeField] private float textTime = 2f;

    [Header("是否永久消失")]
    [SerializeField] private bool disappearForever = false;
    
    private void Start()
    {
        //显示的文本
        text.SetActive(false);
    }

   private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 获取玩家和物体的底部/顶部边界
            //float playerBottom = collision.collider.bounds.min.y;
            //float objectTop = GetComponent<Collider2D>().bounds.max.y;
        
            // 如果玩家从下方碰撞（玩家顶部接近物体底部）
            // 更简单的判断：玩家中心Y < 物体中心Y
            if (collision.transform.position.y < transform.position.y)
            {
                text.SetActive(true);
            }
        }
    }   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!disappearForever)
            {
                StartCoroutine(Wait());
            }
            else
            {
                text.SetActive(false);
            }
            
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(textTime);
        text.SetActive(false);
    }
}
