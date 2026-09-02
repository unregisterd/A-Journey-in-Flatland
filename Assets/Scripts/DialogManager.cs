using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour, IPointerClickHandler
{

    //添加单例模式(确保canvas在场景切换后不会丢失)
    public static DialogManager Instance { get; private set; }

    [Header("打字机设置")]
    [SerializeField] private float typeSpeed = 0.05f; // 每个字符的显示间隔（秒）

    private Queue<DialogLine> dialogQueue = new Queue<DialogLine>();
    private Player currentPlayer;
    private GameObject currentCanvasObj;
    private TMP_Text currentDialogText;
    private static bool isDialogActive = false;
    private bool isTyping = false;          // 是否正在逐字显示
    private Coroutine typingCoroutine;      // 当前打字协程的引用

    private void Awake()
    {
        // 2. 实现单例逻辑，确保场景中只有一个 DialogManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // 3. 关键！让这个游戏对象在加载新场景时不被销毁
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialog(List<DialogLine> lines, Player player)
    {
        
    Debug.Log($"[StartDialog] isDialogActive = {isDialogActive}, lines.Count = {lines.Count}");
    if (isDialogActive) 
    {
        Debug.LogWarning("[StartDialog] 对话已激活，拒绝新对话");
        return;
    }
    isDialogActive = true;

    dialogQueue.Clear();
    foreach (var line in lines)
        dialogQueue.Enqueue(line);
    currentPlayer = player;

    Time.timeScale = 0f;
    if (currentPlayer != null)
        currentPlayer.SetCanMove(false);

    Debug.Log($"[StartDialog] 队列已填充，数量 = {dialogQueue.Count}");
    NextLine();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTyping)
        {
            // 如果正在打字，跳过动画，直接显示完整文本
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            if (currentDialogText != null)
                currentDialogText.maxVisibleCharacters = currentDialogText.text.Length;
            isTyping = false;
        }
        else
        {
            // 文字已完整显示，进入下一句
            NextLine();
        }
    }

    private void NextLine()
    {
         Debug.Log($"[NextLine] 当前队列数量 = {dialogQueue.Count}, isTyping = {isTyping}");
    
        if (currentCanvasObj != null)
        Destroy(currentCanvasObj);

        if (dialogQueue.Count == 0)
        {
            Debug.Log("[NextLine] 队列为空，关闭对话");
            CloseDialog();
            return;
        }
        if (currentCanvasObj != null)
            Destroy(currentCanvasObj);

        if (dialogQueue.Count == 0)
        {
            CloseDialog();
            return;
        }

        DialogLine line = dialogQueue.Dequeue();


        if (line.dialogCanvasPrefab == null)
        {
            Debug.LogError("DialogLine 缺少 Canvas 预制体！");
            CloseDialog();
            return;
        }

        currentCanvasObj = Instantiate(line.dialogCanvasPrefab);
        currentCanvasObj.SetActive(true);

        currentDialogText = currentCanvasObj.GetComponentInChildren<TMP_Text>();
        if (currentDialogText == null)
        {
            Debug.LogError("Canvas 预制体中没有找到 TMP_Text 组件！");
            CloseDialog();
            return;
        }

        // 设置完整文本（稍后通过打字机逐步显示）
        currentDialogText.text = line.text;
        currentDialogText.maxVisibleCharacters = 0;  // 初始不可见
        EnsureCanvasClickable(currentCanvasObj);

        // 开始打字机效果
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(currentDialogText));
    }

    private IEnumerator TypeText(TMP_Text textComponent)
    {
        isTyping = true;
        int totalChars = textComponent.text.Length;
        textComponent.maxVisibleCharacters = 0;

        for (int i = 1; i <= totalChars; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(typeSpeed); // 使用不受 Time.timeScale 影响的等待
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void EnsureCanvasClickable(GameObject canvasObj)
    {
        Image img = canvasObj.GetComponent<Image>();
        if (img == null) img = canvasObj.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.01f);
        img.raycastTarget = true;

        ClickForwarder forwarder = canvasObj.GetComponent<ClickForwarder>();
        if (forwarder == null) forwarder = canvasObj.AddComponent<ClickForwarder>();
        forwarder.dialogManager = this;
    }

    private void CloseDialog()
{
    // 停止所有协程
    if (typingCoroutine != null)
        StopCoroutine(typingCoroutine);
    typingCoroutine = null;
    isTyping = false;

    // 销毁当前显示的 Canvas
    if (currentCanvasObj != null)
        Destroy(currentCanvasObj);
    currentCanvasObj = null;
    currentDialogText = null;

    // 清空队列（防止残留）
    dialogQueue.Clear();

    // 恢复游戏
    Time.timeScale = 1f;
    isDialogActive = false;

    // 恢复玩家移动
    if (currentPlayer != null)
        currentPlayer.SetCanMove(true);
    currentPlayer = null;
}
}