using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePoint_4 : MonoBehaviour
{
    [SerializeField] private GameObject[] memories;

    private bool canExiting;
    private int cur_mNum;
    private int mLen;

    private void Awake()
    {
        canExiting = false;
        cur_mNum = 0;
        mLen = memories.Length;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cur_mNum = 0;
            //每当平台碰到玩家时检查数组中的记忆球玩家是否获取到
            foreach (GameObject memory in memories)
            {
                bool curCheck = memory.GetComponent<MemoryManager>().isTriggered;
                if(curCheck) cur_mNum ++;
            }
            
        }
        if(cur_mNum == mLen) canExiting = true;
        if (canExiting)
        {
            SceneController.instance.nextLevel();
        }
    }
}
