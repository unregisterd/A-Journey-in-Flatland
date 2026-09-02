using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//终点：进行关卡切换
public class CompletePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneController.instance.nextLevel();
        }
    }
}