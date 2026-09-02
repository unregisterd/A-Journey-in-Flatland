using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//功能：可以让指定平台消失的开关

public class DisappearPlatformButton : MonoBehaviour
{
    [SerializeField] private float DisappearTime;//设置平台消失的时间
    [SerializeField] private GameObject platform;   // 在 Inspector 中拖入平台对象
    [SerializeField] private bool DisappearForever=false;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(DisappearForever)
                platform.SetActive(false);
            else
                StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        platform.SetActive(false);
        yield return new WaitForSeconds(DisappearTime);
        platform.SetActive(true);
    }
}
