using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;//场景切换都需要的包
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Player_TeleportState teleportState{ get;private set; }
    public static GameController instance;
    private Player player;
    private AudioSource GameMusic;
    [Header("GUI")]
    [SerializeField] private TMP_Text DeathTimes = null;
    //用二维向量记录复活位置
    SpriteRenderer sp;
    Rigidbody2D rb;
    Vector2 startPos;  // 保留，记录初始位置
    private int DeathTime = 0;
    
    private void Start()
    {   
        GameObject obj = GameObject.Find("Player");
        if (obj != null)
        {
            player = obj.GetComponent<Player>();
        }
        
        if(player != null)
        {
            startPos = player.transform.position;  // 记录起点
            sp = player.SR; 
            rb = player.RB;
        }

        
        teleportState = GetComponent<Player_TeleportState>();
        GameMusic = GetComponent<AudioSource>();
        GameMusic.Play();
        DeathTimes.text = Utilities.ShowDeathCount();

    }



    public void Die()
    {
        if (teleportState != null) teleportState.ResetState();
        
        // 尝试用检查点复活，如果没有激活的检查点，则使用 startPos
        bool hasCheckpoint = Checkpoint.HasActiveCheckpoint();  // 需要新增这个方法（见下方说明）
        if (hasCheckpoint)
        {
            Checkpoint.RespawnPlayer(player.gameObject);
        }
        else
        {
            player.transform.position = startPos;  // 直接设置到起点
        }
        DeathTimes.text = Utilities.UpdateDeathCount(ref DeathTime);

        StartCoroutine(Respawn());//死亡并不是重来
    }
   //死亡后延迟0.5秒再复活
    IEnumerator Respawn()
    {
        sp.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.2f);
        sp.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        // 不再设置 transform.position，因为已经在 Die 中处理好了
    }

    public void PauseGame()
    {
        Utilities.PauseGame();
    }

    public void RecoverGame()
    {
        Utilities.RestartLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        Utilities.InitialGame();
    }

    public void ReturnMenu()
    {
        Utilities.BackToMenu();
    }
}
