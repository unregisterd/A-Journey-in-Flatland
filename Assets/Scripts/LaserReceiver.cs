using UnityEngine;
using UnityEngine.Events;

public class LaserReceiver : MonoBehaviour
{
    public UnityEvent onLaserHit;   // 可拖入开门等方法

    public bool isActivated = false;

    public void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        Debug.Log("Receive!");
        onLaserHit?.Invoke();
        // 例如改变颜色
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    // 可以用来重置状态（如果需要）
    public void Deactivate()
    {
        isActivated = false;
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}