using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorPlatformButton : MonoBehaviour
{
    [SerializeField] private Object platform;

    [Header("移动参数")]
    [SerializeField] private float movingSpeed = 2f;
    [SerializeField] private  Transform[] waysPoints;

    private bool isMovingForward = false;   // 是否正在正向移动
    //private bool isReturning = false;  //是否返回
    private int pointIndex = 1;
    private float moveSpeed;    //电梯当下的速度
    private Rigidbody2D platformRB;

    private void Awake()
    {
        platformRB = platform.GetComponent<Rigidbody2D>();
        moveSpeed = movingSpeed;
    }

    private void Update()
    {
        if (isMovingForward && moveSpeed != 0)
        {
            platformRB.position = Vector2.MoveTowards(
                platformRB.position,
                waysPoints[pointIndex].position,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(platformRB.position, waysPoints[pointIndex].position) < 0.1f)
            {
                pointIndex++;
                if (pointIndex == waysPoints.Length)
                {
                    //不返回,电梯静止
                    isMovingForward = false;
                    moveSpeed = 0;
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && platform != null)
        {
            //Debug.Log("YES!!");
            isMovingForward = true;  
            if(pointIndex == 0)
                pointIndex = 1;
            else if(pointIndex == waysPoints.Length)
                pointIndex = 0;
            moveSpeed = movingSpeed;
        }
    }
    
}