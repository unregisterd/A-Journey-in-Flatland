using UnityEngine;

public class LightUpLift : MonoBehaviour
{
    [Header("上升速度")]
    [SerializeField] private float liftSpeed = 5f;

    private Player player;


    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Trigger stay: " + other.gameObject.name);
        if (!other.CompareTag("Player")) return;
        player = other.GetComponent<Player>();
        player.SetVelocity(player.RB.velocity.x,liftSpeed);
    }

}