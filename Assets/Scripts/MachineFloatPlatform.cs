using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MachineFloatPlatform : MonoBehaviour
{
    [Header("平台顺序列表")]
    public List<GameObject> floatPlatform = new List<GameObject>();  // 玩家必须按此顺序踩踏
    [SerializeField] private bool changingColor = true;

    [Header("总时间限制（秒）")]
    [SerializeField] private float spaceTime = 10f;   // 从踩第一个平台开始计时

    [Header("完成时触发的事件")]
    public UnityEvent onComplete;   // 成功解开时执行

    // 内部状态
    private int currentIndex = 0;
    private float currentTimer = 0f;
    private bool isActive = false;

    [Header("触发事件")]
    private HashSet<GameObject> steppedPlatforms = new HashSet<GameObject>();

    private void Update()
    {
        if (isActive)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                ResetSequence();
            }
        }
    }

    // 使用碰撞检测（玩家必须站在平台上，平台不能是Trigger）
    private void Start()
{
    // 为列表中的每个平台绑定检测事件
    foreach (var platform in floatPlatform)
    {
        if (platform == null) continue;
        var step = platform.GetComponent<Platform>();
        
        step.onPlayerStep.AddListener(OnPlayerStep);
    }
}

    private void OnPlayerStep(GameObject platform)
    {
        // 这里的逻辑与之前的 OnCollisionEnter2D 完全相同
        if (!floatPlatform.Contains(platform)) return;
        if (currentIndex >= floatPlatform.Count) return;

        if (!isActive)
        {
            if (platform == floatPlatform[0])
            {
                StartSequence();
                StepOnPlatform(platform);
            }
            else
            {
                ResetSequence();
            }
        }
        else
        {
            if (steppedPlatforms.Contains(platform)) return;
            if (platform == floatPlatform[currentIndex])
            {
                StepOnPlatform(platform);
            }
            else
            {
                ResetSequence();
            }
        }
    }

    private void StartSequence()
    {
        isActive = true;
        currentTimer = spaceTime;
        steppedPlatforms.Clear();
        currentIndex = 0;
    }

    private void StepOnPlatform(GameObject platform)
    {
        SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();
        if (sr != null && changingColor) sr.color = Color.green;

        steppedPlatforms.Add(platform);
        Debug.Log("+1Platform");
        currentIndex++;

        if (currentIndex >= floatPlatform.Count)
        {
            CompleteSequence();
        }
    }

    private void CompleteSequence()
    {
        Debug.Log("完成！");
        isActive = false;
        onComplete?.Invoke();
    }

    private void ResetSequence()
    {
        // 恢复所有平台颜色
        foreach (var p in floatPlatform)
        {
            SpriteRenderer sr = p.GetComponent<SpriteRenderer>();
            if (sr != null && changingColor) sr.color = new Color32(0, 108, 255, 255);
        }
        isActive = false;
        currentIndex = 0;
        currentTimer = 0f;
        steppedPlatforms.Clear();
        Debug.Log("Restart!!");
    }

    public void ManualReset()
    {
        ResetSequence();
    }
}