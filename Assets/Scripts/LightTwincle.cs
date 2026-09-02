using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; // 如果是URP的Light2D

public class LightTwincle : MonoBehaviour
{
    [Header("显现周期")]
    public float appearDuration = 1f;
    public float disappearDuration = 2f;

    private UnityEngine.Rendering.Universal.Light2D light2D; // 2D灯光组件

    private void Awake()
    {
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (light2D == null)
            Debug.LogError("LightTwincle needs a Light2D component!");
    }

    private void OnEnable()
    {
        StartCoroutine(LightCycle());
    }

    private IEnumerator LightCycle()
    {
        while (true)
        {
            // 消失阶段：关闭灯光，但物体本身保持激活
            if (light2D != null) light2D.enabled = false;
            yield return new WaitForSeconds(disappearDuration);

            // 显现阶段：开启灯光
            if (light2D != null) light2D.enabled = true;
            yield return new WaitForSeconds(appearDuration);
        }
    }
}