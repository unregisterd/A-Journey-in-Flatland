using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballMovement : MonoBehaviour
{
    [SerializeField] private  float moveSpeed = 3f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        while (true)
        {
            //首先向右
            SetVelocity(moveSpeed,0);
            StartCoroutine(Scroll());
            
        }
    }

    private void SetVelocity(float xVelocity,float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity,yVelocity);
    }

    private IEnumerator Scroll()
    {

        yield return new WaitForSeconds(0.5f);
    }
}
