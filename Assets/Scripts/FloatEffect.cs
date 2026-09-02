using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float floatHeight = 0.1f;
    public float floatSpeed = 1.5f;

    private Vector3 startPos;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin((Time.time + timeOffset) * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}