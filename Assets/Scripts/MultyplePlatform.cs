using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultyplePlatform : Platform
{
    [SerializeField] private List<GameObject> platforms = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        foreach(GameObject platform in platforms)
        {
            platform.SetActive(false);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach(GameObject platform in platforms)
            {
                platform.SetActive(true);
            }
        }
    }
    
}
