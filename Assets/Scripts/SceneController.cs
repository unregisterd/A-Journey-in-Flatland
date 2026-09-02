using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//场景切换都需要的包


public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator animator;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public void nextLevel()
    {
        StopAnim();
        //开启携程模式
        StartCoroutine(LoadLevel());
    }

    
    IEnumerator LoadLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);//传场景的下标
        yield return new WaitForSeconds(1);
    }

    private void StopAnim()
    {
        animator.SetTrigger("End");
    }
}
