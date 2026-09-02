using UnityEngine;
using UnityEngine.EventSystems;

public class ClickForwarder : MonoBehaviour, IPointerClickHandler
{
    public DialogManager dialogManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ClickForwarder: 点击被触发");
        if (dialogManager != null)
            dialogManager.OnPointerClick(eventData);
        else
            Debug.LogError("ClickForwarder: dialogManager 为空！");
    }
}