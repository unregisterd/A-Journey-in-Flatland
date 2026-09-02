using System;
using UnityEngine;

[Serializable]
public class DialogLine
{
    [Tooltip("这句话要显示的整个 Canvas 预制体（或场景中的 Canvas 对象）")]
    public GameObject dialogCanvasPrefab;   // 关键：每句话可以指定不同的 Canvas

    [Tooltip("显示在 Canvas 内的文本内容（会赋值给 Canvas 内名为 'DialogText' 的 TMP_Text 组件）")]
    [TextArea(2, 4)]
    public string text;
}