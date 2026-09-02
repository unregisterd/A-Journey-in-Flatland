using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("检查点设置")]
    [SerializeField] private bool isActive = false;
    [SerializeField] private GameObject visualActive;
    [SerializeField] private GameObject visualInactive;

    private static Vector3 lastCheckpointPos;
    private static Checkpoint latestCheckpoint;

    private void Start()
    {
        UpdateVisual();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        if (latestCheckpoint != null && latestCheckpoint != this)
        {
            latestCheckpoint.isActive = false;
            latestCheckpoint.UpdateVisual();
        }

        isActive = true;
        lastCheckpointPos = transform.position;
        latestCheckpoint = this;
        UpdateVisual();

        Debug.Log("Checkpoint activated at " + transform.position);
    }

    private void UpdateVisual()
    {
        if (visualActive != null) visualActive.SetActive(isActive);
        if (visualInactive != null) visualInactive.SetActive(!isActive);
    }

    public static void RespawnPlayer(GameObject player)
    {
        if (lastCheckpointPos != Vector3.zero)
        {
            player.transform.position = lastCheckpointPos;
        }
        else
        {
            Debug.LogWarning("No checkpoint activated, using start position.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isActive ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    public static bool HasActiveCheckpoint()
    {
        return lastCheckpointPos != Vector3.zero;
    }
}